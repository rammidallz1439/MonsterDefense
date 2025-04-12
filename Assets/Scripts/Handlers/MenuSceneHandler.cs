using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneHandler : MonoBehaviour
{
    [Header("Data")]
    public int SelectedLevelIndex;
    public int SelectedLevel;
    public PlayerData PlayerData;
    public List<LevelScriptable> Levels;
    [Space(10)]
    [Header("UI")]
    public Button NextButton;
    public Button PreviousButton;
    public TMP_Text LevelText;
    public GameObject LockObject;
    public TMP_Text EnemiesLevelDescription;
    public List<SkillPiece> SkillPieces;
    public Button SkillTreeButton;
    public ScrollRect SkillTreeRect;
    public TMP_Text CurrencyText;
    public TMP_Text SkillCurrencyText;
    public Button BattleButton;


    [Space(10)]
    [Header("Panels")]
    public GameObject SkillTreeHolder;

    [Space(10)]
    [Header("Dictionaries")]
    public LevelData LevelData = new LevelData();
    public Dictionary<string, int> LevelDict = new Dictionary<string, int>();
}
