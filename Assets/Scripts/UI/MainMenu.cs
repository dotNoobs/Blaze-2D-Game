using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text difficultyNameText;
    [SerializeField] Slider difficultySlider;
    [SerializeField] DifficultyManager difficultyManager;
    [SerializeField] AudioManager audioManager;
    private void Start()
    {
        audioManager.Play(SoundEffectType.mainMenuTheme);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void ZexelosStartGame()
    {
        SceneManager.LoadScene("ZexelosScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void SelectDifficulty()
    {
        if (difficultySlider.value == 0)
        {
            difficultyManager.SelectDifficulty(0);
            difficultyNameText.color = Color.green;
            difficultyNameText.text = $"Easy";
        }
        else if (difficultySlider.value == 1)
        {
            difficultyManager.SelectDifficulty(1);
            difficultyNameText.color = Color.yellow;
            difficultyNameText.text = $"Medium";
        }
        else if (difficultySlider.value == 2)
        {
            difficultyManager.SelectDifficulty(2);
            difficultyNameText.color = Color.red;
            difficultyNameText.text = $"Hard";
        }
    }

}
