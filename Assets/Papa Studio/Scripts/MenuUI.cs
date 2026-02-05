using GamePolygon;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public static MenuUI instance;
    [SerializeField] Button startGameBtn;
    [SerializeField] Button characterBtn;
    [SerializeField] Button tutorialBtn;
    [SerializeField] Button closeTutorialBtn;
    [SerializeField] GameObject tutorialPanel;

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
    }

    void Start()
    {
        startGameBtn.onClick.AddListener(StartGameBtnOnClick);
        characterBtn.onClick.AddListener(CharacterBtnOnClick);

        tutorialBtn.onClick.AddListener(OnOpenTutorial);
        closeTutorialBtn.onClick.AddListener(OnCloseTutorial);
    }


    public void StartGameBtnOnClick()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);

    }
    public void CharacterBtnOnClick()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);

    }

    public void OnOpenTutorial()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);
        tutorialPanel.SetActive(true);
    }

    public void OnCloseTutorial()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);
        tutorialPanel.SetActive(false);
    }
    
}
