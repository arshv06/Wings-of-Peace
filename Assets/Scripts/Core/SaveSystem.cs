using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private string saveFileName = "wings_of_peace_save.json";

    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

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
            warState = warState,
            currentMissionID = currentMissionID,
            completedMissions = completedMissions ?? new List<string>(),
            decisionHistory = decisionHistory ?? new List<string>(),
            dailyEventQueue = dailyEventQueue ?? new List<string>()
        };

        Save(data);
    }

    public void Save(SaveGameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public SaveGameData LoadOrCreate()
    {
        if (!File.Exists(SavePath))
        {
            return new SaveGameData();
        }

        string json = File.ReadAllText(SavePath);
        SaveGameData data = JsonUtility.FromJson<SaveGameData>(json);
        return data ?? new SaveGameData();
    }
}
