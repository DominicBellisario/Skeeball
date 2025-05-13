using UnityEngine;

public class ZAxisSinMovement : SinMovement
{
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (useTranslate) 
        {
            if (useLocalCoordinateSystem)
            {
                transform.Translate(new Vector3(0,0,1) * currentPos * Time.deltaTime, Space.Self);
            }
            else
            {
                transform.Translate(new Vector3(0,0,1) * currentPos * Time.deltaTime, Space.World);
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startingWorldPos.z + currentPos);
        }
    }
}
