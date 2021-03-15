using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public bool easyDifficulty;
    public bool mediumDifficulty;
    public bool hardDifficulty;
    private void Start()
    {
        easyDifficulty = true;
    }
    public void SelectDifficulty(int index)
    {
        if (index == 0)
        {
            easyDifficulty = true;
            mediumDifficulty = false;
            hardDifficulty = false;
        }
        if (index == 1)
        {
            easyDifficulty = false;
            mediumDifficulty = true;
            hardDifficulty = false;
        }
        if (index == 2)
        {
            easyDifficulty = false;
            mediumDifficulty = false;
            hardDifficulty = true;
        }
    }
}
