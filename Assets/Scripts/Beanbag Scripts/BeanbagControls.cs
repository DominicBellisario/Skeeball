using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanbagControls : ObjectControls
{
    [SerializeField]
    int heightMultipliter;
    /// <summary>
    /// launch the beanbag in an arc
    /// </summary>
    protected override void LaunchObject()
    {
        rb.AddForce(-powerPercent * Mathf.Sin(angle) * forceMultiplyer, powerPercent / 2 * heightMultipliter, -powerPercent * Mathf.Cos(angle) * forceMultiplyer);
    }

    /// <summary>
    /// spawn new beanbags and give them this beanbags force at a slight angle
    /// </summary>
    /// <param name="effects"></param>
    protected override void SpawnNewTriObjects(ObjectEffects effects)
    {
        GameObject leftObject = LevelManager.Instance.SpawnNewObject(
            gameObject, 
            new Vector3(gameObject.transform.position.x - .5f, gameObject.transform.position.y, gameObject.transform.position.z),
            new Vector3(
                -powerPercent * Mathf.Sin(angle - triBallAngleRads) * forceMultiplyer, 
                powerPercent / 2 * heightMultipliter,
                -powerPercent * Mathf.Cos(angle - triBallAngleRads) * forceMultiplyer),
            effects.GoldBallEnabled, effects.MarkedBallEnabled, false);
        leftObject.GetComponent<ObjectLevelInteractions>().IsLaunched = true;

        GameObject rightObject = LevelManager.Instance.SpawnNewObject(
            gameObject, 
            new Vector3(gameObject.transform.position.x + .5f, gameObject.transform.position.y, gameObject.transform.position.z),
            new Vector3(
                -powerPercent * Mathf.Sin(angle + triBallAngleRads) * forceMultiplyer,
                powerPercent / 2 * heightMultipliter,
                -powerPercent * Mathf.Cos(angle + triBallAngleRads) * forceMultiplyer),
            effects.GoldBallEnabled, effects.MarkedBallEnabled, false);
        rightObject.GetComponent<ObjectLevelInteractions>().IsLaunched = true;

        
    }
}
