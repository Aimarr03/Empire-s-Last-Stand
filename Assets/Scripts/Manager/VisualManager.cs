using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VisualManager : MonoBehaviour
{
    [SerializeField] Light2D globalLight;
    [SerializeField] Color nightColor;
    public void Awake()
    {
        globalLight.intensity = 1.0f;
        globalLight.color = Color.white;
    }
    public void ChangeToNight()
    {
        DOTween.To(() => globalLight.intensity,
                   x => globalLight.intensity = x,
                   0.22f, 1.5f);
        DOTween.To(() => globalLight.color,
                   x => globalLight.color = x,
                   nightColor, 1.5f);
    }
}
