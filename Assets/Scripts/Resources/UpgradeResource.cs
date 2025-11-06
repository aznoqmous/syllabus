using UnityEngine;

[CreateAssetMenu(fileName ="New Upgrade", menuName ="Upgrade")]
public class UpgradeResource : ScriptableObject
{
    public int StartingCost = 10;
    public string ShopName = "Upgrade";
    public int MaxLevel = 3;
    public UpgradeType Type;
}

public enum UpgradeType
{
    MaxBullets,
    MaxHp,
    Speed,
    AttractionDistance
}