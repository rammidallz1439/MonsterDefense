using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Vault;

public class AppEvent
{
}


public struct EnemySpawnEvent : GameEvent
{
    public Wave Wave;
    public float Timer;

    public EnemySpawnEvent(Wave wave, float timer)
    {
        Wave = wave;
        Timer = timer;
    }
}

public struct IntialLevelSetUpEvent : GameEvent
{

}

public struct EnemyMovementEvent : GameEvent
{
    public NavMeshAgent Agent;
    public Enemy Enemy;

    public EnemyMovementEvent(NavMeshAgent agent, Enemy enemy)
    {
        Agent = agent;
        Enemy = enemy;
    }
}

public struct MakeGridEvent : GameEvent
{

}

public struct BaseSelectedEvent : GameEvent
{
    public BaseHandler BaseHandler;

    public BaseSelectedEvent(BaseHandler baseHandler)
    {
        BaseHandler = baseHandler;
    }
}

public struct SpawnTurretEvent : GameEvent
{
    public GameObject Turret;

    public SpawnTurretEvent(GameObject turret)
    {
        Turret = turret;
    }
}

public struct FindTargetEvent : GameEvent
{
    public ShootingMachine ShootingMachine;

    public FindTargetEvent(ShootingMachine shootingMachine)
    {
        ShootingMachine = shootingMachine;
    }
}

public struct LookAtTargetEvent : GameEvent
{
    public ShootingMachine ShootingMachine;

    public LookAtTargetEvent(ShootingMachine shootingMachine)
    {
        ShootingMachine = shootingMachine;
    }
}

public struct BulletFireEvent : GameEvent
{
    public ShootingMachine ShootingMachine;

    public BulletFireEvent(ShootingMachine shootingMachine)
    {
        ShootingMachine = shootingMachine;
    }
}

public struct UpdateTimerEvent : GameEvent
{

}

public struct BulletEvent : GameEvent
{
    public Transform Target;
    public GameObject Current;
    public float Speed;

    public BulletEvent(Transform target, GameObject current, float speed)
    {
        Target = target;
        Current = current;
        Speed = speed;
    }
}

public struct RocketEvent : GameEvent
{
    public Transform Target;
    public GameObject Current;
    public float Speed;
    public float BlastRadius;
    public float Gravity;

    public RocketEvent(Transform target, GameObject current, float speed, float blastRadius, float gravity)
    {
        Target = target;
        Current = current;
        Speed = speed;
        BlastRadius = blastRadius;
        Gravity = gravity;
    }
}

public struct RocketBlastEvent : GameEvent
{
    public float AttackPower;
    public GameObject Current;
    public float BlastRadius;

    public RocketBlastEvent(float attackPower, GameObject current, float blastRadius)
    {
        AttackPower = attackPower;
        Current = current;
        BlastRadius = blastRadius;
    }
}

public struct CalcualteRocketVelocityEvent : GameEvent
{
    public BulletType BulletType;
    public Bullet Current;
    public float LaunchAngle;
    public float Gravity;

    public CalcualteRocketVelocityEvent(BulletType bulletType, Bullet current, float launchAngle, float gravity)
    {
        BulletType = bulletType;
        Current = current;
        LaunchAngle = launchAngle;
        Gravity = gravity;
    }
}


public struct CoinDobberAnimation : GameEvent 
{
    public GameObject CoinPrefab;
    public CoinDobberAnimation(GameObject coinPrefab)
    {
        CoinPrefab = coinPrefab;
    }
}

public struct LaserShootEvent : GameEvent
{
    public ShootingMachine ShootingMachine;

    public LaserShootEvent(ShootingMachine shootingMachine)
    {
        ShootingMachine = shootingMachine;
    }
}

public struct ChangeToIdleAnimationEvent : GameEvent
{
    public ShootingMachine ShootingMachine;
    public Animator PlayerAnimator;

    public ChangeToIdleAnimationEvent(ShootingMachine shootingMachine, Animator playerAnimator)
    {
        ShootingMachine = shootingMachine;
        PlayerAnimator = playerAnimator;
    }
}

public struct InitHireTimersEvent : GameEvent
{

}

public struct UpdateHireTimerEvent : GameEvent
{
    public ShootingMachine Character;

    public UpdateHireTimerEvent(ShootingMachine character)
    {
        Character = character;
    }
}

public struct RemoveSpawnedCharactersEvent : GameEvent
{
    public ShootingMachine Machine;

    public RemoveSpawnedCharactersEvent(ShootingMachine machine)
    {
        Machine = machine;
    }
}



public struct AddSpawnedBasesEvent : GameEvent
{
    public BaseHandler BaseHandler;

    public AddSpawnedBasesEvent(BaseHandler baseHandler)
    {
        BaseHandler = baseHandler;
    }
}

public struct MenuSceneInitEvent : GameEvent
{

}

public struct LoginManagerIntiEvent : GameEvent
{

}

public struct LoginSceneInitEvent : GameEvent
{

}

public struct SwitchLoginStateEvent : GameEvent
{
    public bool Exsisting;

    public SwitchLoginStateEvent(bool exsisting)
    {
        Exsisting = exsisting;
    }
}


public struct BattleBUttonEvent : GameEvent
{

}


public struct UpdateCurrentCoinsEvent : GameEvent
{
    public int Value;

    public UpdateCurrentCoinsEvent(int value)
    {
        Value = value;
    }
}

public struct GetCurrentAvailableCurrencyEvent : GameEvent
{
    public Action<LevelHandler> Handler;

    public GetCurrentAvailableCurrencyEvent(Action<LevelHandler> handler)
    {
        Handler = handler;
    }
}

public struct SpawnBossEnemyEvent : GameEvent
{
    public GameObject BossObject;
    public int Health;

    public SpawnBossEnemyEvent(GameObject bossObject, int health)
    {
        BossObject = bossObject;
        Health = health;
    }
}

public struct GameOverEvent : GameEvent
{

}

public struct RevertBossAnimationEvent : GameEvent
{
    public Animator Animator;
    public GameObject EnemyWeapon;

    public RevertBossAnimationEvent(Animator animator, GameObject enemyWeapon)
    {
        Animator = animator;
        EnemyWeapon = enemyWeapon;
    }
}

public struct RevertBossIdleAnimationEvent : GameEvent
{
    public Animator Animator;
    public GameObject EnemyWeapon;

    public RevertBossIdleAnimationEvent(Animator animator, GameObject enemyWeapon)
    {
        Animator = animator;
        EnemyWeapon = enemyWeapon;
    }
}

public struct AddBossAsTargetEvent : GameEvent
{
    public Enemy BossEnemy;

    public AddBossAsTargetEvent(Enemy bossEnemy)
    {
        BossEnemy = bossEnemy;
    }
}