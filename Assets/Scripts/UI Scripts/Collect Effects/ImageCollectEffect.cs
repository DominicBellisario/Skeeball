using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCollectEffect : MonoBehaviour
{
    Camera cam;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Rigidbody2D rb;
    /// <summary>
    /// beginning of lerp
    /// </summary>
    Vector3 startPoint;
    /// <summary>
    /// end of lerp
    /// </summary>
    Vector3 endPoint;

    [SerializeField] Sprite[] allSprites;
    /// <summary>
    /// how long before the object begins to move
    /// </summary>
    [SerializeField] float timeBeforeLerp;
    /// <summary>
    /// how long the lerp takes
    /// </summary>
    [SerializeField] float lerpTime;

    /// <summary>
    /// the object bounces off of its end target and "falls" off the screen
    /// </summary>
    bool bounceOff;
    [SerializeField] Vector2 bounceOffForce;

    string functionToCall;

    public void SetValuesAndStart(Camera _cam, Vector3 _worldStartPoint, Vector3 _screenEndPoint, int _spriteNum, bool _bounceOff, string _functionToCall)
    {
        cam = _cam;
        bounceOff = _bounceOff;
        functionToCall = _functionToCall;

        //calculate the position of the object on the UI
        Vector3 startPointUnscaled = cam.WorldToViewportPoint(_worldStartPoint);
        startPoint = new Vector3((startPointUnscaled.x * Screen.width) - (Screen.width / 2), (startPointUnscaled.y * Screen.height) - (Screen.height / 2), 0);

        endPoint = _screenEndPoint;
        GetComponent<Image>().sprite = allSprites[_spriteNum];

        //set its position
        rectTransform.anchoredPosition = startPoint;

        //resize it to fit in the smaller UI
        rectTransform.localScale = Vector3.one;

        //begin the lerp after a bit
        StartCoroutine(WaitUntilLerp());
    }

    private IEnumerator WaitUntilLerp()
    {
        yield return new WaitForSeconds(timeBeforeLerp);
        if (bounceOff) { StartCoroutine(LerpToTarget()); }
        else { StartCoroutine(LerpToTarget()); }
        
    }

    private IEnumerator LerpToTarget()
    {
        float t = 0;
        //lerp to target
        while (t < lerpTime)
        {
            t += Time.deltaTime;
            rectTransform.anchoredPosition = new Vector2(Mathf.SmoothStep(startPoint.x, endPoint.x, t / lerpTime), Mathf.SmoothStep(startPoint.y, endPoint.y, t / lerpTime));
            yield return new WaitForEndOfFrame();
        }

        //once it reaches, run the function that changes the thing needed
        LevelUILogic.Instance.Invoke(functionToCall, 0);

        //then, check if it bounces off or not
        if (bounceOff)
        {
            //bounce it
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(bounceOffForce);

            //play bounce-off sound
            SoundManager.Instance.PlaySound(3, 31, 0.5f, 1f);

            //after a bit, destroy it
            yield return new WaitForSeconds(4);
            Destroy(gameObject);
        }
        else
        {
            //play powerup update sound with a random pitch
            SoundManager.Instance.PlaySound(3, 15, 1, 0.75f + Helper.Instance.RandomInt(1, 50) * 0.01f);
            Destroy(gameObject);
        }
    }
}
