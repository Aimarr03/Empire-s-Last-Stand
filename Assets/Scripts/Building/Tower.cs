using System;
using UnityEngine;

namespace Game.Buildings
{
    [Serializable]
    public struct TowerData
    {
        public int costToBuild;
        public int damage;
        public float range;
        public float attackSpeed;
        public int maxHp;
    }
    public class Tower : Building
    {
        public TowerData[] data;
        public EnemyController currentEnemyTarget;
        public LayerMask enemylayer;
        public float currentInterval = 0;
        public Arrow arrow;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            costToBuild = data[currentLevel].costToBuild;
        }
        public override void Build()
        {
            base.Build();
            maxHP = data[currentLevel].maxHp;
            costToBuild = data[currentLevel].costToBuild;
            currentHP = maxHP;
        }

        public override void Upgrade()
        {
            base.Upgrade();
            if (currentLevel >= maxUpgradeLevel) return;
            maxHP = data[currentLevel].maxHp;
            costToBuild = data[currentLevel].costToBuild;
            currentHP = maxHP;
        }

        protected override void Instance_ChangeState()
        {
            base.Instance_ChangeState();
        }

        void Update()
        {
            if (currentState != BuildingState.Constructed) return;
            if (currentHP == 0) return;
            if (GameplayManager.instance.gameState != GameState.Night || currentLevel == 0) return;

            if(currentEnemyTarget == null)
            {
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, data[currentLevel].range, Vector2.right);
                if(hit.collider.TryGetComponent(out EnemyController enemy))
                {
                    currentEnemyTarget = enemy;
                    currentInterval = 0;
                }
            }
            else
            {
                if(Vector2.Distance(transform.position, currentEnemyTarget.transform.position) > data[currentLevel].range)
                {
                    currentEnemyTarget = null;
                    return;
                }
                currentInterval += Time.deltaTime;
                if(currentInterval < data[currentLevel].attackSpeed)
                {
                    currentInterval = 0;
                    CreateArrow();
                }
            }
        }
        public void CreateArrow()
        {
            if (currentEnemyTarget == null) return;
            Vector3 enemyPos = currentEnemyTarget.transform.position;
            SpriteRenderer enemySprite = currentEnemyTarget.GetComponentInChildren<SpriteRenderer>();

            float halfHeight = (enemySprite.sprite.rect.height / enemySprite.sprite.pixelsPerUnit) * 0.5f;
            halfHeight *= currentEnemyTarget.transform.localScale.y;

            enemyPos.y += halfHeight;

            Vector3 aimDirection = (enemyPos - transform.position).normalized;


            Arrow arrow = Instantiate(this.arrow, transform.position, Quaternion.identity);
            arrow.direction = aimDirection;
            arrow.damage = data[currentLevel].damage;
        }

        
    }
}

