using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class VisualManager : MonoBehaviour
{
    [SerializeField] Light2D globalLight;
    [SerializeField] Color nightColor;
    [SerializeField] CanvasGroup gameplayPanel;
    [SerializeField] private Volume volume;
    private Vignette vignette;
    private float bufferIntensity;

    [Header("Panel #1")]
    [SerializeField] private RectTransform pausePanel;
    [SerializeField] private CanvasGroup loadPanel;
    [SerializeField] private Image backgroundDetail;
    [SerializeField] private TextMeshProUGUI currentNight;
    [SerializeField] private CanvasGroup DisplayResult;
    [SerializeField] private TextMeshProUGUI DisplayText;

    [Header("Panel Game Result Lose")]
    [SerializeField] private CanvasGroup losePanel;
    [Header("Panel Game Result Win")]
    [SerializeField] private CanvasGroup winPanel;
    public void Awake()
    {
        backgroundDetail.gameObject.SetActive(false);
        currentNight.gameObject.SetActive(false);
        
        DisplayResult.gameObject.SetActive(false);
        loadPanel.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(false);
        winPanel.gameObject.SetActive(false);

        globalLight.intensity = 1.0f;
        globalLight.color = Color.white;
        if (volume.profile.TryGet(out vignette))
        {
            bufferIntensity = vignette.intensity.value;
            vignette.intensity.value = 0;
        }
    }
    public void ChangeToNight()
    {
        backgroundDetail.gameObject.SetActive(false);
        currentNight.gameObject.SetActive(false);
        currentNight.text = $"Night <size=200%>{GameplayManager.instance.currentNight + 1}</size>";
        
        backgroundDetail.DOFade(0, 0f);
        currentNight.DOFade(0, 0);
        backgroundDetail.gameObject.SetActive(true);
        currentNight.gameObject.SetActive(true);

        gameplayPanel.GetComponent<RectTransform>().DOAnchorPosY(-300, 0.4f);

        backgroundDetail.DOFade(0.2f, 0.3f).OnComplete(async () =>
        {
            DOTween.To(
                () => vignette.intensity.value,          // Getter
                x => vignette.intensity.value = x,       // Setter
                bufferIntensity,                                     // Target value
                1f                                        // Duration
            );
            await Task.Delay(600);
            backgroundDetail.DOFade(0, 0.6f);
        });
        currentNight.DOFade(1, 0.3f).SetDelay(0.1f).OnComplete(async() =>
        {
            await Task.Delay(1200);
            currentNight.DOFade(0, 0.75f).OnComplete(() =>
            {
                backgroundDetail.gameObject.SetActive(false);
            });
        });
        
        
        currentNight.gameObject.SetActive(true);
        DOTween.To(() => globalLight.intensity,
                   x => globalLight.intensity = x,
                   0.22f, 1.5f);
        DOTween.To(() => globalLight.color,
                   x => globalLight.color = x,
                   nightColor, 1.5f).OnComplete(() =>
                   {
                       Debug.Log("Testing");
                   });
    }
    public void ChangeToDay()
    {
        DOTween.To(
                () => vignette.intensity.value,          // Getter
                x => vignette.intensity.value = x,       // Setter
                bufferIntensity,                                     // Target value
                1f                                        // Duration
        );
        DOTween.To(() => globalLight.intensity,
                   x => globalLight.intensity = x,
                   1f, 1.5f);
        DOTween.To(() => globalLight.color,
                   x => globalLight.color = x,
                   Color.white, 1.5f).OnComplete(async() =>
                   {
                       await Task.Delay(150);
                       gameplayPanel.GetComponent<RectTransform>().DOAnchorPosY(0, 0.4f);
                       GameplayManager.instance.UpdateBattleEnded();
                   });
    }
    public void GoToMainMenu()
    {
        loadPanel.alpha = 0;
        loadPanel.gameObject.SetActive(true);
        loadPanel.DOFade(1, 0.6f).OnComplete(async() =>
        {
            await Task.Delay(300);
            GameplayManager.instance.LoadScene(0);
        });
    }
    public void Restart()
    {
        loadPanel.alpha = 0;
        loadPanel.gameObject.SetActive(true);
        loadPanel.DOFade(1, 0.6f).OnComplete(async () =>
        {
            await Task.Delay(300);
            GameplayManager.instance.LoadScene(1);
        });
    }
    public void DisplayPause(bool value)
    {
        pausePanel.gameObject.SetActive(value);
    }
    public async void DisplayDefeat()
    {
        Debug.Log("Preparing to Displaying Defeat");
        losePanel.alpha = 0;
        losePanel.gameObject.SetActive(true);
        
        DisplayResult.alpha = 0;
        DisplayResult.gameObject.SetActive(true);
        DisplayText.text = "DEFEAT";
        DisplayText.color = Color.red;
        await Task.Delay(600);
        
        Debug.Log("Displaying Result panel");
        await DisplayResult.DOFade(1, 0.5f).AsyncWaitForCompletion();
        await Task.Delay(1000);
        
        await DisplayResult.DOFade(0, 1.5f).AsyncWaitForCompletion();
        await Task.Delay(500);

        Debug.Log("Displaying lose panel");
        losePanel.transform.localScale = Vector3.one * 0.7f;
        losePanel.alpha = 1;
        losePanel.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);

    }
}
