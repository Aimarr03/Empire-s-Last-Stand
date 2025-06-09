using UnityEngine;
using Game.Buildings;

public class AutoDamageOnStart : MonoBehaviour
{
    public Building targetBuilding;  // Assign Barrack/TownHall dari Inspector
    public int damageToDeal = 999;   // Damage besar agar langsung hancur

    void Start()
    {
        if (targetBuilding != null)
        {
            targetBuilding.TakeDamage(damageToDeal);
            Debug.Log($"[AutoDamage] {targetBuilding.name} received {damageToDeal} damage at start.");
        }
        else
        {
            Debug.LogWarning("[AutoDamage] No target building assigned.");
        }
    }
}
