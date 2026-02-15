using UnityEngine;

[CreateAssetMenu(fileName = "MissileData", menuName = "WingsOfPeace/Missile Data")]
public class MissileData : ScriptableObject
{
    public MissileType missileType;
    [Min(0f)] public float damageRadius = 1f;
    [Min(0f)] public float civilianDamageMultiplier = 1f;
    public int propagandaModifier;
    [Tooltip("Optional progression gate, e.g., mission/day/unlock key.")]
    public string unlockRequirement;
}
