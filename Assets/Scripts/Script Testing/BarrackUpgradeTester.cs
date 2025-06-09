using UnityEngine;
using Game.Buildings;  // Pastikan namespace ini sesuai dengan tempat Barrack kamu

public class BarrackUpgradeTester : MonoBehaviour
{
    private Barrack selectedBarrack;  // Barrack yang dipilih untuk upgrade

    void Update()
    {
        // Menggunakan raycast untuk mendeteksi klik pada Barrack
        if (Input.GetMouseButtonDown(0))  // Klik kiri mouse
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<Barrack>() != null)
            {
                selectedBarrack = hit.collider.GetComponent<Barrack>(); // Pilih Barrack yang diklik
                Debug.Log("Barrack terpilih: " + selectedBarrack.name);
            }
        }

        // Menekan tombol 'U' untuk upgrade Barrack yang dipilih
        if (Input.GetKeyDown(KeyCode.U) && selectedBarrack != null)
        {
            UpgradeBarrack();  // Hanya Barrack yang dipilih yang akan di-upgrade
            Debug.Log($"Barrack {selectedBarrack.name} upgraded to level {selectedBarrack.upgradeLevel}");
        }
        else if (selectedBarrack == null)
        {
            Debug.LogWarning("[TEST] Tidak ada Barrack yang dipilih.");
        }
    }

    // Fungsi ini akan dipanggil oleh UI button
    public void UpgradeBarrack()
    {
        if (selectedBarrack != null)
        {
            selectedBarrack.Upgrade();
            Debug.Log($"[UI] Barrack {selectedBarrack.name} upgraded to level {selectedBarrack.upgradeLevel}");
        }
        else
        {
            Debug.LogWarning("[UI] Tidak ada Barrack yang dipilih.");
        }
    }
}
