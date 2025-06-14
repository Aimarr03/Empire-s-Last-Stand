using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Buildings;

namespace Game.Buildings
{
    [System.Serializable]
    public class BarrackStats
    {
        public float hp;
        public int costToBuild;
        public int maxUnits;
        public float spawnInterval;
    }

    public class Barrack : Building
    {
        public string troopType;
        public GameObject troopPrefab;  // Assign prefab unit di Inspector
        public Transform spawnPoint;    // Titik spawn troop
        public float baseSpawnInterval = 10f;
        public Canvas _ui;
        private float currentSpawnInterval;

        [Header("Level-Based Stats")]
        public List<BarrackStats> levelStats = new List<BarrackStats>();

        private float currentSpawnInterval;
        private Coroutine spawnRoutine;

        private int troopSpawned = 0;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(ConstructionDelay());
        }

        private IEnumerator ConstructionDelay()
        {
            yield return new WaitForSeconds(5f);
            FinishConstruction();

            yield return new WaitForSeconds(3f);

            Upgrade();  // Otomatis ke level 1 setelah konstruksi
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

        void SpawnTroop()
        {
            if (troopSpawned >= GetCurrentStats().maxUnits) return;

            Vector3 spawnPos = spawnPoint.position;
            Instantiate(troopPrefab, spawnPos, Quaternion.identity);
            troopSpawned++;
            Debug.Log($"{gameObject.name} spawned troop #{troopSpawned}");
        }

        public override void Upgrade()
        {
            base.Upgrade();

            if (upgradeLevel - 1 < levelStats.Count)
            {
                var stats = levelStats[upgradeLevel - 1];

                // Terapkan stat dari level
                currentHP = (int)stats.hp;
                maxHP = (int)stats.hp;
                cost = stats.costToBuild;
                currentSpawnInterval = stats.spawnInterval;

                Debug.Log($"[Barrack] Level {upgradeLevel} stats applied: HP={currentHP}, MaxUnits={stats.maxUnits}, Cost={cost}, Interval={currentSpawnInterval}");

                if (troopSpawned < stats.maxUnits)
                    StartSpawning();
            }
            else
            {
                Debug.LogWarning($"[Barrack] No stat data found for level {upgradeLevel}");
            }
        }

        private BarrackStats GetCurrentStats()
        {
            int index = Mathf.Clamp(upgradeLevel - 1, 0, levelStats.Count - 1);
            return levelStats[index];
        }

        public override void FinishConstruction()
        {
            base.FinishConstruction();
            _ui.gameObject.SetActive(true);
        }
    }
}
