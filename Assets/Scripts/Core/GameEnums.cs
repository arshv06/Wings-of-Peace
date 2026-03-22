using System;

public enum DifficultyLevel
{
    Easy = 1,
    Medium = 2,
    Hard = 3,
    Extreme = 4
}

public enum MissileType
{
    Precision = 0,
    Cluster = 1,
    Heavy = 2,
    EMP = 3
}

public enum MissionLifecycleState
{
    Idle,
    Briefing,
    AwaitingLaunch,
    Resolving,
    Resolved
}

public enum EndgameType
{
    None,
    RegimeCollapse,
    MilitaryVictory,
    CivilianUprising,
    Assassination,
    FamilyConsequenceEvent
}
