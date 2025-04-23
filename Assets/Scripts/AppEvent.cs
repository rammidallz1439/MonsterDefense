using Syntax.Game;
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
    public ShootingMachineBase Target;
    public Transform Current;
    public Action<bool> isInRange;

    public FindTargetEvent(ShootingMachineBase target, Transform current, Action<bool> isInRange)
    {
        Target = target;
        Current = current;
        this.isInRange = isInRange;
    }
}

public struct LookAtTargetEvent : GameEvent
{
    public Enemy Target;
    public Transform PartToRotate;

    public LookAtTargetEvent(Enemy target, Transform partToRotate)
    {
        Target = target;
        PartToRotate = partToRotate;
    }
}

public struct BulletFireEvent : GameEvent
{
    public ShootingMachineBase ShootingMachineBase;
    public Transform SpawnPoint;

    public BulletFireEvent(ShootingMachineBase shootingMachineBase, Transform spawnPoint)
    {
        ShootingMachineBase = shootingMachineBase;
        SpawnPoint = spawnPoint;
    }
}


public struct SpawnBulletEvent : GameEvent
{
    public ShootingMachineBase ShootingMachineBase;
    public Transform SpawnPoint;

    public SpawnBulletEvent(ShootingMachineBase shootingMachineBase, Transform spawnPoint)
    {
        ShootingMachineBase = shootingMachineBase;
        SpawnPoint = spawnPoint;
    }
}

public struct UpdateTimerEvent : GameEvent
{

}

public struct BulletEvent : GameEvent
{
    public Bullet Bullet;

    public BulletEvent(Bullet bullet)
    {
        Bullet = bullet;
    }
}

public struct RocketEvent : GameEvent
{
    public Bullet Bullet;

    public RocketEvent(Bullet bullet)
    {
        Bullet = bullet;
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
    public ShootingMachineBase ShootingMachineBase;

    public LaserShootEvent(ShootingMachineBase shootingMachineBase)
    {
        ShootingMachineBase = shootingMachineBase;
    }
}

public struct ChangeToIdleAnimationEvent : GameEvent
{
    public Animator PlayerAnimator;

    public ChangeToIdleAnimationEvent(Animator playerAnimator)
    {
        PlayerAnimator = playerAnimator;
    }
}

public struct InitHireTimersEvent : GameEvent
{

}

/*public struct UpdateHireTimerEvent : GameEvent
{
    public ShootingMachine Character;

    public UpdateHireTimerEvent(ShootingMachine character)
    {
        Character = character;
    }
}*/

/*public struct RemoveSpawnedCharactersEvent : GameEvent
{
    public ShootingMachine Machine;

    public RemoveSpawnedCharactersEvent(ShootingMachine machine)
    {
        Machine = machine;
    }
}
*/


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

public struct OnSkilluyButtonEvent : GameEvent
{
    public SkillPiece SkillPiece;

    public OnSkilluyButtonEvent(SkillPiece skillPiece)
    {
        SkillPiece = skillPiece;
    }
}


public struct OnAttackSkillEvent : GameEvent
{
    public Action<float> AttackPower;
    public ShootingMachineBase ShootingMachineBase;

    public OnAttackSkillEvent( ShootingMachineBase shootingMachineBase, Action<float> attackPower)
    {
        AttackPower = attackPower;
        ShootingMachineBase = shootingMachineBase;
    }
}

public struct OnHealthSkillEvent : GameEvent
{
    public float Health;
    public Action<float> HealthIncreased;

    public OnHealthSkillEvent(float health, Action<float> healthIncreased)
    {
        Health = health;
        HealthIncreased = healthIncreased;
    }
}

public struct OnElfSkillEvent : GameEvent
{

}
public struct OnSorcererSkillEvent : GameEvent
{

}

public struct OnAdventurerSkillEvent : GameEvent
{

}

public struct OnDwarfSkillEvent : GameEvent
{

}