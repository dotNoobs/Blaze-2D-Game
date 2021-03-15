public class Weapon : Item
{
    public Weapon(string name, ItemType itemType, int spriteIndex, int power, int agility, int wisdom, WeaponVariation weaponVariation, WeaponType weaponType, int damage, float attackSpeed) : base(name, itemType, spriteIndex, power, agility, wisdom)
    {
        WeaponVariation = weaponVariation;
        WeaponType = weaponType;
        Damage = damage;
        AttackSpeed = attackSpeed;
    }

    public WeaponVariation WeaponVariation { get; }
    public WeaponType WeaponType { get; }
    public int Damage { get; }
    public float AttackSpeed { get; }
}
