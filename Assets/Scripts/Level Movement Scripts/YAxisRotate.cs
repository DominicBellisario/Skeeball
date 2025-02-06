using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YAxisRotate : LevelRotate
{
    // Update is called once per frame
    void Update()
    {
        //rotate the shape on the y axis
        transform.Rotate(new Vector3(0, 1, 0), rotateSpeed * Time.deltaTime);
    }
}
