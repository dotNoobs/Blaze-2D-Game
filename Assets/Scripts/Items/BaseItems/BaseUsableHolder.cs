using System.Collections.Generic;
using UnityEngine;

public class BaseUsableHolder : MonoBehaviour
{
    [SerializeField] List<BaseUsable> baseUsableList;

    public List<BaseUsable> BaseUsableList => baseUsableList;
}
