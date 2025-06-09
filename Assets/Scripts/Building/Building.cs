using System;
using UnityEngine;
using System.Collections; // Diperlukan untuk Coroutine

namespace Game.Buildings
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Building : MonoBehaviour
    {
        [Header("Building Stats")]
        public int maxHP = 100;
        public int currentHP;

        public int cost = 100;
        public int upgradeLevel = 1;
        public int maxUpgradeLevel = 2;

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
            currentHP = maxHP;
            
            // Panggil metode untuk memulai visual konstruksi (akan di-override di subclass)
            StartConstructionVisual(); 

            // Panggil FinishConstruction setelah constructionTime detik
            Invoke("FinishConstruction", constructionTime); 
        }

        // --- Metode Visual ---

        // Metode ini akan dipanggil di Start(), dan di-override di subclass
        // untuk menampilkan animasi konstruksi spesifik.
        protected virtual void StartConstructionVisual()
        {
            currentVisualState = BuildingVisualState.Constructing;
            // Di sini, di base class, kita mungkin tidak punya sprite konstruksi default.
            // Biarkan subclass yang menampilkannya. Jika Anda ingin sprite placeholder,
            // tambahkan public Sprite defaultConstructionSprite; dan set di sini.
            Debug.Log($"{gameObject.name} memulai visual konstruksi.");
        }

        // Metode untuk mengatur state visual bangunan secara umum
        public virtual void SetBuildingVisualState(BuildingVisualState newState)
        {
            // Jangan ubah currentVisualState di sini, karena StartConstructionVisual
            // dan metode turunan lain mungkin sudah mengaturnya.
            // Cukup handle perubahan sprite/coroutine.

            // Hentikan coroutine animasi sebelumnya jika ada
            if (currentVisualCoroutine != null)
            {
                StopCoroutine(currentVisualCoroutine);
                currentVisualCoroutine = null;
            }

            switch (newState)
            {
                case BuildingVisualState.Constructing:
                    // Logika visual konstruksi sekarang ditangani sepenuhnya oleh StartConstructionVisual()
                    // atau override-nya di subclass.
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
                    currentVisualState = BuildingVisualState.Ready; // Update state di akhir
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
                    currentVisualState = BuildingVisualState.Destroyed; // Update state di akhir
                    break;
            }
        }

        // Metode FinishConstruction dipanggil setelah waktu konstruksi
        public virtual void FinishConstruction()
        {
            SetBuildingVisualState(BuildingVisualState.Ready); // Mengatur visual ke Ready
            Debug.Log($"{gameObject.name} construction finished.");
        }


        // Metode TakeDamage() sekarang memanggil Destroyed() saat HP <= 0
        public virtual void TakeDamage(int damage)
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
            SetBuildingVisualState(BuildingVisualState.Destroyed); // Mengatur visual ke Destroyed
            Debug.Log($"{gameObject.name} destroyed.");
            OnDestroyed?.Invoke(this);
            // Optional: Destroy(gameObject, 2f); // Hancurkan objek setelah 2 detik animasi destroy selesai
        }

        public virtual void Upgrade()
        {
            if (upgradeLevel < maxUpgradeLevel)
            {
                upgradeLevel++;
                Debug.Log($"{gameObject.name} upgraded to level {upgradeLevel}");
                // Jika Anda ingin sprite khusus untuk upgrade, panggil di sini
                // SetUpgradedSprite(); atau set visual state lain
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


        // Ini adalah metode generik untuk animasi frame-by-frame.
        // Dapat digunakan oleh subclass.
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
                        currentFrameIndex = 0; // Ulangi dari awal
                    }
                    else
                    {
                        yield break; // Selesai
                    }
                }
            }
        }
    }
}