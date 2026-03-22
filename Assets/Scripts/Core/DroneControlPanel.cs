using UnityEngine;

public class DroneControlPanel : MonoBehaviour
{
    [SerializeField] private MissionManager missionManager;

    private GridCoordinate pendingCoordinate;
    private MissileType pendingMissileType = MissileType.Precision;
    private string pendingDecodedMessage = string.Empty;

    public bool SetCoordinateInput(int x, int y, GridDefinition gridDefinition)
    {
        GridCoordinate input = new GridCoordinate(x, y);
        if (gridDefinition != null && !gridDefinition.IsWithinBounds(input))
        {
            return false;
        }

        pendingCoordinate = input;
        return true;
    }

    public void SetMissileSelection(MissileType missileType)
    {
        pendingMissileType = missileType;
    }

    public void SetDecodedMessage(string decodedMessage)
    {
        pendingDecodedMessage = decodedMessage;
    }

    public bool Launch(out MissionResult missionResult)
    {
        return missionManager.TrySubmitLaunch(pendingCoordinate, pendingMissileType, pendingDecodedMessage, out missionResult);
    }
}
