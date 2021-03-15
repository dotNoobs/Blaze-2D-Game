using UnityEngine;

public class WallScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.DestroyProjectile(SoundEffectType.projectileDestroyOnWall);
        }
    }
}
