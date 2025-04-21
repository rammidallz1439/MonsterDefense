using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using Vault;

namespace Syntax.Game
{
    public class BulletShooter : ShootingMachineBase
    {



        public override void Excute()
        {
            FireAction();
        }

        public override void FireAction()
        {

            EventManager.Instance.TriggerEvent(new FindTargetEvent(this, transform, (isInRange) =>
            {
                if (isInRange)
                {
                    EventManager.Instance.TriggerEvent(new LookAtTargetEvent(Target, PartToRotate));
                    EventManager.Instance.TriggerEvent(new BulletFireEvent(this, transform));
                 //   EventManager.Instance.TriggerEvent(new SpawnBulletEvent(this, transform));

                }
            }));
      
        }


    }
}

