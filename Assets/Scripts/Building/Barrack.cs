using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Buildings;
using UnityEngine.UI;

namespace Game.Buildings
{
    [System.Serializable]
    public class BarrackStats
    {
        public int maxHP;
        public int costToBuild;
        public int maxUnits;
        public float spawnInterval;
    }

    public class Barrack : Building
    {
        public string troopType;
        public UnitController troopPrefab;  // Assign prefab unit di Inspector
        public Transform spawnPoint;    // Titik spawn troop
        public float baseSpawnInterval = 10f;
        private float currentSpawnInterval;

        [Header("Level-Based Stats")]
        public List<BarrackStats> levelStats = new List<BarrackStats>();
        private Coroutine spawnRoutine;

        private int troopSpawned = 0;
        private float currentInterval;

        [SerializeField] private Image backgroundProgressDeploy;
        [SerializeField] private Image ProgressDeploy;
        protected override void Start()
        {
            base.Start();
            SetConstructionSprite();   
        }

        protected override void Instance_ChangeState()
        {
            base.Instance_ChangeState();
            if (GameplayManager.instance.gameState == GameState.Morning)
            {
                backgroundProgressDeploy.gameObject.SetActive(false);
            }
            else
            {
                if (currentLevel > 0)
                {
                    backgroundProgressDeploy.gameObject.SetActive(true);
                }
                else
                {

                }
            }
        }

        public override void SetConstructionSprite()
        {
            base.SetConstructionSprite();
            upgradable = true;
            currentLevel = 0;
            currentState = BuildingState.UnderConstruction;
            costToBuild = GetCurrentStats().costToBuild;
            backgroundProgressDeploy.gameObject.SetActive(false);
        }
        void StartSpawning()
        {
            if (spawnRoutine == null && troopSpawned < GetCurrentStats().maxUnits)
            {
                spawnRoutine = StartCoroutine(SpawnTroopsRoutine());
            }
        }

        IEnumerator SpawnTroopsRoutine()
        {
            while (troopSpawned < GetCurrentStats().maxUnits)
            {
                SpawnTroop();
                yield return new WaitForSeconds(currentSpawnInterval);
            }

            Debug.Log($"[{gameObject.name}] Done spawning. Total troop: {troopSpawned}/{GetCurrentStats().maxUnits}");
            spawnRoutine = null;
        }
        public override void SetReadySprite()
        {
            base.SetReadySprite();
        }

        void SpawnTroop()
        {
            if (troopSpawned >= GetCurrentStats().maxUnits)
            {
                return;
            }

            Vector3 spawnPos = spawnPoint.position;
            Instantiate(troopPrefab, spawnPos, Quaternion.identity);
            troopSpawned++;
            Debug.Log($"{gameObject.name} spawned troop #{troopSpawned}");
        }
        private void Update()
        {
            if (currentHP == 0) return;
            if (GameplayManager.instance.gameState != GameState.Night || currentLevel == 0) return;

            if(troopSpawned >= GetCurrentStats().maxUnits) return;
            backgroundProgressDeploy.gameObject.SetActive(true);
            currentInterval += Time.deltaTime;
            ProgressDeploy.fillAmount = currentInterval / currentSpawnInterval;
            if(currentInterval >= currentSpawnInterval)
            {
                currentInterval = 0;
                ProgressDeploy.fillAmount = 1;
                SpawnTroop();
            }

        }
        public override void Upgrade()
        {
            base.Upgrade();
            UpdateDataBuilding();
        }
        public override void Build()
        {
            base.Build();
            UpdateDataBuilding();
        }
        private void UpdateDataBuilding()
        {
            if (currentLevel - 1 < levelStats.Count)
            {
                var stats = levelStats[currentLevel - 1];

                // Terapkan stat dari level
                maxHP = stats.maxHP;
                currentHP = maxHP;
                currentSpawnInterval = stats.spawnInterval;

                Debug.Log($"[Barrack] Level {currentLevel} stats applied: HP={currentHP}, MaxUnits={stats.maxUnits}, Cost={costToBuild}, Interval={currentSpawnInterval}");
            }
            else
            {
                Debug.LogWarning($"[Barrack] No stat data found for level {currentLevel}");
            }
        }

        private BarrackStats GetCurrentStats()
        {
            int index = Mathf.Clamp(currentLevel - 1, 0, levelStats.Count - 1);
            return levelStats[index];
        }

        public override void FinishConstruction()
        {
            base.FinishConstruction();
        }
    }
}