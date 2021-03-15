using UnityEngine;

[CreateAssetMenu(fileName = "BaseUsable", menuName = "BaseItem/BaseUsable", order = 2)]
public class BaseUsable : BaseItem
{
    [SerializeField] UsableType usableType;
    [SerializeField] Ability ability;

    public UsableType UsableType => usableType;
    public Ability Ability => ability;
}
