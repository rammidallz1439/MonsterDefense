using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

namespace Syntax.Game
{
    public class LaserShooter : ShootingMachineBase
    {

        public override void Excute()
        {
            FireAction();
        }

        public override void FireAction()
        {
            // EventManager.Instance.TriggerEvent(new BulletFireEvent(CoolDown, FireRate, Target, TurretDataScriptable, SpawnPoint, Animator));

            EventManager.Instance.TriggerEvent(new FindTargetEvent(this, transform, (isInRange) =>
            {
                if (isInRange)
                {
                    EventManager.Instance.TriggerEvent(new LaserShootEvent(CoolDown, FireRate, Target, TurretDataScriptable, LaserPointer));
                }
            }));

            //    EventManager.Instance.TriggerEvent(new LookAtTargetEvent(Target, PartToRotate));
        }

    }
}

