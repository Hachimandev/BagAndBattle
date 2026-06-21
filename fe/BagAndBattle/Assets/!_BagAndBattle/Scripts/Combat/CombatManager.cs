using UnityEngine;
using System.Collections.Generic;

public enum GameState { Preparation, Combat, Reward, GameOver }

public class CombatItemTracker
{
    public StoredObject storedObject;
    public ItemData itemData;
    public float currentTimer;

    public CombatItemTracker(StoredObject obj, ItemData data)
    {
        storedObject = obj;
        itemData = data;
        currentTimer = 0f;
    }
}

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    [Header("Entities")]
    public PlayerEntity player;
    public MonsterEntity monster;

    [Header("Settings")]
    public float gameSpeedMultiplier = 1f;

    public GameState CurrentState { get; private set; } = GameState.Preparation;

    private List<CombatItemTracker> activeItems = new List<CombatItemTracker>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartCombat()
    {
        if (CurrentState != GameState.Preparation) return;

        CurrentState = GameState.Combat;

        if (Inventory.Instance != null)
        {
            Inventory.Instance.IsLocked = true;
            InitializeCombatItems(Inventory.Instance.grid.stored);
        }

        Debug.Log("Combat Started! Inventory Locked.");
    }

    private void InitializeCombatItems(List<StoredObject> inventoryItems)
    {
        activeItems.Clear();
        foreach (var item in inventoryItems)
        {
            if (item.storable != null)
            {
                Item itemComponent = item.storable.GetComponentInParent<Item>();
                if (itemComponent != null && itemComponent.Data != null)
                {
                    activeItems.Add(new CombatItemTracker(item, itemComponent.Data));
                }
            }
        }
    }

    private void Update()
    {
        if (CurrentState != GameState.Combat) return;

        // Check Win/Loss conditions
        if (monster != null && monster.IsDead)
        {
            EndCombat(GameState.Reward);
            return;
        }

        if (player != null && player.IsDead)
        {
            EndCombat(GameState.GameOver);
            return;
        }

        // Execution loop for items
        float deltaTime = Time.deltaTime * gameSpeedMultiplier;

        foreach (var tracker in activeItems)
        {
            // Skip items with no combat trigger speed (or 0 to avoid div/0 or instant loop)
            if (tracker.itemData.triggerSpeed <= 0f) continue;

            tracker.currentTimer += deltaTime;

            if (tracker.currentTimer >= tracker.itemData.triggerSpeed)
            {
                tracker.currentTimer -= tracker.itemData.triggerSpeed;
                ProcessItemEffects(tracker.itemData, tracker.storedObject.storable.gameObject.name);
            }
        }
    }

    private void ProcessItemEffects(ItemData data, string itemName)
    {
        Debug.Log($"[CombatManager] Item '{itemName}' triggers! Damage: {data.damage}, Heal: {data.heal}, Shield: {data.shield}");

        if (monster != null && data.damage > 0)
        {
            monster.TakeDamage(data.damage);
        }

        if (player != null)
        {
            if (data.heal > 0)
                player.Heal(data.heal);
            
            if (data.shield > 0)
                player.AddShield(data.shield);
        }
    }

    public void SetGameSpeed(float multiplier)
    {
        gameSpeedMultiplier = multiplier;
        Debug.Log($"[CombatManager] Game speed set to {multiplier}x");
    }

    private void EndCombat(GameState resultState)
    {
        CurrentState = resultState;
        
        if (Inventory.Instance != null)
        {
            Inventory.Instance.IsLocked = false;
        }

        if (resultState == GameState.Reward)
        {
            Debug.Log("[CombatManager] Monster Defeated! Transitioning to Reward Phase...");
            // TODO: Trigger stage rewards phase
        }
        else if (resultState == GameState.GameOver)
        {
            Debug.Log("[CombatManager] Player Died! Permadeath triggered. Returning to Main Menu...");
            // TODO: Wipe run data, process metagame currency, return to menu
        }
    }
}
