using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private string saveKey = "wings_of_peace_save_v1";

    /// <summary>
    /// Stores save data in PlayerPrefs JSON. On WebGL this persists in browser storage (IndexedDB).
    /// </summary>
    public void AutoSaveEndOfDay(
        int currentDay,
        WarState warState,
        string currentMissionID,
        List<string> completedMissions,
        List<string> decisionHistory,
        List<string> dailyEventQueue)
    {
        SaveGameData data = new SaveGameData
        {
            currentDay = currentDay,
            warState = CloneWarState(warState),
            currentMissionID = currentMissionID,
            completedMissions = completedMissions != null ? new List<string>(completedMissions) : new List<string>(),
            decisionHistory = decisionHistory != null ? new List<string>(decisionHistory) : new List<string>(),
            dailyEventQueue = dailyEventQueue != null ? new List<string>(dailyEventQueue) : new List<string>()
        };

        Save(data);
    }

    public void Save(SaveGameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
    }

    public SaveGameData LoadOrCreate()
    {
        if (!PlayerPrefs.HasKey(saveKey))
        {
            return new SaveGameData();
        }

        string json = PlayerPrefs.GetString(saveKey, string.Empty);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new SaveGameData();
        }

        try
        {
            SaveGameData data = JsonUtility.FromJson<SaveGameData>(json);
            return data ?? new SaveGameData();
        }
        catch (Exception)
        {
            return new SaveGameData();
        }
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteKey(saveKey);
        PlayerPrefs.Save();
    }

    private static WarState CloneWarState(WarState source)
    {
        if (source == null)
        {
            return new WarState();
        }

        return new WarState
        {
            warScore = source.warScore,
            regimeStability = source.regimeStability,
            civilianUnrest = source.civilianUnrest,
            internationalPressure = source.internationalPressure,
            familySafetyIndex = source.familySafetyIndex
        };
    }
}
