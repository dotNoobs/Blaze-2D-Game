using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Inventory playerInventory;
    [SerializeField] Animator weaponAnimator;
    [SerializeField] GameObject playerProjectilePrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform meleeHitPoint;
    [SerializeField] float meleeHitRadius = 0.2f;

    WeaponType currentMeleeType;
    WeaponType currentRangedType;

    void OnEnable()
    {
        playerInventory.OnInventoryChanged += UpdateWeapons;
    }

    void OnDisable()
    {
        playerInventory.OnInventoryChanged -= UpdateWeapons;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // 0 - Sword, 1 - Axe
            weaponAnimator.SetInteger("AttackType", (int)currentMeleeType);
        }
        else if (Input.GetMouseButton(1))
        {
            // 2 - Bow, 3 - Staff
            weaponAnimator.SetInteger("AttackType", (int)currentRangedType);
        }
        else
            weaponAnimator.SetInteger("AttackType", -1);
    }

    public void RangedAttack()
    {
        var instance = Instantiate(playerProjectilePrefab, shootPoint.position, shootPoint.rotation);
        instance.GetComponent<Projectile>().Damage = player.RangedDamage;

        if (currentRangedType == WeaponType.Bow)
            AudioManager.Instance.Play(SoundEffectType.bowAttack);
        else if (currentRangedType == WeaponType.Staff)
            AudioManager.Instance.Play(SoundEffectType.magicAttack);
    }

    public void MeleeAttack()
    {
        AudioManager.Instance.Play(SoundEffectType.attackMelee);

        Collider2D[] hits = Physics2D.OverlapCircleAll(meleeHitPoint.position, meleeHitRadius);

        if (hits != null)
        {
            foreach (var hit in hits)
            {
                UnitCombat enemy = hit.transform.gameObject.GetComponentInChildren<UnitCombat>();

                if (enemy != null)
                    enemy.TakeDamage(player.MeleeDamage);
            }
        }
    }

    void UpdateWeapons()
    {
        currentMeleeType = playerInventory.MeleeSlot.WeaponType;
        currentRangedType = playerInventory.RangedSlot.WeaponType;
    }
}