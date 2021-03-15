using System;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public Action PlayerAttributesChanged;

    AbilityHolder ability;
    Inventory inventory;
    PlayerMovement playerMovement;
    [SerializeField] Animator weaponAnimator;
    [SerializeField] CharacterStatsUI statsUI;

    public int Level => level;
    public int MaxHP => maxHP;
    public float MeleeAttackSpeed => meleeAttackSpeed;
    public float RangedAttackSpeed => rangedAttackSpeed;
    public float MovementSpeed => movementSpeed;
    public float CdReduction => cdReduction;
    public int MeleeDamage => meleeDamage;
    public int RangedDamage => rangedDamage;

    [SerializeField] int level;
    [SerializeField] int maxHP;
    [SerializeField] float meleeAttackSpeed;
    [SerializeField] float rangedAttackSpeed;
    [SerializeField] float movementSpeed;
    [SerializeField] float cdReduction;
    [SerializeField] int meleeDamage;
    [SerializeField] int rangedDamage;
    [SerializeField] BaseAttributes baseAttributes;

    private int powerToHP = 20;
    private float powerToDamage = 0.5f;
    private float agilityToAttackSpeed = 0.005f;
    private float agilityToMoveSpeed = 0.005f;
    private float wisdomToCdReduction = 0.5f;

    private void OnEnable()
    {
        inventory = GetComponent<Inventory>();
        inventory.OnInventoryChanged += CalculateAttributes;
    }

    private void OnDisable()
    {
        inventory.OnInventoryChanged -= CalculateAttributes;
    }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        ability = GetComponent<AbilityHolder>();
    }

    private void CalculateAttributes()
    {
        if (inventory.AbilitySlot != null)
        {

            maxHP = baseAttributes.BaseHP + ((inventory.MeleeSlot.Power + inventory.RangedSlot.Power + inventory.AbilitySlot.Power + statsUI.TotalPower) * powerToHP);
            meleeAttackSpeed = baseAttributes.BaseAttackSpeed + inventory.MeleeSlot.AttackSpeed + ((inventory.MeleeSlot.Agility + inventory.RangedSlot.Agility + inventory.AbilitySlot.Agility + statsUI.TotalAgility) * agilityToAttackSpeed);
            rangedAttackSpeed = baseAttributes.BaseAttackSpeed + inventory.RangedSlot.AttackSpeed + ((inventory.MeleeSlot.Agility + inventory.RangedSlot.Agility + inventory.AbilitySlot.Agility + statsUI.TotalAgility) * agilityToAttackSpeed);
            movementSpeed = baseAttributes.BaseMovementSpeed + ((inventory.MeleeSlot.Agility + inventory.RangedSlot.Agility + inventory.AbilitySlot.Agility + statsUI.TotalAgility) * agilityToMoveSpeed);
            meleeDamage = baseAttributes.BaseDamage + inventory.MeleeSlot.Damage + (int)((inventory.MeleeSlot.Power + inventory.RangedSlot.Power + inventory.AbilitySlot.Power + statsUI.TotalPower) * powerToDamage);
            rangedDamage = baseAttributes.BaseDamage + inventory.RangedSlot.Damage + (int)((inventory.MeleeSlot.Power + inventory.RangedSlot.Power + inventory.AbilitySlot.Power + statsUI.TotalPower) * powerToDamage);
            cdReduction = baseAttributes.BaseCdReduction + ((inventory.MeleeSlot.Wisdom + inventory.RangedSlot.Wisdom + inventory.AbilitySlot.Wisdom + statsUI.TotalWisdom) * wisdomToCdReduction);
        }
        else
        {
            maxHP = baseAttributes.BaseHP + ((inventory.MeleeSlot.Power + inventory.RangedSlot.Power + statsUI.TotalPower) * powerToHP);
            meleeAttackSpeed = baseAttributes.BaseAttackSpeed + inventory.MeleeSlot.AttackSpeed + ((inventory.MeleeSlot.Agility + inventory.RangedSlot.Agility + statsUI.TotalAgility) * agilityToAttackSpeed);
            rangedAttackSpeed = baseAttributes.BaseAttackSpeed + inventory.RangedSlot.AttackSpeed + ((inventory.MeleeSlot.Agility + inventory.RangedSlot.Agility + statsUI.TotalAgility) * agilityToAttackSpeed);
            movementSpeed = baseAttributes.BaseMovementSpeed + ((inventory.MeleeSlot.Agility + inventory.RangedSlot.Agility + statsUI.TotalAgility) * agilityToMoveSpeed);
            meleeDamage = baseAttributes.BaseDamage + inventory.MeleeSlot.Damage + (int)((inventory.MeleeSlot.Power + inventory.RangedSlot.Power + statsUI.TotalPower) * powerToDamage);
            rangedDamage = baseAttributes.BaseDamage + inventory.RangedSlot.Damage + (int)((inventory.MeleeSlot.Power + inventory.RangedSlot.Power + statsUI.TotalPower) * powerToDamage);
            cdReduction = baseAttributes.BaseCdReduction + ((inventory.MeleeSlot.Wisdom + inventory.RangedSlot.Wisdom + statsUI.TotalWisdom) * wisdomToCdReduction);
        }
        PlayerAttributesChanged?.Invoke();
    }

    public void BoostSkill()
    {
        if (ability.BoostChecker)
        {

            meleeAttackSpeed += ability.AttackSpeed;
            rangedAttackSpeed += ability.AttackSpeed;
            movementSpeed += ability.MovementSpeed;
            PlayerAttributesChanged?.Invoke();
            playerMovement.MoveSpeed = movementSpeed;
            weaponAnimator.SetFloat("MAttackSpeed", meleeAttackSpeed);
            weaponAnimator.SetFloat("RAttackSpeed", rangedAttackSpeed);
        }
        else
        {
            meleeAttackSpeed -= ability.AttackSpeed;
            rangedAttackSpeed -= ability.AttackSpeed;
            movementSpeed -= ability.MovementSpeed;
            PlayerAttributesChanged?.Invoke();
            playerMovement.MoveSpeed = movementSpeed;
            weaponAnimator.SetFloat("MAttackSpeed", meleeAttackSpeed);
            weaponAnimator.SetFloat("RAttackSpeed", rangedAttackSpeed);
        }
    }


}
