using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public InputActionReference actionCommand;
    public InputActionReference pauseAction;

    public bool pause { private set; get; }
    public GameState gameState { private set; get; }
    public int currentNight { private set; get; }

    private Coroutine EngageToBattle;
    private float preparation_curentDuration;
    private float preparation_maxDuration;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
    }

}
public enum GameState
{
    Morning,
    Night
}
