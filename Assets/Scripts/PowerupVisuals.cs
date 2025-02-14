using UnityEngine;

public class PowerupVisuals : MonoBehaviour
{
    [SerializeField] float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        //rotate the shape
        transform.Rotate(new Vector3(0, 1, 0), rotateSpeed * Time.deltaTime);
    }
}
