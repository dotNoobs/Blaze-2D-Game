using UnityEngine;

[CreateAssetMenu(fileName = "AttributesData", menuName = "ScriptableObjects/Attributes", order = 1)]
public class BaseAttributes : ScriptableObject
{
    public int startLevel => startlevel;
    public int BaseHP => baseHP;
    public int BasePower => basePower;
    public int BaseAgility => baseAgility;
    public int BaseWisdom => baseWisdom;
    public float BaseAttackSpeed => baseAttackSpeed;
    public float BaseMovementSpeed => baseMovementSpeed;
    public float BaseCdReduction => baseCdReduction;
    public int BaseDamage => baseDamage;


    [SerializeField] int startlevel;
    [SerializeField] int baseHP;
    [SerializeField] int basePower;
    [SerializeField] int baseAgility;
    [SerializeField] int baseWisdom;
    [SerializeField] float baseAttackSpeed;
    [SerializeField] float baseMovementSpeed;
    [SerializeField] float baseCdReduction;
    [SerializeField] int baseDamage;
}
