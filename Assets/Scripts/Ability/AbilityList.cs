using System.Collections.Generic;
using UnityEngine;

public class AbilityList : MonoBehaviour
{
    [SerializeField] List<Ability> skillList;
    [SerializeField] List<Ability> potionList;

    public List<Ability> SkillList => skillList;
    public List<Ability> PotionList => potionList;
}
