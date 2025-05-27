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

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
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
        SelectedUnits.Clear();
    }
    public void Deselect(GameObject unitToAdd)
    {
        Debug.Log("Selection - Remove Unit " + unitToAdd.name);
        SelectedUnits.Remove(unitToAdd);
    }
}
