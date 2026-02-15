using System;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private MissionManager missionManager;
    [SerializeField] private WarStateManager warStateManager;

    private readonly List<string> decisionHistory = new List<string>();

    public event Action<ChoiceData[]> OnChoicesPresented;
    public event Action<ChoiceData> OnChoiceApplied;

    public IReadOnlyList<string> DecisionHistory => decisionHistory;

    public void PresentOptions(ChoiceData[] choices)
    {
        OnChoicesPresented?.Invoke(choices);
    }

    public void ApplyChoice(ChoiceData choice)
    {
        if (choice == null)
        {
            return;
        }

        warStateManager.ApplyChoiceModifiers(choice.warModifier, choice.familyModifier);

        if (!string.IsNullOrWhiteSpace(choice.unlockMissionID))
        {
            missionManager.UnblockMission(choice.unlockMissionID);
        }

        if (!string.IsNullOrWhiteSpace(choice.lockMissionID))
        {
            missionManager.BlockMission(choice.lockMissionID);
        }

        decisionHistory.Add(choice.choiceID);
        OnChoiceApplied?.Invoke(choice);
    }

    public void RestoreDecisionHistory(List<string> savedHistory)
    {
        decisionHistory.Clear();
        if (savedHistory == null)
        {
            return;
        }

        decisionHistory.AddRange(savedHistory);
    }
}
