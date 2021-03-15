using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    ProgressManager progressManager;
    AudioManager audioManager;
    [SerializeField] TMP_Text winText;
    [SerializeField] TMP_Text loseText;
    private void Start()
    {
        progressManager = FindObjectOfType<ProgressManager>();
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Stop(SoundEffectType.battleMusic1);
        audioManager.Stop(SoundEffectType.battleMusic2);
        audioManager.Stop(SoundEffectType.battleMusic3);
        audioManager.Stop(SoundEffectType.battleMusic4);
        audioManager.Stop(SoundEffectType.battleMusic5);
        DisplayGameOver();
    }
    void DisplayGameOver()
    {
        if (progressManager.playerWin)
        {
            winText.gameObject.SetActive(true);
            loseText.gameObject.SetActive(false);
            audioManager.Play(SoundEffectType.gameWin);
        }
        else
        {
            winText.gameObject.SetActive(false);
            loseText.gameObject.SetActive(true);
            audioManager.Play(SoundEffectType.gameLose);
        }
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
