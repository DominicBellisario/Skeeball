using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControls : ObjectControls
{
    /// <summary>
    /// launch the ball in a straight line
    /// </summary>
    protected override void LaunchObject()
    {
        base.LaunchObject();
        rb.AddForce(-powerPercent * Mathf.Sin(angle) * forceMultiplyer, 0, -powerPercent * Mathf.Cos(angle) * forceMultiplyer);
    }

    /// <summary>
    /// spawn a new ball and give it this balls force at a slight angle
    /// </summary>
    /// <param name="effects"></param>
    protected override void SpawnNewTriObjects(BallEffects effects)
    {
        base.SpawnNewTriObjects(effects);
        LevelManager.Instance.SpawnNewBall(new Vector3(gameObject.transform.position.x - .5f, gameObject.transform.position.y, gameObject.transform.position.z),
                        new Vector3(-powerPercent * Mathf.Sin(angle - triBallAngleRads) * forceMultiplyer, 0, -powerPercent * Mathf.Cos(angle - triBallAngleRads) * forceMultiplyer),
                        effects.GoldBallEnabled, effects.MarkedBallEnabled, effects.TriBallEnabled);
        LevelManager.Instance.SpawnNewBall(new Vector3(gameObject.transform.position.x + .5f, gameObject.transform.position.y, gameObject.transform.position.z),
                        new Vector3(-powerPercent * Mathf.Sin(angle + triBallAngleRads) * forceMultiplyer, 0, -powerPercent * Mathf.Cos(angle + triBallAngleRads) * forceMultiplyer),
                        effects.GoldBallEnabled, effects.MarkedBallEnabled, effects.TriBallEnabled);
    }
}
