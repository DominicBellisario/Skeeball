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
            bool alreadyAtMax = Manager.Instance.UpdatePowerup(ref Manager.Instance.goldBallPow, 1);
            Manager.Instance.SpawnCollectEffect(trigger.gameObject.transform.position, LevelUILogic.Instance.GoldBallButtonPos, LevelUILogic.Instance.gameObject, 1, alreadyAtMax, "UpdateGoldBall");
            //LevelUILogic.Instance.UpdatePowerups();

            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.CompareTag("MarkedBallPowerup"))
        {
            bool alreadyAtMax = Manager.Instance.UpdatePowerup(ref Manager.Instance.markedBallPow, 1);
            Manager.Instance.SpawnCollectEffect(trigger.gameObject.transform.position, LevelUILogic.Instance.MarkedBallButtonPos, LevelUILogic.Instance.gameObject, 2, alreadyAtMax, "UpdateMarkedBall");
            //LevelUILogic.Instance.UpdatePowerups();
            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.CompareTag("TriBallPowerup"))
        {
            bool alreadyAtMax = Manager.Instance.UpdatePowerup(ref Manager.Instance.triBallPow, 1);
            Manager.Instance.SpawnCollectEffect(trigger.gameObject.transform.position, LevelUILogic.Instance.TriBallButtonPos, LevelUILogic.Instance.gameObject, 3, alreadyAtMax, "UpdateTriBall");
            //LevelUILogic.Instance.UpdatePowerups();

            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.CompareTag("LobBallPowerup"))
        {
            bool alreadyAtMax = Manager.Instance.UpdatePowerup(ref Manager.Instance.lobBallPow, 1);
            Manager.Instance.SpawnCollectEffect(trigger.gameObject.transform.position, LevelUILogic.Instance.LobBallButtonPos, LevelUILogic.Instance.gameObject, 4, alreadyAtMax, "UpdateLobBall");
            //LevelUILogic.Instance.UpdatePowerups();

            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.CompareTag("2BallPowerup"))
        {
            Manager.Instance.UpdateObjects(2);
            Manager.Instance.SpawnCollectEffect(trigger.gameObject.transform.position, LevelUILogic.Instance.BallsTextPos, LevelUILogic.Instance.gameObject, 5, false, "UpdateBalls");
            Destroy(trigger.gameObject);
        }

        //if the object hits a coin, destroy it and add to the coin count
        else if (trigger.gameObject.CompareTag("Coin"))
        {
            Manager.Instance.UpdateCoins(trigger.gameObject.GetComponent<Coin>().Value);
            Manager.Instance.SpawnCollectEffect(trigger.gameObject.transform.position, LevelUILogic.Instance.CoinsTextPos, LevelUILogic.Instance.gameObject, 0, false, "UpdateCoins");
            trigger.gameObject.GetComponent<Coin>().SpawnDeathParticles();
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
                Manager.Instance.SpawnTextEffect(transform.position, LevelUILogic.Instance.MultiplierTextPos, -300, LevelUILogic.Instance.gameObject, Manager.Instance.MultiplierIncreaseAmt.ToString(), "UpdateMultiplier");
            }

            //x2 points if gold ball
            if (GetComponent<ObjectEffects>().GoldBallEnabled)
            {
                points *= 2;
                gold = true;
            }

            //add points to point count
            Manager.Instance.UpdateScore(points);
            Manager.Instance.SpawnTextEffect(transform.position, LevelUILogic.Instance.ScoreTextPos, 0, LevelUILogic.Instance.gameObject, points.ToString(), "UpdateScore");
            if (Manager.Instance.Endless)
            {
                Manager.Instance.UpdateTotalScore(points);
                Manager.Instance.SpawnTextEffect(transform.position, LevelUILogic.Instance.TotalScoreTextPos, -150, LevelUILogic.Instance.gameObject, Mathf.RoundToInt(points * Manager.Instance.Multiplier).ToString(), "UpdateTotalScore");
            }

            //spawn hole text and ring
            trigger.gameObject.GetComponentInParent<HoleVariables>().SpawnHoleEffects(gold, transform.position);

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
