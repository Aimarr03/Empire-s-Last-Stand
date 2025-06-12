using UnityEngine;

public class SelectableUnits : MonoBehaviour
{
    private void Start()
    {
        if (Manager_UnitSelection.Instance == null) return;
        Manager_UnitSelection.Instance.AllUnits.Add(gameObject);
    }
    private void OnDestroy()
    {
        if (Manager_UnitSelection.Instance == null) return;
        Manager_UnitSelection.Instance.AllUnits.Remove(gameObject);
    }
}
