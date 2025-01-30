using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanbagControls : ObjectControls
{
    [SerializeField]
    int heightMultipliter;

    [SerializeField]
    float rotRange;

    public Vector3 PredictVelocity(float _angle)
    {
        return new Vector3(-powerPercent * Mathf.Sin(_angle) * forceMultiplyer, powerPercent * heightMultipliter, -powerPercent * Mathf.Cos(_angle) * forceMultiplyer);
    }

    /// <summary>
    /// launch the beanbag in an arc
    /// </summary>
    protected override void LaunchObject()
    {
        rb.AddForce(-powerPercent * Mathf.Sin(angle) * forceMultiplyer, powerPercent * heightMultipliter, -powerPercent * Mathf.Cos(angle) * forceMultiplyer);
        rb.AddTorque(new Vector3(Random.Range(-rotRange, rotRange), Random.Range(-rotRange, rotRange), Random.Range(-rotRange, rotRange)));
    }

    /// <summary>
    /// spawn new beanbags, give them this beanbags force at a slight angle, give them the powerup states of this bean bag, and give them a random rotation
    /// </summary>
    /// <param name="effects"></param>
    protected override void SpawnNewTriObjects(ObjectEffects effects)
    {
        SpawnTriBeanbag(effects, -.5f, -triBallAngleRads);
        SpawnTriBeanbag(effects, .5f, triBallAngleRads);
    }

    void SpawnTriBeanbag(ObjectEffects effects, float triPos, float triBallAngle)
    {
        GameObject beanbag = LevelManager.Instance.SpawnNewObject(
            gameObject,
            new Vector3(gameObject.transform.position.x + triPos, gameObject.transform.position.y, gameObject.transform.position.z),
            new Vector3(
                -powerPercent * Mathf.Sin(angle + triBallAngle) * forceMultiplyer,
                powerPercent * heightMultipliter,
                -powerPercent * Mathf.Cos(angle + triBallAngle) * forceMultiplyer),
            effects.GoldBallEnabled, effects.MarkedBallEnabled, false);
        beanbag.GetComponent<ObjectLevelInteractions>().IsLaunched = true;
        beanbag.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-rotRange, rotRange), Random.Range(-rotRange, rotRange), Random.Range(-rotRange, rotRange)));
    }
}
