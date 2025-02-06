using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAxisRotate : LevelRotate
{
    // Update is called once per frame
    void Update()
    {
        //rotate the shape on the z axis
        transform.Rotate(new Vector3(0, 0, 1), rotateSpeed * Time.deltaTime);
    }
}
