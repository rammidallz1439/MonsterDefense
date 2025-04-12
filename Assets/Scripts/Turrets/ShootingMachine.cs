using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vault;

public class ShootingMachine : MonoBehaviour
{
    public Transform PartToRotate;
    public Transform SpawnPoint;
    public Enemy Target = null;

    public TurretDataScriptable TurretDataScriptable;
    private ITurretAction turretAction;

    public float CoolDown;
    public float FireRate;
    public Animator Animator;

    [Space(10)]
    [Header("Laser Specific")]
    public Bullet LaserPointer = null;


    [Space(10)]
    [Header("Ui")]
    public TMP_Text TimerText;

    [Space(10)]
    [SerializeField] private Camera Camera = null;


    [Space(10)]
    [Header("Extra Data")]
    public int Index;
    public int BaseIndex;
    public float Timer;
    public BaseHandler SpawnedBase = null;


    private void Start()
    {
        CheckTurretType();
        Camera = Camera.main;
    }
    private void Update()
    {
        if (turretAction is not null)
            turretAction.Excute();

        /*if (TimerText != null)
            MonoHelper.Instance.FaceCamera(Camera, TimerText.transform);*/

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
