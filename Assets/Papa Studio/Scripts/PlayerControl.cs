using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeautifulTransitions.Scripts.Transitions;
using MoreMountains.Feedbacks;
using System;
using GamePolygon;


public class PlayerControl : MonoBehaviour
{
    Rigidbody rb;
    Transform Spawn;
    public GameObject LeftStick, RightStick, stickMan;
    public MMFeedbacks text;
    Animator anim;

    public Character[] characters;
    public Character SelectedCharacter;

    public PlayerStatus playerStatus;

    private float baseGravityForce = 9.8f;
    private float gravityMultiplier = 1f;
    private bool isFalling = false;


    // Start is called before the first frame update
    void Start()
    {
        Spawn = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        Vector3 pos = Spawn.position;
        transform.position = pos;

        rb = GetComponent<Rigidbody>();
        int PlayerPos = PlayerPrefs.GetInt("CURRENT_CHARACTER", 0);
        characters[PlayerPos].gameObject.SetActive(true);
        SelectedCharacter = characters[PlayerPos];
        playerStatus.SetCurrentCharacterStatus(SelectedCharacter);

        gravityMultiplier = SelectedCharacter.gravityMultiplier;

        anim = SelectedCharacter.gameObject.GetComponent<Animator>();
        TransitionHelper.TransitionIn(LeftStick);
        TransitionHelper.TransitionIn(RightStick);
        TransitionHelper.TransitionIn(stickMan);
        anim.SetBool("Down", true);
        rb.linearDamping = 50;
        isFalling = false;
        rb.sleepThreshold = 0.0f;

        StartCoroutine(waitforstart());

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.GameEnd && !GameManager.instance.GameEndWin)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TransitionHelper.TransitionIn(LeftStick);
                TransitionHelper.TransitionIn(RightStick);
                TransitionHelper.TransitionIn(stickMan);
                SoundManager.Instance.PlaySound(SoundManager.Instance.Drum);

                anim.SetBool("Down", true);

                rb.linearDamping = 50;
                isFalling = false;
                rb.WakeUp();

            }
            if (Input.GetMouseButtonUp(0))
            {
                rb.linearDamping = 0f;
                isFalling = true;

                TransitionHelper.TransitionOut(LeftStick);
                TransitionHelper.TransitionOut(RightStick);
                TransitionHelper.TransitionOut(stickMan);
                anim.SetBool("Down", false);
                rb.WakeUp();
            }

        }
        else if (GameManager.instance.GameEndWin)
        {
            rb.linearDamping = 50f;
            isFalling = false;
            rb.WakeUp();
        }
        else
        {
            rb.linearDamping = 0f;
            isFalling = true;
            rb.WakeUp();
        }

    }

    void FixedUpdate()
    {
        if (!isFalling) return;

        Vector3 gravity = Vector3.down * baseGravityForce * gravityMultiplier;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    IEnumerator waitforstart()
    {
        yield return new WaitForSeconds(2f);
        rb.linearDamping = 0f;
        isFalling = true;
        TransitionHelper.TransitionOut(LeftStick);
        TransitionHelper.TransitionOut(RightStick);
        TransitionHelper.TransitionOut(stickMan);
        anim.SetBool("Down", false);
        rb.WakeUp();
    }

}
