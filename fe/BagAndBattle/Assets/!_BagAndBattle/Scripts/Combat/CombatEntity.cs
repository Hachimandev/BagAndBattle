using UnityEngine;

using System.Collections.Generic;

public abstract class CombatEntity
{
    public int MaxHealth { get; protected set; }

    public int CurrentHealth { get; protected set; }

    public int Shield { get; protected set; }

    public List<StatusEffect> StatusEffects { get; private set; }

    public bool IsDead => CurrentHealth <= 0;

    protected CombatEntity(int maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;

        Shield = 0;

        StatusEffects = new List<StatusEffect>();
    }

    public virtual void TakeDamage(int damage)
    {
        int remainingDamage = damage;

        if (Shield > 0)
        {
            int absorbed = Mathf.Min(Shield, damage);

            Shield -= absorbed;

            remainingDamage -= absorbed;
        }

        CurrentHealth -= remainingDamage;

        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
    }

    public virtual void Heal(int amount)
    {
        CurrentHealth += amount;

        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    public virtual void AddShield(int amount)
    {
        Shield += amount;
    }
}