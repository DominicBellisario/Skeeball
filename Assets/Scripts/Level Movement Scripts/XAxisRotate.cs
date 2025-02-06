using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XAxisRotate : LevelRotate
{
    // Update is called once per frame
    void Update()
    {
        //rotate the shape on the x axis
        transform.Rotate(new Vector3(1, 0, 0), rotateSpeed * Time.deltaTime);
    }
}
