using UnityEngine;

public class ZAxisConstantRotate : ConstantRotate
{
    // Update is called once per frame
    void Update()
    {
        //rotate the shape on the z axis
        transform.Rotate(new Vector3(0, 0, 1), rotateSpeed * Time.deltaTime);
    }
}
