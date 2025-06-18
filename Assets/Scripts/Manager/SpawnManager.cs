using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct LevelData
{
    public int torcherSpawn;
    public int throwerSpawn;
    public int bomberSpawn;
    public int moneyGain;
}
public class SpawnManager : MonoBehaviour
{
    public List<LevelData> levelDatas = new List<LevelData>();
    public Dictionary<string, int> enemySpawned;
    public Transform SpawnPoint;
    [Header("Enemy")]
    public Torcher torcher;
    public Thrower thrower;
    [SerializeField] private float baseInterval = 0.2f;
    private int totalSpawned;
    private void Awake()
    {
        enemySpawned = new Dictionary<string, int>();
        enemySpawned.Add("torcher", 0);
        enemySpawned.Add("thrower", 0);
        enemySpawned.Add("bomber", 0);
        totalSpawned = 0;
    }
    void Start()
    {
        GameplayManager.instance.ChangeState += Instance_ChangeState;
        EnemyController.EnemyDie += EnemyController_EnemyDie;
    }
    private void OnDestroy()
    {
        GameplayManager.instance.ChangeState -= Instance_ChangeState;
        EnemyController.EnemyDie -= EnemyController_EnemyDie;
    }
    private void Instance_ChangeState()
    {
        foreach (var key in enemySpawned.Keys.ToList())
        {
            enemySpawned[key] = 0;
        }
        totalSpawned = 0;
        if (GameplayManager.instance.gameState == GameState.Night)
        {
            StartCoroutine(StartSpawningEnemy());
        }
    }
    private void EnemyController_EnemyDie(string enemyType)
    {
        enemySpawned[enemyType]--;
        totalSpawned--;
        if(totalSpawned <= 0)
        {
            Debug.Log("We defend the city!");
            GameplayManager.instance.BattleEnded();
            GameplayManager.instance.GainMoney(levelDatas[GameplayManager.instance.currentNight].moneyGain);
        }
    }
    private IEnumerator StartSpawningEnemy()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Enemy Begin Spawning");
        LevelData currentLevelData = levelDatas[GameplayManager.instance.currentNight];
        Debug.Log($"Torcher: {currentLevelData.torcherSpawn} -- Thrower: {currentLevelData.throwerSpawn} -- Bomber: {currentLevelData.bomberSpawn}");
        Vector3 startPos = SpawnPoint.transform.position;

        Vector3 effectPos = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), 0);
        for (int i = 0; i < currentLevelData.torcherSpawn; i++)
        {
            effectPos = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), 0);
            Torcher newTorcher = Instantiate(torcher, startPos + effectPos, Quaternion.identity);
            enemySpawned["torcher"]++;
            totalSpawned++;
            yield return new WaitForSeconds(baseInterval);
        }
        for (int i = 0; i < currentLevelData.throwerSpawn; i++)
        {
            effectPos = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), 0);
            Thrower newTorcher = Instantiate(thrower, startPos + effectPos, Quaternion.identity);
            enemySpawned["thrower"]++;
            totalSpawned++;
            yield return new WaitForSeconds(baseInterval);
        }
    }
    void Update()
    {
        
    }
}
