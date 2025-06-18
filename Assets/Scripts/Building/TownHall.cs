using System.Collections;
using UnityEngine;
using Game.Buildings;

namespace Game.Buildings
{
    public class TownHall : Building
    {
        protected override void Destroyed()
        {
            base.Destroyed();
            GameplayManager.instance.LoseTheGame();
        }


        protected override void Start()
        {
            base.Start();
            upgradable = false;
            currentLevel = 1;
            currentState = BuildingState.Constructed;
            SetReadySprite();
            UI_Canva.gameObject.SetActive(true);
            currentHP = maxHP;
            //StartCoroutine(ConstructionDelay());
        }

        /*private IEnumerator ConstructionDelay()
        {
            yield return new WaitForSeconds(5f);  // Delay 5 detik
            FinishConstruction();                  // Ganti sprite ke ready (Castle_Blue)
        }

        protected override void Destroyed()
        {
            SetDestroyedSprite();                  // Ganti sprite ke destroyed (Castle_Destroyed)
            Debug.Log("TownHall destroyed! Triggering Game Over.");
            OnGameOver?.Invoke();
            // Logic game over (UI/scene) bisa ditangani listener event ini
        }*/
    }
}
