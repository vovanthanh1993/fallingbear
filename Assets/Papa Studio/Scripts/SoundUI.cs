using GamePolygon;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    [SerializeField] Image soundBG;
    [SerializeField] Image soundIcon;
    [SerializeField] Sprite soundOnIcon;
    [SerializeField] Sprite soundOffIcon;

    public Button musicButton;

    private bool isMuteSound;

    private const string MUTE_PREF_KEY = "MutePreference";
    private const int MUTED = 1;
    private const int UN_MUTED = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMuteSound = PlayerPrefs.GetInt(MUTE_PREF_KEY, UN_MUTED) == MUTED;
        ApplyMusicState();

        musicButton.onClick.AddListener(ToggleMute);
    }

    void ApplyMusicState()
    {
        soundIcon.sprite = isMuteSound ? soundOffIcon : soundOnIcon;
        soundBG.color = isMuteSound ? Color.red : Color.green;

    }

    public void ToggleMute()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);
        SoundManager.Instance.ToggleMute();
        isMuteSound = PlayerPrefs.GetInt(MUTE_PREF_KEY, UN_MUTED) == MUTED;
        ApplyMusicState();
    }
}
