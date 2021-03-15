using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStatsUI : MonoBehaviour
{
    [SerializeField] GameObject itemStatsUI;
    [SerializeField] TMP_Text itemPower;
    [SerializeField] TMP_Text itemAgility;
    [SerializeField] TMP_Text itemWisdom;
    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text itemDamage;
    [SerializeField] TMP_Text itemAS;
    [SerializeField] TMP_Text eqPower;
    [SerializeField] TMP_Text eqAgility;
    [SerializeField] TMP_Text eqWisdom;
    [SerializeField] TMP_Text eqDamage;
    [SerializeField] TMP_Text eqAS;
    [SerializeField] Image redArrowPow;
    [SerializeField] Image greenArrowPow;
    [SerializeField] Image redArrowAgi;
    [SerializeField] Image greenArrowAgi;
    [SerializeField] Image redArrowWis;
    [SerializeField] Image greenArrowWis;
    [SerializeField] Image redArrowDamage;
    [SerializeField] Image greenArrowDamage;
    [SerializeField] Image redArrowAS;
    [SerializeField] Image greenArrowAS;
    ItemHolder itemToCheck;
    Weapon tmpWeapon;
    Inventory playerInventory;
    Player player;

    private void Start()
    {
        playerInventory = GetComponent<Inventory>();
        player = GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ItemHolder>() != null)
        {
            itemStatsUI.gameObject.SetActive(true);
            itemToCheck = collision.GetComponent<ItemHolder>();
            itemStatsUI.transform.localPosition = itemToCheck.transform.localPosition + new Vector3(125, 0, 0);

            if (itemToCheck.Item.ItemType == ItemType.Weapon)
            {
                if (((itemToCheck.Item as Weapon).WeaponVariation == WeaponVariation.Melee || ((itemToCheck.Item as Weapon).WeaponVariation == WeaponVariation.Ranged)))
                {
                    tmpWeapon = itemToCheck.Item as Weapon;
                    itemName.text = $"NAME:{tmpWeapon.Name}";
                    itemDamage.text = $"DMG:{tmpWeapon.Damage}";
                    itemAS.text = $"AS:{tmpWeapon.AttackSpeed:n2}";
                    itemPower.text = $"POW:{tmpWeapon.Power}";
                    itemAgility.text = $"AGI:{tmpWeapon.Agility}";
                    itemWisdom.text = $"WIS:{tmpWeapon.Wisdom}";
                    CompareWeapon(tmpWeapon);
                }

            }
            else if (itemToCheck.Item.ItemType == ItemType.Usable)
            {
                if ((itemToCheck.Item as Usable).UsableType == UsableType.Ability)
                {
                    itemName.text = $"NAME:{itemToCheck.Item.Name}";
                    itemDamage.text = $"SKILL:{(itemToCheck.Item as Usable).Ability}";
                    itemPower.text = $"POW:{itemToCheck.Item.Power}";
                    itemAgility.text = $"AGI:{itemToCheck.Item.Agility}";
                    itemWisdom.text = $"WIS:{itemToCheck.Item.Wisdom}";
                    itemAS.text = $"";
                    CompareAbility(itemToCheck.Item as Usable);
                }
                else if ((itemToCheck.Item as Usable).UsableType == UsableType.Potion)
                {
                    itemName.text = $"Potion";
                    itemDamage.text = $"{(itemToCheck.Item as Usable).Ability}";
                    itemPower.text = $"HP:{CalculateHpOnPotion((itemToCheck.Item as Usable).Ability.ToString())}";
                    itemAgility.text = $"";
                    itemWisdom.text = $"";
                    itemAS.text = $"";
                    ComparePotion(itemToCheck.Item as Usable);
                }
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ClearCompareUI();
        itemStatsUI.gameObject.SetActive(false);
        itemToCheck = null;
    }
    void CompareWeapon(Weapon weapon)
    {
        if (playerInventory.MeleeSlot != null)
        {
            if (weapon.WeaponVariation == WeaponVariation.Melee)
            {
                eqPower.text = $"{playerInventory.MeleeSlot.Power}";
                eqDamage.text = $"{playerInventory.MeleeSlot.Damage}";
                eqAgility.text = $"{playerInventory.MeleeSlot.Agility}";
                eqWisdom.text = $"{playerInventory.MeleeSlot.Wisdom}";
                eqAS.text = $"{playerInventory.MeleeSlot.AttackSpeed:n2}";
                if (weapon.Power < playerInventory.MeleeSlot.Power)
                    redArrowPow.gameObject.SetActive(true);
                else if (weapon.Power > playerInventory.MeleeSlot.Power)
                    greenArrowPow.gameObject.SetActive(true);
                if (weapon.Agility < playerInventory.MeleeSlot.Agility)
                    redArrowAgi.gameObject.SetActive(true);
                else if (weapon.Agility > playerInventory.MeleeSlot.Agility)
                    greenArrowAgi.gameObject.SetActive(true);
                if (weapon.Wisdom < playerInventory.MeleeSlot.Wisdom)
                    redArrowWis.gameObject.SetActive(true);
                else if (weapon.Wisdom > playerInventory.MeleeSlot.Wisdom)
                    greenArrowWis.gameObject.SetActive(true);
                if (weapon.Damage < playerInventory.MeleeSlot.Damage)
                    redArrowDamage.gameObject.SetActive(true);
                else if (weapon.Damage > playerInventory.MeleeSlot.Damage)
                    greenArrowDamage.gameObject.SetActive(true);
                if (weapon.AttackSpeed < playerInventory.MeleeSlot.AttackSpeed)
                    redArrowAS.gameObject.SetActive(true);
                else if (weapon.AttackSpeed > playerInventory.MeleeSlot.AttackSpeed)
                    greenArrowAS.gameObject.SetActive(true);
            }
            else if (weapon.WeaponVariation == WeaponVariation.Ranged)
            {
                eqPower.text = $"{playerInventory.RangedSlot.Power}";
                eqDamage.text = $"{playerInventory.RangedSlot.Damage}";
                eqAgility.text = $"{playerInventory.RangedSlot.Agility}";
                eqWisdom.text = $"{playerInventory.RangedSlot.Wisdom}";
                eqAS.text = $"{playerInventory.RangedSlot.AttackSpeed:n2}";
                if (weapon.Power < playerInventory.RangedSlot.Power)
                    redArrowPow.gameObject.SetActive(true);
                else if (weapon.Power > playerInventory.RangedSlot.Power)
                    greenArrowPow.gameObject.SetActive(true);
                if (weapon.Agility < playerInventory.RangedSlot.Agility)
                    redArrowAgi.gameObject.SetActive(true);
                else if (weapon.Agility > playerInventory.RangedSlot.Agility)
                    greenArrowAgi.gameObject.SetActive(true);
                if (weapon.Wisdom < playerInventory.RangedSlot.Wisdom)
                    redArrowWis.gameObject.SetActive(true);
                else if (weapon.Wisdom > playerInventory.RangedSlot.Wisdom)
                    greenArrowWis.gameObject.SetActive(true);
                if (weapon.Damage < playerInventory.RangedSlot.Damage)
                    redArrowDamage.gameObject.SetActive(true);
                else if (weapon.Damage > playerInventory.RangedSlot.Damage)
                    greenArrowDamage.gameObject.SetActive(true);
                if (weapon.AttackSpeed < playerInventory.RangedSlot.AttackSpeed)
                    redArrowAS.gameObject.SetActive(true);
                else if (weapon.AttackSpeed > playerInventory.RangedSlot.AttackSpeed)
                    greenArrowAS.gameObject.SetActive(true);
            }

        }


    }
    void CompareAbility(Usable ability)
    {
        if (playerInventory.AbilitySlot != null)
        {
            eqPower.text = $"{playerInventory.AbilitySlot.Power}";
            eqDamage.text = $"{playerInventory.AbilitySlot.Ability}";
            eqAgility.text = $"{playerInventory.AbilitySlot.Agility}";
            eqWisdom.text = $"{playerInventory.AbilitySlot.Wisdom}";

            if (ability.Power < playerInventory.AbilitySlot.Power)
                redArrowPow.gameObject.SetActive(true);
            else if (ability.Power > playerInventory.AbilitySlot.Power)
                greenArrowPow.gameObject.SetActive(true);
            if (ability.Agility < playerInventory.AbilitySlot.Agility)
                redArrowAgi.gameObject.SetActive(true);
            else if (ability.Agility > playerInventory.AbilitySlot.Agility)
                greenArrowAgi.gameObject.SetActive(true);
            if (ability.Wisdom < playerInventory.AbilitySlot.Wisdom)
                redArrowWis.gameObject.SetActive(true);
            else if (ability.Wisdom > playerInventory.AbilitySlot.Wisdom)
                greenArrowWis.gameObject.SetActive(true);
            //if (ability.Ability < playerInventory.AbilitySlot.Ability)
            //    redArrowDamage.gameObject.SetActive(true);
            //else if (ability.Ability > playerInventory.AbilitySlot.Ability)
            //    greenArrowDamage.gameObject.SetActive(true);
        }

    }
    void ComparePotion(Usable potion)
    {
        if (playerInventory.PotionSlot != null)
        {
            int eqPotion = CalculateHpOnPotion(playerInventory.PotionSlot.Ability.ToString());
            int groudPotion = CalculateHpOnPotion(potion.Ability.ToString());
            eqDamage.text = $"{playerInventory.PotionSlot.Ability}";
            eqPower.text = $"HP:{CalculateHpOnPotion(playerInventory.PotionSlot.Ability.ToString())}";
            if (groudPotion < eqPotion)
                redArrowPow.gameObject.SetActive(true);
            else if (groudPotion > eqPotion)
                greenArrowPow.gameObject.SetActive(true);

        }

    }
    int CalculateHpOnPotion(string typeOfPotion)
    {
        if (typeOfPotion == "Small")
            return (int)(player.HP * 0.05f);
        else if (typeOfPotion == "Medium")
            return (int)(player.HP * 0.10f);
        else if (typeOfPotion == "Big")
            return (int)(player.HP * 0.15f);
        else if (typeOfPotion == "Giant")
            return (int)(player.HP * 0.2f);
        return 0;
    }
    void ClearCompareUI()
    {

        eqPower.text = $"";
        eqAgility.text = $"";
        eqWisdom.text = $"";
        eqAS.text = $"";
        eqDamage.text = $"";
        redArrowPow.gameObject.SetActive(false);
        greenArrowPow.gameObject.SetActive(false);
        redArrowAgi.gameObject.SetActive(false);
        greenArrowAgi.gameObject.SetActive(false);
        redArrowDamage.gameObject.SetActive(false);
        greenArrowDamage.gameObject.SetActive(false);
        redArrowAS.gameObject.SetActive(false);
        greenArrowAS.gameObject.SetActive(false);
        redArrowWis.gameObject.SetActive(false);
        greenArrowWis.gameObject.SetActive(false);
    }
}
