using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridDefinition", menuName = "WingsOfPeace/Grid Definition")]
public class GridDefinition : ScriptableObject
{
    [Serializable]
    public struct CivilianZone
    {
        public string zoneId;
        public RectInt tileBounds;
        [Min(0f)] public float proximityFalloff;

        public Vector2 Center => tileBounds.center;

        public bool Contains(GridCoordinate coordinate)
        {
            return tileBounds.Contains(new Vector2Int(coordinate.x, coordinate.y));
        }
    }

    [Header("Grid Bounds")]
    public int minX = 0;
    public int maxX = 99;
    public int minY = 0;
    public int maxY = 99;

    [Header("Civilian Zones")]
    public List<CivilianZone> civilianZones = new List<CivilianZone>();

    public bool IsWithinBounds(GridCoordinate coordinate)
    {
        return coordinate.x >= minX && coordinate.x <= maxX && coordinate.y >= minY && coordinate.y <= maxY;
    }

    public float GetCivilianProximityFactor(GridCoordinate impactPoint)
    {
        float highestFactor = 0f;

        for (int i = 0; i < civilianZones.Count; i++)
        {
            CivilianZone zone = civilianZones[i];

            if (zone.Contains(impactPoint))
            {
                return 1f;
            }

            float distance = Vector2.Distance(impactPoint.ToVector2(), zone.Center);
            if (zone.proximityFalloff <= 0f)
            {
                continue;
            }

            float factor = Mathf.Clamp01(1f - (distance / zone.proximityFalloff));
            highestFactor = Mathf.Max(highestFactor, factor);
        }

        return highestFactor;
    }
}
