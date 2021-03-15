using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Unit/Basic Unit", order = 1)]
public class BaseUnit : ScriptableObject
{
    [SerializeField] string unitName;
    [SerializeField] UnitType unitType;
    [SerializeField] int damage;
    [SerializeField] int hp;
    [SerializeField] int xp;
    [SerializeField] float moveSpeed;
    [SerializeField] float attackSpeed;

    public string Name => unitName;
    public UnitType UnitType => unitType;
    public int Damage => damage;
    public int HP => hp;
    public int XP => xp;
    public float MoveSpeed => moveSpeed;
    public float AttackSpeed => attackSpeed;
}
