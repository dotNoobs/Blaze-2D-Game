public class Usable : Item
{
    public Usable(string name, ItemType itemType, int spriteIndex, int power, int agility, int wisdom, UsableType usableType, AbilityType ability, float cooldown) : base(name, itemType, spriteIndex, power, agility, wisdom)
    {
        UsableType = usableType;
        Ability = ability;
        Cooldown = cooldown;
    }

    public UsableType UsableType { get; }
    public AbilityType Ability { get; }
    public float Cooldown { get; }
}
