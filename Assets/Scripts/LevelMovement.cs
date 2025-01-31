using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelMovement : MonoBehaviour
{
    //angles per second
    [SerializeField]
    protected float speed;
    //how far the object travels
    [SerializeField]
    protected float magnitude;
    //the angle where the object starts in the wave
    [SerializeField]
    protected float startingWavePos = 0;
    //where the obkect starts in the world
    protected Vector3 startingWorldPos;
    
    protected float currentPos;
    float currentAngle;

    protected void Start()
    {
        currentAngle = startingWavePos * Mathf.Deg2Rad;
        startingWorldPos = transform.position;
    }
    protected virtual void Update()
    {
        currentAngle += speed * Mathf.Deg2Rad * Time.deltaTime;
        currentPos = Mathf.Sin(currentAngle) * magnitude;
    }
}
