using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    [SerializeField] private UI_Gameplay ui_gameplay;
    [SerializeField] private VisualManager visualManager;

    public InputActionReference actionCommand;
    public InputActionReference pauseAction;
    public InputActionReference cameraMovement;
    
    
    public Transform objectToFollow;
    public Collider2D boundaryCamera;
    
    public float cameraMovSpeed;
    public float edgeLimit;

    public bool pause { private set; get; }
    public GameState gameState { private set; get; }
    public int currentNight { private set; get; }

    private Coroutine EngageToBattle;
    private float preparation_curentDuration;
    private float preparation_maxDuration;

    private int currentMoney;

    public event Action ChangeState;
    public string resultStatus {private set; get; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GainMoney(10);
        }
        else
        {
            Destroy(gameObject);
        }
        Bounds boundary = boundaryCamera.bounds;
        Debug.Log(boundary.min);
        Debug.Log(boundary.max);
    }
    private void OnEnable()
    {
        pauseAction.action.performed += ActionPause_performed;
        actionCommand.action.performed += Action_performed;
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        BattleStart();
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= ActionPause_performed;
    }
    void Update()
    {
        //Debug.Log(cameraMovement.action.ReadValue<Vector2>());
        if (pause) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        HandleCameraMovement();
    }
    private void HandleCameraMovement()
    {
        Vector2 movDir = cameraMovement.action.ReadValue<Vector2>();
        //Debug.Log("Camera Pos: " + Input.mousePosition);
        Vector2 mousePos = Input.mousePosition;
        
        if (mousePos.x < edgeLimit) movDir.x = -1;
        else if (mousePos.x > Screen.width - edgeLimit) movDir.x = 1;
        if (mousePos.y < edgeLimit) movDir.y = -1;
        else if (mousePos.y > Screen.height - edgeLimit) movDir.y = 1;
        
        Vector3 cameraPos = objectToFollow.transform.position;
        cameraPos += cameraMovSpeed * Time.deltaTime * (Vector3)movDir;
        
        Bounds boundary = boundaryCamera.bounds;
        float halfCameraHeightSize = Camera.main.orthographicSize;
        float halfCameraWidthSize = halfCameraHeightSize * Camera.main.aspect;

        cameraPos.x = Mathf.Clamp(cameraPos.x, boundary.min.x + halfCameraWidthSize, boundary.max.x - halfCameraWidthSize);
        cameraPos.y = Mathf.Clamp(cameraPos.y, boundary.min.y + halfCameraHeightSize, boundary.max.y - halfCameraHeightSize);

        objectToFollow.position = cameraPos;
    }


    private void ActionPause_performed(InputAction.CallbackContext obj)
    {
        pause = !pause;
        Debug.Log("Action - Check Paused = " + pause);
    }
    public void BattleStart()
    {
        if(gameState == GameState.Night)
        {
            Debug.Log("Already Night! Defend the Town Hall");
            return;
        }
        Debug.Log("Action - Enemy Incoming - Wave: " + currentNight);
        gameState = GameState.Night;
        visualManager.ChangeToNight();
        ChangeState?.Invoke();
        //TestChange();
    }
    private async void DelayFight()
    {
        await Task.Delay(1500);
        gameState = GameState.Night;
        visualManager.ChangeToNight();
        ChangeState?.Invoke();
    }
    public async void TestChange()
    {
        await Task.Delay(2500);
        BattleEnded();
    }
    public void BattleEnded()
    {
        gameState = GameState.Morning;
        currentNight++;
        visualManager.ChangeToDay();
        
    }
    public void UpdateBattleEnded()
    {
        ChangeState?.Invoke();
    }
    public void LoseTheGame()
    {
        pause = true;
        gameState = GameState.Over;
        resultStatus = "lose";
        visualManager.DisplayDefeat();
    }

    public bool CheckMoney(int cost) => currentMoney >= cost;

    public void UseMoney(int cost)
    {
        if (CheckMoney(cost))
        {
            currentMoney -= cost;
            ui_gameplay.UpdateGoldText(currentMoney);
            Debug.Log($"Economy - Money Spent: {cost} - Remaining: {currentMoney}");
        }
        else
        {
            Debug.Log($"Economy - Money not sufficient");
        }
    }
    public void GainMoney(int reward)
    {
        currentMoney += reward;
        Debug.Log($"Economy - Money Gain: {reward} - Total Current Money: {currentMoney}");
        ui_gameplay.UpdateGoldText(currentMoney);
    }
}
public enum GameState
{
    Morning,
    Night,
    Over
}
