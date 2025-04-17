using Syntax.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireTimerHandler : MonoBehaviour
{
    [Header("Timer")]
    public float HireDuration;
    public DateTime HireEndTime;
    public bool IsHired;

    [Space(10)]
    [Header("PlaceMentData")]
    public List<BaseHandler> BaseHandlers;
    public List<ShootingMachineBase> Characters;
    public SavedHireData SavedHireData;

    [Space(10)]
    [Header("Saving Data")]
    public List<HireData> HireDataToSave;
}
