using UnityEngine;

public class XAxisSinMovement : SinMovement
{
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (useTranslate) 
        {
            if (useLocalCoordinateSystem)
            {
                transform.Translate(new Vector3(1,0,0) * currentPos * Time.deltaTime, Space.Self);
            }
            else
            {
                transform.Translate(new Vector3(1,0,0) * currentPos * Time.deltaTime, Space.World);
            }
        }
        else
        {
            transform.position = new Vector3(startingWorldPos.x + currentPos, transform.position.y, transform.position.z);
        }
    }
}
