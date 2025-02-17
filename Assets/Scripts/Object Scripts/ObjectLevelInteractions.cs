using System.Linq;
using UnityEngine;

public abstract class ObjectLevelInteractions : MonoBehaviour
{
    protected Rigidbody rb;

    //how long the object has before it is destoryed if it moves too slow
    [SerializeField] protected float tooSlowDespawnTime;
    protected float tooSlowDespawnTimer;

    protected bool isLaunched;

    public bool IsLaunched { get { return isLaunched; } set { isLaunched = value; } }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tooSlowDespawnTimer = tooSlowDespawnTime;
        isLaunched = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //destroy the object if it moves too slow for too long
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
            Manager.Instance.DestroyObject(gameObject);
        }
    }

    protected virtual void OnTriggerEnter(Collider trigger)
    {
        //if the object enters a hole, disable its collision with the floor so it can pass through
        if (trigger.gameObject.CompareTag("DisableFloorTrigger"))
        {
            rb.excludeLayers = LayerMask.GetMask("Surface", "Ball");
        }

        //destroy the object if it hits the death plane
        else if (trigger.gameObject.CompareTag("DeathPlain"))
        {
            Manager.Instance.DestroyObject(gameObject);
        }

        //if the object hits a powerup, destroy it and add it to the inventory
        else if (trigger.gameObject.CompareTag("GoldenBallPowerup"))
        {
            Manager.Instance.GoldBallPow++;
            LevelUILogic.Instance.UpdatePowerups();
            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.CompareTag("MarkedBallPowerup"))
        {
            Manager.Instance.MarkedBallPow++;
            LevelUILogic.Instance.UpdatePowerups();
            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.CompareTag("TriBallPowerup"))
        {
            Manager.Instance.TriBallPow++;
            LevelUILogic.Instance.UpdatePowerups();
            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.CompareTag("LobBallPowerup"))
        {
            Manager.Instance.LobBallPow++;
            LevelUILogic.Instance.UpdatePowerups();
            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.CompareTag("2BallPowerup"))
        {
            Manager.Instance.UpdateObjects(2);
            Destroy(trigger.gameObject);
        }

        //if the object hits a coin, destroy it and add to the coin count
        else if (trigger.gameObject.CompareTag("Coin"))
        {
            Manager.Instance.UpdateCoins(trigger.gameObject.GetComponent<Coin>().Value);
            Destroy(trigger.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //apply a force in the direction of the fan if it is within its effect
        if (other.gameObject.CompareTag("Fan"))
        {
            rb.AddForce(other.gameObject.transform.up * other.gameObject.GetComponent<FanVariables>().ForceMultiplier * Time.deltaTime);
        }
    }

    protected virtual void OnTriggerExit(Collider trigger)
    {
        //if the object is fully within a hole
        if (trigger.gameObject.CompareTag("HoleActivateTrigger"))
        {
            bool gold = false;
            //get the hole's value
            int points = trigger.GetComponentInParent<HoleVariables>().Points;

            //if the hole was an activated multiplier hole, add to the multiplier
            if (Manager.Instance.ActivatedMultiHoles.Contains(trigger.gameObject.GetComponentInParent<HoleVariables>().gameObject))
            {
                Debug.Log("multi");
                Manager.Instance.UpdateMultiplier(0.5f);
            }

            //x2 points if gold ball
            if (GetComponent<ObjectEffects>().GoldBallEnabled)
            {
                points *= 2;
                gold = true;
            }

            //add points to point count
            Manager.Instance.UpdateScore(points);
            if (Manager.Instance.Endless)
            {
                Manager.Instance.UpdateTotalScore(points);
            }

            //spawn hole text
            trigger.gameObject.GetComponentInParent<HoleVariables>().SpawnHoleText(gold);

            //if the object was marked, double the hole's point value and make it glow
            if (GetComponent<ObjectEffects>().MarkedBallEnabled)
            {
                trigger.gameObject.GetComponentInParent<HoleVariables>().MarkHole();
            }

            //destroy the object if the hole requires that
            if (trigger.GetComponentInParent<HoleVariables>().DestroyBall)
            {
                Manager.Instance.DestroyObject(gameObject);
            }
        }
    }
}
