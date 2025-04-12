using Google.Impl;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vault;
using static UnityEngine.GraphicsBuffer;

public class TurretManager
{
    protected TurretHandler Handler;

    #region Event Handler
    protected void BulletFireEventHandler(BulletFireEvent e)
    {
        if (e.ShootingMachine.CoolDown <= 0)
        {
            if (e.ShootingMachine.Target != null && e.ShootingMachine.TurretDataScriptable != null)
            {
                GenericEventsController.Instance.PlayNonloopAnimation(e.ShootingMachine.Animator, GameConstants.CharacterAttack);
                if (e.ShootingMachine.TurretDataScriptable.Bullet.gameObject != null)
                {

                    GameObject ammo = ObjectPoolManager.Instance.Get(e.ShootingMachine.TurretDataScriptable.Bullet.gameObject.name, true);
                    if(ammo != null)
                    {
                        ammo.transform.position = e.ShootingMachine.SpawnPoint.position;
                        ammo.transform.GetComponent<Bullet>().AttackPower = e.ShootingMachine.TurretDataScriptable.AttackPower;
                        ammo.transform.GetComponent<Bullet>().Target = e.ShootingMachine.Target.transform;
                        ammo.transform.GetComponent<Bullet>().TargetPoint = e.ShootingMachine.Target.transform.position;
                    }
                }

            }
            e.ShootingMachine.CoolDown = 1f / e.ShootingMachine.FireRate;
        }
        e.ShootingMachine.CoolDown -= Time.deltaTime;
    }
    protected void LookAtTargetEventHandler(LookAtTargetEvent e)
    {
        if (e.ShootingMachine.Target == null)
            return;

        if (e.ShootingMachine.Target != null)
        {
            if (e.ShootingMachine.Target.gameObject.activeSelf)
            {
                Vector3 direction = e.ShootingMachine.Target.transform.position - e.ShootingMachine.PartToRotate.position;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                if (e.ShootingMachine.PartToRotate is not null)
                {
                    Vector3 rotation = Quaternion.Lerp(e.ShootingMachine.PartToRotate.rotation, lookRotation, Time.deltaTime * Handler.RotationSpeed).eulerAngles;
                    e.ShootingMachine.PartToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);
                }
            }

        }

    }

    protected void BulletEventhandler(BulletEvent e)
    {
        if (e.Target == null)
        {
            Vault.ObjectPoolManager.Instance.ReturnToPool(e.Current);
            return; // Exit early if there's no target
        }

        // Calculate the target position with additional height
        Vector3 additionalHeight = e.Target.position + new Vector3(0, 1.5f, 0);
        Vector3 direction = (additionalHeight - e.Current.transform.position).normalized;
        float distanceThisFrame = e.Speed * Time.deltaTime;

        // Translate the bullet towards the target
        e.Current.transform.Translate(direction * distanceThisFrame, Space.World);

        // Constrain rotation to the Y-axis only
        if (direction != Vector3.zero) // Ensure the direction isn't zero to avoid errors
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            e.Current.transform.rotation = Quaternion.Euler(-90, targetRotation.eulerAngles.y, 0);
        }
    }


    protected void RocketEventHandler(RocketEvent e)
    {
        /*
        e.Current.transform.GetComponent<Bullet>().velocity += new Vector3(0, e.Gravity * Time.deltaTime, 0);

        e.Current.transform.position += e.Current.transform.GetComponent<Bullet>().velocity * Time.deltaTime;

        if (e.Current.transform.GetComponent<Bullet>().velocity != Vector3.zero)
        {
            e.Current.transform.rotation = Quaternion.LookRotation(e.Current.transform.GetComponent<Bullet>().velocity);
        }
        */
        
        e.Current.transform.GetComponent<Bullet>().velocity += new Vector3(0, e.Gravity, 0) * Time.deltaTime;

        // Update the position based on the current velocity
        e.Current.transform.position += e.Current.transform.GetComponent<Bullet>().velocity * (e.Current.transform.GetComponent<Bullet>()._speed * Time.deltaTime);

        // Calculate the rotation to face the direction of the velocity
        if (e.Current.transform.GetComponent<Bullet>().velocity != Vector3.zero)
        {
            e.Current.transform.rotation = Quaternion.LookRotation(e.Current.transform.GetComponent<Bullet>().velocity);
        }
    }


    protected void RocketBlastEventHandler(RocketBlastEvent e)
    {
        Collider[] colliders = Physics.OverlapSphere(e.Current.transform.position, e.BlastRadius);
        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(e.AttackPower);
                GameObject effect = MonoHelper.Instance.InstantiateObject(e.Current.transform.GetComponent<Bullet>().BlastEffect,
                    e.Current.transform.position, Quaternion.identity);
                effect.transform.GetComponent<ParticleSystem>().Play();
                MonoHelper.Instance.DestroyObject(effect, 1f);
            }
        }
    }

    protected void CalculateRocketVelocityEventHandler(CalcualteRocketVelocityEvent e)
    {
        if (e.BulletType == BulletType.Rocket && e.Current.Target != null)
        {
            Vector3 targetPosition = e.Current.Target.transform.position;
            Vector3 startPosition = e.Current.transform.position;

            Vector3 direction = targetPosition - startPosition;
            float h = direction.y;
            direction.y = 0;
            float distance = direction.magnitude;

            float angleDeg = 45f;
            float gravity = 9.81f;

            float angle = angleDeg * Mathf.Deg2Rad;

            float velocitySqr = (gravity * distance * distance) / 
                                (2 * (h - Mathf.Tan(angle) * distance) * Mathf.Pow(Mathf.Cos(angle), 2));

            if (velocitySqr <= 0 || float.IsNaN(velocitySqr))
            {
                Debug.LogWarning("Invalid velocity calculation. Check values!");
                return;
            }

            float velocityMagnitude = Mathf.Sqrt(velocitySqr);
            Vector3 velocityY = Vector3.up * velocityMagnitude * Mathf.Sin(angle);
            Vector3 velocityXZ = direction.normalized * velocityMagnitude * Mathf.Cos(angle);

            Bullet bullet = e.Current.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.velocity = velocityXZ + velocityY;
            }
        }
    }



    protected void LaserShootEventHandler(LaserShootEvent e)
    {
        if (e.ShootingMachine.CoolDown <= 0)
        {
            if (e.ShootingMachine.Target != null && e.ShootingMachine.TurretDataScriptable != null)
            {
                if (e.ShootingMachine.LaserPointer != null)
                {
                    e.ShootingMachine.LaserPointer.gameObject.SetActive(true);
                    e.ShootingMachine.LaserPointer.transform.position = e.ShootingMachine.Target.transform.position + new Vector3(0, 1.5f, 0);
                    e.ShootingMachine.LaserPointer.AttackPower = e.ShootingMachine.TurretDataScriptable.AttackPower;
                    e.ShootingMachine.LaserPointer.Target = e.ShootingMachine.Target.transform;
                }

            }
            e.ShootingMachine.CoolDown = 1f / e.ShootingMachine.FireRate;
        }
        e.ShootingMachine.CoolDown -= Time.deltaTime;
    }

    protected void ChangeToIdleAnimationEventHandler(ChangeToIdleAnimationEvent e)
    {
        GenericEventsController.Instance.ChangeAnimationEvent(e.PlayerAnimator, GameConstants.CharacterIdle);

    }

    protected void UpdateHireTimerEventHandler(UpdateHireTimerEvent e)
    {
        if (e.Character.Timer > 0)
        {
            e.Character.Timer -= Time.deltaTime;
            UpdateTimer(e.Character.TimerText, e.Character.Timer);
        }
        else
        {
            e.Character.Timer = 0;
            UpdateTimer(e.Character.TimerText, e.Character.Timer);
            EventManager.Instance.TriggerEvent(new RemoveSpawnedCharactersEvent(e.Character));
            MonoHelper.Instance.DestroyObject(e.Character.gameObject);

        }

    }


    #endregion

    #region Functions

    private void UpdateTimer(TMP_Text TimerText, float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    #endregion
}
