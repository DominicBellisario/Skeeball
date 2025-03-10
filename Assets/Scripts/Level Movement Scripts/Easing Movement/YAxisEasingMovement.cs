using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YAxisEasingMovement : EasingMovement
{
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.position = new Vector3(transform.position.x, startingPos.y + currentPos, transform.position.z);
    }
}
