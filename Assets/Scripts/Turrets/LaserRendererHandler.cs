using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRendererHandler : MonoBehaviour
{
    [SerializeField] private Transform _originPoint;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private LineRenderer _lineRenderer;


    private void Update()
    {
        UpdateLine();
    }

    void UpdateLine()
    {
        _lineRenderer.SetPosition(0,_originPoint.position);
        _lineRenderer.SetPosition(1,_targetPoint.position);
    }
}
