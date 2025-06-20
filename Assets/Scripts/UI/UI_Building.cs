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

    private RectTransform content;
    private void Awake()
    {
        content = transform.GetChild(0).GetComponent<RectTransform>();
    }
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
            case "tower":
                HandleTower();
                break;
            case "goldmine":
                HandleGoldMine();
                break;
        }
    }
    private void HandleBarracks()
    {
        Barrack barrackBuilding = currentBuilding as Barrack;
        buildingName.text = "Barracks";
        buildingLevel.text = $"Level: {barrackBuilding.currentLevel}";
        buildingDescription.text = "Descriptions: " +
            "This is your barracks. It will deploy troop to defend your kingdom\n" +
            $"Troop Type: {barrackBuilding.troopType}";

        HandleButtonUpgrade();
    }
    private void HandleGoldMine()
    {
        GoldMine towerBuilding = currentBuilding as GoldMine;
        buildingName.text = "Gold Mine";
        buildingLevel.text = $"Level: {towerBuilding.currentLevel}";
        buildingDescription.text = "Descriptions: " +
            "This is a Gold Mine. Build it, and you gain money per night\n" +
            $"But you must make sure it is not destroyed";
        HandleButtonUpgrade();
    }
    private void HandleTower()
    {
        Tower towerBuilding = currentBuilding as Tower;
        buildingName.text = "Tower";
        buildingLevel.text = $"Level: {towerBuilding.currentLevel}";
        buildingDescription.text = "Descriptions: " +
            "This shall be your main Defense. It will help your  army defend your base";

        HandleButtonUpgrade();
    }
    public void TestButton()
    {
        Debug.Log("Test Button");
    }
    public void BuildAction()
    {
        if (currentBuilding == null)
        {
            Debug.Log("Building is not selected!");
            return;
        }
        bool upgradable = currentBuilding.currentLevel < currentBuilding.maxUpgradeLevel;
        if (currentBuilding.currentState == BuildingState.UnderConstruction)
        {
            Debug.Log("Upgrade Building: " +currentBuilding.gameObject.name);
            currentBuilding.Build();
            SetupBuilding(currentBuilding);
            content.gameObject.SetActive(false);
        }
        else if (currentBuilding.currentState == BuildingState.Constructed)
        {
            currentBuilding.Upgrade();
            SetupBuilding(currentBuilding);
        }
    }
    private void HandleButtonUpgrade()
    {
        bool upgradable = currentBuilding.currentLevel < currentBuilding.maxUpgradeLevel;
        bool buyable = GameplayManager.instance.CheckMoney(currentBuilding.costToBuild);

        Debug.Log($"Upgradable : {upgradable} | Buyable: {buyable}");
        //upgradeButton.onClick.RemoveAllListeners();
        if (currentBuilding.currentState == BuildingState.UnderConstruction)
        {
            upgradeButtonText.text = "Construct";
        }
        else if(currentBuilding.currentState == BuildingState.Constructed)
        {
            upgradeButtonText.text = upgradable ? "Upgrade" : "Max!";
        }
        buildingNextLevelCost.transform.parent.gameObject.SetActive(upgradable);
        upgradeButton.interactable = buyable;
        if (upgradable)
        {
            buildingNextLevelCost.text = $"{currentBuilding.costToBuild}";
        }
        else
        {
            upgradeButton.interactable = false;
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
