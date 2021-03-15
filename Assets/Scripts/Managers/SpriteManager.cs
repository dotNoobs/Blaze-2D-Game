using UnityEngine;

public class SpriteManager : MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }

    public void Startup()
    {
        Debug.Log("Sprite manager starting...");

        Status = ManagerStatus.Started;
    }
}
