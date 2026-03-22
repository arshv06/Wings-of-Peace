using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionData", menuName = "WingsOfPeace/Mission Data")]
public class MissionData : ScriptableObject
{
    [Header("Identity")]
    public string missionID;
    public string title;
    [TextArea(3, 10)] public string description;

    [Header("Mission Parameters")]
    public DifficultyLevel difficultyLevel;
    [TextArea(2, 8)] public string encryptedOrderText;
    public string correctDecodedMessage;
    public GridCoordinate coordinateTarget;
    public List<MissileType> allowedMissileTypes = new List<MissileType>();

    [Header("Scoring Weights")]
    [Range(0, 100)] public int civilianRiskLevel = 10;
    [Range(-100, 100)] public int propagandaValue = 10;
    [Range(-100, 100)] public int moralImpact = 0;

    [Header("Outcome Routing")]
    public string successOutcomeID;
    public string failureOutcomeID;

    [Header("Mission Pool")]
    [Min(1)] public int baseSelectionWeight = 1;
    [Min(1)] public int minDay = 1;
    [Min(1)] public int maxDay = 999;
}
