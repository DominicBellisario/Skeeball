using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YAxisSinRotate : SinRotate
{
    protected override void Start()
    {
        base.Start();
        startingAngle = transform.localRotation.eulerAngles.y;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, startingAngle + currentAngle, transform.localRotation.eulerAngles.z);
    }
}
