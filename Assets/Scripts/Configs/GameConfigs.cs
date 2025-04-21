using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigs
{
}


#region Configs

[Serializable]
public class WaveData
{
    public List<Wave> Waves;
}

[Serializable]
public class Wave
{
    public List<EnemyData> EnemyData;
    public int WaveTime;
    public float EnemiesInterval;
}


[Serializable]
public class EnemyData
{
    public EnemyScriptable EnemyScriptable;
    public float MaxCount;
    public float TimeToStart;

}

[System.Serializable]
public class GridData
{
    public List<bool> grid;
    public int rows;
    public int columns;
}

[System.Serializable]
public class SavedHireData
{
    public List<HireData> HireData;
}

[System.Serializable]
public class HireData
{
    public int BaseIndex;
    public int CharacterIndex;
    public float SavedTimer;
    public DateTime LastSavedTime;
}

[FirestoreData]
public class UserData
{
    [FirestoreProperty] public string DisplayName { get; set; }
    [FirestoreProperty] public string UserId { get; set; }

}


[FirestoreData]
public class LevelData
{
    [FirestoreProperty] public Dictionary<string, int> LevelDict { get; set; }

}

[Serializable]
public class SkillTreeData
{
    public List<SkillTreeItem> SkillTreeItems;
}

[Serializable]
public class SkillTreeItem
{
    public SkillTreeDetails skillTreeDetail;
    public bool Brought;
}


[Serializable]
public class SkillTreeDetails
{
    public int Id;
    public int Price;
    public string Description;
}

[FirestoreData]
public class PlayerData
{
    [FirestoreProperty] public int Currency { get; set; }
    [FirestoreProperty] public int CurrentLevel { get; set; }
    [FirestoreProperty] public int CurrentSelectedLevel { get; set; }
}


[FirestoreData]
public class SKillTreeCloudData
{
    [FirestoreProperty] public List<int> SkillsIndex { get; set; }
}
#endregion

#region Enums
public enum EnemyType
{
    None = 0,
    Minion = 1,
    Elite = 2,
    Boss = 3
}

public enum BulletType
{
    None = 0,
    Bullet = 1,
    Rocket = 2,
    Laser = 3
}

#endregion