using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] FloatingWeapons floatingWeapons;

    PlayerAttributes attributes;
    AbilityHolder ability;

    public int HP
    {
        get { return hp;}
        set { hp = value;}
    }
    public float MeleeAttackSpeed => meleeAttackSpeed;
    public float RangedAttackSpeed => rangedAttackSpeed;
    public float MovementSpeed => movementSpeed;
    public float CdReduction => cdReduction;
    public int MeleeDamage => meleeDamage;
    public int RangedDamage => rangedDamage;
    public bool CanTakeDamage
    {
        get { return canTakeDamage; }
        set { canTakeDamage = value; }
    }
    public int CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }
    [SerializeField] Bars healthBar;
    int currentHP = int.MaxValue;
    [SerializeField] int hp;
    [SerializeField] float meleeAttackSpeed;
    [SerializeField] float rangedAttackSpeed;
    [SerializeField] float movementSpeed;
    [SerializeField] float cdReduction;
    [SerializeField] int meleeDamage;
    [SerializeField] int rangedDamage;

    bool canTakeDamage = true;
    GameObject portal = null;

    void OnEnable()
    {
        attributes = GetComponent<PlayerAttributes>();
        ability = GetComponent<AbilityHolder>();
        attributes.PlayerAttributesChanged += RefreshStats;
    }

    void OnDisable()
    {
        attributes.PlayerAttributesChanged -= RefreshStats;
    }

    //This should not really be here
    void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.GetComponent<Projectile>();

        if (collision.gameObject.CompareTag("Portal"))
            portal = collision.gameObject;

        if (projectile != null && projectile.canDamagePlayer == true)
        {
            TakeDamage(projectile.Damage);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
            portal = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            if (portal != null)
            {
                Managers.progress.StageLevel += 1;
                Managers.progress.UpdateMobMultipler();
                Managers.MapGenerator.tileBaseSetType = (BaseTileSetType)Managers.progress.SetTileTypeForLevel();
                Managers.MapGenerator.GenerateMap();
                Managers.progress.StopProgress = false;
                Destroy(portal);
            }
    }

    public void RefreshStats()
    {
        if (currentHP > hp)
        {
            hp = attributes.MaxHP;
            currentHP = hp;
            healthBar.SetMaxHealth(hp);
            meleeAttackSpeed = attributes.MeleeAttackSpeed;
            rangedAttackSpeed = attributes.RangedAttackSpeed;
            movementSpeed = attributes.MovementSpeed;
            cdReduction = attributes.CdReduction;
            meleeDamage = attributes.MeleeDamage;
            rangedDamage = attributes.RangedDamage;
        }
        else
        {
            hp = attributes.MaxHP;
            meleeAttackSpeed = attributes.MeleeAttackSpeed;
            rangedAttackSpeed = attributes.RangedAttackSpeed;
            movementSpeed = attributes.MovementSpeed;
            cdReduction = attributes.CdReduction;
            meleeDamage = attributes.MeleeDamage;
            rangedDamage = attributes.RangedDamage;

        }

        floatingWeapons.UpdateWeaponsStats(meleeAttackSpeed, rangedAttackSpeed);
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage)
        {
            currentHP -= damage;
            healthBar.SetHealth(currentHP);
            Debug.Log($"Player took {damage} damage");
            if (currentHP <= 0)
            {
                AudioManager.Instance.Play(SoundEffectType.playerDead);
                Managers.progress.MoveToGameOverScene("lose");
            }
        }
    }
}
