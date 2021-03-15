using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] Slider sliderBossHp;

    public void SetBossMaxHealth(int maxHP)
    {
        sliderBossHp.maxValue = maxHP;
        sliderBossHp.value = maxHP;
    }
    public void SetBossHealth(int currentHP)
    {
        sliderBossHp.value = currentHP;
    }
    
}
