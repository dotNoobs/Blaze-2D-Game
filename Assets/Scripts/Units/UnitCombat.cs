using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public enum UnitState
{
    Idle,
    Run,
    Attack,
    Hit,
    Dead
}

public class UnitCombat : MonoBehaviour
{
    [SerializeField] GameObject unitProjectilePrefab;
    [SerializeField] GameObject portalPrefab;
    [SerializeField] EnemyHealthBar healthBar;
    [SerializeField] BossHealthBar bossHealthBar;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform parentTransform;
    [SerializeField] Transform attackRotation;
    [SerializeField] Transform hitPoint;
    [SerializeField] float sightRange;
    [SerializeField] float attackRadius = 0.2f;
    [SerializeField] List<SpriteLibraryAsset> projectileLibraryAssets;
    [SerializeField] int shoots = 1;

    Transform playerTransform;
    AbilityHolder playerAbility;
    UnitHolder unit;
    Animator animator;
    public UnitState currentState;
    int projectileSpriteIndex;
    float distanceToPlayer;
    float attackRange;
    int currentHP;
    int damage;
    float moveSpeed;

    void Start()
    {
        unit = GetComponent<UnitHolder>();
        animator = GetComponent<Animator>();
        playerTransform = FindObjectOfType<Player>().transform;
        playerAbility = FindObjectOfType<AbilityHolder>();
        currentHP = unit.Unit.HP;
        damage = unit.Unit.Damage;
        moveSpeed = unit.Unit.MoveSpeed;
        animator.SetFloat("AttackSpeed", unit.Unit.AttackSpeed);

        if (unit.Unit.UnitType == UnitType.Boss)
            bossHealthBar.SetBossMaxHealth(currentHP);
        else
            healthBar.SetMaxHealth(currentHP);

        attackRotation.localPosition = -transform.localPosition;
        GetComponent<Collider2D>().offset = new Vector2(-transform.localPosition.x, -transform.localPosition.y);
        attackRange = hitPoint.localPosition.x;
        hitPoint.localScale = Vector2.one * attackRadius * 2;

        if (unit.Unit.UnitType == UnitType.Ranged)
            projectileSpriteIndex = Random.Range(0, projectileLibraryAssets.Count);

        //Set initial state to idle
        SetUnitState(UnitState.Idle);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.GetComponent<Projectile>();

        if (projectile != null && projectile.canDamageMonsters == true)
        {
            TakeDamage(projectile.Damage);
            projectile.DestroyProjectile(SoundEffectType.projectileDestroy);
        }
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(parentTransform.position, playerTransform.position);

        if (currentState != UnitState.Dead)
        {
            //If player is not in a sight range
            if (distanceToPlayer > sightRange)
                //Set state to idle
                SetUnitState(UnitState.Idle);
            //If player is in a sight range && is not in attack range 
            else if (distanceToPlayer <= sightRange && distanceToPlayer > attackRange)
            {
                //Set state to run (chase)
                SetUnitState(UnitState.Run);
            }
            //If player is in attack range
            else if (distanceToPlayer <= attackRange)
            {
                //Set state to attack
                SetUnitState(UnitState.Attack);
            }

            if (playerAbility.Invisible)
            {
                SetUnitState(UnitState.Idle);
            }

            //If unit animator is not playing an attack animation
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(UnitState.Attack.ToString()))
            {
                if (currentState == UnitState.Run)
                {
                    //Chase the player
                    //parentTransform.position = Vector3.MoveTowards(parentTransform.position, playerTransform.position, moveSpeed * Time.deltaTime);
                    rb.MovePosition(Vector3.MoveTowards(parentTransform.position, playerTransform.position, moveSpeed * Time.deltaTime));
                }

                //If unit is on the right side of the player
                if (parentTransform.position.x >= playerTransform.position.x)
                {
                    //Change its scale on X to face left
                    parentTransform.localScale = new Vector3(-1, 1, 1);
                    attackRotation.localScale = new Vector3(-1, 1, 1);
                }
                //If unit is on the left side of the player
                else if (parentTransform.position.x < playerTransform.position.x)
                {
                    //Change its scale on X to face right
                    parentTransform.localScale = Vector3.one;
                    attackRotation.localScale = Vector3.one;
                }

                if (unit.Unit.UnitType == UnitType.Melee)
                {
                    Vector3 parentToPlayerDir = (playerTransform.position - parentTransform.position).normalized;
                    float angle = Mathf.Atan2(parentToPlayerDir.y, parentToPlayerDir.x) * Mathf.Rad2Deg;

                    attackRotation.rotation = Quaternion.Euler(0, 0, angle);
                }
            }

            if (unit.Unit.UnitType == UnitType.Ranged || unit.Unit.UnitType == UnitType.Boss)
            {
                Vector3 parentToPlayerDir = (playerTransform.position - parentTransform.position).normalized;
                float angle = Mathf.Atan2(parentToPlayerDir.y, parentToPlayerDir.x) * Mathf.Rad2Deg;

                attackRotation.rotation = Quaternion.Euler(0, 0, angle);
            }

            if (unit.Unit.UnitType == UnitType.Boss && distanceToPlayer <= 12f)
                bossHealthBar.gameObject.SetActive(true);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (unit.Unit.UnitType == UnitType.Boss)
            bossHealthBar.SetBossHealth(currentHP);
        else
            healthBar.SetHealth(currentHP, unit.Unit.HP);

        if (currentHP <= 0)
        {
            SetUnitState(UnitState.Dead);
        }
        else
        {
            //UpdateUnitState(UnitState.Hit);
            AudioManager.Instance.Play(SoundEffectType.pain);

            Debug.Log($"{gameObject.name} took {damage:n2} damage");

            animator.SetTrigger(UnitState.Hit.ToString());
        }
    }

    void SetUnitState(UnitState newState)
    {
        currentState = newState;
        animator.SetInteger("UnitState", (int)newState);
    }

    //This method is invoked in the Attack Animation
    public void RangedAttack()
    {
        Vector3 attackToPlayerDir = (playerTransform.position - attackRotation.position).normalized;
        var instance = Instantiate(unitProjectilePrefab, attackRotation.position + attackToPlayerDir / 3, attackRotation.rotation);
        instance.GetComponent<Projectile>().Damage = damage;
        SpriteLibrary library = instance.GetComponentInChildren<SpriteLibrary>();
        library.spriteLibraryAsset = projectileLibraryAssets[projectileSpriteIndex];
        library.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    //This method is invoked in the Attack Animation
    public void SpreadRangedAttack()
    {
        for (int i = 1; i <= shoots; i++)
        {
            var instance = Instantiate(unitProjectilePrefab, attackRotation.position, Quaternion.Euler(attackRotation.eulerAngles - new Vector3(0, 0, 90f) + new Vector3(0, 0, 18f) * i));
            instance.GetComponent<Projectile>().Damage = damage;
            SpriteLibrary library = instance.GetComponentInChildren<SpriteLibrary>();
            library.spriteLibraryAsset = projectileLibraryAssets[projectileSpriteIndex];
            library.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    //This method is invoked in the Attack Animation
    public void MeleeAttack()
    {
        AudioManager.Instance.Play(SoundEffectType.attackMelee);

        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPoint.position, attackRadius);

        if (hits != null)
        {
            foreach (var hit in hits)
            {
                Player player = hit.transform.gameObject.GetComponent<Player>();

                if (player != null)
                    player.TakeDamage(damage);
            }
        }
    }

    //This method is invoked in the Death Animation
    public void Die()
    {
        //Play death sound here
        AudioManager.Instance.Play(SoundEffectType.deathMeleeUnit);

        //Give exp to the player before dying
        Managers.PlayerXP.IncreaseLevel(unit.Unit.XP);

        //Spawn random item before dying
        Managers.ItemGenerator.SpawnItem(parentTransform.position);

        //Spawn portal if this unit was a boss
        if (unit.Unit.UnitType == UnitType.Boss)
        {
            bossHealthBar.gameObject.SetActive(false);

            Managers.progress.stageLevelCounter += 1;
            Instantiate(portalPrefab, Managers.MapGenerator.BossSpawnPosition, Quaternion.identity);
            if (Managers.progress.stageLevelCounter == 11)
            {
                Managers.progress.MoveToGameOverScene("win");
            }
        }

        Debug.Log($"{gameObject.name} died");

        //Destroy this game object
        Destroy(parentTransform.gameObject);
    }
}
