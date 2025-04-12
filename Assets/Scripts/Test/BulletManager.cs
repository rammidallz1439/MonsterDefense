using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    public Vector3 TargetPos;
    private Vector3 positionToGo;
    private void Start()
    {
        CalculateVelocity();
        Destroy(gameObject, 2f);
    }

    private void Update()
    {
        transform.position += positionToGo* _bulletSpeed * Time.deltaTime; 
    }


    void CalculateVelocity()
    {
        Vector3 direction = TargetPos - transform.position;
        direction.y = 0;
        float velocityMagnitude = Mathf.Sqrt((direction.magnitude * Mathf.Abs(-9.8f)) / Mathf.Sin(2 * 45 * Mathf.Deg2Rad));
        Vector3 Y = Vector3.up * velocityMagnitude * Mathf.Sin(45 * Mathf.Deg2Rad);
        Vector3 Z = direction.normalized * velocityMagnitude * Mathf.Cos(45 * Mathf.Deg2Rad);
        positionToGo = Z + Y;
    }
}
