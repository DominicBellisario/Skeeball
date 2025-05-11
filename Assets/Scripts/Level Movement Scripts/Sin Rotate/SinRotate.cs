using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SinRotate : MonoBehaviour
{
    [SerializeField] protected float rotateSpeed;
    [SerializeField] protected float magnitude;
    [SerializeField] protected float startingWavePos;
    protected float currentWavePos;
    protected float currentAngle;
    protected float startingAngle;

    protected virtual void Start()
    {
        currentWavePos = startingWavePos * Mathf.Deg2Rad;
        startingAngle = transform.localRotation.eulerAngles.x;
    }

    protected virtual void Update()
    {
        currentWavePos += rotateSpeed * Mathf.Deg2Rad * Time.deltaTime;
        currentAngle = Mathf.Sin(currentWavePos) * magnitude;
    }
}
