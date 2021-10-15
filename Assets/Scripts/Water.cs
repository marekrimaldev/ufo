using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float _horizontalSpeed = 1;
    [SerializeField] private float _verticalSpeed = 1;
    [SerializeField] private float _horizontalDistance = 1;
    [SerializeField] private float _verticalDistance = 1;
    [SerializeField] private bool _startLeft = true;
    [SerializeField] private bool _startUp = true;
    private Vector3[] _verticalDestinations = new Vector3[2];
    private Vector3[] _horizontalDestinations = new Vector3[2];
    private int _currVerticalIdx = 0;
    private int _currHorizontalIdx = 0;

    private Vector3 _translation;

    private void Start()
    {
        _verticalDestinations[0] = _startLeft ? transform.position + Vector3.left * _horizontalDistance : transform.position + Vector3.right * _horizontalDistance;
        _verticalDestinations[1] = _startLeft ? transform.position + Vector3.right * _horizontalDistance : transform.position + Vector3.left * _horizontalDistance;

        _horizontalDestinations[0] = _startUp ? transform.position + Vector3.up * _verticalDistance : transform.position + Vector3.down * _verticalDistance;
        _horizontalDestinations[1] = _startUp ? transform.position + Vector3.down * _verticalDistance : transform.position + Vector3.up * _verticalDistance;

        StartCoroutine(AnimateWater());
    }

    private void AddHorizontalTranslation()
    {
        Vector3 vecToDestination = _verticalDestinations[_currVerticalIdx] - transform.position;
        Vector3 dir = vecToDestination.normalized;
        _translation += dir * _horizontalSpeed;

        if (vecToDestination.magnitude < 0.01f)
        {
            _currVerticalIdx++;
            _currVerticalIdx %= _verticalDestinations.Length;
        }
    }

    private void AddVerticalTranslation()
    {
        Vector3 vecToDestination = _horizontalDestinations[_currHorizontalIdx] - transform.position;
        Vector3 dir = vecToDestination.normalized;
        transform.Translate(dir * _horizontalSpeed * Time.deltaTime, Space.World);
        _translation += dir * _verticalSpeed;

        if (vecToDestination.magnitude < 0.01f)
        {
            _currHorizontalIdx++;
            _currHorizontalIdx %= _horizontalDestinations.Length;
        }
    }

    private IEnumerator AnimateWater()
    {
        while (true)
        {
            _translation = Vector3.zero;
            AddHorizontalTranslation();
            AddVerticalTranslation();
            transform.Translate(_translation * Time.deltaTime, Space.World);

            yield return null;
        }
    }
}
