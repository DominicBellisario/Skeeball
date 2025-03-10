using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XAxisEasingMovement : EasingMovement
{
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.position = new Vector3(startingPos.x + currentPos, transform.position.y, transform.position.z);
    }
}
