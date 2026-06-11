using UnityEngine;

public class MonsterEntity : CombatEntity
{
    public string MonsterName;

    public MonsterEntity(
        string monsterName,
        int maxHealth)
        : base(maxHealth)
    {
        MonsterName = monsterName;
    }
}
