using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAxisSinRotate : SinRotate
{
    protected override void Start()
    {
        base.Start();
        startingAngle = transform.localRotation.eulerAngles.z;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, startingAngle + currentAngle);
    }
}
