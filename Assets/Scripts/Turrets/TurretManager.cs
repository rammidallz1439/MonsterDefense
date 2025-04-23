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
        if (e.ShootingMachineBase.CoolDown <= 0)
        {
            if (e.ShootingMachineBase.Target != null)
            {
                GameObject ammo = ObjectPoolManager.Instance.Get(e.ShootingMachineBase.TurretDataScriptable.Bullet.gameObject.name, true);
                ammo.transform.localPosition = e.ShootingMachineBase.SpawnPoint.localPosition;
                Bullet b = ammo.transform.GetComponent<Bullet>();
                b.AttackPower = e.ShootingMachineBase.AttackPower;
                b.Target = e.ShootingMachineBase.Target.transform;
                b.TargetPoint = e.ShootingMachineBase.Target.transform.position;
                GenericEventsController.Instance.ChangeAnimationEvent(e.ShootingMachineBase.Animator, GameConstants.CharacterAttack);

            }
            e.ShootingMachineBase.CoolDown = 1f / e.ShootingMachineBase.FireRate;
        }
        e.ShootingMachineBase.CoolDown -= Time.deltaTime;
    }

    protected void SpawnBulletEventHandler(SpawnBulletEvent e)
    {
        GameObject ammo = ObjectPoolManager.Instance.Get(e.ShootingMachineBase.TurretDataScriptable.Bullet.gameObject.name, true);
        // ammo.transform.localPosition = e.ShootingMachineBase.SpawnPoint.localPosition;
        Bullet b = ammo.transform.GetComponent<Bullet>();
        b.AttackPower = e.ShootingMachineBase.AttackPower;

        if (e.ShootingMachineBase.Target != null)
        {
            b.Target = e.ShootingMachineBase.Target.transform;
            b.TargetPoint = e.ShootingMachineBase.Target.transform.position;
        }
    }
    protected void LookAtTargetEventHandler(LookAtTargetEvent e)
    {
        if (e.Target == null)
            return;

        if (e.Target != null)
        {
            if (e.Target.gameObject.activeSelf)
            {
                Vector3 direction = e.Target.transform.position - e.PartToRotate.position;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                if (e.PartToRotate is not null)
                {
                    Vector3 rotation = Quaternion.Lerp(e.PartToRotate.rotation, lookRotation, Time.deltaTime * Handler.RotationSpeed).eulerAngles;
                    e.PartToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);
                }
            }

        }

    }

    protected void BulletEventhandler(BulletEvent e)
    {
        if (e.Bullet.Target == null)
        {
            Vault.ObjectPoolManager.Instance.ReturnToPool(e.Bullet.gameObject);
            return; // Exit early if there's no target
        }

        // Calculate the target position with additional height
        Vector3 additionalHeight = e.Bullet.Target.position + new Vector3(0, 1.5f, 0);
        Vector3 direction = (additionalHeight - e.Bullet.transform.position).normalized;
        float distanceThisFrame = e.Bullet._speed * Time.deltaTime;

        // Translate the bullet towards the target
        e.Bullet.transform.Translate(direction * distanceThisFrame, Space.World);

        // Constrain rotation to the Y-axis only
        if (direction != Vector3.zero) // Ensure the direction isn't zero to avoid errors
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            e.Bullet.transform.rotation = Quaternion.Euler(-90, targetRotation.eulerAngles.y, 0);
        }
    }


    protected void RocketEventHandler(RocketEvent e)
    {

        e.Bullet.velocity += new Vector3(0, e.Bullet._garvity, 0) * Time.deltaTime;

        // Update the position based on the current velocity
        e.Bullet.transform.position += e.Bullet.velocity * e.Bullet._speed * Time.deltaTime;

        // Calculate the rotation to face the direction of the velocity
        if (e.Bullet.velocity != Vector3.zero)
        {
            e.Bullet.transform.rotation = Quaternion.LookRotation(e.Bullet.velocity);
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
        if (e.ShootingMachineBase.CoolDown <= 0)
        {
            if (e.ShootingMachineBase.Target != null )
            {
                if (e.ShootingMachineBase.LaserPointer != null)
                {
                    e.ShootingMachineBase.LaserPointer.gameObject.SetActive(true);
                    e.ShootingMachineBase.LaserPointer.transform.position = e.ShootingMachineBase.Target.transform.position + new Vector3(0, 1.5f, 0);
                    e.ShootingMachineBase.LaserPointer.AttackPower = e.ShootingMachineBase.AttackPower;
                    e.ShootingMachineBase.LaserPointer.Target = e.ShootingMachineBase.Target.transform;
                }

            }
            e.ShootingMachineBase.CoolDown = 1f / e.ShootingMachineBase.FireRate;
        }
        e.ShootingMachineBase.CoolDown -= Time.deltaTime;
    }

    protected void ChangeToIdleAnimationEventHandler(ChangeToIdleAnimationEvent e)
    {
        GenericEventsController.Instance.ChangeAnimationEvent(e.PlayerAnimator, GameConstants.CharacterIdle);

    }

    /*    protected void UpdateHireTimerEventHandler(UpdateHireTimerEvent e)
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
    */

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
