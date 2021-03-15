using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour, IGameManager
{
    public bool easyDifficulty = false;
    public bool mediumDifficulty = false;
    public bool hardDifficulty = false;
    bool stopProgress = false;
    public bool playerWin;
    int maxRatio = 3;
    int stageLevel = 1;
    public int stageLevelCounter = 1;

    [SerializeField] float xpMultipler = 1.2f;
    [SerializeField] float baseDamage = 1.2f;
    [SerializeField] float baseHP = 1.2f;
    [SerializeField] float baseItemMultipler = 1;
    //[SerializeField] float baseItemDamage = 1.2f;
    [SerializeField] float baseItemAS = 1;
    [SerializeField] float finalDamageMobMultipler;
    [SerializeField] float finalHpMobMultipler;
    [SerializeField] float finalItemMultipler;
    //[SerializeField] float finalItemDamage;
    [SerializeField] float finalItemAsMultipler = 0;
    [SerializeField] float finalMobAsMultipler = 0;
    [SerializeField] float finalMobMovementMultipler = 0;
    [SerializeField] float damageMultipler = 0.015f;
    [SerializeField] float hpMultipler = 0.015f;
    [SerializeField] float itemMultipler = 0.05f;
    [SerializeField] float itemAsMultipler = 0.0005f;

    DifficultyManager difficultyManager;
    public float XpMultipler => xpMultipler;
    public float FinalDamageMobMultipler => finalDamageMobMultipler;
    public float FinalHpMobMultipler => finalHpMobMultipler;
    public float FinalItemMultipler => finalItemMultipler;
    public float FinalItemAsMultipler => finalItemAsMultipler;
    public float FinalMobAsMultipler => finalMobAsMultipler;
    public float FinalMobMovementMultipler => finalMobMovementMultipler;
    public int StageLevel
    {
        get { return stageLevel; }
        set { stageLevel = value; }
    }
    public bool StopProgress
    {
        get { return stopProgress; }
        set { stopProgress = value; }
    }
    public ManagerStatus Status { get; private set; }
    public void Startup()
    {
        Debug.Log("Progress manager starting...");
        Status = ManagerStatus.Started;
    }

    private void Awake()
    {
        difficultyManager = FindObjectOfType<DifficultyManager>();
        easyDifficulty = difficultyManager.easyDifficulty;
        mediumDifficulty = difficultyManager.mediumDifficulty;
        hardDifficulty = difficultyManager.hardDifficulty;
        SetValuesForMultipliers();
        finalDamageMobMultipler = baseDamage;
        finalHpMobMultipler = baseHP;
        finalItemMultipler = baseItemMultipler;
        finalItemAsMultipler = baseItemAS;
    }

    void Start()
    {
        Managers.MapGenerator.tileBaseSetType = (BaseTileSetType)SetTileTypeForLevel();
        Managers.MapGenerator.GenerateMap();
    }

    void Update()
    {
        if (stopProgress != true)
        {
            finalDamageMobMultipler = baseDamage + (damageMultipler * Time.time / 5);
            finalHpMobMultipler = baseHP + (hpMultipler * Time.time / 5);
            finalItemMultipler = baseItemMultipler + (itemMultipler * Time.time / 5);
            finalItemAsMultipler = baseItemAS + (itemAsMultipler * Time.time / 5);
            MultiplerLimiter();
        }
    }
    void SetValuesForMultipliers()
    {
        if (easyDifficulty)
        {
            xpMultipler = 1.2f;
            baseDamage = 1;
            baseHP = 1;
            baseItemAS = 1;
            baseItemMultipler = 1;
            damageMultipler = 0.010f;
            hpMultipler = 0.010f;
            itemMultipler = 0.05f;
            itemAsMultipler = 0.0005f;
            stopProgress = false;
        }
        else if (mediumDifficulty)
        {
            xpMultipler = 1.2f;
            baseDamage = 1.2f;
            baseHP = 1.2f;
            baseItemAS = 1;
            baseItemMultipler = 1;
            damageMultipler = 0.015f;
            hpMultipler = 0.015f;
            itemMultipler = 0.05f;
            itemAsMultipler = 0.0005f;
            stopProgress = false;
        }
        else if (hardDifficulty)
        {
            xpMultipler = 1.2f;
            baseDamage = 1.5f;
            baseHP = 1.5f;
            baseItemAS = 1;
            baseItemMultipler = 1;
            damageMultipler = 0.025f;
            hpMultipler = 0.025f;
            itemMultipler = 0.05f;
            itemAsMultipler = 0.0005f;
            stopProgress = false;
        }
    }
    public int SetTileTypeForLevel()
    {
        if (stageLevelCounter >= 1 && stageLevelCounter <= 3)
        {
            AudioManager.Instance.Play(SoundEffectType.battleMusic1);
            return 0;
        }
        else if (stageLevelCounter >= 4 && stageLevelCounter <= 6)
        {
            AudioManager.Instance.Stop(SoundEffectType.battleMusic1);
            AudioManager.Instance.Play(SoundEffectType.battleMusic3);
            return 1;
        }
        else if (stageLevelCounter >= 7 && stageLevelCounter <= 9)
        {
            AudioManager.Instance.Stop(SoundEffectType.battleMusic3);
            AudioManager.Instance.Play(SoundEffectType.battleMusic4);
            return 2;
        }
        else if (stageLevelCounter == 10)
        {
            AudioManager.Instance.Stop(SoundEffectType.battleMusic4);
            AudioManager.Instance.Play(SoundEffectType.battleMusic2);
            return 3;
        }
        return 0;
    }

    public void UpdateMobMultipler()
    {
        finalMobAsMultipler = finalDamageMobMultipler / 10;
        finalMobMovementMultipler = finalDamageMobMultipler / 20;
    }
    void MultiplerLimiter()
    {
        if (stageLevel == 1)
        {
            if (finalDamageMobMultipler > maxRatio * baseDamage || finalItemMultipler > maxRatio * baseItemMultipler)
            {
                finalDamageMobMultipler = maxRatio * baseDamage;
                finalHpMobMultipler = maxRatio * baseHP;
                finalItemMultipler = maxRatio * baseItemMultipler;
                stopProgress = true;
            }
        }
        else if (stageLevel == 2)
        {
            if (finalDamageMobMultipler > (maxRatio * stageLevel) * baseDamage || finalItemMultipler > (maxRatio * stageLevel) * baseItemMultipler)
            {

                finalDamageMobMultipler = (maxRatio * stageLevel) * baseDamage;
                finalHpMobMultipler = (maxRatio * stageLevel) * baseHP;
                finalItemMultipler = (maxRatio * stageLevel) * baseItemMultipler;
                stopProgress = true;
            }
        }
        else if (stageLevel == 3)
        {
            if (finalDamageMobMultipler > (maxRatio * stageLevel) * baseDamage || finalItemMultipler > (maxRatio * stageLevel) * baseItemMultipler)
            {

                finalDamageMobMultipler = (maxRatio * stageLevel) * baseDamage;
                finalHpMobMultipler = (maxRatio * stageLevel) * baseHP;
                finalItemMultipler = (maxRatio * stageLevel) * baseItemMultipler;
                stopProgress = true;
            }
        }
        else if (stageLevel == 4)
        {
            if (finalDamageMobMultipler > (maxRatio * stageLevel) * baseDamage || finalItemMultipler > (maxRatio * stageLevel) * baseItemMultipler)
            {

                finalDamageMobMultipler = (maxRatio * stageLevel) * baseDamage;
                finalHpMobMultipler = (maxRatio * stageLevel) * baseHP;
                finalItemMultipler = (maxRatio * stageLevel) * baseItemMultipler;
                stopProgress = true;
            }
        }
        else if (stageLevel == 5)
        {
            if (finalDamageMobMultipler > (maxRatio * stageLevel) * baseDamage || finalItemMultipler > (maxRatio * stageLevel) * baseItemMultipler)
            {

                finalDamageMobMultipler = (maxRatio * stageLevel) * baseDamage;
                finalHpMobMultipler = (maxRatio * stageLevel) * baseHP;
                finalItemMultipler = (maxRatio * stageLevel) * baseItemMultipler;
                stopProgress = true;
            }

        }
        else if (stageLevel == 6)
        {
            if (finalDamageMobMultipler > (maxRatio * stageLevel) * baseDamage || finalItemMultipler > (maxRatio * stageLevel) * baseItemMultipler)
            {

                finalDamageMobMultipler = (maxRatio * stageLevel) * baseDamage;
                finalHpMobMultipler = (maxRatio * stageLevel) * baseHP;
                finalItemMultipler = (maxRatio * stageLevel) * baseItemMultipler;
                stopProgress = true;
            }
        }
        else if (stageLevel == 7)
        {
            if (finalDamageMobMultipler > (maxRatio * stageLevel) * baseDamage || finalItemMultipler > (maxRatio * stageLevel) * baseItemMultipler)
            {

                finalDamageMobMultipler = (maxRatio * stageLevel) * baseDamage;
                finalHpMobMultipler = (maxRatio * stageLevel) * baseHP;
                finalItemMultipler = (maxRatio * stageLevel) * baseItemMultipler;
                stopProgress = true;
            }
        }
        else if (stageLevel == 8)
        {
            if (finalDamageMobMultipler > (maxRatio * stageLevel) * baseDamage || finalItemMultipler > (maxRatio * stageLevel) * baseItemMultipler)
            {

                finalDamageMobMultipler = (maxRatio * stageLevel) * baseDamage;
                finalHpMobMultipler = (maxRatio * stageLevel) * baseHP;
                finalItemMultipler = (maxRatio * stageLevel) * baseItemMultipler;
                stopProgress = true;
            }
        }
        else if (stageLevel == 9)
        {
            if (finalDamageMobMultipler > (maxRatio * stageLevel) * baseDamage || finalItemMultipler > (maxRatio * stageLevel) * baseItemMultipler)
            {

                finalDamageMobMultipler = (maxRatio * stageLevel) * baseDamage;
                finalHpMobMultipler = (maxRatio * stageLevel) * baseHP;
                finalItemMultipler = (maxRatio * stageLevel) * baseItemMultipler;
                stopProgress = true;
            }
        }
        else
        {
            if (finalDamageMobMultipler > (maxRatio * stageLevel) * baseDamage || finalItemMultipler > (maxRatio * stageLevel) * baseItemMultipler)
            {

                finalDamageMobMultipler = (maxRatio * stageLevel) * baseDamage;
                finalHpMobMultipler = (maxRatio * stageLevel) * baseHP;
                finalItemMultipler = (maxRatio * stageLevel) * baseItemMultipler;
                stopProgress = true;
            }
        }
    }
    public void MoveToGameOverScene(string name)
    {
        if (name == "win")
        {
            playerWin = true;
            SceneManager.LoadScene("GameOverScene");
        }
        else
        {
            playerWin = false;
            SceneManager.LoadScene("GameOverScene");
            
        }
    }
}
