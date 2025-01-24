using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditorInternal;
using UnityEngine;

public class BallLevelInteractions : MonoBehaviour
{
    Rigidbody rb;

    //how long the ball has before it is destoryed if it moves too slow
    [SerializeField]
    float tooSlowDespawnTime;
    float tooSlowDespawnTimer;

    bool isLaunched;

    public bool IsLaunched { get { return isLaunched; } set { isLaunched = value; } }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        isLaunched = false;
    }

    // Update is called once per frame
    void Update()
    {   
        //destroy the ball if it moves too slow for too long
        if (rb.velocity.magnitude <= 1 && isLaunched)
        {
            tooSlowDespawnTimer -= Time.deltaTime;
        }
        else
        {
            tooSlowDespawnTimer = tooSlowDespawnTime;
        }
        if (tooSlowDespawnTimer <= 0)
        {
            LevelManager.Instance.DestroyBall(gameObject);
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        //if the ball enters a hole, disable its collision with the floor so it can pass through
        if (trigger.gameObject.CompareTag("DisableFloorTrigger"))
        {
            rb.excludeLayers = LayerMask.GetMask("Surface", "Ball");
        }

        //destroy the ball if it hits the death plane
        else if (trigger.gameObject.CompareTag("DeathPlain"))
        {
            LevelManager.Instance.DestroyBall(gameObject);
        }

        //if the ball hits a powerup, destroy it and add it to the inventory
        else if (trigger.gameObject.CompareTag("GoldenBallPowerup"))
        {
            LevelManager.Instance.GoldBallPow++;
            LevelUILogic.Instance.UpdatePowerups();
            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.CompareTag("MarkedBallPowerup"))
        {
            LevelManager.Instance.MarkedBallPow++;
            LevelUILogic.Instance.UpdatePowerups();
            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.CompareTag("TriBallPowerup"))
        {
            LevelManager.Instance.TriBallPow++;
            LevelUILogic.Instance.UpdatePowerups();
            Destroy(trigger.gameObject);
        }
    }

    private void OnTriggerExit(Collider trigger)
    {
        //if the ball is fully within a hole
        if (trigger.gameObject.CompareTag("HoleActivateTrigger"))
        {
            //add points to the total point count.  x2 points if gold ball
            if (GetComponent<BallEffects>().GoldBallEnabled)
            {
                LevelManager.Instance.UpdateScore(trigger.GetComponentInParent<HoleVariables>().Points * 2);
            }
            else
            {
                LevelManager.Instance.UpdateScore(trigger.GetComponentInParent<HoleVariables>().Points);
            }

            //if the ball was marked, double the hole's point value
            if (GetComponent<BallEffects>().MarkedBallEnabled)
            {
                trigger.gameObject.GetComponentInParent<HoleVariables>().Points *= 2;
            }

            //destroy the ball if the hole requires that
            if (trigger.GetComponentInParent<HoleVariables>().DestroyBall)
            {
                LevelManager.Instance.DestroyBall(gameObject);
            }
        }
    }
}

