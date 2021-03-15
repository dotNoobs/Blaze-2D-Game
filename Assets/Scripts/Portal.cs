using UnityEngine;

public class Portal : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.Play(SoundEffectType.portal);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            Managers.MapGenerator.GenerateMap();
            Destroy(gameObject);
        }
    }
}
