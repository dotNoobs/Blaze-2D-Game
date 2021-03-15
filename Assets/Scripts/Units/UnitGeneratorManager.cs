using System.Collections.Generic;
using UnityEngine;

public class UnitGeneratorManager : MonoBehaviour, IGameManager
{
    [SerializeField] BaseUnitHolder baseUnitHolder;

    public ManagerStatus Status { get; private set; }
    public List<GameObject> CurrentUnits { get; private set; } = new List<GameObject>();

    int xp;
    public int playerOldLevel = 0;

    public void Startup()
    {
        Debug.Log("Unit Generator Manager starting...");
        Status = ManagerStatus.Started;
    }

    #region Unit Spawners
    /// <summary>
    /// Spawn random <see cref="Unit"/> at given position
    /// </summary>
    /// <param name="position"><see cref="Vector3"/> position to where spawn a <see cref="Unit"/></param>
    public void SpawnUnit(Vector3 position)
    {
        GameObject unitPrefab = default;
        Unit newUnit = GenerateUnit();

        switch (newUnit.UnitType)
        {
            case UnitType.Melee:
                unitPrefab = baseUnitHolder.MeleeUnits[Random.Range(0, baseUnitHolder.MeleeUnits.Count)];
                break;
            case UnitType.Ranged:
                unitPrefab = baseUnitHolder.RangedUnits[Random.Range(0, baseUnitHolder.RangedUnits.Count)];
                break;
            default:
                Debug.Log("Could not assign unitPrefab based on rolled unitType");
                break;
        }

        var unitInstance = Instantiate(unitPrefab, position, Quaternion.identity);
        unitInstance.GetComponentInChildren<UnitHolder>().Unit = newUnit;

        CurrentUnits.Add(unitInstance);
    }

    /// <summary>
    /// Spawn random <see cref="Unit"/> with specific <see cref="UnitType"/> at given position
    /// </summary>
    /// <param name="unitType">Specific <see cref="UnitType"/> to spawn</param>
    /// <param name="position"><see cref="Vector3"/> position to where spawn a <see cref="Unit"/></param>
    public void SpawnUnit(UnitType unitType, Vector3 position)
    {
        GameObject unitPrefab = default;
        Unit newUnit = GenerateUnit(unitType);

        switch (newUnit.UnitType)
        {
            case UnitType.Melee:
                unitPrefab = baseUnitHolder.MeleeUnits[Random.Range(0, baseUnitHolder.MeleeUnits.Count)];
                break;
            case UnitType.Ranged:
                unitPrefab = baseUnitHolder.RangedUnits[Random.Range(0, baseUnitHolder.RangedUnits.Count)];
                break;
            case UnitType.Boss:
                unitPrefab = baseUnitHolder.BossUnits[Random.Range(0, baseUnitHolder.BossUnits.Count)];
                break;
        }

        var unitInstance = Instantiate(unitPrefab, position, Quaternion.identity);
        unitInstance.GetComponentInChildren<UnitHolder>().Unit = newUnit;

        CurrentUnits.Add(unitInstance);
    }

    /// <summary>
    /// Spawn given <see cref="Unit"/> at given position
    /// </summary>
    /// <param name="unit"><see cref="Unit"/> to spawn</param>
    /// <param name="position"><see cref="Vector3"/> position to where spawn a <see cref="Unit"/></param>
    public void SpawnUnit(Unit unit, Vector3 position)
    {
        GameObject unitPrefab = default;

        switch (unit.UnitType)
        {
            case UnitType.Melee:
                unitPrefab = baseUnitHolder.MeleeUnits[Random.Range(0, baseUnitHolder.MeleeUnits.Count)];
                break;
            case UnitType.Ranged:
                unitPrefab = baseUnitHolder.RangedUnits[Random.Range(0, baseUnitHolder.RangedUnits.Count)];
                break;
            default:
                Debug.Log("Could not assign unitPrefab based on rolled unitType");
                break;
        }

        var unitInstance = Instantiate(unitPrefab, position, Quaternion.identity);
        unitInstance.GetComponentInChildren<UnitHolder>().Unit = unit;

        CurrentUnits.Add(unitInstance);
    }
    #endregion

    #region Unit Generators
    /// <summary>
    /// Generate random <see cref="Unit"/>
    /// </summary>
    /// <returns><see cref="Unit"/></returns>
    public Unit GenerateUnit()
    {
        Unit temp;

        if (Random.Range(0f, 1f) > 0.5f)
            temp = GenerateUnit(UnitType.Melee);
        else
            temp = GenerateUnit(UnitType.Ranged);

        return temp;
    }

    /// <summary>
    /// Generate random <see cref="Unit"/> with specific <see cref="UnitType"/>
    /// </summary>
    /// <param name="unitType">Specific <see cref="UnitType"/></param>
    /// <returns><see cref="Unit"/></returns>
    public Unit GenerateUnit(UnitType unitType)
    {
        List<BaseUnit> specificBaseUnitList = new List<BaseUnit>();

        foreach (BaseUnit baseUnit in baseUnitHolder.BaseUnitList)
        {
            if (baseUnit.UnitType == unitType)
                specificBaseUnitList.Add(baseUnit);
        }

        BaseUnit rolledBaseUnit = specificBaseUnitList[Random.Range(0, specificBaseUnitList.Count)];

        CalculateXp(rolledBaseUnit.XP);

        Unit newUnit = new Unit(
            rolledBaseUnit.Name,
            rolledBaseUnit.UnitType,
            Mathf.RoundToInt(rolledBaseUnit.Damage * Managers.progress.FinalDamageMobMultipler),
            Mathf.RoundToInt(rolledBaseUnit.HP * Managers.progress.FinalHpMobMultipler),
            xp,
            rolledBaseUnit.MoveSpeed + (rolledBaseUnit.MoveSpeed * Managers.progress.FinalMobMovementMultipler),
            rolledBaseUnit.AttackSpeed + (rolledBaseUnit.AttackSpeed * Managers.progress.FinalMobAsMultipler)
            );

        return newUnit;
    }
    #endregion

    void CalculateXp(float xp)
    {
            this.xp = Mathf.RoundToInt(xp * Managers.progress.XpMultipler * Managers.PlayerXP.CurrentLevel);
    }
}
