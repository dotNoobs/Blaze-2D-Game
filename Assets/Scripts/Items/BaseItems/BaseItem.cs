using UnityEngine;

public class BaseItem : ScriptableObject
{
    [SerializeField] string itemName;
    [SerializeField] ItemType itemType;
    [SerializeField] int power;
    [SerializeField] int agility;
    [SerializeField] int wisdom;

    public string Name => itemName;
    public ItemType ItemType => itemType;
    public int Power => power;
    public int Agility => agility;
    public int Wisdom => wisdom;
}