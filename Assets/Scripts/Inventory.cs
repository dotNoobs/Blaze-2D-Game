using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    AbilityHolder ability;
    ItemHolder groundItem;
    Player player;

    public Action OnInventoryChanged;

    public Weapon MeleeSlot { get; private set; }
    public Weapon RangedSlot { get; private set; }
    public Usable AbilitySlot { get; private set; }
    public Usable PotionSlot { get; private set; }

    void Start()
    {
        player = GetComponent<Player>();
        ability = GetComponent<AbilityHolder>();
        MeleeSlot = Managers.ItemGenerator.GenerateWeapon(WeaponVariation.Melee);
        RangedSlot = Managers.ItemGenerator.GenerateWeapon(WeaponVariation.Ranged);
        PotionSlot = Managers.ItemGenerator.GenerateUsable(UsableType.Potion);
        OnInventoryChanged?.Invoke();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (groundItem != null && ability.skillInUse != true)
            {
                AudioManager.Instance.Play(SoundEffectType.pickUpItem);
                UpdateWeaponStatus();
                groundItem.UpdateIcon();
            }
        }
        if (Time.time > ability.nextUseTimePotion)
        {
            if (Input.GetKeyDown(KeyCode.T) && PotionSlot != null)
            {
                ability.UseSkill("HealUp");
                ability.nextUseTimePotion = Time.time + (PotionSlot.Cooldown - PotionSlot.Cooldown * player.CdReduction / 100);

            }
        }
        if (Time.time > ability.nextUseTimeAbility)
        {
            if (Input.GetKeyDown(KeyCode.R) && AbilitySlot != null)
            {
                ability.UseSkill(AbilitySlot.Ability.ToString());
                Debug.Log($"Cooldown:{AbilitySlot.Cooldown - AbilitySlot.Cooldown * player.CdReduction / 100}");
                ability.nextUseTimeAbility = Time.time + (AbilitySlot.Cooldown - AbilitySlot.Cooldown * player.CdReduction / 100);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ItemHolder>() != null)
            groundItem = collision.GetComponent<ItemHolder>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<ItemHolder>() != null)
            groundItem = null;
    }

    public void UpdateWeaponStatus()
    {
        if (groundItem.Item.ItemType == ItemType.Weapon)
        {
            if ((groundItem.Item as Weapon).WeaponVariation == WeaponVariation.Melee)
            {
                Item tmp = MeleeSlot;
                MeleeSlot = groundItem.Item as Weapon;
                groundItem.Item = tmp;
            }
            else if ((groundItem.Item as Weapon).WeaponVariation == WeaponVariation.Ranged)
            {
                Item tmp = RangedSlot;
                RangedSlot = groundItem.Item as Weapon;
                groundItem.Item = tmp;
            }
        }
        else if (groundItem.Item.ItemType == ItemType.Usable)
        {
            if ((groundItem.Item as Usable).UsableType == UsableType.Ability)
            {
                Item tmp = AbilitySlot;
                AbilitySlot = groundItem.Item as Usable;
                groundItem.Item = tmp;
            }
            else if ((groundItem.Item as Usable).UsableType == UsableType.Potion)
            {
                Item tmp = PotionSlot;
                PotionSlot = groundItem.Item as Usable;
                groundItem.Item = tmp;
            }
        }
        OnInventoryChanged?.Invoke();
    }

}
