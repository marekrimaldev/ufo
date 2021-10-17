using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryChecker : MonoBehaviour
{
    [SerializeField] private Transform _leftDownCorner;
    [SerializeField] private Transform _topRightCorner;

    private void Update()
    {
        KeepInsideBoundary();
    }

    private void KeepInsideBoundary()
    {
        Vector3 newPosition = transform.position;
        if (transform.position.x < _leftDownCorner.position.x)
        {
            newPosition.x = _leftDownCorner.position.x;
        }
        else if (transform.position.x > _topRightCorner.position.x)
        {
            newPosition.x = _topRightCorner.position.x;
        }

        if (transform.position.y < _leftDownCorner.position.y)
        {
            newPosition.y = _leftDownCorner.position.y;
        }
        else if (transform.position.y > _topRightCorner.position.y)
        {
            newPosition.y = _topRightCorner.position.y;
        }

        transform.position = newPosition;
    }
}
