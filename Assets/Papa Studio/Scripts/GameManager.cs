using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using GamePolygon;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    int CurrentLevel;
    public static GameManager instance;
    public GameObject GameOverUI, Blast;
    public Text currentText, nextText;
    public GameObject hangPlayer;
    [HideInInspector]
    public bool GameEnd, GameEndWin;
    public GameObject ActiveBody, Deadbody, pole;
    public MMFeedbacks GameOverFeedBack;

    public GameObject[] Level;

    PlayerControl playerControl;
    PlayerStatus playerStatus;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);

        }
        CurrentLevel = PlayerPrefs.GetInt("Level", 0);
        Instantiate(Level[CurrentLevel]);
    }

    private void Start()
    {
        GameEnd = false;
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlayMusic(SoundManager.Instance.Game);
        currentText.text = "" + (CurrentLevel);
        nextText.text = "" + (CurrentLevel + 1);

        hangPlayer.SetActive(true);
        playerStatus = hangPlayer.GetComponent<PlayerStatus>();
        playerStatus.onPlayerDied += GameOver;

    }

    public void GameWin()
    {
        GameEndWin = true;
        StartCoroutine(ShowGameWin());

    }

    public void PlayerTakeDmg()
    {
        playerStatus.PlayerTakeDamage(1);
    }

    public void PlayerInKillZone()
    {
        playerStatus.PlayerKillZone();
    }

    public void OnCollectImmortalItem(float timeBuff)
    {
        playerStatus.PlayerGetImmortal(timeBuff);
    }

    public void OnCollectFreezeItem(float timeFreeze)
    {
        StartCoroutine(SlowTime(timeFreeze));
    }
    IEnumerator SlowTime(float timeFreeze)
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(timeFreeze);
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.GameOVer);
        pole.SetActive(false);
        GameEnd = true;
        StartCoroutine(ShowGameOver());


    }

    IEnumerator ShowGameWin()
    {

        Blast.SetActive(true);
        PlayerPrefs.SetInt("Level", CurrentLevel + 1);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("GameWin");


    }

    IEnumerator ShowGameOver()
    {
        SoundManager.Instance.StopMusic();

        SoundManager.Instance.PlaySound(SoundManager.Instance.Die);

        yield return new WaitForSeconds(2.0f);
        Debug.Log("GameOVer");
        GameOverUI.SetActive(true);

    }


    public void Reload()
    {
        LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {

        SceneManager.LoadScene("Menu");
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadAsyncRoutine(sceneName));
    }

    IEnumerator LoadAsyncRoutine(string sceneToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
    

}
