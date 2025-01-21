using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupVisuals : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed;

    [SerializeField]
    float bobSpeed;
    [SerializeField]
    float bobStrength;
    float currentBob;

    private void Start()
    {
        currentBob = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //rotate the shape
        transform.Rotate(new Vector3(0, 1, 0), rotateSpeed * Time.deltaTime);
        
        currentBob += Time.deltaTime;

        //bob the shape
        transform.position = new Vector3(transform.position.x, Mathf.Sin(currentBob * bobSpeed) * bobStrength, transform.position.z);
    }
}
