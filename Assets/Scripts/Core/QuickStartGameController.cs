using UnityEngine;

/// <summary>
/// Beginner-friendly bootstrapper that starts a playable loop using the modular systems.
/// Attach this to an empty GameObject and assign references in Inspector.
/// </summary>
public class QuickStartGameController : MonoBehaviour
{
    [Header("Core References")]
    [SerializeField] private MissionManager missionManager;
    [SerializeField] private DayManager dayManager;
    [SerializeField] private DroneControlPanel droneControlPanel;

    [Header("Quick Test Launch Defaults")]
    [SerializeField] private int quickLaunchX;
    [SerializeField] private int quickLaunchY;
    [SerializeField] private MissileType quickLaunchMissile = MissileType.Precision;
    [SerializeField] private string quickDecodedMessage = string.Empty;

    private void Start()
    {
        if (missionManager == null || dayManager == null)
        {
            Debug.LogError("QuickStartGameController: Missing required references.");
            return;
        }

        missionManager.OnMissionStart += HandleMissionStart;
        missionManager.OnMissionSuccess += HandleMissionSuccess;
        missionManager.OnMissionFailure += HandleMissionFailure;
        missionManager.OnMissionResolved += HandleMissionResolved;

        dayManager.StartDay();
        missionManager.StartNextMission();
    }

    private void OnDestroy()
    {
        if (missionManager == null)
        {
            return;
        }

        missionManager.OnMissionStart -= HandleMissionStart;
        missionManager.OnMissionSuccess -= HandleMissionSuccess;
        missionManager.OnMissionFailure -= HandleMissionFailure;
        missionManager.OnMissionResolved -= HandleMissionResolved;
    }

    [ContextMenu("Quick Submit Launch")]
    public void QuickSubmitLaunch()
    {
        if (droneControlPanel == null)
        {
            Debug.LogError("QuickStartGameController: DroneControlPanel reference is missing.");
            return;
        }

        droneControlPanel.SetCoordinateInput(quickLaunchX, quickLaunchY, null);
        droneControlPanel.SetMissileSelection(quickLaunchMissile);
        droneControlPanel.SetDecodedMessage(quickDecodedMessage);

        bool launched = droneControlPanel.Launch(out MissionResult result);
        if (!launched)
        {
            Debug.LogWarning("QuickStartGameController: Launch submission was rejected. Check mission state or missile catalog wiring.");
            return;
        }

        Debug.Log($"Quick launch submitted. Success={result.IsSuccess}, OutcomeID={result.resolvedOutcomeID}, Propaganda={result.propagandaScore}, CivilianDamage={result.civilianDamageScore}");
    }

    private static void HandleMissionStart(MissionData mission)
    {
        if (mission == null)
        {
            return;
        }

        Debug.Log($"Mission started: {mission.missionID} | {mission.title}");
    }

    private static void HandleMissionSuccess(MissionData mission, MissionResult result)
    {
        Debug.Log($"Mission success: {mission.missionID} | Propaganda={result.propagandaScore} | Morale={result.moraleShift}");
    }

    private static void HandleMissionFailure(MissionData mission, MissionResult result)
    {
        Debug.LogWarning($"Mission failure: {mission.missionID} | CivilianDamage={result.civilianDamageScore}");
    }

    private static void HandleMissionResolved(MissionData mission, MissionResult result)
    {
        Debug.Log($"Mission resolved: {mission.missionID} | Outcome={result.resolvedOutcomeID}");
    }
}
