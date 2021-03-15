public class Unit
{
    public string Name { get; }
    public UnitType UnitType { get; }
    public int Damage { get; }
    public int HP { get; }
    public int XP { get; }
    public float MoveSpeed { get; }
    public float AttackSpeed { get; }

    public Unit(string unitName, UnitType unitType, int damage, int hp, int xp, float moveSpeed, float attackSpeed)
    {
        Name = unitName;
        UnitType = unitType;
        Damage = damage;
        HP = hp;
        XP = xp;
        MoveSpeed = moveSpeed;
        AttackSpeed = attackSpeed;
    }

    public override string ToString()
    {
        return $"Unit:\nName:{Name}\nUnitType:{UnitType}\nDamage:{Damage}\nHP:{HP}";
    }
}
