using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private DayManager dayManager;
    [SerializeField] private WarStateManager warStateManager;
    [SerializeField] private GridDefinition gridDefinition;

    [Header("Data")]
    [SerializeField] private List<MissionData> missionPool = new List<MissionData>();
    [SerializeField] private List<MissileData> missileCatalog = new List<MissileData>();

    public event Action<MissionData> OnMissionStart;
    public event Action<MissionData, MissionResult> OnMissionSuccess;
    public event Action<MissionData, MissionResult> OnMissionFailure;
    public event Action<MissionData, MissionResult> OnMissionResolved;

    public MissionData CurrentMission { get; private set; }
    public MissionLifecycleState CurrentState { get; private set; } = MissionLifecycleState.Idle;

    private readonly HashSet<string> blockedMissionIds = new HashSet<string>();

    public void StartNextMission()
    {
        if (CurrentState != MissionLifecycleState.Idle && CurrentState != MissionLifecycleState.Resolved)
        {
            return;
        }

        CurrentMission = SelectMissionForDay(dayManager.CurrentDay);
        CurrentState = MissionLifecycleState.Briefing;

        if (CurrentMission != null)
        {
            CurrentState = MissionLifecycleState.AwaitingLaunch;
            OnMissionStart?.Invoke(CurrentMission);
        }
    }

    public bool TrySubmitLaunch(GridCoordinate coordinate, MissileType missileType, string decodedMessage, out MissionResult result)
    {
        result = default;

        if (CurrentState != MissionLifecycleState.AwaitingLaunch || CurrentMission == null)
        {
            return false;
        }

        MissileData missileData = missileCatalog.Find(x => x.missileType == missileType);
        if (missileData == null)
        {
            return false;
        }

        CurrentState = MissionLifecycleState.Resolving;
        result = MissionValidationEngine.Validate(CurrentMission, missileData, coordinate, decodedMessage, gridDefinition);

        warStateManager.ApplyMissionResult(CurrentMission, result);
        dayManager.QueueDailyEvent($"MISSION:{CurrentMission.missionID}:{result.resolvedOutcomeID}");

        if (result.IsSuccess)
        {
            OnMissionSuccess?.Invoke(CurrentMission, result);
        }
        else
        {
            OnMissionFailure?.Invoke(CurrentMission, result);
        }

        OnMissionResolved?.Invoke(CurrentMission, result);
        CurrentState = MissionLifecycleState.Resolved;
        return true;
    }

    public void BlockMission(string missionId)
    {
        if (!string.IsNullOrWhiteSpace(missionId))
        {
            blockedMissionIds.Add(missionId);
        }
    }

    public void UnblockMission(string missionId)
    {
        if (!string.IsNullOrWhiteSpace(missionId))
        {
            blockedMissionIds.Remove(missionId);
        }
    }

    private MissionData SelectMissionForDay(int currentDay)
    {
        List<MissionData> candidates = new List<MissionData>();
        int targetDifficulty = Mathf.Clamp(1 + (currentDay - 1) / 3, 1, 4);

        for (int i = 0; i < missionPool.Count; i++)
        {
            MissionData mission = missionPool[i];
            if (mission == null || blockedMissionIds.Contains(mission.missionID))
            {
                continue;
            }

            if (currentDay < mission.minDay || currentDay > mission.maxDay)
            {
                continue;
            }

            int distance = Mathf.Abs((int)mission.difficultyLevel - targetDifficulty);
            int adjustedWeight = Mathf.Max(1, mission.baseSelectionWeight - distance);

            for (int j = 0; j < adjustedWeight; j++)
            {
                candidates.Add(mission);
            }
        }

        if (candidates.Count == 0)
        {
            return null;
        }

        int index = UnityEngine.Random.Range(0, candidates.Count);
        return candidates[index];
    }
}
