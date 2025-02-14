using UnityEngine;

public class XAxisMovement : LevelMovement
{
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.position = new Vector3(startingWorldPos.x + currentPos, transform.position.y, transform.position.z);
    }
}
