using DG.Tweening;
using Game.Buildings;
using System.Collections.Generic;
using UnityEngine;

public class Manager_UnitSelection : MonoBehaviour
{
    private static Manager_UnitSelection _instance;
    public static Manager_UnitSelection Instance
    { 
        get { return _instance; } 
        private set { _instance = value; }
    }

    public List<GameObject> AllUnits = new List<GameObject>();
    public List<GameObject> SelectedUnits = new List<GameObject>();
    public UI_Building ui_building;
    public RectTransform selectedBuildingUI;
    private GameObject _selectedBuilding;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DeselectAll();
        }
        else
        {
            Destroy(_instance.gameObject);
            Debug.Log("Singleton - Already have more than one Manager Unit Selection");
        }
    }
    private void Start()
    {
        GameplayManager.instance.ChangeState += Instance_ChangeState;
    }
    private void OnDestroy()
    {
        GameplayManager.instance.ChangeState -= Instance_ChangeState;
    }

    private void Instance_ChangeState()
    {
        ui_building.gameObject.SetActive(false);
    }

    public void SelectUnit(GameObject unitToAdd)
    {
        DeselectAll();
        Debug.Log("Selection - Select Unit " + unitToAdd.name);
        SelectedUnits.Add(unitToAdd);
        unitToAdd.GetComponent<UnitController>().selection.gameObject.SetActive(true);
    }
    public void SelectBuilding(GameObject building)
    {
        DeselectAll();
        if(GameplayManager.instance.gameState != GameState.Morning)
        {
            return;
        }
        _selectedBuilding = building;
        Debug.Log("Selection - Select Building " + building.name);
        HandleUI();
    }
    private void HandleUI()
    {
        if (_selectedBuilding == null)
        {
            selectedBuildingUI.gameObject.SetActive(false);
        }
        else
        {
            selectedBuildingUI.gameObject.SetActive(true);
            selectedBuildingUI.transform.localScale = new Vector3(0.7f,0.7f,0.7f);
            selectedBuildingUI.transform.DOScale(1, 0.25f).SetEase(Ease.OutBack);

            Vector3 buildingPos = _selectedBuilding.transform.position;
            //Debug.Log("Building Pos in World: " + buildingPos);

            Vector3 ScreenBuildingPos = Camera.main.WorldToScreenPoint(buildingPos);
            Vector3 ViewBuildingPos = Camera.main.WorldToViewportPoint(buildingPos);
            //Debug.Log("Building Pos in ScreenPoint: " + ScreenBuildingPos);
            //Debug.Log("Building Pos in Viewport: " + ViewBuildingPos);

            float halfWidth = Screen.width / 2;
            float halfHeight = Screen.height / 2;
            //Debug.Log("Screen Size: " + halfHeight + " half height | " + halfWidth + " half width");
            //Debug.Log("UI Building " + selectedBuildingUI.transform.position);

            Vector3 offsetPos = new Vector3(4, 2, 0);
            offsetPos.x = ScreenBuildingPos.x > halfWidth ? -80 : 80;
            offsetPos.y = ScreenBuildingPos.y > halfHeight? -40 : 40;
            
            //selectedBuildingUI.transform.position = ScreenBuildingPos + offsetPos;
            
            Building building = _selectedBuilding.GetComponent<Building>();
            ui_building.SetupBuilding(building);
        }
    }
    public void CommandTroops(Vector3 pos)
    {
        if (SelectedUnits.Count == 0) return;
        float row = 0;
        float column = 0;
        int index = 0;
        AudioManager.instance.PlayClick();
        foreach (var unit in SelectedUnits)
        {
            UnitController unitController = unit.GetComponent<UnitController>();
            Vector2 targetPos = new Vector2(pos.x + column, pos.y + row);
            unitController.CommandTroop(targetPos);
            index++;
            if (index % 4 == 0)
            {
                row += 1.1f;
                column = 0;
            }
            else
            {
                column += 1.1f;
            }
        }
    }
        
    public void ShiftSelectUnit(GameObject unitToAdd)
    {
        bool notContainUnit = !SelectedUnits.Contains(unitToAdd);
        if (notContainUnit)
        {
            Debug.Log("Selection - Add New Unit " + unitToAdd.name);
            SelectedUnits.Add(unitToAdd);
            unitToAdd.GetComponent<UnitController>().selection.gameObject.SetActive(true);
        }
        else
        {
            Deselect(unitToAdd);
        }
    }
    public void DragSelect(GameObject unitToAdd)
    {
        if (!unitToAdd.CompareTag("Unit")) return;
        bool notContainUnit = !SelectedUnits.Contains(unitToAdd);
        if (notContainUnit)
        {
            Debug.Log("Selection - Add New Unit " + unitToAdd.name);
            SelectedUnits.Add(unitToAdd);
            unitToAdd.GetComponent<UnitController>().selection.gameObject.SetActive(true);
        }
    }
    public void DeselectAll()
    {
        Debug.Log("Selection - Remove All Unit ");
        _selectedBuilding = null;
        for (int i = 0; i < SelectedUnits.Count; i++)
        {
            GameObject unit = SelectedUnits[i];
            if(unit != null) unit.GetComponent<UnitController>().selection.gameObject.SetActive(false);
        }
        /*foreach (GameObject unitToRemove in SelectedUnits)
        {
            unitToRemove.GetComponent<UnitController>().selection.gameObject.SetActive(false);
        }*/
        SelectedUnits.Clear();
        HandleUI();
    }
    public void Deselect(GameObject unitToRemove)
    {
        Debug.Log("Selection - Remove Unit " + unitToRemove.name);
        if(unitToRemove.TryGetComponent(out UnitController unit))
        {
            unit.selection.gameObject.SetActive(false);
        }
        SelectedUnits.Remove(unitToRemove);
    }
}
