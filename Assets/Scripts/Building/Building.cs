using System;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Game.Buildings
{
    public class Building : MonoBehaviour
    {
        public int maxHP = 100;
        public int currentHP;

        public int cost = 100;
        public int upgradeLevel = 1;
        public int maxUpgradeLevel = 2;

        public Sprite spriteConstruction;
        public Sprite spriteReady;
        public Sprite spriteDestroyed;

        protected SpriteRenderer spriteRenderer;

        public event Action<Building> OnDestroyed;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void Start()
        {
            currentHP = maxHP;
            SetConstructionSprite();
        }

        public virtual void SetConstructionSprite()
        {
            if(spriteRenderer != null && spriteConstruction != null)
            {
                spriteRenderer.sprite = spriteConstruction;
            }
        }

        public virtual void SetReadySprite()
        {
            if(spriteRenderer != null && spriteReady != null)
            {
                spriteRenderer.sprite = spriteReady;
            }
        }

        public virtual void SetDestroyedSprite()
        {
            if(spriteRenderer != null && spriteDestroyed != null)
            {
                spriteRenderer.sprite = spriteDestroyed;
            }
        }

        public virtual void FinishConstruction()
        {
            SetReadySprite();
            Debug.Log($"{gameObject.name} construction finished.");
        }

        public virtual void TakeDamage(int damage)
        {
            currentHP -= damage;
            Debug.Log($"{gameObject.name} took {damage} damage, HP left: {currentHP}");

            if(currentHP <= 0)
            {
                Destroyed();
            }
        }

        protected virtual void Destroyed()
        {
            SetDestroyedSprite();
            Debug.Log($"{gameObject.name} destroyed.");
            OnDestroyed?.Invoke(this);
            // Bisa destroy object setelah efek hancur jika mau
            // Destroy(gameObject);
        }

        public virtual void Upgrade()
        {
            if(upgradeLevel < maxUpgradeLevel)
            {
                upgradeLevel++;
                Debug.Log($"{gameObject.name} upgraded to level {upgradeLevel}");
            }
            else
            {
                Debug.Log($"{gameObject.name} is already max level.");
            }
        }

        public virtual void NextRound()
        {
            Debug.Log($"{gameObject.name} NextRound called.");
        }
    }
}
