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
    [System.Serializable]
    public class UnitStats
    {
        public float maxHP;
        public float attackRange;
        public float damage;
        public float attackCooldown;
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
        public List<UnitStats> unitStats = new List<UnitStats>();
        private Coroutine spawnRoutine;
        private List<UnitController> unitsDeployed = new List<UnitController>();

        private int troopSpawned = 0;
        private float currentInterval;

        [SerializeField] private Image backgroundProgressDeploy;
        [SerializeField] private Image ProgressDeploy;
        protected override void Start()
        {
            base.Start();
            UnitController.OnKilled += UnitController_OnKilled;
            SetConstructionSprite();
        }
        private void OnDestroy()
        {
            UnitController.OnKilled -= UnitController_OnKilled;
        }

        private void UnitController_OnKilled(UnitController unit)
        {
            if(unitsDeployed.Contains(unit)) unitsDeployed.Remove(unit);
            troopSpawned = unitsDeployed.Count;
            backgroundProgressDeploy.gameObject.SetActive(true);
        }

        protected override void Instance_ChangeState()
        {
            base.Instance_ChangeState();
            if (GameplayManager.instance.gameState == GameState.Morning)
            {
                backgroundProgressDeploy.gameObject.SetActive(false);
                Vector2 spawnPos = transform.position;
                spawnPos.x += 2.5f;
                spawnPos.y += 1.5f;
                
                float bufferPosX = spawnPos.x;
                
                for (int i = 0; i < unitsDeployed.Count; i++)
                {
                    UnitController unit = unitsDeployed[i];
                    unit.currentHP = unit.maxHP;

                    unit.CommandTroop(spawnPos);

                    spawnPos.x += 1;
                    if (i == 0)
                    {
                        spawnPos.x = bufferPosX;
                        spawnPos.y += 1;
                    }
                }
            }
            else
            {
                if (currentLevel > 0)
                {
                    if(troopSpawned < GetCurrentStats().maxUnits) backgroundProgressDeploy.gameObject.SetActive(true);
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
        void SetupTroops()
        {
            troopSpawned = 0;
            for(int i = 0; i < unitsDeployed.Count; i++)
            {
                UnitController troop = unitsDeployed[i];
                Destroy(troop.gameObject);
            }
            Debug.Log("Setup Troops for: " + GetCurrentStats().maxUnits);
            unitsDeployed.Clear();
            Vector2 spawnPos = transform.position;
            spawnPos.x += 2.5f;
            spawnPos.y += 1.5f;
            float bufferPosX = spawnPos.x;
            for(int i = 0; i < GetCurrentStats().maxUnits; i++)
            {
                UnitController newUnit = Instantiate(troopPrefab, spawnPos, Quaternion.identity);
                unitsDeployed.Add(newUnit);
                newUnit.SetupTroop(unitStats[currentLevel - 1], spawnPos);
                troopSpawned++;

                spawnPos.x += 1;
                if(troopSpawned % 4 == 0)
                {
                    spawnPos.x = bufferPosX;
                    spawnPos.y += 1;
                }
            }
        }
        void SpawnTroop()
        {
            if (troopSpawned >= GetCurrentStats().maxUnits)
            {
                return;
            }

            Vector3 spawnPos = spawnPoint.position;
            float x = spawnPos.x;
            x = UnityEngine.Random.Range(transform.position.x+1, x + 1.5f);
            float y = spawnPos.y;
            y = UnityEngine.Random.Range(transform.position.y - 1, y + 0.5f);

            spawnPos.x = x;
            spawnPos.y = y;
            
            UnitController unit = Instantiate(troopPrefab, spawnPos, Quaternion.identity);
            troopSpawned++;
            unit.SetupTroop(unitStats[currentLevel-1], spawnPos);
            unit.spriteRenderer.sortingOrder += troopSpawned;
            unitsDeployed.Add(unit);
            if(troopSpawned >= GetCurrentStats().maxUnits)
            {
                backgroundProgressDeploy.gameObject.SetActive(false);
                return;
            }
            Debug.Log($"{gameObject.name} spawned troop #{troopSpawned}");
        }
        private void Update()
        {
            if (currentHP == 0) return;
            if (GameplayManager.instance.gameState == GameState.Over) return;
            if (GameplayManager.instance.pause) return;
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
            SetupTroops();
        }
        public override void Build()
        {
            base.Build();
            UpdateDataBuilding();
            SetupTroops();
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