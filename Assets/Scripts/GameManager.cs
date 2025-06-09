using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentGold = 0;

    public void AddGold(int amount)
    {
        currentGold += amount;
        Debug.Log("Gold bertambah: " + amount + ", total: " + currentGold);
    }
}