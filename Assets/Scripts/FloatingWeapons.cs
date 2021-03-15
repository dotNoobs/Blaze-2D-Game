using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class FloatingWeapons : MonoBehaviour
{
    [SerializeField] GameObject playerProjectilePrefab;
    [SerializeField] BoxCollider2D playerCollider;
    [SerializeField] Inventory playerInventory;
    [SerializeField] Transform bowHodler;
    [SerializeField] Transform rangedBack;
    [SerializeField] Transform slash;
    [SerializeField] Animator weaponAnimator;
    [SerializeField] PlayerAttributes playerAttributes;

    void OnEnable()
    {
        playerInventory.OnInventoryChanged += UpdateWeapon;
    }

    void OnDisable()
    {
        playerInventory.OnInventoryChanged -= UpdateWeapon;
    }

    void UpdateWeapon()
    {
        SpriteResolver bullet = playerProjectilePrefab.GetComponentInChildren<SpriteResolver>();

        if (playerInventory.RangedSlot.WeaponType == WeaponType.Bow)
        {
            bullet.transform.rotation = Quaternion.Euler(0, 0, -135);
            rangedBack.rotation = Quaternion.Euler(Vector3.zero);
        }
        else if (playerInventory.RangedSlot.WeaponType == WeaponType.Staff)
        {
            bullet.transform.rotation = Quaternion.Euler(Vector3.zero);
            rangedBack.rotation = Quaternion.Euler(0, 0, 45);
        }
    }

    public void UpdateWeaponsStats(float mAttackSpeed, float rAttackSpeed)
    {
        weaponAnimator.SetFloat("MAttackSpeed", mAttackSpeed);
        weaponAnimator.SetFloat("RAttackSpeed", rAttackSpeed);
    }

    void Update()
    {
        Vector3 playerColliderCenter = playerCollider.bounds.center;
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 cursorToPlayerDir = (cursorPos - playerColliderCenter).normalized;
        float angle = Mathf.Atan2(cursorToPlayerDir.y, cursorToPlayerDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (Vector2.Dot(playerInventory.gameObject.transform.right, cursorToPlayerDir) < 0)
        {
            slash.localScale = new Vector3(1, -1, 1);
            bowHodler.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            slash.localScale = Vector3.one;
            bowHodler.localScale = new Vector3(1, 1, 1);
        }
    }
}
