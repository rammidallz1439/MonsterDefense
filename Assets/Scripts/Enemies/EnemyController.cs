using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class EnemyController : EnemyManager, IController
{
    public EnemyController(EnemyHandler handler)
    {
        Handler = handler;
    }
    public void OnInitialized()
    {
    }

    public void OnRegisterListeners()
    {
        EventManager.Instance.AddListener<EnemySpawnEvent>(EnemySpawnEventHandler);
        EventManager.Instance.AddListener<EnemyMovementEvent>(EnemyMovementEventHandler);
        EventManager.Instance.AddListener<FindTargetEvent>(FindTargetEventHandler);
        EventManager.Instance.AddListener<SpawnBossEnemyEvent>(SpawnBossEnemyEventHandler);
        EventManager.Instance.AddListener<RevertBossAnimationEvent>(RevertBossAnimationEventHandler);
        EventManager.Instance.AddListener<RevertBossIdleAnimationEvent>(RevertBossIdleAnimationEventHandler);
    }

    public void OnRelease()
    {
    }

    public void OnRemoveListeners()
    {
        EventManager.Instance.RemoveListener<EnemySpawnEvent>(EnemySpawnEventHandler);
        EventManager.Instance.RemoveListener<EnemyMovementEvent>(EnemyMovementEventHandler);
        EventManager.Instance.RemoveListener<FindTargetEvent>(FindTargetEventHandler);
        EventManager.Instance.RemoveListener<SpawnBossEnemyEvent>(SpawnBossEnemyEventHandler);
        EventManager.Instance.RemoveListener<RevertBossAnimationEvent>(RevertBossAnimationEventHandler);
        EventManager.Instance.RemoveListener<RevertBossIdleAnimationEvent>(RevertBossIdleAnimationEventHandler);

    }

    public void OnStarted()
    {
    }


    public void OnVisible()
    {
    }
}
