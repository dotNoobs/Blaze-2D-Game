using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class ItemGeneratorManager : MonoBehaviour, IGameManager
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] BaseWeaponHolder baseWeaponHolder;
    [SerializeField] BaseUsableHolder baseUsableHolder;
    [SerializeField] AbilityList abilityList;

    SpriteLibrary spriteLibrary;
    public ManagerStatus Status { get; private set; }

    public List<GameObject> CurrentItems { get; private set; } = new List<GameObject>();

    public void Startup()
    {
        Debug.Log("Item generator starting...");

        spriteLibrary = GetComponent<SpriteLibrary>();

        Status = ManagerStatus.Started;
    }

    #region Item Spawners
    /// <summary>
    /// Spawns a random item at given <see cref="Vector3"/> position.
    /// </summary>
    /// <param name="position">Desired <see cref="Vector3"/> position of spawned item.<param>
    public void SpawnItem(Vector3 position)
    {
        var newItem = Instantiate(itemPrefab, position, Quaternion.identity);
        newItem.GetComponent<ItemHolder>().Item = GenerateItem();
        CurrentItems.Add(newItem);
    }

    /// <summary>
    /// Spawns a specific item at given <see cref="Vector3"/> position.
    /// </summary>
    /// <param name="item"><see cref="Item"/> you want to spawn</param>
    /// <param name="position">Desired <see cref="Vector3"/> position of spawned item.</param>
    public void SpawnItem(Item item, Vector3 position)
    {
        var newItem = Instantiate(itemPrefab, position, Quaternion.identity);
        newItem.GetComponent<ItemHolder>().Item = item;
        CurrentItems.Add(newItem);
    }
    #endregion

    #region Item Generators
    /// <summary>
    /// Generate a random <see cref="Item"/>
    /// </summary>
    /// <returns><see cref="Item"/></returns>
    public Item GenerateItem()
    {
        Item temp;

        if (Random.Range(0f, 1f) < 0.7f)
            temp = GenerateWeapon();
        else
            temp = GenerateUsable();

        return temp;
    }

    /// <summary>
    /// Generate a random <see cref="Weapon"/>.
    /// </summary>
    /// <returns><see cref="Weapon"/></returns>
    public Weapon GenerateWeapon()
    {
        BaseWeapon rolledBaseWeapon = baseWeaponHolder.BaseWeaponList[Random.Range(0, baseWeaponHolder.BaseWeaponList.Count)];

        int spriteIndex = Random.Range(0, GetSpriteIndex(rolledBaseWeapon.WeaponType));

        Weapon newWeapon = new Weapon(
            rolledBaseWeapon.Name,
            rolledBaseWeapon.ItemType,
            spriteIndex,
            Mathf.RoundToInt(rolledBaseWeapon.Power * Managers.progress.FinalItemMultipler),
            Mathf.RoundToInt(rolledBaseWeapon.Agility * Managers.progress.FinalItemMultipler),
            Mathf.RoundToInt(rolledBaseWeapon.Wisdom * Managers.progress.FinalItemMultipler),
            rolledBaseWeapon.WeaponVariation,
            rolledBaseWeapon.WeaponType,
            Mathf.RoundToInt(rolledBaseWeapon.Damage * Managers.progress.FinalItemMultipler),
            rolledBaseWeapon.AttackSpeed * Managers.progress.FinalItemAsMultipler
            );

        return newWeapon;
    }

    /// <summary>
    /// Generate a random weapon with specific <see cref="WeaponVariation"/>.
    /// </summary>
    /// <param name="weaponVariation">Specific <see cref="WeaponVariation"/></param>
    /// <returns><see cref="Weapon"/></returns>
    public Weapon GenerateWeapon(WeaponVariation weaponVariation)
    {
        List<BaseWeapon> specificBaseWeaponList = new List<BaseWeapon>();

        foreach (BaseWeapon baseWeapon in baseWeaponHolder.BaseWeaponList)
        {
            if (baseWeapon.WeaponVariation == weaponVariation)
                specificBaseWeaponList.Add(baseWeapon);
        }

        BaseWeapon rolledBaseWeapon = specificBaseWeaponList[Random.Range(0, specificBaseWeaponList.Count)];

        int spriteIndex = Random.Range(0, GetSpriteIndex(rolledBaseWeapon.WeaponType));

        Weapon newWeapon = new Weapon(
            rolledBaseWeapon.Name,
            rolledBaseWeapon.ItemType,
            spriteIndex,
            Mathf.RoundToInt(rolledBaseWeapon.Power * Managers.progress.FinalItemMultipler),
            Mathf.RoundToInt(rolledBaseWeapon.Agility * Managers.progress.FinalItemMultipler),
            Mathf.RoundToInt(rolledBaseWeapon.Wisdom * Managers.progress.FinalItemMultipler),
            rolledBaseWeapon.WeaponVariation,
            rolledBaseWeapon.WeaponType,
            Mathf.RoundToInt(rolledBaseWeapon.Damage * Managers.progress.FinalItemMultipler),
            rolledBaseWeapon.AttackSpeed * Managers.progress.FinalItemAsMultipler
            );

        return newWeapon;
    }

    /// <summary>
    /// Generate random <see cref="Usable"/>.
    /// </summary>
    /// <returns><see cref="Usable"/></returns>
    public Usable GenerateUsable()
    {
        Usable temp;

        if (Random.Range(0f, 1f) < 0.5f)
            temp = GenerateUsable(UsableType.Ability);
        else
            temp = GenerateUsable(UsableType.Potion);

        return temp;
    }

    /// <summary>
    /// Generate a random <see cref="Usable"/> with specific <see cref="UsableType"/>.
    /// </summary>
    /// <param name="usableType">Specific <see cref="UsableType"/></param>
    /// <returns><see cref="Usable"/></returns>
    public Usable GenerateUsable(UsableType usableType)
    {
        List<BaseUsable> specificBaseUsableList = new List<BaseUsable>();

        foreach (BaseUsable baseUsable in baseUsableHolder.BaseUsableList)
        {
            if (baseUsable.UsableType == usableType)
                specificBaseUsableList.Add(baseUsable);
        }

        BaseUsable rolledBaseUsable = specificBaseUsableList[Random.Range(0, specificBaseUsableList.Count)];

        int spriteIndex = Random.Range(0, GetSpriteIndex(rolledBaseUsable.UsableType));

        int ability;

        Usable newUsable = default;

        if (usableType == UsableType.Ability)
        {
            ability = Random.Range(0, abilityList.SkillList.Count);

            newUsable = new Usable(rolledBaseUsable.Name,
            rolledBaseUsable.ItemType,
            spriteIndex,
            Mathf.RoundToInt(rolledBaseUsable.Power * Managers.progress.FinalItemMultipler),
            Mathf.RoundToInt(rolledBaseUsable.Agility * Managers.progress.FinalItemMultipler),
            Mathf.RoundToInt(rolledBaseUsable.Wisdom * Managers.progress.FinalItemMultipler),
            rolledBaseUsable.UsableType,
            (AbilityType)ability,
            abilityList.SkillList[ability].Cooldown);
        }
        else if (usableType == UsableType.Potion)
        {
            ability = Random.Range(0, abilityList.PotionList.Count) + abilityList.SkillList.Count;

            newUsable = new Usable(rolledBaseUsable.Name,
            rolledBaseUsable.ItemType,
            spriteIndex,
            rolledBaseUsable.Power,
            rolledBaseUsable.Agility,
            rolledBaseUsable.Wisdom,
            rolledBaseUsable.UsableType,
            (AbilityType)ability,
            abilityList.PotionList[ability - abilityList.SkillList.Count].Cooldown);
        }

        return newUsable;
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// Get a number of sprites in the <see cref="SpriteLibrary"/> with given <see cref="WeaponType"/> category.
    /// </summary>
    /// <param name="weaponType">Category name <see cref="WeaponType"/></param>
    /// <returns></returns>
    int GetSpriteIndex(WeaponType weaponType)
    {
        var sprites = spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(weaponType.ToString());

        int number = 0;

        foreach (var sprite in sprites)
        {
            number++;
        }

        return number;
    }

    /// <summary>
    /// Get a number of sprites in the <see cref="SpriteLibrary"/> with given <see cref="UsableType"/> category.
    /// </summary>
    /// <param name="usableType">Category name <see cref="UsableType"/></param>
    /// <returns></returns>
    int GetSpriteIndex(UsableType usableType)
    {
        var sprites = spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(usableType.ToString());

        int number = 0;

        foreach (var sprite in sprites)
        {
            number++;
        }

        return number;
    }
    #endregion
}