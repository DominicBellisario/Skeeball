using UnityEngine;

public abstract class ObjectControls : MonoBehaviour
{
    protected ObjectLevelInteractions objectLevelInteractions;

    //the in game and pixel locations of where the object starts
    protected Vector3 objectOrigin;
    protected Vector2 objectPixelOrigin;

    //the object's rigidbody
    protected Rigidbody rb;

    //the radius of the circle the object can move within while aiming
    [SerializeField] protected float aimingCircleRadius;

    //if the ball is within this radius when it is released, it does not launch
    [SerializeField] protected float tooCloseRadius;

    //the pixel radius of the circle the player can move their finger in to control the object's speed
    protected float pixelAimingCircleRadius;

    //wether or not the player is aiming the object
    protected bool isHeld;
    protected bool isLaunched;

    //the angle that the ball will travel in once released
    protected float angle;

    //the percent of max launch force that will be applied to the object
    protected float powerPercent;

    [SerializeField] protected int forceMultiplyer;

    [SerializeField] protected float triBallAngle;
    protected float triBallAngleRads;

    //properties

    //used by BallEffects to determine the visibility, angle, length, and color of the aiming line
    public bool IsHeld { get { return isHeld; } }
    public bool IsLaunched { get { return isLaunched; } set { isLaunched = value; } }
    public float Angle { get { return angle; } }
    public float PowerPercent { get { return powerPercent; } }
    public float TriBallAngleRads { get { return triBallAngleRads; } }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectLevelInteractions = GetComponent<ObjectLevelInteractions>();

        objectOrigin = gameObject.transform.position;
        objectPixelOrigin = new Vector2(Screen.width / 2, Screen.height / 7f);

        pixelAimingCircleRadius = Screen.width * 0.25f;

        isHeld = false;
        isLaunched = false;

        angle = 0;
        powerPercent = 0;

        triBallAngleRads = Mathf.Deg2Rad * triBallAngle;
    }

    // Update is called once per frame
    void Update()
    {
        //the player is aiming the object
        if (isHeld)
        {
            rb.Sleep();
            //get the angle between the pixel origin and current mouse pos
            angle = Mathf.Atan2(Input.mousePosition.x - objectPixelOrigin.x, Input.mousePosition.y - objectPixelOrigin.y);

            //get the percentage of max force that will be applied to the object upon release
            powerPercent = Vector2.Distance(objectPixelOrigin, Input.mousePosition) / pixelAimingCircleRadius;
            if (powerPercent > 1)
            {
                powerPercent = 1;
            }

            //update the object position
            transform.position = new Vector3(
                objectOrigin.x + (Mathf.Sin(angle) * aimingCircleRadius * powerPercent),
                objectOrigin.y,
                objectOrigin.z + (Mathf.Cos(angle) * aimingCircleRadius * powerPercent)
                );
        }
        //Debug.Log(rb.velocity.magnitude);

        //if the player releases the mouse
        if (!Input.GetMouseButton(0))
        {
            //if the player was previously holding the object and just released it
            if (isHeld)
            {
                Manager manager = Manager.Instance;
                ObjectEffects effects = GetComponent<ObjectEffects>();

                //if it was within the too close radius, reset it
                if (Vector2.Distance(new Vector2(objectOrigin.x, objectOrigin.z), new Vector2(transform.position.x, transform.position.z)) <= tooCloseRadius)
                {
                    transform.position = objectOrigin;
                }
                //if not, launch it
                else
                {
                    LaunchObject();
                    isLaunched = true;
                    objectLevelInteractions.IsLaunched = true;

                    effects.ResetParticleTrail();

                    //use powerups if they were applied to the ball
                    if (effects.GoldBallEnabled)
                    {
                        manager.GoldBallPow--;
                    }
                    if (effects.MarkedBallEnabled)
                    {
                        manager.MarkedBallPow--;
                    }
                    if (effects.TriBallEnabled)
                    {
                        manager.TriBallPow--;
                        effects.DisableTriBalls();
                        //spawn 2 new objects and make their powerup states the same as the parent
                        SpawnNewTriObjects(effects);
                    }
                    if (manager.LobBallEnabled)
                    {
                        manager.LobBallPow--;
                        manager.LobBallEnabled = false;
                    }

                    if (manager.SwitchCameraOnLaunch)
                    {
                        manager.SwitchCameraView(1);
                    }
                    LevelUILogic.Instance.UpdatePowerups();
                }
                isHeld = false;
            }
        }
    }

    //if the player clicks the ball in the main camera view, they are holding it
    protected void OnMouseDown()
    {
        if (Manager.Instance.MainCamera.activeInHierarchy)
        {
            isHeld = true;
        }
    }

    protected abstract void LaunchObject();

    protected abstract void SpawnNewTriObjects(ObjectEffects effects);
}

