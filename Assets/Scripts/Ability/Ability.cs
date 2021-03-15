using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Item/Ability", order = 3)]
public class Ability : ScriptableObject
{
    public AbilityType abilityEnum;
    public float Cooldown;

}
