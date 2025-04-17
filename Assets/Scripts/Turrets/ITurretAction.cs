/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public interface ITurretAction
{
    public void Excute();
}


public class BulletShooterAction : ITurretAction
{
    private ShootingMachine shootingMachine;

    public BulletShooterAction(ShootingMachine shootingMachine)
    {
        this.shootingMachine = shootingMachine;
    }
    
    public void Excute()
    {
        EventManager.Instance.TriggerEvent(new FindTargetEvent(shootingMachine));
        EventManager.Instance.TriggerEvent(new LookAtTargetEvent(shootingMachine));
        EventManager.Instance.TriggerEvent(new BulletFireEvent(shootingMachine));
     //   EventManager.Instance.TriggerEvent(new UpdateHireTimerEvent(shootingMachine));
      
    }

}

public class RocketShooterAction : ITurretAction
{
    private ShootingMachine shootingMachine;

    public RocketShooterAction(ShootingMachine shootingMachine)
    {
        this.shootingMachine = shootingMachine;
    }

    public void Excute()
    {
        EventManager.Instance.TriggerEvent(new FindTargetEvent(shootingMachine));
        EventManager.Instance.TriggerEvent(new LookAtTargetEvent(shootingMachine));
        EventManager.Instance.TriggerEvent(new BulletFireEvent(shootingMachine));
      //  EventManager.Instance.TriggerEvent(new UpdateHireTimerEvent(shootingMachine));

    }
}

public class LaserShooterAction : ITurretAction
{
    private ShootingMachine shootingMachine;

    public LaserShooterAction(ShootingMachine shootingMachine)
    {
        this.shootingMachine = shootingMachine;
    }

    public void Excute()
    {
        EventManager.Instance.TriggerEvent(new FindTargetEvent(shootingMachine));

        EventManager.Instance.TriggerEvent(new LaserShootEvent(shootingMachine));
       // EventManager.Instance.TriggerEvent(new UpdateHireTimerEvent(shootingMachine));

    }
}
*/