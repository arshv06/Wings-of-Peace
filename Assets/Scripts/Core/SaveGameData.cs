using System;
using System.Collections.Generic;

[Serializable]
public class SaveGameData
{
    public int currentDay = 1;
    public WarState warState = new WarState();
    public string currentMissionID;
    public List<string> completedMissions = new List<string>();
    public List<string> decisionHistory = new List<string>();
    public List<string> dailyEventQueue = new List<string>();
}
