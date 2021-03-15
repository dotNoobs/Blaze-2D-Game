using UnityEngine;

[CreateAssetMenu(fileName = "BaseWeapon", menuName = "BaseItem/BaseWeapon", order = 1)]
public class BaseWeapon : BaseItem
{
    [SerializeField] WeaponVariation weaponVariation;
    [SerializeField] WeaponType weaponType;
    [SerializeField] int damage;
    [SerializeField] float attackSpeed;

    public WeaponVariation WeaponVariation => weaponVariation;
    public WeaponType WeaponType => weaponType;
    public int Damage => damage;
    public float AttackSpeed => attackSpeed;
}
