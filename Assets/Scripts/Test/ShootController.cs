using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootController : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _spawnPoint;
    public LayerMask groundLayer;
  
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPoint = hit.point;
            GameObject bullet = Instantiate(_bullet, _spawnPoint.position, Quaternion.identity);
            LaunchProjectile(bullet, targetPoint);
        }
    }
    void LaunchProjectile(GameObject bullet, Vector3 targetPoint)
    {
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        Vector3 direction = targetPoint - _spawnPoint.position;
        Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z);
        float horizontalDistance = horizontalDirection.magnitude; 

        float heightDifference = direction.y; 
        float gravity = Mathf.Abs(Physics.gravity.y);

        float launchAngle = 45f * Mathf.Deg2Rad;


        float initialVelocityXZ = Mathf.Sqrt(gravity * horizontalDistance * horizontalDistance /
                                  (2 * (horizontalDistance * Mathf.Tan(launchAngle) - heightDifference)));

        float velocityX = initialVelocityXZ * horizontalDirection.normalized.x;
        float velocityZ = initialVelocityXZ * horizontalDirection.normalized.z;
        float velocityY = initialVelocityXZ * Mathf.Tan(launchAngle);

        Vector3 initialVelocity = new Vector3(velocityX, velocityY, velocityZ);
        bulletRb.velocity = initialVelocity;
    }


}
