using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private RectTransform options;
    [SerializeField] private TextMeshProUGUI sfx_volume, music_volume;
    [SerializeField] private RectTransform credits;
    [SerializeField] private Toggle sfxToggle, musicToggle;
    [SerializeField] private CanvasGroup Loading;
    bool loading = false;
    private void Awake()
    {
        
    }
    void Start()
    {
        sfxToggle.isOn = !AudioManager.instance.sfxMuted;
        musicToggle.isOn = !AudioManager.instance.musicMuted;
        music_volume.text = ((int)(AudioManager.instance.musicVolume * 100)).ToString();
        sfx_volume.text = ((int)(AudioManager.instance.sfxVolume * 100)).ToString();
        AudioManager.instance.PlayMusic("menu");
    }
    public void PlayGame()
    {
        if (loading) return;
        Debug.Log("Play Game");
        AudioManager.instance.PlayClick();
        loading = true;
        Loading.alpha = 0;
        Loading.gameObject.SetActive(true);
        Loading.DOFade(1, 1.5f).OnComplete(async () =>
        {
            await Task.Delay(100);
            await SceneManager.LoadSceneAsync(1);
            Debug.Log("Loading Complete");
        });
    }
    public void OpenOption(bool value)
    {
        if (loading) return;
        options.gameObject.SetActive(value);
        if (value) AudioManager.instance.PlayClick();
        else AudioManager.instance.PlayCancel();
    }
    public void OpenCredits(bool value)
    {
        if (loading) return;
        credits.gameObject.SetActive(value);
        if (value) AudioManager.instance.PlayClick();
        else AudioManager.instance.PlayCancel();
    }
    public void ExitGame()
    {
        Debug.Log("Exit Game");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
        AudioManager.instance.PlayCancel();
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
