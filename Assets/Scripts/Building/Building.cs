using System;
using UnityEngine;
using System.Collections; // Diperlukan untuk Coroutine

namespace Game.Buildings
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Building : MonoBehaviour
    {
        [Header("Building Stats")]
        public float maxHP = 100f; // Menggunakan float sesuai rubrik
        public float currentHP;    // Menggunakan float

        public int cost = 100; // Cost To Build
        public int upgradeLevel = 1;
        public int maxUpgradeLevel = 2; // Default, bisa di-override di subclass jika ada lebih dari 2 level

        [Header("Visual Sprites (Base Class)")]
        public Sprite spriteReady;     // Sprite default ketika bangunan siap
        public Sprite spriteDestroyed; // Sprite ketika bangunan hancur

        public float constructionTime = 5f; // Waktu dalam detik untuk konstruksi

        protected SpriteRenderer spriteRenderer;
        protected Coroutine currentVisualCoroutine; // Untuk mengelola coroutine animasi visual

        public event Action<Building> OnDestroyed;

        public enum BuildingVisualState { Constructing, Ready, Destroyed }
        protected BuildingVisualState currentVisualState;


        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void Start()
        {
            currentHP = maxHP; // Inisialisasi HP saat Start
            
            StartConstructionVisual(); 

            Invoke("FinishConstruction", constructionTime); 
        }

        // --- Metode Visual ---

        protected virtual void StartConstructionVisual()
        {
            currentVisualState = BuildingVisualState.Constructing;
            Debug.Log($"{gameObject.name} memulai visual konstruksi.");
        }

        public virtual void SetBuildingVisualState(BuildingVisualState newState)
        {
            if (currentVisualState == newState && newState != BuildingVisualState.Constructing) return; // Perbaikan: hindari ganti state yang sama kecuali constructing

            // Hentikan coroutine animasi sebelumnya jika ada
            if (currentVisualCoroutine != null)
            {
                StopCoroutine(currentVisualCoroutine);
                currentVisualCoroutine = null;
            }

            switch (newState)
            {
                case BuildingVisualState.Constructing:
                    // Logika visual konstruksi ditangani oleh StartConstructionVisual() di subclass
                    break;

                case BuildingVisualState.Ready:
                    if (spriteRenderer != null && spriteReady != null)
                    {
                        spriteRenderer.sprite = spriteReady;
                    }
                    else
                    {
                        Debug.LogWarning($"Ready sprite not assigned for {gameObject.name} or SpriteRenderer missing!");
                    }
                    Debug.Log($"{gameObject.name} visual state: Ready.");
                    break;

                case BuildingVisualState.Destroyed:
                    if (spriteRenderer != null && spriteDestroyed != null)
                    {
                        spriteRenderer.sprite = spriteDestroyed;
                    }
                    else
                    {
                        Debug.LogWarning($"Destroyed sprite not assigned for {gameObject.name} or SpriteRenderer missing!");
                    }
                    Debug.Log($"{gameObject.name} visual state: Destroyed.");
                    break;
            }
            currentVisualState = newState; // Pindahkan update state ke sini setelah semua logika visual
        }

        public virtual void FinishConstruction()
        {
            SetBuildingVisualState(BuildingVisualState.Ready); 
            Debug.Log($"{gameObject.name} construction finished.");
        }

        public virtual void TakeDamage(float damage) // Ubah parameter damage menjadi float
        {
            currentHP -= damage;
            Debug.Log($"{gameObject.name} took {damage} damage, HP left: {currentHP}");

            if (currentHP <= 0)
            {
                Destroyed();
            }
        }

        protected virtual void Destroyed()
        {
            SetBuildingVisualState(BuildingVisualState.Destroyed); 
            Debug.Log($"{gameObject.name} destroyed.");
            OnDestroyed?.Invoke(this);
            // Optional: Destroy(gameObject, 2f); 
        }

        public virtual void Upgrade()
        {
            if (upgradeLevel < maxUpgradeLevel)
            {
                upgradeLevel++;
                Debug.Log($"{gameObject.name} upgraded to level {upgradeLevel}");
            }
            else
            {
                Debug.Log($"{gameObject.name} is already at max level.");
            }
        }

        public virtual void NextRound()
        {
            Debug.Log($"{gameObject.name} NextRound called.");
        }

        protected IEnumerator AnimateSpriteFrames(Sprite[] frames, float frameRate, bool loop)
        {
            if (frames == null || frames.Length == 0 || spriteRenderer == null) yield break;

            int currentFrameIndex = 0;
            while (true)
            {
                spriteRenderer.sprite = frames[currentFrameIndex];
                yield return new WaitForSeconds(frameRate);

                currentFrameIndex++;
                if (currentFrameIndex >= frames.Length)
                {
                    if (loop)
                    {
                        currentFrameIndex = 0; 
                    }
                    else
                    {
                        yield break; 
                    }
                }
            }
        }
    }
}