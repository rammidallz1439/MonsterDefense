using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyName", menuName = "HomeDefense/EnemyScriptable")]
public class EnemyScriptable : ScriptableObject
{
    public int Health;
    public GameObject EnemyPrefab;
    public EnemyType EnemyType;
}
