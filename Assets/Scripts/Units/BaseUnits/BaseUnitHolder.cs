using System.Collections.Generic;
using UnityEngine;

public class BaseUnitHolder : MonoBehaviour
{
    [SerializeField] List<BaseUnit> baseUnitList;
    [SerializeField] List<GameObject> meleeUnits;
    [SerializeField] List<GameObject> rangedUnits;
    [SerializeField] List<GameObject> bossUnits;

    public List<BaseUnit> BaseUnitList => baseUnitList;
    public List<GameObject> MeleeUnits => meleeUnits;
    public List<GameObject> RangedUnits => rangedUnits;
    public List<GameObject> BossUnits => bossUnits;
}
