using UnityEngine;
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] SpriteRenderer fill;
    Color col;
    float percentage;
    float maxValue;
    float value;

    public void SetMaxHealth(int maxHP)
    {
        //maxValue = maxHP;

        //fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(float currentHP, float maxHP)
    {
        value = currentHP;
        maxValue = maxHP;
        //Debug.Log($"value:{value}, maxValue:{maxValue}");
        percentage = currentHP / maxHP;
        //Debug.Log($"{percentage} percenage");
        if (percentage > -0.01)
        {
            fill.transform.localScale = new Vector3(percentage, 1, 0);
        }

        if (fill.transform.localScale.x < 0.75f)
        {
            fill.color = Color.yellow;
        }
        if (fill.transform.localScale.x < 0.5f)
        {
            col.r = 0.9811321f;
            col.g = 0.5905269f;
            col.b = 0;
            col.a = 1;
            fill.color = col;
        }
        if (fill.transform.localScale.x < 0.25f)
        {

            fill.color = Color.red;
        }


    }
}
