using UnityEngine;
using Game.Buildings;
using System.Collections;

public class BuildingGoldMine : Building // Mewarisi dari Building
{
    // Properti Gold Mine dari Rubrik
    // HP sudah ada di base class (Building.maxHP)
    public float goldPerRound = 2f; // Diambil dari rubrik: Gold Per Round (float) = 2

    [Header("Gold Mine Specific Sprites")]
    public Sprite[] constructionFrames; 
    public float constructionFrameRate = 0.1f; 

    public Sprite[] miningFrames;
    public float miningFrameRate = 0.1f;
    private Coroutine miningAnimationCoroutine;

    private GameManager gameManager;

    protected override void Awake()
    {
        base.Awake(); // Panggil Awake dari base class untuk mendapatkan spriteRenderer
        gameManager = FindObjectOfType<GameManager>(); 
        if (gameManager == null)
        {
            Debug.LogError("GameManager tidak ditemukan di scene! Pastikan ada GameObject dengan script GameManager.");
        }
    }

    protected override void Start()
    {
        base.Start(); // Panggil Start dari base class untuk inisialisasi HP, memulai visual konstruksi, dan Invoke FinishConstruction
        maxHP = 45f; // Diambil dari rubrik: HP (float) = 45
        currentHP = maxHP; // Pastikan HP diatur sesuai maxHP yang baru
        cost = 3; // Diambil dari rubrik: Cost To Build (int) = 3
    }

    protected override void StartConstructionVisual()
    {
        base.StartConstructionVisual(); 
        
        if (constructionFrames != null && constructionFrames.Length > 0)
        {
            currentVisualCoroutine = StartCoroutine(AnimateSpriteFrames(constructionFrames, constructionFrameRate, false)); 
        }
        else
        {
            Debug.LogWarning("No construction frames provided for GoldMine. Defaulting to ready sprite after delay.");
        }
    }

    public override void NextRound()
    {
        base.NextRound(); 

        if (currentHP > 0)
        {
            if (gameManager != null)
            {
                gameManager.AddGold(goldPerRound);
                Debug.Log($"{gameObject.name} memberikan {goldPerRound} emas. Total Emas Pemain: {gameManager.currentGold}");

                if (currentVisualState == BuildingVisualState.Ready && miningFrames != null && miningFrames.Length > 0)
                {
                    if (miningAnimationCoroutine != null)
                    {
                        StopCoroutine(miningAnimationCoroutine);
                    }
                    miningAnimationCoroutine = StartCoroutine(PlayMiningAnimationOnce());
                }
            }
            else
            {
                Debug.LogWarning("GameManager belum ditemukan!");
            }
        }
    }

    private IEnumerator PlayMiningAnimationOnce()
    {
        yield return StartCoroutine(AnimateSpriteFrames(miningFrames, miningFrameRate, false)); 
        if (currentVisualState == BuildingVisualState.Ready) 
        {
            spriteRenderer.sprite = spriteReady; 
        }
    }

    public override void Upgrade()
    {
        // Gold Mine tidak memiliki level upgrade di rubrik Anda, jadi kita bisa menjaga agar tidak bisa di-upgrade
        // atau menambahkan logika upgrade jika Anda berencana menambahkannya nanti.
        // Untuk saat ini, kita bisa membiarkan base.Upgrade() terpanggil, tapi tidak ada perubahan stat.
        // Jika Anda ingin Gold Mine TIDAK BISA di-upgrade sama sekali, Anda bisa menghapus base.Upgrade()
        // dan hanya menampilkan pesan "sudah level maksimal" atau sejenisnya.

        // Jika Anda mengaktifkan upgrade level 2 di rubrik (di masa depan), bisa seperti ini:
        if (upgradeLevel < maxUpgradeLevel)
        {
            base.Upgrade();
            goldPerRound += 1f; // Contoh peningkatan kecil jika upgrade
            maxHP += 10f; // Contoh peningkatan HP
            currentHP = maxHP; // Reset HP setelah upgrade
            Debug.Log($"{gameObject.name} di-upgrade ke level {upgradeLevel}. Produksi: {goldPerRound} emas/ronde, Max HP: {maxHP}, Current HP: {currentHP}.");
        }
        else
        {
            Debug.Log($"{gameObject.name} sudah level maksimal.");
        }
    }
}