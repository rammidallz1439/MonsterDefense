using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vault;

namespace Syntax.Game
{
    public abstract class ShootingMachineBase : MonoBehaviour
    {
        public Transform AmmoPoint;
        public Transform PartToRotate;
        public Transform SpawnPoint;
        public Enemy Target = null;
        public TurretDataScriptable TurretDataScriptable;
        public float CoolDown;
        public float FireRate;
        public Animator Animator;

        [Space(10)]
        [Header("Laser Specific")]
        public Bullet LaserPointer = null;
      

        [Space(10)]
        public Camera Camera = null;

        private void Update()
        {
            Excute();
        }

        /// <summary>
        /// call this in all the shooters
        /// </summary>
        public abstract void Excute();


        /// <summary>
        /// Fires the Ammo 
        /// </summary>
        /// <param name="coolDown"></param>
        /// <param name="target"></param>
        /// <param name="turretDataScriptable"></param>
        /// <param name="spawnPoint"></param>
        /// <param name="anim"></param>
        public abstract void FireAction();
        
        
    }

}
