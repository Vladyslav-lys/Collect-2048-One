using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBtn : MonoBehaviour
{
    public Transform wallTransform;
    public Vector3 offset;
    public float wallSpeed;
    private Vector3 _targetVector;

    private void Awake()
    {
        _targetVector = wallTransform.position + offset;
    }

    private void Update()
    {
        wallTransform.position = 
            Vector3.MoveTowards(wallTransform.position, _targetVector, wallSpeed*Time.deltaTime);

        if (wallTransform.position == _targetVector)
            enabled = false;
    }
}
