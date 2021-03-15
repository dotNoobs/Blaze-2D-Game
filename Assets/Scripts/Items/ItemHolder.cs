using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class ItemHolder : MonoBehaviour
{
    public Item Item { get; set; }

    SpriteResolver spriteResolver;

    void Start()
    {
        spriteResolver = GetComponent<SpriteResolver>();
        UpdateIcon();
    }

    public void UpdateIcon()
    {
        if (Item != null)
        {
            if (Item.ItemType == ItemType.Weapon)
                spriteResolver.SetCategoryAndLabel((Item as Weapon).WeaponType.ToString(), Item.SpriteIndex.ToString());
            else if (Item.ItemType == ItemType.Usable)
                spriteResolver.SetCategoryAndLabel((Item as Usable).UsableType.ToString(), Item.SpriteIndex.ToString());
        }
        else
            Destroy(gameObject);
    }
}
