using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private RectTransform options;
    [SerializeField] private TextMeshProUGUI sfx_volume, music_volume;
    [SerializeField] private RectTransform credits;
    private void Awake()
    {
        music_volume.text = ((int)(AudioManager.instance.musicVolume * 100)).ToString();
        sfx_volume.text = ((int)(AudioManager.instance.sfxVolume * 100)).ToString();
    }
    void Start()
    {
        AudioManager.instance.PlayMusic("menu");
    }
    public void PlayGame()
    {
        Debug.Log("Play Game");
    }
    public void OpenOption(bool value)
    {
        options.gameObject.SetActive(value);
    }
    public void OpenCredits(bool value)
    {
        credits.gameObject.SetActive(value);
    }
    public void ExitGame()
    {
        Debug.Log("Exit Game");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void Music_VolumeUp()
    {
        AudioManager.instance.VolumeUp_Music();
        music_volume.text = ((int)(AudioManager.instance.musicVolume * 100)).ToString();
    }
    public void Music_VolumeDown()
    {
        AudioManager.instance.VolumeDown_Music();
        music_volume.text = ((int)(AudioManager.instance.musicVolume * 100)).ToString();
    }
    public void SFX_VolumeUp()
    {
        AudioManager.instance.VolumeUp_SFX();
        sfx_volume.text = ((int)(AudioManager.instance.sfxVolume * 100)).ToString();
    }
    public void SFX_VolumeDown()
    {
        AudioManager.instance.VolumeDown_SFX();
        sfx_volume.text = ((int)(AudioManager.instance.sfxVolume * 100)).ToString();
    }

}
