using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAxisEasingMovement : EasingMovement
{

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.position = new Vector3 (transform.position.x, transform.position.y, startingPos.z + currentPos);
    }
}
