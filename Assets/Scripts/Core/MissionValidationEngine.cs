using UnityEngine;

public static class MissionValidationEngine
{
    public static MissionResult Validate(
        MissionData mission,
        MissileData missile,
        GridCoordinate attemptedCoordinate,
        string decodedMessage,
        GridDefinition gridDefinition)
    {
        bool decodeCorrect = string.Equals(
            Normalize(decodedMessage),
            Normalize(mission.correctDecodedMessage),
            System.StringComparison.OrdinalIgnoreCase);

        bool coordinateWithinBounds = gridDefinition == null || gridDefinition.IsWithinBounds(attemptedCoordinate);
        bool coordinateCorrect = coordinateWithinBounds && attemptedCoordinate.x == mission.coordinateTarget.x && attemptedCoordinate.y == mission.coordinateTarget.y;
        bool missileValid = mission.allowedMissileTypes.Contains(missile.missileType);

        float proximityFactor = gridDefinition == null ? 0f : gridDefinition.GetCivilianProximityFactor(attemptedCoordinate);
        int civilianDamage = Mathf.RoundToInt(missile.damageRadius * missile.civilianDamageMultiplier * proximityFactor * 100f);

        int successQuality = CalculateSuccessQuality(decodeCorrect, coordinateCorrect, missileValid);
        int missionDifficultyValue = (int)mission.difficultyLevel;
        int propagandaScore = (missionDifficultyValue * successQuality) + missile.propagandaModifier;
        int moraleShift = mission.moralImpact + (successQuality * 5) - Mathf.RoundToInt(civilianDamage * 0.1f);

        return new MissionResult
        {
            wasDecodedCorrectly = decodeCorrect,
            coordinatesCorrect = coordinateCorrect,
            missileTypeValid = missileValid,
            civilianDamageScore = civilianDamage,
            moraleShift = moraleShift,
            propagandaScore = propagandaScore,
            successQuality = successQuality,
            resolvedOutcomeID = successQuality > 0 ? mission.successOutcomeID : mission.failureOutcomeID
        };
    }

    private static int CalculateSuccessQuality(bool decodeCorrect, bool coordinateCorrect, bool missileValid)
    {
        int score = 0;
        if (decodeCorrect) score += 1;
        if (coordinateCorrect) score += 1;
        if (missileValid) score += 1;
        return score;
    }

    private static string Normalize(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
    }
}
