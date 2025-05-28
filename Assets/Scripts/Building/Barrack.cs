using System.Collections;
using UnityEngine;
using Game.Buildings;

namespace Game.Buildings
{
    public class Barrack : Building
    {
        public GameObject troopPrefab;  // Assign prefab unit di Inspector
        public Transform spawnPoint;    // Titik spawn troop
        public float baseSpawnInterval = 10f;
        private float currentSpawnInterval;

        public int baseTroopCount = 1;

        private Coroutine spawnRoutine;

        // Radius penyebaran spawn troop supaya tidak menumpuk
        public float spawnSpreadRadius = 1.5f;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(ConstructionDelay());
        }

        private IEnumerator ConstructionDelay()
        {
            yield return new WaitForSeconds(5f);  // Delay konstruksi
            FinishConstruction();
            currentSpawnInterval = baseSpawnInterval;

            // Delay 3 detik sebelum troop pertama muncul
            yield return new WaitForSeconds(3f);

            StartSpawning();
        }

        void StartSpawning()
        {
            if (spawnRoutine == null)
            {
                spawnRoutine = StartCoroutine(SpawnTroopsRoutine());
            }
        }

        IEnumerator SpawnTroopsRoutine()
        {
            while (true)
            {
                SpawnTroops();
                yield return new WaitForSeconds(currentSpawnInterval);
            }
        }

        void SpawnTroops()
        {
            int troopsToSpawn = baseTroopCount * upgradeLevel;
            for (int i = 0; i < troopsToSpawn; i++)
            {
                // Hitung posisi spawn dengan offset random agar troop tidak menumpuk
                Vector2 offset = Random.insideUnitCircle * spawnSpreadRadius;
                Vector3 spawnPos = spawnPoint.position + new Vector3(offset.x, offset.y, 0);

                Instantiate(troopPrefab, spawnPos, Quaternion.identity);
            }
            Debug.Log($"{gameObject.name} spawned {troopsToSpawn} troops.");
        }

        public override void Upgrade()
        {
            base.Upgrade();
            // Perpendek interval spawn tapi minimal 3 detik
            currentSpawnInterval = Mathf.Max(3f, baseSpawnInterval - (upgradeLevel - 1) * 3f);
            Debug.Log($"{gameObject.name} spawn interval now {currentSpawnInterval} seconds.");
        }
    }
}
