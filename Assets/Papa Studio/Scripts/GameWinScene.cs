using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GamePolygon;

public class GameWinScene : MonoBehaviour
{

    public GameObject[] Players;
    GameObject SelectedPlayer;

    [SerializeField] Button homeBtn;
    [SerializeField] Button continueBtn;
    [SerializeField] Button shopBtn;


    public void Start()
    {
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlayMusic(SoundManager.Instance.Menu);
        SoundManager.Instance.PlaySound(SoundManager.Instance.Finish);
        int PlayerPos = (PlayerPrefs.GetInt("CURRENT_CHARACTER", 0));
        Players[PlayerPos].SetActive(true);
        SelectedPlayer = Players[PlayerPos];

        homeBtn.onClick.AddListener(PlaySoundButtonClick);
        continueBtn.onClick.AddListener(PlaySoundButtonClick);
        shopBtn.onClick.AddListener(PlaySoundButtonClick);
    }

    public void Next()
    {
        SceneManager.LoadScene("Game");

    }

    public void Menu()
    {

        SceneManager.LoadScene("Menu");
    }
    public void Shop()
    {

        SceneManager.LoadScene("Shop");
    }
    public void PlaySoundButtonClick()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);
    }
}
