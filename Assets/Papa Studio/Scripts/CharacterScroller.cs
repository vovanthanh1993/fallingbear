using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GamePolygon;

public class CharacterScroller : MonoBehaviour
{
    [Header("Scroller Config")]
    public float minScale = 1f;
    public float maxScale = 1.5f;
    public float characterSpace = 3f;
    public float moveForwardAmount = 2f;
    public float swipeThresholdX = 5f;
    public float swipeThresholdY = 30f;
    public float rotateSpeed = 30f;
    public float snapTime = 0.3f;
    public float resetRotateSpeed = 180f;
    [Range(0.1f, 1f)]
    public float scrollSpeedFactor = 0.25f;
    public Vector3 centerPoint;
    public Vector3 originalScale = Vector3.one;
    public Vector3 originalRotation = Vector3.zero;

    [Header("Object References")]
    public Text totalCoins;
    public Text priceText;
    public Image priceImg;
    public Text hPText;
    public Image hPImg;
    public Text scaleText;
    public Image scaleImg;
    public Button selectButon;
    public Button unlockButton;
    public Button lockButton;
    public Color lockColor = Color.black;

    List<GameObject> listCharacter = new List<GameObject>();
    GameObject currentCharacter;
    GameObject lastCurrentCharacter;
    Character currentCharacterData; // Cache Character component để tránh GetComponent mỗi lần (tối ưu cho WebGL)
    IEnumerator rotateCoroutine;
    Vector3 startPos;
    Vector3 endPos;
    float startTime;
    float endTime;
    bool isCurrentCharacterRotating = false;
    bool hasMoved = false;

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        //lockColor.a = 0;    // need this for later setting material colors to work
        SoundManager.Instance.PlayMusic(SoundManager.Instance.Shop);

        int currentCharacterIndex = CharacterManager.Instance.CurrentCharacterIndex;
        currentCharacterIndex = Mathf.Clamp(currentCharacterIndex, 0, CharacterManager.Instance.characters.Length - 1);

        for (int i = 0; i < CharacterManager.Instance.characters.Length; i++)
        {
            int deltaIndex = i - currentCharacterIndex;

            GameObject character = (GameObject)Instantiate(CharacterManager.Instance.characters[i], centerPoint, Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z));
            Character charData = character.GetComponent<Character>();
            charData.characterSequenceNumber = i;
            listCharacter.Add(character);
            character.transform.localScale = originalScale;
            character.transform.position = centerPoint + new Vector3(deltaIndex * characterSpace, 0, 0);
            character.transform.parent = transform;
        }

        // Highlight current character
        currentCharacter = listCharacter[currentCharacterIndex];
        currentCharacter.transform.localScale = maxScale * originalScale;
        currentCharacter.transform.position += moveForwardAmount * Vector3.forward;
        lastCurrentCharacter = null;
        // Cache Character component
        currentCharacterData = currentCharacter.GetComponent<Character>();
        StartRotateCurrentCharacter();

        // Khởi tạo UI một lần
        totalCoins.text = CoinManager.Instance.Coins.ToString();
        UpdateCharacterInfoUI();

        // Đăng ký event để cập nhật coin khi thay đổi
        CoinManager.CoinsUpdated += OnCoinsUpdated;

        selectButon.onClick.AddListener(PlayClickSound);
        unlockButton.onClick.AddListener(PlayClickSound);
        lockButton.onClick.AddListener(PlayClickSound);
    }

    void OnDestroy()
    {
        // Hủy đăng ký event khi object bị destroy
        CoinManager.CoinsUpdated -= OnCoinsUpdated;
    }

    void OnCoinsUpdated(int newCoinsValue)
    {
        totalCoins.text = newCoinsValue.ToString();
        // Cập nhật lại UI button unlock/lock vì có thể đã đủ coin để unlock
        UpdateCharacterInfoUI();
    }

    // Update is called once per frame
    void Update()
    {
        #region Scrolling
        // Do the scrolling stuff
        if (Input.GetMouseButtonDown(0))    // first touch
        {
            startPos = Input.mousePosition;
            startTime = Time.time;
            hasMoved = false;
        }
        else if (Input.GetMouseButton(0))   // touch stays
        {
            endPos = Input.mousePosition;
            endTime = Time.time;

            float deltaX = Mathf.Abs(startPos.x - endPos.x);
            float deltaY = Mathf.Abs(startPos.y - endPos.y);

            if (deltaX >= swipeThresholdX && deltaY <= swipeThresholdY)
            {
                hasMoved = true;

                if (isCurrentCharacterRotating)
                    StopRotateCurrentCharacter(true);

                float speed = deltaX / (endTime - startTime);
                Vector3 dir = (startPos.x - endPos.x < 0) ? Vector3.right : Vector3.left;
                Vector3 moveVector = dir * (speed / 10) * scrollSpeedFactor * Time.deltaTime;

                // Move and scale the children
                for (int i = 0; i < listCharacter.Count; i++)
                {
                    MoveAndScale(listCharacter[i].transform, moveVector);
                }

                // Update for next step
                startPos = endPos;
                startTime = endTime;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (hasMoved)
            {
                // Store the last currentCharacter
                lastCurrentCharacter = currentCharacter;

                // Update current character to the one nearest to center point
                currentCharacter = FindCharacterNearestToCenter();
                // Cache Character component khi character thay đổi
                if (currentCharacter != null)
                {
                    currentCharacterData = currentCharacter.GetComponent<Character>();
                }

                // Snap
                float snapDistance = centerPoint.x - currentCharacter.transform.position.x;
                StartCoroutine(SnapAndRotate(snapDistance));
            }
        }

        #endregion
    }

    void MoveAndScale(Transform tf, Vector3 moveVector)
    {
        // Move
        tf.position += moveVector;

        // Scale and move forward according to distance from current position to center position
        float d = Mathf.Abs(tf.position.x - centerPoint.x);
        if (d < (characterSpace / 2))
        {
            float factor = 1 - d / (characterSpace / 2);
            float scaleFactor = Mathf.Lerp(minScale, maxScale, factor);
            tf.localScale = scaleFactor * originalScale;

            float fd = Mathf.Lerp(0, moveForwardAmount, factor);
            Vector3 pos = tf.position;
            pos.z = centerPoint.z + fd;
            tf.position = pos;
        }
        else
        {
            tf.localScale = minScale * originalScale;
            Vector3 pos = tf.position;
            pos.z = centerPoint.z;
            tf.position = pos;
        }
    }

    GameObject FindCharacterNearestToCenter()
    {
        float min = -1;
        GameObject nearestObj = null;

        for (int i = 0; i < listCharacter.Count; i++)
        {
            float d = Mathf.Abs(listCharacter[i].transform.position.x - centerPoint.x);
            if (d < min || min < 0)
            {
                min = d;
                nearestObj = listCharacter[i];
            }
        }

        return nearestObj;
    }

    void UpdateCharacterInfoUI()
    {
        // Sử dụng cached Character component thay vì GetComponent mỗi lần (tối ưu cho WebGL)
        if (currentCharacterData == null && currentCharacter != null)
        {
            currentCharacterData = currentCharacter.GetComponent<Character>();
        }

        if (currentCharacterData == null) return;

        Character charData = currentCharacterData;

        if (!charData.isFree)
        {
            priceText.gameObject.SetActive(true);
            priceText.text = charData.price.ToString();
            priceImg.gameObject.SetActive(true);
        }
        else
        {
            priceText.gameObject.SetActive(false);
            priceImg.gameObject.SetActive(false);
        }

        hPText.text = charData.characterHealth.ToString();
        scaleText.text = charData.gravityMultiplier.ToString();

        if (charData.IsUnlocked)
        {
            unlockButton.gameObject.SetActive(false);
            lockButton.gameObject.SetActive(false);
            selectButon.gameObject.SetActive(true);
        }
        else
        {
            selectButon.gameObject.SetActive(false);
            if (CoinManager.Instance.Coins >= charData.price)
            {
                unlockButton.gameObject.SetActive(true);
                lockButton.gameObject.SetActive(false);
            }
            else
            {
                unlockButton.gameObject.SetActive(false);
                lockButton.gameObject.SetActive(true);
            }
        }
    }

    IEnumerator SnapAndRotate(float snapDistance)
    {
        float snapDistanceAbs = Mathf.Abs(snapDistance);
        float snapSpeed = snapDistanceAbs / snapTime;
        float sign = snapDistance / snapDistanceAbs;
        float movedDistance = 0;

        //  SoundManager.Instance.PlaySound(SoundManager.Instance.Tick);

        while (Mathf.Abs(movedDistance) < snapDistanceAbs)
        {
            float d = sign * snapSpeed * Time.deltaTime;
            float remainedDistance = Mathf.Abs(snapDistanceAbs - Mathf.Abs(movedDistance));
            d = Mathf.Clamp(d, -remainedDistance, remainedDistance);

            Vector3 moveVector = new Vector3(d, 0, 0);
            for (int i = 0; i < listCharacter.Count; i++)
            {
                MoveAndScale(listCharacter[i].transform, moveVector);
            }

            movedDistance += d;
            yield return null;
        }

        if (currentCharacter != lastCurrentCharacter || !isCurrentCharacterRotating)
        {
            // Stop rotating the last current character
            StopRotateCurrentCharacter();

            // Now rotate the new current character
            StartRotateCurrentCharacter();

            // Cập nhật UI khi character thay đổi
            UpdateCharacterInfoUI();
        }
    }

    void StartRotateCurrentCharacter()
    {
        StopRotateCurrentCharacter(false);   // stop previous rotation if any
        rotateCoroutine = CRRotateCharacter(currentCharacter.transform);
        StartCoroutine(rotateCoroutine);
        isCurrentCharacterRotating = true;
    }

    void StopRotateCurrentCharacter(bool resetRotation = false)
    {
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }

        isCurrentCharacterRotating = false;

        if (resetRotation)
            StartCoroutine(CRResetCharacterRotation(currentCharacter.transform));
    }

    IEnumerator CRRotateCharacter(Transform charTf)
    {
        while (true)
        {
            charTf.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
            yield return null;
        }
    }

    IEnumerator CRResetCharacterRotation(Transform charTf)
    {
        Vector3 startRotation = charTf.rotation.eulerAngles;
        Vector3 endRotation = originalRotation;
        float timePast = 0;
        float rotateAngle = Mathf.Abs(endRotation.y - startRotation.y);
        float rotateTime = rotateAngle / resetRotateSpeed;

        while (timePast < rotateTime)
        {
            timePast += Time.deltaTime;
            Vector3 rotation = Vector3.Lerp(startRotation, endRotation, timePast / rotateTime);
            charTf.rotation = Quaternion.Euler(rotation);
            yield return null;
        }
    }

    public void UnlockButton()
    {
        if (currentCharacterData == null) return;
        
        bool unlockSucceeded = currentCharacterData.Unlock();
        if (unlockSucceeded)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.coin);

            Renderer renderer = currentCharacter.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.SetColor("_Color", Color.white);
            }
            // Cập nhật UI sau khi unlock
            UpdateCharacterInfoUI();
        }
    }

    public void SelectButton()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);

        if (currentCharacterData != null)
        {
            CharacterManager.Instance.CurrentCharacterIndex = currentCharacterData.characterSequenceNumber;
        }
        Exit();
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }
    public void PlayClickSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);
    }
}
