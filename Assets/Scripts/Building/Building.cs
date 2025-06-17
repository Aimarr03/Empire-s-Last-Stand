using DG.Tweening;
using System;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

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

        [Header("UI")]
        [SerializeField] protected Canvas UI_Canva;
        [SerializeField] protected Image backgroundHpBar;
        [SerializeField] protected Image hpBar;
        [SerializeField] protected RectTransform ui_detail;
        [SerializeField] protected Light2D Light;
        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            Collider = GetComponent<Collider2D>();
            Light.intensity = 0;
            UI_Canva.gameObject.SetActive(false);
        }

        protected virtual void Start()
        {
            GameplayManager.instance.ChangeState += Instance_ChangeState;
            UI_Canva.gameObject.SetActive(false);
            backgroundHpBar.gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            GameplayManager.instance.ChangeState -= Instance_ChangeState;
        }

        protected virtual void Instance_ChangeState()
        {
            Collider.enabled = currentLevel > 0;
            if (GameplayManager.instance.gameState == GameState.Morning)
            {
                if (currentLevel > 0)
                {
                    DOTween.To(() => Light.intensity,
                       x => Light.intensity = x,
                       0f, 1.5f);
                    backgroundHpBar.gameObject.SetActive(false);
                }
                else
                {
                    Collider.enabled = true;
                }
                
            }
            else
            {
                if (currentLevel > 0)
                {
                    UI_Canva.gameObject.SetActive(true);
                    DOTween.To(() => Light.intensity,
                       x => Light.intensity = x,
                       1f, 1.5f);
                }
                else
                {
                    Collider.enabled = false;
                }
                
            }
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
                UI_Canva.gameObject.SetActive(true);
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
            Debug.Log($"{gameObject.name} took {damage} damage, HP left: {currentHP} | percentage: {((float)currentHP)/maxHP}");
            backgroundHpBar.gameObject.SetActive(true);
            hpBar.fillAmount = ((float)currentHP) / maxHP;
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
            UI_Canva.gameObject.SetActive(false);
            OnDestroyed?.Invoke(this);
        }
        public virtual void Build()
        {
            Debug.Log($"Build Building {gameObject.name}");
            if (currentState != BuildingState.UnderConstruction)
            {
                Debug.Log("Building is Already Built!");
                return;
            }
            Debug.Log($"Build Building {gameObject.name}");
            GameplayManager.instance.UseMoney(costToBuild);
            currentState = BuildingState.Constructed;
            currentLevel++;
            SetReadySprite();
        }
        public virtual void Upgrade()
        {
            if (!upgradable) return;
            if (currentLevel < maxUpgradeLevel)
            {
                GameplayManager.instance.UseMoney(costToBuild);
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