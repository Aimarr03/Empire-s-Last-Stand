using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;


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
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
public enum GameState
{
    Morning,
    Night
}
