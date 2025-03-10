using System.Collections;
using UnityEngine;

public class EasingMovement : MonoBehaviour
{
    [SerializeField] protected float start;
    [SerializeField] protected float end;
    [SerializeField] protected float duration;

    float startTime;
    protected Vector3 startingPos;
    protected float currentPos;
    float t;
    bool forwards = true;

    protected void Start()
    {
        startTime = Time.time;
        startingPos = transform.position;
    }

    protected virtual void Update()
    {
        t = (Time.time - startTime + 0.001f) / duration;
        if (forwards) { currentPos = Mathf.SmoothStep(start, end, t); }
        else { currentPos = Mathf.SmoothStep(end, start, t); }
        
        if (t <= 0 || t >= 1)
        {
            forwards = !forwards;
            startTime = Time.time;
        }
    }
}
