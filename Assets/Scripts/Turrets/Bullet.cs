using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{

    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] internal float _speed;
    [SerializeField] private BulletType _bulletType;
    public Transform Target = null;
    public float AttackPower;

    [Header("Rocket Data")]
    public float _blastRadius;
    public float _garvity;
    public float _launchAngle;
    public Vector3 velocity;
    public GameObject BlastEffect = null;
    public Vector3 TargetPoint;

    private void OnEnable()
    {
        if (Target == null || Target.gameObject.activeSelf == false)
        {
            if (_bulletType != BulletType.Laser)
                Vault.ObjectPoolManager.Instance.ReturnToPool(gameObject);
        }

        if (_bulletType == BulletType.Rocket && Target != null)
        {
            CalculateInitialVelocity();
        }
    }

    void CalculateInitialVelocity()
    {
        Vector3 direction = Target.position - transform.position;  // Direction to the target
        float h = direction.y;  // Height difference
        direction.y = 0;
        float distance = direction.magnitude - 1;  // Horizontal distance
        float angle = _launchAngle * Mathf.Deg2Rad;  // Throw angle in radians (adjust as needed)

        // Calculate initial velocity magnitude
        float velocityMagnitude = Mathf.Sqrt(distance * Mathf.Abs(_garvity) / Mathf.Sin(2 * angle));
        Vector3 velocityY = Vector3.up * velocityMagnitude * Mathf.Sin(angle);
        Vector3 velocityXZ = direction.normalized * velocityMagnitude * Mathf.Cos(angle);

        // Combine to get the initial velocity
        velocity = velocityXZ + velocityY;
    }

    /*    void LaunchProjectile()
        {
            Rigidbody bulletRb = _rigidBody;
            Vector3 direction = TargetPoint - transform.position;
            Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z);
            float horizontalDistance = horizontalDirection.magnitude;

            float heightDifference = direction.y;
            float gravity = Mathf.Abs(Physics.gravity.y);

            float launchAngle = _launchAngle * Mathf.Deg2Rad;


            float initialVelocityXZ = Mathf.Sqrt(gravity * horizontalDistance * horizontalDistance /
                                                 (2 * (horizontalDistance * Mathf.Tan(launchAngle) - heightDifference)));

            float velocityX = initialVelocityXZ * horizontalDirection.normalized.x;
            float velocityZ = initialVelocityXZ * horizontalDirection.normalized.z;
            float velocityY = initialVelocityXZ * Mathf.Tan(launchAngle);

            Vector3 initialVelocity = new Vector3(velocityX, velocityY * _garvity * Time.deltaTime, velocityZ);
            bulletRb.velocity = initialVelocity;
            transform.rotation = Quaternion.LookRotation(bulletRb.velocity);
        }
    */

    private void Update()
    {
        if (Target == null || Target.gameObject.activeSelf == false)
        {
            if (_bulletType != BulletType.Laser)
                Destroy(gameObject);
        }

        CheckBulletType(_bulletType);
    }


    private void OnTriggerEnter(Collider other)
    {
        CheckAttackType(_bulletType, other);
        if (other.gameObject.tag is "Enemy" || other.gameObject.tag is "Ground")
        {
            if (_bulletType != BulletType.Laser)
                Destroy(gameObject);

        }

    }


    private void CheckBulletType(BulletType type)
    {
        switch (type)
        {
            case BulletType.None:
                break;
            case BulletType.Bullet:
                EventManager.Instance.TriggerEvent(new BulletEvent(this));
                break;
            case BulletType.Rocket:
                EventManager.Instance.TriggerEvent(new RocketEvent(this));
                // LaunchProjectile();
                break;
            case BulletType.Laser:
                break;
            default:
                break;
        }
    }

    private void CheckAttackType(BulletType type, Collider other)
    {
        switch (type)
        {
            case BulletType.None:
                break;
            case BulletType.Bullet:
                if (other.gameObject.tag is "Enemy")
                    other.transform.GetComponent<Enemy>().TakeDamage(AttackPower);
                break;
            case BulletType.Rocket:
                if (other.gameObject.tag is "Enemy" || other.gameObject.tag is "Ground")
                    EventManager.Instance.TriggerEvent(new RocketBlastEvent(AttackPower, gameObject, _blastRadius));
                break;
            case BulletType.Laser:
                if (other.gameObject.tag is "Enemy")
                {
                    if (other.transform != null)
                        other.transform.GetComponent<Enemy>().TakeDamage(AttackPower);

                    MEC.Timing.CallDelayed(0.3f, () =>
                    {
                        transform.position = transform.parent.transform.position;
                        gameObject.SetActive(false);
                    });

                }
                break;
            default:
                break;
        }
    }


}
