using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponHolder : MonoBehaviour
{
    [SerializeField] List<BaseWeapon> baseWeaponList;

    public List<BaseWeapon> BaseWeaponList => baseWeaponList;
}
