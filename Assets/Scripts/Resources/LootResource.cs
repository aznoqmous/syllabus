using UnityEngine;

[CreateAssetMenu(fileName ="Loot", menuName ="New loot")]
public class LootResource : ScriptableObject
{
    public GameObject Model;
    public LootEffect Effect;
    public float Value = 1f;
}

public enum LootEffect
{
    RestoreHealth,
    RestoreBullet
}