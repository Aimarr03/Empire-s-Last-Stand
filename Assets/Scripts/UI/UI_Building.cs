using Game.Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Building : MonoBehaviour
{
    private Building currentBuilding;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI buildingName;
    [SerializeField] private TextMeshProUGUI buildingLevel;
    [SerializeField] private TextMeshProUGUI upgradeButtonText;
    [SerializeField] private TextMeshProUGUI buildingNextLevelCost;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI buildingDescription;

    public void SetupBuilding(Building building)
    {
        currentBuilding = building;
        switch (building.buildingName)
        {
            case "townhall":
                HandleTownHallInformation();
                break;
            case "barracks":
                HandleBarracks();
                break;
        }
    }
    private void HandleBarracks()
    {
        Barrack barrackBuilding = currentBuilding as Barrack;
        buildingName.text = "Barracks";
        buildingLevel.text = $"Level: {barrackBuilding.upgradeLevel}";
        buildingDescription.text = "Descriptions: " +
            "This is your barracks. It will deploy troop to defend your kingdom\n" +
            $"Troop Type: {barrackBuilding.troopType}";

        HandleButtonUpgrade();
    }
    private void HandleButtonUpgrade()
    {
        bool upgradable = currentBuilding.upgradeLevel < currentBuilding.maxUpgradeLevel;
        bool buyable = GameplayManager.instance.CheckMoney(currentBuilding.cost);
        upgradeButtonText.text = upgradable ? "Upgrade" : "Max!";
        
        buildingNextLevelCost.transform.parent.gameObject.SetActive(upgradable);
        upgradeButton.interactable = buyable && upgradable;
        if (upgradable)
        {
            buildingNextLevelCost.text = $"{currentBuilding.cost}";
        }
    }
    private void HandleTownHallInformation()
    {
        upgradeButton.interactable = false;
        buildingDescription.text = "Description: \n" +
            "This is your Town Hall, defend it at all cost from the enemies\n" +
            $"Health: {currentBuilding.currentHP}";
        buildingNextLevelCost.transform.parent.gameObject.SetActive(false);
        buildingName.text = "Town Hall";
        buildingLevel.text = "";
        upgradeButtonText.text = "";
    }
}
