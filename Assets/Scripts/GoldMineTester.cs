using UnityEngine;

public class GoldMineTester : MonoBehaviour // <--- Pastikan ada ": MonoBehaviour" di sini!
{
    [Header("Referensi Bangunan")]
    public BuildingGoldMine targetGoldMine;

    [Header("Pengaturan Test")]
    public KeyCode nextRoundKey = KeyCode.N;
    public KeyCode upgradeKey = KeyCode.U;
    public KeyCode damageKey = KeyCode.D;
    public int testDamage = 20;

    void Update()
    {
        if (targetGoldMine == null)
        {
            Debug.LogError("Target Gold Mine belum diset di GoldMineTester!");
            return;
        }

        if (Input.GetKeyDown(nextRoundKey))
        {
            Debug.Log("--- Memicu NextRound ---");
            targetGoldMine.NextRound();
        }

        if (Input.GetKeyDown(upgradeKey))
        {
            Debug.Log("--- Memicu Upgrade ---");
            targetGoldMine.Upgrade();
        }

        if (Input.GetKeyDown(damageKey))
        {
            Debug.Log($"--- Memicu TakeDamage ({testDamage}) ---");
            targetGoldMine.TakeDamage(testDamage);
        }
    }
}