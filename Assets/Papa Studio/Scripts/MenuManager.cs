using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GamePolygon;
public class MenuManager : MonoBehaviour {


	public Text CurrentLevelText;
   

    int CurrentLevel;
	

	// Use this for initialization
	void Start () {
        SoundManager.Instance.StopMusic();

        SoundManager.Instance.PlayMusic(SoundManager.Instance.Menu);
        CurrentLevel = PlayerPrefs.GetInt("Level", 1);
        CurrentLevelText.text = "LEVEL " + CurrentLevel;
        
	}


    public void LoadLevel()
    {

        // SceneManager.LoadSceneAsync ("Game");
        LoadSceneAsync("Game");
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

        // yield return new WaitForSeconds(0.5f);

        operation.allowSceneActivation = true;
    }




    public void Restart()
    {

        // SceneManager.LoadSceneAsync("Game");
        // LoadSceneAsync();
    }

    public void Home()
    {
        SceneManager.LoadSceneAsync("Menu");

    }

    public void Shop()
    {
        SceneManager.LoadSceneAsync("Shop");

    }

   



}
