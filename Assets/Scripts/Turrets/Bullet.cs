using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class Bullet : MonoBehaviour
{

    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] internal float _speed;
    [SerializeField] private BulletType _bulletType;
    public Transform Target = null;
    public float AttackPower;

    [Header("Rocket Data")]
    [SerializeField] private float _blastRadius;
    [SerializeField] private float _garvity;
    [SerializeField] private float _launchAngle;
    public Vector3 velocity;
    public GameObject BlastEffect = null;
    public Vector3 TargetPoint;

    private void OnEnable()
    {
        if (Target == null || Target.gameObject.activeSelf == false)
        {
            if (_bulletType != BulletType.Laser)
                Vault.ObjectPoolManager.Instance.ReturnToPool(gameObject);

            return;
        }
    }



    void LaunchProjectile()
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


    private void Update()
    {
        if(Target == null || Target.gameObject.activeSelf == false)
        {
            if (_bulletType != BulletType.Laser)
                Vault.ObjectPoolManager.Instance.ReturnToPool(gameObject);

            return;
        }
        CheckBulletType(_bulletType);
    }


    private void OnTriggerEnter(Collider other)
    {
        CheckAttackType(_bulletType, other);
        if (other.gameObject.tag is "Enemy" || other.gameObject.tag is "Ground")
        {
            if (_bulletType != BulletType.Laser)
                Vault.ObjectPoolManager.Instance.ReturnToPool(gameObject);

        }

    }


    private void CheckBulletType(BulletType type)
    {
        switch (type)
        {
            case BulletType.None:
                break;
            case BulletType.Bullet:
                EventManager.Instance.TriggerEvent(new BulletEvent(Target, gameObject, _speed));
                break;
            case BulletType.Rocket:
                // EventManager.Instance.TriggerEvent(new RocketEvent(Target, gameObject, _speed, _blastRadius, _garvity));
                LaunchProjectile();
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
