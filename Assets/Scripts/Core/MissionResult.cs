using System;

[Serializable]
public struct MissionResult
{
    public bool wasDecodedCorrectly;
    public bool coordinatesCorrect;
    public bool missileTypeValid;
    public int civilianDamageScore;
    public int moraleShift;

    public int propagandaScore;
    public int successQuality;
    public string resolvedOutcomeID;

    public bool IsSuccess => wasDecodedCorrectly && coordinatesCorrect && missileTypeValid;
}
