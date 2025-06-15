using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Buildings
{
    [Serializable]
    public struct GoldMineData
    {
        public int costToBuild;
        public int goldGain;
        public int maxHp;
    }
    public class GoldMine : Building
    {
        public GoldMineData[] data;
        public int currentGoldGain;
        protected override void Start()
        {
            base.Start();
            costToBuild = data[currentLevel].costToBuild;
            currentGoldGain = 0;
        }
        public override void Build()
        {
            base.Build();
            costToBuild = data[currentLevel].costToBuild;
            currentGoldGain = data[currentLevel].goldGain;
            maxHP = data[currentLevel].maxHp;
            currentHP = maxHP;
        }

        public override void Upgrade()
        {
            base.Upgrade();
            if(currentLevel <= maxUpgradeLevel)
            {
                costToBuild = data[currentLevel].costToBuild;
                currentGoldGain = data[currentLevel].goldGain;
                maxHP = data[currentLevel].maxHp;
                currentHP = maxHP;
            }
        }

        protected override void Instance_ChangeState()
        {
            base.Instance_ChangeState();
            if(GameplayManager.instance.gameState == GameState.Morning)
            {
                if (currentState == BuildingState.Constructed)
                {
                    GameplayManager.instance.GainMoney(currentGoldGain);
                }
            }
            
        }
    }
}

