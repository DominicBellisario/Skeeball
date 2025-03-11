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
            if (!Helper.Instance.HasMaxPowerups(Manager.Instance.GoldBallPow))
            {
                Manager.Instance.GoldBallPow++;
                Manager.Instance.SpawnCollectEffect(trigger.gameObject.transform.position, LevelUILogic.Instance.GoldBallButtonPos, LevelUILogic.Instance.gameObject, 1);
                LevelUILogic.Instance.UpdatePowerups();
            }

            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.CompareTag("MarkedBallPowerup"))
        {
            if (!Helper.Instance.HasMaxPowerups(Manager.Instance.MarkedBallPow))
            {
                Manager.Instance.MarkedBallPow++;
                Manager.Instance.SpawnCollectEffect(trigger.gameObject.transform.position, LevelUILogic.Instance.MarkedBallButtonPos, LevelUILogic.Instance.gameObject, 2);
                LevelUILogic.Instance.UpdatePowerups();
            }
            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.CompareTag("TriBallPowerup"))
        {
            if (!Helper.Instance.HasMaxPowerups(Manager.Instance.TriBallPow))
            {
                Manager.Instance.TriBallPow++;
                Manager.Instance.SpawnCollectEffect(trigger.gameObject.transform.position, LevelUILogic.Instance.TriBallButtonPos, LevelUILogic.Instance.gameObject, 3);
                LevelUILogic.Instance.UpdatePowerups();
            }
            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.CompareTag("LobBallPowerup"))
        {
            if (!Helper.Instance.HasMaxPowerups(Manager.Instance.LobBallPow))
            {
                Manager.Instance.LobBallPow++;
                Manager.Instance.SpawnCollectEffect(trigger.gameObject.transform.position, LevelUILogic.Instance.LobBallButtonPos, LevelUILogic.Instance.gameObject, 4);
                LevelUILogic.Instance.UpdatePowerups();
            }
            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.CompareTag("2BallPowerup"))
        {
            Manager.Instance.UpdateObjects(2);
            Manager.Instance.SpawnCollectEffect(trigger.gameObject.transform.position, LevelUILogic.Instance.BallsTextPos, LevelUILogic.Instance.gameObject, 5);
            Destroy(trigger.gameObject);
        }

        //if the object hits a coin, destroy it and add to the coin count
        else if (trigger.gameObject.CompareTag("Coin"))
        {
            Manager.Instance.UpdateCoins(trigger.gameObject.GetComponent<Coin>().Value);
            Manager.Instance.SpawnCollectEffect(trigger.gameObject.transform.position, LevelUILogic.Instance.CoinsTextPos, LevelUILogic.Instance.gameObject, 0);
            Destroy(trigger.gameObject);
        }

        //if the object hits a spring, launch it in the direction the spring is facing
        else if (trigger.gameObject.CompareTag("Spring"))
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(trigger.gameObject.transform.up * trigger.gameObject.GetComponentInParent<Spring>().ForceMultiplier);
            trigger.gameObject.GetComponentInParent<Spring>().ActivateSpring();
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

            //if the hole was not bad, they scored
            if (points >= 0)
            {
                Manager.Instance.Scored = true;
            }

            //if the hole was an activated multiplier hole, add to the multiplier
            if (Manager.Instance.ActivatedMultiHoles.Contains(trigger.gameObject.GetComponentInParent<HoleVariables>().gameObject))
            {
                Manager.Instance.UpdateMultiplier(Manager.Instance.MultiplierIncreaseAmt);
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
            trigger.gameObject.GetComponentInParent<HoleVariables>().SpawnHoleText(gold, transform.position);

            //if the object was marked, double the hole's point value and make it glow
            if (GetComponent<ObjectEffects>().MarkedBallEnabled)
            {
                trigger.gameObject.GetComponentInParent<HoleVariables>().MakeMarkedHole(true);
            }

            //destroy the object if the hole requires that
            if (trigger.GetComponentInParent<HoleVariables>().DestroyBall)
            {
                Manager.Instance.DestroyObject(gameObject);
            }
        }
        //if the object is fully within a star hole
        if (trigger.gameObject.CompareTag("StarHoleActivateTrigger"))
        {
            int points;

            //the next level will be secret
            Manager.Instance.NextLevelIsSecret = true;

            //the hole is not bad, they scored
            Manager.Instance.Scored = true;

            //player is guarenteed enough points to win
            if (Manager.Instance.MinScore > Manager.Instance.Score)
            {
                points = Manager.Instance.MinScore - Manager.Instance.Score;
            }
            else
            {
                points = 0;
            }
            Manager.Instance.UpdateScore(points);
            Manager.Instance.UpdateTotalScore(points);

            //spawn hole text
            trigger.gameObject.GetComponentInParent<StarHoleVariables>().SpawnHoleText(points, transform.position);

            //destroy the object if the hole requires that
            if (trigger.GetComponentInParent<StarHoleVariables>().DestroyBall)
            {
                Manager.Instance.DestroyObject(gameObject);
            }
        }
    }
}
