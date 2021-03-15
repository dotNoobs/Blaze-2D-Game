using UnityEngine;

public class PlayerXpManager : MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }
    [SerializeField] Bars expBar;
    [SerializeField] int currentLevel = 1;
    [SerializeField] int currentExperience = 0;
    [SerializeField] int maxExperience = 100;
    [SerializeField] int xpMultipler = 2;
    [SerializeField] int addPoints = 5;
    [SerializeField] CharacterStatsUI statsUI;
    [SerializeField] Player player;
    public int CurrentLevel => currentLevel;
    private void Start()
    {
        expBar.SetExp(currentExperience);
        expBar.SetMaxExp(maxExperience);
    }
    public void Startup()
    {
        Debug.Log("Player XP manager starting...");
        Status = ManagerStatus.Started;
    }
    public void IncreaseLevel(int xp)
    {
        currentExperience += xp;
        expBar.SetExp(currentExperience);
        while (currentExperience >= maxExperience)
        {
            AudioManager.Instance.Play(SoundEffectType.levelUp);
            Managers.UnitGenerator.playerOldLevel = currentLevel;
            currentLevel++;
            statsUI.LeftPoints += addPoints;
            player.HP += 20;
            currentExperience -= maxExperience;
            expBar.SetExp(currentExperience);
            maxExperience *= xpMultipler;
            expBar.SetMaxExp(maxExperience);
        }
    }
}
