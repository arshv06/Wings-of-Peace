using System.Collections.Generic;
using UnityEngine;

public class GameFlowCoordinator : MonoBehaviour
{
    [SerializeField] private MissionManager missionManager;
    [SerializeField] private DayManager dayManager;
    [SerializeField] private WarStateManager warStateManager;
    [SerializeField] private ChoiceManager choiceManager;
    [SerializeField] private NewspaperGenerator newspaperGenerator;
    [SerializeField] private SaveSystem saveSystem;

    private readonly List<string> completedMissionIds = new List<string>();

    public NewspaperIssue LastGeneratedIssue { get; private set; }

    private void Awake()
    {
        SaveGameData save = saveSystem.LoadOrCreate();
        dayManager.InitializeFromSave(save);
        warStateManager.Initialize(save.warState);
        choiceManager.RestoreDecisionHistory(save.decisionHistory);

        missionManager.OnMissionResolved += HandleMissionResolved;
    }

    private void OnDestroy()
    {
        missionManager.OnMissionResolved -= HandleMissionResolved;
    }

    private void HandleMissionResolved(MissionData mission, MissionResult result)
    {
        if (mission != null && !completedMissionIds.Contains(mission.missionID))
        {
            completedMissionIds.Add(mission.missionID);
        }

        LastGeneratedIssue = newspaperGenerator.GenerateIssue(
            warStateManager.CurrentState,
            mission,
            result,
            choiceManager.DecisionHistory);

        bool advanced = dayManager.AdvanceDay(
            warStateManager.CurrentState,
            mission != null ? mission.missionID : string.Empty,
            completedMissionIds,
            new List<string>(choiceManager.DecisionHistory));

        if (advanced)
        {
            missionManager.StartNextMission();
        }
    }
}
