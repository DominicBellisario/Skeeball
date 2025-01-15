using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class BallHoleInteractions : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider trigger)
    {
        //if the ball enters a hole, disable its collision with the floor so it can pass through
        if (trigger.gameObject.tag == "DisableFloorTrigger")
        {
            rb.excludeLayers = LayerMask.GetMask("Surface");
        }
    }

    private void OnTriggerExit(Collider trigger)
    {
        //if the ball is fully within a hole
        if (trigger.gameObject.tag == "HoleActivateTrigger")
        {
            //add points to the total point count
            LevelManager.Instance.UpdateScore(trigger.GetComponentInParent<HoleVariables>().Points);

            //destroy the ball if needed
            if (trigger.GetComponentInParent<HoleVariables>().DestroyBall)
            {
                Destroy(gameObject, 0);
            }
        }
    }
}
