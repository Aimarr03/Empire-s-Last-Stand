using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ClickSelectionLogic : MonoBehaviour
{
    public LayerMask clickableLayer;
    public LayerMask ground;
    public LayerMask uiLayer;

    //public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
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
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("On a UI");
                return;
            }
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, clickableLayer);
            //Debug.Log("Collider " + hit.collider);
            if (hit.collider != null)
            {
                GameObject hitObject = hit.collider.gameObject;
                HandleTypeOfSelectableUnit(hitObject);
            }
            else
            {
                Manager_UnitSelection.Instance.DeselectAll();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("On a UI");
                return;
            }
            Manager_UnitSelection.Instance.CommandTroops(worldPoint);
        }
    }
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == uiLayer)
                return true;
        }
        return false;
    }
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    void HandleTypeOfSelectableUnit(GameObject hitObject)
    {
        if (hitObject.CompareTag("Unit"))
        {
            HandleTroopsType(hitObject);
        }
        else if (hitObject.CompareTag("Building"))
        {
            Manager_UnitSelection.Instance.SelectBuilding(hitObject);
        }
    }
    void HandleTroopsType(GameObject hitObject)
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
