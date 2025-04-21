using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class TurretController : TurretManager, IController
{
    public TurretController (TurretHandler handler)
    {
        Handler = handler;
    }
    public void OnInitialized()
    {
    }

    public void OnRegisterListeners()
    {
        EventManager.Instance.AddListener<BulletFireEvent>(BulletFireEventHandler);
        EventManager.Instance.AddListener<SpawnBulletEvent>(SpawnBulletEventHandler);
        EventManager.Instance.AddListener<LookAtTargetEvent>(LookAtTargetEventHandler);
        EventManager.Instance.AddListener<BulletEvent>(BulletEventhandler);
        EventManager.Instance.AddListener<RocketEvent>(RocketEventHandler);
        EventManager.Instance.AddListener<RocketBlastEvent>(RocketBlastEventHandler);
        EventManager.Instance.AddListener<CalcualteRocketVelocityEvent>(CalculateRocketVelocityEventHandler);
        EventManager.Instance.AddListener<LaserShootEvent>(LaserShootEventHandler);
        EventManager.Instance.AddListener<ChangeToIdleAnimationEvent>(ChangeToIdleAnimationEventHandler);
        
    }

    public void OnRelease()
    {
       
    }

    public void OnRemoveListeners()
    {
        EventManager.Instance.RemoveListener<BulletFireEvent>(BulletFireEventHandler);
        EventManager.Instance.RemoveListener<SpawnBulletEvent>(SpawnBulletEventHandler);
        EventManager.Instance.RemoveListener<LookAtTargetEvent>(LookAtTargetEventHandler);
        EventManager.Instance.RemoveListener<BulletEvent>(BulletEventhandler);
        EventManager.Instance.RemoveListener<RocketEvent>(RocketEventHandler);
        EventManager.Instance.RemoveListener<RocketBlastEvent>(RocketBlastEventHandler);
        EventManager.Instance.RemoveListener<CalcualteRocketVelocityEvent>(CalculateRocketVelocityEventHandler);
        EventManager.Instance.RemoveListener<LaserShootEvent>(LaserShootEventHandler);
        EventManager.Instance.RemoveListener<ChangeToIdleAnimationEvent>(ChangeToIdleAnimationEventHandler);
    }

    public void OnStarted()
    {
    }

    public void OnVisible()
    {
    }
}
