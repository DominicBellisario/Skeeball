using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControls : MonoBehaviour
{
    BallLevelInteractions ballLevelInteractions;

    //the in game and pixel locations of where the ball starts
    Vector3 ballOrigin;
    Vector2 ballPixelOrigin;

    //the ball's rigidbody
    Rigidbody rb;

    //the radius of the circle the ball can move within while aiming
    [SerializeField]
    float aimingCircleRadius;

    //the pixel radius of the circle the player can move their finger in to control the ball's speed
    float pixelAimingCircleRadius;

    //wether or not the player is aiming the ball
    bool isHeld;
    bool isLaunched;

    //the angle that the ball will travel in once released
    float angle;

    //the percent of max launch force that will be applied to the ball
    float powerPercent;

    [SerializeField]
    int forceMultiplyer;

    [SerializeField]
    bool ballCamOnLaunch;

    //properties

    //used by BallEffects to determine the visibility, angle, length, and color of the aiming line
    public bool IsHeld { get{ return isHeld; } }
    public bool IsLaunched {  get{ return isLaunched; } }
    public float Angle { get { return angle; } }
    public float PowerPercent{ get { return powerPercent; } }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ballLevelInteractions = GetComponent<BallLevelInteractions>();

        ballOrigin = gameObject.transform.position;
        ballPixelOrigin = new Vector2(Screen.width / 2, Screen.height / 7f);

        pixelAimingCircleRadius = Screen.width * 0.25f;

        isHeld = false;
        isLaunched = false;

        angle = 0;
        powerPercent = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //the player is aiming the ball
        if (isHeld)
        {
            //get the angle between the pixel origin and current mouse pos
            angle = Mathf.Atan2(Input.mousePosition.x - ballPixelOrigin.x, Input.mousePosition.y - ballPixelOrigin.y);

            //get the percentage of max force that will be applied to the ball upon release
            powerPercent = DistanceBetweenTwoPoints(ballPixelOrigin, Input.mousePosition) / pixelAimingCircleRadius;
            if (powerPercent > 1)
            {
                powerPercent = 1;
            }

            //update the ball position
            transform.position = new Vector3(
                ballOrigin.x + (Mathf.Sin(angle) * aimingCircleRadius * powerPercent),
                ballOrigin.y,
                ballOrigin.z + (Mathf.Cos(angle) * aimingCircleRadius * powerPercent));

            //Debug.Log("held " + powerPercent);
        }

        //if the player releases the mouse, they release the ball
        if (!Input.GetMouseButton(0))
        {
            //if the player was previously holding the ball and just released it, apply a force to it
            if (isHeld)
            {
                rb.AddForce(-powerPercent * Mathf.Sin(angle) * forceMultiplyer, 0, -powerPercent * Mathf.Cos(angle) * forceMultiplyer);
                isLaunched = true;
                ballLevelInteractions.IsLaunched = true;

                //use powerups if they were applied to the ball
                if (GetComponent<BallEffects>().GoldBallEnabled)
                {
                    LevelManager.Instance.GoldBallPow--;
                    LevelUILogic.Instance.UpdatePowerups();
                }

                //switch cam view automatically (toggle in settings later)
                if (ballCamOnLaunch)
                {
                    LevelManager.Instance.SwitchCameraView();
                }
                
            }
            isHeld = false;
        }
    }

    //if the player clicks the ball in the main camera view, they are holding it
    private void OnMouseDown()
    {
        if (!LevelManager.Instance.BallCamera.activeInHierarchy)
        {
            isHeld = true;
        }
        
    }

    public float DistanceBetweenTwoPoints(Vector2 point1, Vector2 point2)
    {
        return Mathf.Sqrt(Mathf.Pow(point2.x - point1.x, 2) + Mathf.Pow(point2.y - point1.y, 2));
    }
}
