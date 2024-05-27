using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu]
public class UnitData :ScriptableObject
{
    public UnitType unitType;
    public float maxHp;
    public float atkPower;
    public float atkDelay;
    public int atkRange;
    public float moveSpeed = 0.5f;

    public Sprite sprite;
    public SpriteLibraryAsset spriteLibrary;

    public int rewardGold;
}
