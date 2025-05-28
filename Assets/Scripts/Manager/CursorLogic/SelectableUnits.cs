using UnityEngine;

public class SelectableUnits : MonoBehaviour
{
    private void Start()
    {
        Manager_UnitSelection.Instance.AllUnits.Add(gameObject);
    }
    private void OnDestroy()
    {
        Manager_UnitSelection.Instance.AllUnits.Remove(gameObject);
    }
}
