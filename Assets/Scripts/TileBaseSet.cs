using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "RuleTileSet", menuName = "RuleTile/RuleTileSet", order = 1)]
public class TileBaseSet : ScriptableObject
{
    public TileBase floorRuleTile;
    public TileBase voidRuleTile;
    public TileBase wallRuleTile;
    public TileBase wallTopDRuleTile;
    public TileBase wallTopURuleTile;
    public TileBase wallTopsLRuleTile;
    public TileBase wallTopsRRuleTile;
}
