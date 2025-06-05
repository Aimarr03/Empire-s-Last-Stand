using TMPro;
using UnityEngine;

public class UI_Gameplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Gold_Text;
    [SerializeField] private TextMeshProUGUI Night_Text;
    public void UpdateGoldText(int gold)
    {
        Gold_Text.text = gold.ToString();
    }
}
