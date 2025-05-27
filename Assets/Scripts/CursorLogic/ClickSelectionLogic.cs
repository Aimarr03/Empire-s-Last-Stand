using UnityEngine;

public class ClickSelectionLogic : MonoBehaviour
{
    public LayerMask clickableLayer;
    public LayerMask ground;

    public Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, clickableLayer);
            if (hit.collider != null)
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.CompareTag("Unit"))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        Manager_UnitSelection.Instance.ShiftSelectUnit(hitObject);
                    }
                    else
                    {
                        Manager_UnitSelection.Instance.SelectUnit(hitObject);
                    }
                }
                
            }
            else
            {
                Manager_UnitSelection.Instance.DeselectAll();
            }
        }
    }
}
