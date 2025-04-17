/*using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vault;

public class ShootingMachine : ShootingMachineBase
{
    


    private void Start()
    {
        CheckTurretType();
        Camera = Camera.main;
    }
    private void Update()
    {
        if (turretAction is not null)
            turretAction.Excute();

        *//*if (TimerText != null)
            MonoHelper.Instance.FaceCamera(Camera, TimerText.transform);*//*

    }

    private void CheckTurretType()
    {
        if (TurretDataScriptable is BulletShooter shooter)
        {
            turretAction = new BulletShooterAction(this);
        }
        else if (TurretDataScriptable is RocketShooter rocket)
        {
            turretAction = new RocketShooterAction(this);
        }
        else if (TurretDataScriptable is LaserShooter laser)
        {
            turretAction = new LaserShooterAction(this);
        }
    }

    private void OnDestroy()
    {
       
    }


}
*/