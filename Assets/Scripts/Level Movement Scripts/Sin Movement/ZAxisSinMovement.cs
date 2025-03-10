using UnityEngine;

public class ZAxisSinMovement : SinMovement
{
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.position = new Vector3(transform.position.x, transform.position.y, startingWorldPos.z + currentPos);
    }
}
