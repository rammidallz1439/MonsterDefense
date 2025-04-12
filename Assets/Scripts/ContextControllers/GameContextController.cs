using Vault;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BGS.Game;

public class GameContextController : Registerer
{
    [SerializeField] private EnemyHandler _enemyHandler;
    [SerializeField] private LevelHandler _levelHandler;
    [SerializeField] private TurretHandler _turretHandler;
  //  [SerializeField] private HireTimerHandler _hireTimerHandler;
    public override void Enable()
    {
    }

    public override void OnAwake()
    {
        AddController(new EnemyController(_enemyHandler));
        AddController(new LevelController(_levelHandler));
        AddController(new TurretController(_turretHandler));
       // AddController(new HireTimerController(_hireTimerHandler));  
    }

    public override void OnStart()
    {
    }
}
