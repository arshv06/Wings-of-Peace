using UnityEngine;

[CreateAssetMenu(fileName = "ChoiceData", menuName = "WingsOfPeace/Choice Data")]
public class ChoiceData : ScriptableObject
{
    public string choiceID;
    [TextArea(2, 6)] public string description;
    public int warModifier;
    public int familyModifier;
    public string unlockMissionID;
    public string lockMissionID;
}
