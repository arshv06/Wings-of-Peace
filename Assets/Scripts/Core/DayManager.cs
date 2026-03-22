using System;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    [SerializeField] private int maxDays = 30;
    [SerializeField] private SaveSystem saveSystem;

    private readonly Queue<string> dailyEventQueue = new Queue<string>();

    public event Action<int> OnDayStarted;
    public event Action<int> OnDayAdvanced;

    public int CurrentDay { get; private set; } = 1;
    public int MaxDays => maxDays;

    public IReadOnlyCollection<string> DailyEvents => dailyEventQueue;

    public void InitializeFromSave(SaveGameData save)
    {
        if (save == null)
        {
            return;
        }

        CurrentDay = Mathf.Clamp(save.currentDay, 1, maxDays);
        dailyEventQueue.Clear();
        for (int i = 0; i < save.dailyEventQueue.Count; i++)
        {
            dailyEventQueue.Enqueue(save.dailyEventQueue[i]);
        }
    }

    public void StartDay()
    {
        OnDayStarted?.Invoke(CurrentDay);
    }

    public void QueueDailyEvent(string eventId)
    {
        if (!string.IsNullOrWhiteSpace(eventId))
        {
            dailyEventQueue.Enqueue(eventId);
        }
    }

    public bool AdvanceDay(WarState currentWarState, string currentMissionId, List<string> completedMissions, List<string> decisionHistory)
    {
        if (CurrentDay >= maxDays)
        {
            return false;
        }

        CurrentDay++;
        saveSystem.AutoSaveEndOfDay(CurrentDay, currentWarState, currentMissionId, completedMissions, decisionHistory, new List<string>(dailyEventQueue));
        dailyEventQueue.Clear();
        OnDayAdvanced?.Invoke(CurrentDay);
        return true;
    }
}
