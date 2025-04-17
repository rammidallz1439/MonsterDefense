using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

namespace Syntax.Game
{
    public class RocketShooter : ShootingMachineBase
    {

        public override void Excute()
        {
            FireAction();
        }

        public override void FireAction()
        {

            EventManager.Instance.TriggerEvent(new FindTargetEvent(this, transform));

            EventManager.Instance.TriggerEvent(new LookAtTargetEvent(Target, PartToRotate));
            EventManager.Instance.TriggerEvent(new BulletFireEvent(this,transform));

        }

    }

}
