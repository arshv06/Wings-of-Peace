using System;
using UnityEngine;

public class WarStateManager : MonoBehaviour
{
    [SerializeField] private int minBound = 0;
    [SerializeField] private int maxBound = 200;

    public event Action<WarState> OnWarStateUpdated;
    public event Action<EndgameType> OnEndgameTriggered;

    public WarState CurrentState { get; private set; } = new WarState();

    public void Initialize(WarState state)
    {
        CurrentState = state ?? new WarState();
        ClampValues();
        OnWarStateUpdated?.Invoke(CurrentState);
    }

    public void ApplyMissionResult(MissionData mission, MissionResult result)
    {
        CurrentState.warScore += result.propagandaScore + mission.propagandaValue;
        CurrentState.regimeStability += result.IsSuccess ? 4 : -10;
        CurrentState.civilianUnrest += Mathf.RoundToInt(result.civilianDamageScore * 0.05f) + (result.IsSuccess ? -2 : 3);
        CurrentState.internationalPressure += Mathf.RoundToInt(result.civilianDamageScore * 0.03f);
        CurrentState.familySafetyIndex += result.moraleShift >= 0 ? 1 : -5;

        ClampValues();
        OnWarStateUpdated?.Invoke(CurrentState);
        EvaluateEndgame();
    }

    public void ApplyChoiceModifiers(int warModifier, int familyModifier)
    {
        CurrentState.warScore += warModifier;
        CurrentState.familySafetyIndex += familyModifier;
        CurrentState.civilianUnrest += warModifier < 0 ? 2 : -1;

        ClampValues();
        OnWarStateUpdated?.Invoke(CurrentState);
        EvaluateEndgame();
    }

    private void ClampValues()
    {
        CurrentState.warScore = Mathf.Clamp(CurrentState.warScore, minBound, maxBound);
        CurrentState.regimeStability = Mathf.Clamp(CurrentState.regimeStability, minBound, maxBound);
        CurrentState.civilianUnrest = Mathf.Clamp(CurrentState.civilianUnrest, minBound, maxBound);
        CurrentState.internationalPressure = Mathf.Clamp(CurrentState.internationalPressure, minBound, maxBound);
        CurrentState.familySafetyIndex = Mathf.Clamp(CurrentState.familySafetyIndex, minBound, maxBound);
    }

    private void EvaluateEndgame()
    {
        EndgameType endgame = EndgameType.None;

        if (CurrentState.regimeStability <= 0)
        {
            endgame = EndgameType.RegimeCollapse;
        }
        else if (CurrentState.civilianUnrest >= 100)
        {
            endgame = EndgameType.CivilianUprising;
        }
        else if (CurrentState.warScore >= 200)
        {
            endgame = EndgameType.MilitaryVictory;
        }
        else if (CurrentState.internationalPressure >= 180)
        {
            endgame = EndgameType.Assassination;
        }
        else if (CurrentState.familySafetyIndex <= 20)
        {
            endgame = EndgameType.FamilyConsequenceEvent;
        }

        if (endgame != EndgameType.None)
        {
            OnEndgameTriggered?.Invoke(endgame);
        }
    }
}
