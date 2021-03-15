using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public enum ProjectileType
{
    Arrow,
    Energy
}

public class SpriteController : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Inventory inventory;
    [SerializeField] SpriteResolver meleeBackSpriteResolver;
    [SerializeField] SpriteResolver rangedBackSpriteResolver;
    [SerializeField] SpriteLibrary bowSpriteLibrary;
    [SerializeField] List<SpriteLibraryAsset> bowAnimations;
    [SerializeField] List<SpriteLibraryAsset> arrowAnimations;
    [SerializeField] List<SpriteLibraryAsset> energyAnimations;

    void OnEnable()
    {
        inventory.OnInventoryChanged += UpdatePlayerAnimationSprites;
    }

    void OnDisable()
    {
        inventory.OnInventoryChanged -= UpdatePlayerAnimationSprites;
    }

    void UpdatePlayerAnimationSprites()
    {
        meleeBackSpriteResolver.SetCategoryAndLabel(inventory.MeleeSlot.WeaponType.ToString(), inventory.MeleeSlot.SpriteIndex.ToString());

        SpriteLibrary bulletLibrary = projectilePrefab.GetComponentInChildren<SpriteLibrary>();

        if (inventory.RangedSlot.WeaponType == WeaponType.Bow)
        {
            bowSpriteLibrary.spriteLibraryAsset = bowAnimations[inventory.RangedSlot.SpriteIndex];
            rangedBackSpriteResolver.SetCategoryAndLabel("Quiver", inventory.RangedSlot.SpriteIndex.ToString());
            bulletLibrary.spriteLibraryAsset = arrowAnimations[inventory.RangedSlot.SpriteIndex];
        }
        else if (inventory.RangedSlot.WeaponType == WeaponType.Staff)
        {
            rangedBackSpriteResolver.SetCategoryAndLabel(inventory.RangedSlot.WeaponType.ToString(), inventory.RangedSlot.SpriteIndex.ToString());
            bulletLibrary.spriteLibraryAsset = energyAnimations[Random.Range(0, energyAnimations.Count)];
        }
    }
}
