using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class VisualManager : MonoBehaviour
{
    [SerializeField] Light2D globalLight;
    [SerializeField] Color nightColor;

    [SerializeField] private Image backgroundDetail;
    [SerializeField] private TextMeshProUGUI currentNight;
    public void Awake()
    {
        backgroundDetail.gameObject.SetActive(false);
        currentNight.gameObject.SetActive(false);
        globalLight.intensity = 1.0f;
        globalLight.color = Color.white;
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
        
        backgroundDetail.DOFade(0.2f, 0.3f).OnComplete(async () =>
        {
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
                   nightColor, 1.5f);
    }
    public void ChangeToDay()
    {
        DOTween.To(() => globalLight.intensity,
                   x => globalLight.intensity = x,
                   1f, 1.5f);
        DOTween.To(() => globalLight.color,
                   x => globalLight.color = x,
                   Color.white, 1.5f).OnComplete(async() =>
                   {
                       await Task.Delay(150);
                       GameplayManager.instance.UpdateBattleEnded();
                   });
    }
    public void DisplayDefeat()
    {

    }
}
