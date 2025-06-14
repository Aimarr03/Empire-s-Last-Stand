using System;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Game.Buildings
{
    public enum BuildingState
    {
        UnderConstruction,
        Constructed,
        Destructed
    }
    public class Building : MonoBehaviour
    {
        public string buildingName;
        public int maxHP;
        public int currentHP;

        [Header("Upgrade Settings")]
        public int costToBuild;
        public bool upgradable = true;
        public int currentLevel = 0;
        public int maxUpgradeLevel = 2;

        [Header("Sprite States")]
        public Sprite spriteConstruction;
        public Sprite spriteReady;
        public Sprite spriteDestroyed;

        protected SpriteRenderer spriteRenderer;
        protected Collider2D Collider;
        public BuildingState currentState;

        public event Action<Building> OnDestroyed;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            Collider = GetComponent<Collider2D>();
        }

        protected virtual void Start()
        {
            
        }

        public virtual void SetConstructionSprite()
        {
            if (spriteRenderer != null && spriteConstruction != null)
            {
                spriteRenderer.sprite = spriteConstruction;
            }
        }

        public virtual void SetReadySprite()
        {
            if (spriteRenderer != null && spriteReady != null)
            {
                spriteRenderer.sprite = spriteReady;
            }
        }

        public virtual void SetDestroyedSprite()
        {
            if (spriteRenderer != null && spriteDestroyed != null)
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

            if (currentHP <= 0)
            {
                Destroyed();
            }
        }

        protected virtual void Destroyed()
        {
            currentState = BuildingState.Destructed;
            SetDestroyedSprite();
            Debug.Log($"{gameObject.name} destroyed.");
            OnDestroyed?.Invoke(this);
        }
        public void Build()
        {
            Debug.Log($"Build Building {gameObject.name}");
            if (currentState != BuildingState.UnderConstruction)
            {
                Debug.Log("Building is Already Built!");
                return;
            }
            Debug.Log($"Build Building {gameObject.name}");
            currentState = BuildingState.Constructed;
            currentLevel++;
            SetReadySprite();
        }
        public virtual void Upgrade()
        {
            if (!upgradable) return;
            if (currentLevel < maxUpgradeLevel)
            {
                currentLevel++;
                Debug.Log($"{gameObject.name} upgraded to level {currentLevel}");
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
        public int GetCurrentHP() => currentHP;
        public int GetMaxHP() => maxHP;
    }
}