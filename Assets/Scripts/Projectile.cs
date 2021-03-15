using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage { get; set; }
    public bool canDamagePlayer;
    public bool canDamageMonsters;

    [SerializeField] float lifeTime;
    [SerializeField] float projectileSpeed;

    Vector3 moveAmount;

    void Update()
    {
        //Set how much projectile has to move this frame
        moveAmount = transform.right * projectileSpeed * Time.deltaTime;

        //Move projectile by given amount
        transform.position += moveAmount;

        //Destroy this gameobject after given amount of time have passed
        Destroy(gameObject, lifeTime);
    }

    public void DestroyProjectile(SoundEffectType soundType)
    {
        AudioManager.Instance.Play(soundType);

        Destroy(gameObject);
    }
}
