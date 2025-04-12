using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Vault;

public class EnemyManager
{
    protected EnemyHandler Handler;

    #region EventHandlers
    protected void EnemySpawnEventHandler(EnemySpawnEvent e)
    {
       
        if (Handler.CoolDownTime <= 0)
        {
            foreach (EnemyData item in e.Wave.EnemyData)
            {
                if (item.TimeToStart <= 0)
                {
                    SpawnEnemies(item.EnemyScriptable.EnemyPrefab, item);
                }

                if (e.Timer <= e.Wave.WaveTime - item.TimeToStart)
                {
                    SpawnEnemies(item.EnemyScriptable.EnemyPrefab, item);
                }

            }

            if (e.Wave.EnemyData.Count > 0)
                Handler.CoolDownTime = 1f / e.Wave.EnemiesInterval;
        }
        Handler.CoolDownTime -= Time.deltaTime;
    }

    protected void EnemyMovementEventHandler(EnemyMovementEvent e)
    {
        if (e.Enemy.Reached == false)
        {
            if (!e.Enemy.IsEnemyBoss)
            {
                e.Agent.SetDestination(Handler.House.position);

            }
            else
            {
                e.Agent.SetDestination(Handler.BossEnemyStopPoint.position);
                StopBossEnemy(e.Enemy,e.Agent); 
            }
        }
        else
        {
            e.Agent.isStopped = true;
        }

    }


    private void StopBossEnemy(Enemy e,NavMeshAgent agent)
    {
        float distance = Vector3.Distance(e.transform.position, Handler.BossEnemyStopPoint.position);
        if(distance <= 0.35f)
        {
            MonoHelper.Instance.PrintMessage("Boss Reached the Stop point", "red");
            e.Reached = true;
            agent.isStopped = true;
            GenericEventsController.Instance.ChangeAnimationEvent(e.Animator, GameConstants.EnemyAttack);
        }
    }
    protected void FindTargetEventHandler(FindTargetEvent e)
    {
        if (e.ShootingMachine.Target == null)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    float distanceToEnemy = Vector3.Distance(e.ShootingMachine.transform.position, enemy.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                    }
                }
                else
                {

                    e.ShootingMachine.Target = null;
                }
            }

            if (nearestEnemy != null && shortestDistance <= e.ShootingMachine.TurretDataScriptable.Range)
            {
                Enemy enemyComponent = nearestEnemy.GetComponent<Enemy>();

                if (enemyComponent != null)
                {
                    e.ShootingMachine.Target = enemyComponent;
                }
                else
                {
                    e.ShootingMachine.Target = null;
                }

            }
            else
            {
                e.ShootingMachine.Target = null;
                EventManager.Instance.TriggerEvent(new ChangeToIdleAnimationEvent(e.ShootingMachine,e.ShootingMachine.Animator));
            }
        }


    }


  protected void SpawnBossEnemyEventHandler(SpawnBossEnemyEvent e)
    {
        GameObject boss = MonoHelper.Instance.InstantiateObject(e.BossObject);
        boss.transform.position = Handler.EnemySpawnPoints[0].transform.position;
        boss.transform.GetComponent<Enemy>().Health = e.Health;
        GenericEventsController.Instance.ChangeAnimationEvent(boss.transform.GetComponent<Enemy>().Animator, GameConstants.EnemyWalk);
    }

    protected void RevertBossAnimationEventHandler(RevertBossAnimationEvent e)
    {
        GenericEventsController.Instance.ChangeAnimationEvent(e.Animator, GameConstants.EnemyIdle);
        e.EnemyWeapon.SetActive(false);
    }

    protected void RevertBossIdleAnimationEventHandler(RevertBossIdleAnimationEvent e)
    {
        GenericEventsController.Instance.ChangeAnimationEvent(e.Animator, GameConstants.EnemyAttack);
        e.EnemyWeapon.SetActive(true);
    }


    #endregion

    #region Functions

    void SpawnEnemies(GameObject enemyObject, EnemyData data)
    {
        Collider cubeCollider = Handler.Platform.GetComponent<Collider>();

        Transform spawnPoint = Handler.EnemySpawnPoints[Random.Range(0, Handler.EnemySpawnPoints.Count)];

        GameObject enemy = MonoHelper.Instance.InstantiateObject(enemyObject, spawnPoint.position, Quaternion.identity);
        enemy.transform.GetComponent<Enemy>().Health = data.EnemyScriptable.Health;
        GenericEventsController.Instance.ChangeAnimationEvent(enemy.transform.GetComponent<Enemy>().Animator, GameConstants.EnemyWalk);
    }

    #endregion
}
