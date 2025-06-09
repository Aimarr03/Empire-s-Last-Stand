using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float currentGold = 0f; // Ubah ke float sesuai dengan goldPerRound di BuildingGoldMine

    public void AddGold(float amount) // Ubah parameter ke float
    {
        currentGold += amount;
        Debug.Log("Gold bertambah: " + amount + ", total: " + currentGold);
    }
}