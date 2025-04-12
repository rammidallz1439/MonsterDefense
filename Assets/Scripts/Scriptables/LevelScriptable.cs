using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_1", menuName = "HomeDefense/LevelScriptable")]
public class LevelScriptable : ScriptableObject
{
    public int Id;
    public Sprite LevelIcon;
    public WaveData WaveData;
    public EnemyScriptable BossEnemy;
}
