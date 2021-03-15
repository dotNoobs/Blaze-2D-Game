using TMPro;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] SpriteLibraryAsset spriteLibrary;
    [SerializeField] Image meleeSlot;
    [SerializeField] Image rangeSlot;
    [SerializeField] Image abilitySlot;
    [SerializeField] Image potionSlot;
    [SerializeField] TMP_Text abilityCooldownText;
    [SerializeField] TMP_Text potionCooldownText;

    Inventory playerInventory;
    AbilityHolder ability;
    int abilityCooldown;
    int potionCooldown;
    int timeAbility;
    int timePotion;

    void OnEnable()
    {
        playerInventory = GetComponent<Inventory>();
        playerInventory.OnInventoryChanged += UpdateIcons;
    }

    void OnDisable()
    {
        playerInventory.OnInventoryChanged -= UpdateIcons;
    }
    private void Start()
    {
        ability = GetComponent<AbilityHolder>();
    }
    private void Update()
    {
        timeAbility = (int)Time.time;
        timePotion = (int)Time.time;
        abilityCooldown = (int)(ability.nextUseTimeAbility - timeAbility);
        potionCooldown = (int)(ability.nextUseTimePotion - timePotion);
        if (abilityCooldown <= 0)
        {
            abilityCooldown = 0;
        }
        if (potionCooldown <= 0)
        {
            potionCooldown = 0;
        }
        CooldownTime();

    }

    void UpdateIcons()
    {
        if (playerInventory.MeleeSlot != null)
            meleeSlot.sprite = spriteLibrary.GetSprite(playerInventory.MeleeSlot.WeaponType.ToString(), playerInventory.MeleeSlot.SpriteIndex.ToString());
        if (playerInventory.RangedSlot != null)
            rangeSlot.sprite = spriteLibrary.GetSprite(playerInventory.RangedSlot.WeaponType.ToString(), playerInventory.RangedSlot.SpriteIndex.ToString());
        if (playerInventory.AbilitySlot != null)
            abilitySlot.sprite = spriteLibrary.GetSprite(playerInventory.AbilitySlot.UsableType.ToString(), playerInventory.AbilitySlot.SpriteIndex.ToString());
        if (playerInventory.PotionSlot != null)
            potionSlot.sprite = spriteLibrary.GetSprite(playerInventory.PotionSlot.UsableType.ToString(), playerInventory.PotionSlot.SpriteIndex.ToString());
    }
    void CooldownTime()
    {
        if (playerInventory.AbilitySlot != null)
        {
            abilityCooldownText.gameObject.SetActive(true);
            abilityCooldownText.text = $"{abilityCooldown}";
            timeAbility = 0;
        }
        if (playerInventory.PotionSlot != null)
        {
            potionCooldownText.gameObject.SetActive(true);
            potionCooldownText.text = $"{potionCooldown}";
            timePotion = 0;
        }
    }
}
