public class Item
{
    public string Name { get; }
    public ItemType ItemType { get; }
    public int SpriteIndex { get; }
    public int Power { get; }
    public int Agility { get; }
    public int Wisdom { get; }

    public Item(string name, ItemType itemType, int spriteIndex, int power, int agility, int wisdom)
    {
        Name = name;
        ItemType = itemType;
        SpriteIndex = spriteIndex;
        Power = power;
        Agility = agility;
        Wisdom = wisdom;
    }
}
