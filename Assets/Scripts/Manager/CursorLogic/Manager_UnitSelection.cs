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
    public Canvas selectedBuildingUI;
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
    public void SelectUnit(GameObject unitToAdd)
    {
        DeselectAll();
        Debug.Log("Selection - Select Unit " + unitToAdd.name);
        SelectedUnits.Add(unitToAdd);
    }
    public void SelectBuilding(GameObject building)
    {
        DeselectAll();
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
            Vector3 buildingPos = _selectedBuilding.transform.position;

            Vector3 ScreenBuildingPos = Camera.main.WorldToScreenPoint(buildingPos);
            float halfWidth = Screen.width / 2;
            float halfHeight = Screen.height / 2;

            Vector3 offsetPos = new Vector3(4, 2, 0);
            offsetPos.x = ScreenBuildingPos.x > halfWidth ? -4 : 4;
            offsetPos.y = ScreenBuildingPos.y > halfHeight? -2 : 2;
            selectedBuildingUI.transform.position = buildingPos + offsetPos;
        }
    }
    public void ShiftSelectUnit(GameObject unitToAdd)
    {
        bool notContainUnit = !SelectedUnits.Contains(unitToAdd);
        if (notContainUnit)
        {
            Debug.Log("Selection - Add New Unit " + unitToAdd.name);
            SelectedUnits.Add(unitToAdd);
        }
        else
        {
            Deselect(unitToAdd);
        }
    }
    public void DragSelect(GameObject unitToAdd)
    {
        bool notContainUnit = !SelectedUnits.Contains(unitToAdd);
        if (notContainUnit)
        {
            Debug.Log("Selection - Add New Unit " + unitToAdd.name);
            SelectedUnits.Add(unitToAdd);
        }
    }
    public void DeselectAll()
    {
        Debug.Log("Selection - Remove All Unit ");
        _selectedBuilding = null;
        SelectedUnits.Clear();
        HandleUI();
    }
    public void Deselect(GameObject unitToAdd)
    {
        Debug.Log("Selection - Remove Unit " + unitToAdd.name);
        SelectedUnits.Remove(unitToAdd);
    }
}
