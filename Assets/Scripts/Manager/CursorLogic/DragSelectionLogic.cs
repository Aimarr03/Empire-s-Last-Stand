using UnityEngine;

public class DragSelectionLogic : MonoBehaviour
{
    Camera cam;

    [SerializeField] private RectTransform visualSelection;
    Vector2 startPos, endPos;
    Rect selectionBox;
    private void Awake()
    {
        cam = Camera.main;
    }
    private void Start()
    {
        startPos = Vector2.zero; 
        endPos = Vector2.zero;
        selectionBox = new Rect();
        DrawVisual();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }
        if (Input.GetMouseButtonUp(0))
        {
            SelectUnit();
            startPos = Vector2.zero;
            endPos = Vector2.zero;
            
            DrawVisual();
        }
    }
    void DrawVisual()
    {
        Vector2 boxStart = startPos;
        Vector2 boxEnd = endPos;

        Vector2 centerPos = (boxStart + boxEnd) / 2;
        
        visualSelection.position = centerPos;
        visualSelection.sizeDelta = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
    }
    void DrawSelection()
    {
        Vector2 mousePos = Input.mousePosition;
        //Dragging from left to right
        if(mousePos.x > startPos.x)
        {
            selectionBox.xMin = startPos.x;
            selectionBox.xMax = mousePos.x;
        }
        //Dragging from right to left
        if(mousePos.x < startPos.x)
        {
            selectionBox.xMin = mousePos.x;
            selectionBox.xMax = startPos.x;
        }
        //Dragging from bottom to up
        if(mousePos.y > startPos.y)
        {
            selectionBox.yMin = startPos.y;
            selectionBox.yMax = mousePos.y;
        }
        //Dragging from up to bottom
        if(mousePos.y < startPos.y)
        {
            selectionBox.yMin = mousePos.y;
            selectionBox.yMax = startPos.y;
        }
    }
    void SelectUnit()
    {
        //Loop through Units to check if unit is within selection of drag selection
        foreach(var unit in Manager_UnitSelection.Instance.AllUnits)
        {
            if (selectionBox.Contains(cam.WorldToScreenPoint(unit.transform.position)))
            {
                Manager_UnitSelection.Instance.DragSelect(unit);
            }
        }
    }
}
