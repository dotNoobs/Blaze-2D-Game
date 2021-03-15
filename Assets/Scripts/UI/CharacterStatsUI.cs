using TMPro;
using UnityEngine;

public class CharacterStatsUI : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Inventory inventory;
    [SerializeField] TMP_Text powerText;
    [SerializeField] TMP_Text agilityText;
    [SerializeField] TMP_Text wisdomText;
    [SerializeField] TMP_Text meleeDMGText;
    [SerializeField] TMP_Text rangedDMGText;
    [SerializeField] TMP_Text meleeAttackSpeedText;
    [SerializeField] TMP_Text rangedAttackSpeedText;
    [SerializeField] TMP_Text movementSpeedText;
    [SerializeField] TMP_Text cooldoownReductionText;
    [SerializeField] TMP_Text leftPointsText;
    [SerializeField] BaseAttributes baseAttributes;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text maxPlayerHPText;
    [SerializeField] TMP_Text currentPlayerHPText;

    int power;
    int agility;
    int wisdom;
    int totalPower;
    int totalAgility;
    int totalWisdom;
    int leftPoints;
    int tempPower;
    int tempAgility;
    int tempWisdom;
    bool applyChanges = false;
    public TMP_Text PowerText => powerText;
    public int TotalPower => totalPower;
    public int TotalAgility => totalAgility;
    public int TotalWisdom => totalWisdom;
    public int LeftPoints
    {
        get { return leftPoints; }
        set { leftPoints = value; }
    }
    private void Start()
    {
        if (inventory.AbilitySlot != null)
        {
            powerText.text = $"{inventory.MeleeSlot.Power + inventory.RangedSlot.Power + inventory.AbilitySlot.Power + baseAttributes.BasePower}";
            agilityText.text = $"{inventory.MeleeSlot.Agility + inventory.RangedSlot.Agility + inventory.AbilitySlot.Agility + baseAttributes.BaseAgility}";
            wisdomText.text = $"{inventory.MeleeSlot.Wisdom + inventory.RangedSlot.Wisdom + inventory.AbilitySlot.Wisdom + baseAttributes.BaseWisdom}";
            leftPointsText.text = $"Points left:{leftPoints}";
            movementSpeedText.text = $"{player.MovementSpeed:n2}";
            meleeAttackSpeedText.text = $"{player.MeleeAttackSpeed:n2}";
            rangedAttackSpeedText.text = $"{player.RangedAttackSpeed:n2}";
            meleeDMGText.text = $"{player.MeleeDamage}";
            rangedDMGText.text = $"{player.RangedDamage}";
            maxPlayerHPText.text = $"{player.HP}";
            currentPlayerHPText.text = $"{player.CurrentHP}";
        }
        else
        {
            powerText.text = $"{inventory.MeleeSlot.Power + inventory.RangedSlot.Power + baseAttributes.BasePower}";
            agilityText.text = $"{inventory.MeleeSlot.Agility + inventory.RangedSlot.Agility + baseAttributes.BaseAgility}";
            wisdomText.text = $"{inventory.MeleeSlot.Wisdom + inventory.RangedSlot.Wisdom + baseAttributes.BaseWisdom}";
            leftPointsText.text = $"Points left:{leftPoints}";
            movementSpeedText.text = $"{player.MovementSpeed:n2}";
            meleeAttackSpeedText.text = $"{player.MeleeAttackSpeed:n2}";
            rangedAttackSpeedText.text = $"{player.RangedAttackSpeed:n2}";
            meleeDMGText.text = $"{player.MeleeDamage}";
            rangedDMGText.text = $"{player.RangedDamage}";
            maxPlayerHPText.text = $"{player.HP}";
            currentPlayerHPText.text = $"{player.CurrentHP}";
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            powerText.text = $"{inventory.MeleeSlot.Power + inventory.RangedSlot.Power + inventory.AbilitySlot.Power + baseAttributes.BasePower}";
            agilityText.text = $"{inventory.MeleeSlot.Agility + inventory.RangedSlot.Agility + inventory.AbilitySlot.Agility + baseAttributes.BaseAgility}";
            wisdomText.text = $"{inventory.MeleeSlot.Wisdom + inventory.RangedSlot.Wisdom + inventory.AbilitySlot.Wisdom + baseAttributes.BaseWisdom}";
            leftPointsText.text = $"Points left:{leftPoints}";
            meleeDMGText.text = $"{player.MeleeDamage}";
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log($"Total power: {totalPower}:n2");
        }
    }
    public void addButon(string nameOfAtt)
    {
        applyChanges = false;
        if (nameOfAtt == "power" && leftPoints > 0)
        {
            power++;
            tempPower = power;
            leftPoints--;
            powerText.text = $"{totalPower + power + inventory.MeleeSlot.Power + inventory.RangedSlot.Power + inventory.AbilitySlot.Power + baseAttributes.BasePower}";
            leftPointsText.text = $"Points left:{leftPoints}";
        }
        if (nameOfAtt == "agility" && leftPoints > 0)
        {
            agility++;
            leftPoints--;
            agilityText.text = $"{totalAgility + agility + inventory.MeleeSlot.Agility + inventory.RangedSlot.Agility + inventory.AbilitySlot.Agility + baseAttributes.BaseAgility}";
            leftPointsText.text = $"Points left:{leftPoints}";
        }
        if (nameOfAtt == "wisdom" && leftPoints > 0)
        {
            wisdom++;
            leftPoints--;
            wisdomText.text = $"{totalWisdom + wisdom + inventory.MeleeSlot.Wisdom + inventory.RangedSlot.Wisdom + inventory.AbilitySlot.Wisdom + baseAttributes.BaseWisdom}";
            leftPointsText.text = $"Points left:{leftPoints}";
        }
    }
    public void subButton(string nameOfAtt)
    {
        applyChanges = false;
        if (nameOfAtt == "power" && leftPoints <= 5 && (power + inventory.MeleeSlot.Power + inventory.RangedSlot.Power + inventory.AbilitySlot.Power + baseAttributes.BasePower) > (inventory.MeleeSlot.Agility + inventory.RangedSlot.Power + inventory.AbilitySlot.Power + baseAttributes.BasePower))
        {
            power--;
            leftPoints++;
            powerText.text = $"{totalPower + power + inventory.MeleeSlot.Power + inventory.RangedSlot.Power + inventory.AbilitySlot.Power + baseAttributes.BasePower}";
            leftPointsText.text = $"Points left:{leftPoints}";
        }
        if (nameOfAtt == "agility" && leftPoints <= 5 && (agility + inventory.MeleeSlot.Agility + inventory.RangedSlot.Agility + inventory.AbilitySlot.Agility + baseAttributes.BaseAgility) > (inventory.MeleeSlot.Agility + baseAttributes.BaseAgility + inventory.RangedSlot.Agility + inventory.AbilitySlot.Agility))
        {
            agility--;
            leftPoints++;
            agilityText.text = $"{totalAgility + agility + inventory.MeleeSlot.Agility + inventory.RangedSlot.Agility + inventory.AbilitySlot.Agility + baseAttributes.BaseAgility}";
            leftPointsText.text = $"Points left:{leftPoints}";
        }
        if (nameOfAtt == "wisdom" && leftPoints <= 5 && (wisdom + inventory.MeleeSlot.Wisdom + inventory.RangedSlot.Wisdom + inventory.AbilitySlot.Wisdom + baseAttributes.BaseWisdom) > (inventory.MeleeSlot.Wisdom + baseAttributes.BaseWisdom + inventory.RangedSlot.Wisdom + inventory.AbilitySlot.Wisdom))
        {
            wisdom--;
            leftPoints++;
            wisdomText.text = $"{totalWisdom + wisdom + inventory.MeleeSlot.Wisdom + inventory.RangedSlot.Wisdom + inventory.AbilitySlot.Wisdom + baseAttributes.BaseWisdom}";
            leftPointsText.text = $"Points left:{leftPoints}";
        }
    }
    public void applyButton()
    {
        totalPower += power;
        totalAgility += agility;
        totalWisdom += wisdom;
        applyChanges = true;
        tempPower = power;
        tempAgility = agility;
        tempWisdom = wisdom;
        power = 0;
        agility = 0;
        wisdom = 0;
        TextKeeper();
        inventory.OnInventoryChanged?.Invoke();
    }
    void CheckIfSaved()
    {
        if (applyChanges == false)
        {
            power = 0;
            agility = 0;
            wisdom = 0;
        }
    }
    public void RefreshTexts()
    {
        CheckIfSaved();
        TextKeeper();
    }
    void TextKeeper()
    {
        if (inventory.AbilitySlot != null)
        {

            powerText.text = $"{totalPower + inventory.MeleeSlot.Power + inventory.RangedSlot.Power + inventory.AbilitySlot.Power + baseAttributes.BasePower}";
            agilityText.text = $"{totalAgility + inventory.MeleeSlot.Agility + inventory.RangedSlot.Agility + inventory.AbilitySlot.Agility + baseAttributes.BaseAgility}";
            wisdomText.text = $"{totalWisdom + inventory.MeleeSlot.Wisdom + inventory.RangedSlot.Wisdom + inventory.AbilitySlot.Wisdom + baseAttributes.BaseWisdom}";
            meleeDMGText.text = $"{player.MeleeDamage}";
            rangedDMGText.text = $"{player.RangedDamage}";
            meleeAttackSpeedText.text = $"{player.MeleeAttackSpeed:n2}";
            rangedAttackSpeedText.text = $"{player.RangedAttackSpeed:n2}";
            movementSpeedText.text = $"{player.MovementSpeed:n2}";
            cooldoownReductionText.text = $"{player.CdReduction:n2}";
            levelText.text = $"{Managers.PlayerXP.CurrentLevel}";
            leftPointsText.text = $"Points left:{leftPoints}";
            maxPlayerHPText.text = $"{player.HP}";
            currentPlayerHPText.text = $"{player.CurrentHP}";
        }
        else
        {
            powerText.text = $"{totalPower + inventory.MeleeSlot.Power + inventory.RangedSlot.Power + baseAttributes.BasePower}";
            agilityText.text = $"{totalAgility + inventory.MeleeSlot.Agility + inventory.RangedSlot.Agility + baseAttributes.BaseAgility}";
            wisdomText.text = $"{totalWisdom + inventory.MeleeSlot.Wisdom + inventory.RangedSlot.Wisdom + baseAttributes.BaseWisdom}";
            meleeDMGText.text = $"{player.MeleeDamage}";
            rangedDMGText.text = $"{player.RangedDamage}";
            meleeAttackSpeedText.text = $"{player.MeleeAttackSpeed:n2}";
            rangedAttackSpeedText.text = $"{player.RangedAttackSpeed:n2}";
            movementSpeedText.text = $"{player.MovementSpeed:n2}";
            cooldoownReductionText.text = $"{player.CdReduction:n2}";
            levelText.text = $"{Managers.PlayerXP.CurrentLevel}";
            leftPointsText.text = $"Points left:{leftPoints}";
            maxPlayerHPText.text = $"{player.HP}";
            currentPlayerHPText.text = $"{player.CurrentHP}";
        }

    }
}
