using UnityEngine;

public class XAxisSinRotate : SinRotate
{
    protected override void Start()
    {
        base.Start();
        startingAngle = transform.localRotation.eulerAngles.x;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.localRotation = Quaternion.Euler(startingAngle + currentAngle, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }
}
