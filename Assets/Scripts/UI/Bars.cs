using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
    [SerializeField] Slider sliderHP;
    [SerializeField] Slider sliderEXP;

    public void SetMaxHealth(int maxHP)
    {
        sliderHP.maxValue = maxHP;
        sliderHP.value = maxHP;
    }
    public void SetHealth(int currentHP)
    {
        sliderHP.value = currentHP;
    }
    public void SetMaxExp(int maxEXP)
    {
        sliderEXP.maxValue = maxEXP;

    }
    public void SetExp(int currentEXP)
    {
        sliderEXP.value = currentEXP;
    }
}
