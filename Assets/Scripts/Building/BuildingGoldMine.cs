using UnityEngine;
using Game.Buildings;
using System.Collections; // Diperlukan untuk Coroutine

public class BuildingGoldMine : Building // Mewarisi dari Building
{
    public int goldPerRound = 50;

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

    // Override metode ini untuk menampilkan visual konstruksi Gold Mine
    protected override void StartConstructionVisual()
    {
        base.StartConstructionVisual(); // Panggil base method untuk set currentVisualState
        
        if (constructionFrames != null && constructionFrames.Length > 0)
        {
            // Mulai animasi frame-by-frame untuk konstruksi Gold Mine
            currentVisualCoroutine = StartCoroutine(AnimateSpriteFrames(constructionFrames, constructionFrameRate, false)); // Non-looping
        }
        else
        {
            Debug.LogWarning("No construction frames provided for GoldMine. Defaulting to ready sprite after delay.");
            // Fallback: Jika tidak ada frame konstruksi, set sprite default (misal, ready) tapi setelah delay.
            // Namun, ini hanya akan terlihat setelah constructionTime selesai jika sprite ready di-set.
            // Untuk memastikan ada visual selama konstruksi, Anda mungkin ingin sprite placeholder.
            // Misalnya: spriteRenderer.sprite = somePlaceholderSprite;
        }
    }

    public override void NextRound()
    {
        base.NextRound(); // Panggil NextRound dari Building base class

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
        base.Upgrade(); 
        goldPerRound += 30;
        maxHP += 50;
        currentHP = maxHP; 
        Debug.Log($"{gameObject.name} di-upgrade ke level {upgradeLevel}. Produksi: {goldPerRound} emas/ronde, Max HP: {maxHP}, Current HP: {currentHP}.");
    }
}