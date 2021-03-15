using UnityEngine;
using UnityEngine.Tilemaps;

public class AbilityHolder : MonoBehaviour
{
    [SerializeField] GameObject shieldParticle;
    [SerializeField] TrailRenderer boostTrail;
    [SerializeField] ParticleSystem teleportParticle;
    [SerializeField] Tilemap floorMap;

    SpriteRenderer playerSprite;
    PlayerAttributes playerAttributes;
    PlayerMovement playerMovement;
    Player player;
    Inventory inventory;
    [SerializeField] Bars healthBar;
    Color col;

    public int MovementSpeed => movementSpeed;
    public int AttackSpeed => attackSpeed;
    public bool ActivateShield => shield;
    public bool BoostChecker => boost;
    public bool Invisible => invisible;

    bool invisible = false;
    bool shield = false;
    bool boost = false;
    public bool teleport = false;
    public float duration = 0;
    int attackSpeed = 3;
    int movementSpeed = 7;
    float teleportDistance = 2.5f;
    public float nextUseTimeAbility = 0;
    public float nextUseTimePotion = 0;
    public bool skillInUse = false;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttributes = GetComponent<PlayerAttributes>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        duration += Time.deltaTime;
        if (invisible && duration >= 3)
        {
            invisible = false;
            skillInUse = false;
            col.a = 1f;
            playerSprite.color = col;

        }
        if (shield && duration >= 4)
        {
            shield = false;
            skillInUse = false;
            player.CanTakeDamage = true;
            shieldParticle.gameObject.SetActive(false);
        }
        if (boost && duration >= 4)
        {
            boost = false;
            skillInUse = false;
            boostTrail.gameObject.SetActive(false);
            col.r = 255;
            col.g = 255;
            col.b = 255;
            playerSprite.color = col;
            playerAttributes.BoostSkill();
        }
    }

    public void UseSkill(string name)
    {
        switch (name)
        {
            case "Stealth":
                Stealth();
                break;
            case "Shield":
                Shield();
                break;
            case "Teleport":
                UseTeleport();
                break;
            case "Boost":
                Boost();
                break;
            case "HealUp":
                HealUp();
                break;
            default:
                Debug.Log("wrong name skill");
                break;
        }
    }

    void Stealth()
    {
        invisible = true;
        skillInUse = true;
        AudioManager.Instance.Play(SoundEffectType.stealthSkill);
        col.a = 0.2f;
        col.r = 255;
        col.g = 255;
        col.b = 255;
        duration = 0;
        playerSprite.color = col;


    }

    public void Shield()
    {
        shield = true;
        skillInUse = true;
        player.CanTakeDamage = false;
        AudioManager.Instance.Play(SoundEffectType.shieldSkill);
        shieldParticle.gameObject.SetActive(true);
        duration = 0;
    }

    void UseTeleport()
    {
        Vector2 playerPos = player.gameObject.transform.position;
        Vector2 cursonrPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerToCursorDir = cursonrPos - playerPos;
        Vector2 teleportLocation = playerPos + playerToCursorDir.normalized * teleportDistance;
        Vector3Int gridteleportLocation = floorMap.WorldToCell(new Vector3(teleportLocation.x, teleportLocation.y, 0));

        if (floorMap.GetTile(gridteleportLocation) != null)
        {
            player.transform.position = new Vector3(teleportLocation.x, teleportLocation.y, 0);


            AudioManager.Instance.Play(SoundEffectType.teleportSkill);
            teleportParticle.gameObject.transform.position = playerMovement.PlayerRigidbody.position;
            teleportParticle.Play();
        }
    }

    void Boost()
    {
        boost = true;
        skillInUse = true;
        AudioManager.Instance.Play(SoundEffectType.boostSkill);
        boostTrail.gameObject.SetActive(true);
        col.r = 255;
        col.g = 0;
        col.b = 0;
        col.a = 1;
        playerSprite.color = col;
        duration = 0;
        playerAttributes.BoostSkill();
    }
    void HealUp()
    {
        AudioManager.Instance.Play(SoundEffectType.potion);

        if (inventory.PotionSlot.Ability.ToString() == "Small")
            player.CurrentHP += (int)(player.CurrentHP * 0.05f);
        else if (inventory.PotionSlot.Ability.ToString() == "Medium")
            player.CurrentHP += (int)(player.CurrentHP * 0.10f);
        else if (inventory.PotionSlot.Ability.ToString() == "Big")
            player.CurrentHP += (int)(player.CurrentHP * 0.15f);
        else if (inventory.PotionSlot.Ability.ToString() == "Giant")
            player.CurrentHP += (int)(player.CurrentHP * 0.2f);

        healthBar.SetHealth(player.CurrentHP);

    }

}
