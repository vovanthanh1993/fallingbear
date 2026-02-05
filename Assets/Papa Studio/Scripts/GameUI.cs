using GamePolygon;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    [SerializeField] Button homeBtn;
    [SerializeField] Button retryBtn;
    [SerializeField] Button retryGameOverBtn;
    [SerializeField] Button homeGameOverBtn;
    [SerializeField] Transform HealthUI;

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
        homeBtn.onClick.AddListener(HomeBtnOnClick);
        retryBtn.onClick.AddListener(RetryBtnOnClick);
        retryGameOverBtn.onClick.AddListener(PlaySoundButtonClick);
        homeGameOverBtn.onClick.AddListener(PlaySoundButtonClick);
    }

    public void UpdateHealthUI(float currentHealth)
    {
        for (int i = 0; i < HealthUI.childCount; i++)
        {
            bool isActive = i < currentHealth;
            HealthUI.GetChild(i).gameObject.SetActive(isActive);
        }
    }

    public void HomeBtnOnClick()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);
        GameManager.instance.Menu();
    }
    public void RetryBtnOnClick()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);
        GameManager.instance.Reload();
    }
    public void PlaySoundButtonClick()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);
    }
    
}
