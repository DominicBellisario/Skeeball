using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageCollectEffect : MonoBehaviour
{
    Camera cam;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] ParticleSystem particles;
    /// <summary>
    /// beginning of lerp
    /// </summary>
    Vector2 startPoint;
    /// <summary>
    /// end of lerp
    /// </summary>
    Vector2 endPoint;

    [SerializeField] Sprite[] allSprites;
    [SerializeField] Color[] allColors;
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

    public void SetValuesAndStart(Canvas canvas, Camera worldCam, Camera uiCam, Vector3 worldStartPoint, Vector3 screenEndPoint, int spriteNum, bool bounceOff, string functionToCall)
    {
        this.bounceOff = bounceOff;
        this.functionToCall = functionToCall;

        // Convert world start point to screen point
        Vector3 screenStartPoint = RectTransformUtility.WorldToScreenPoint(worldCam, worldStartPoint);

        // Convert to local UI position
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenStartPoint, uiCam, out startPoint);

        // Convert screen end point (already in screen space) to local UI position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenEndPoint, uiCam, out endPoint);

        // Set sprite and particles
        GetComponent<Image>().sprite = allSprites[spriteNum];
        particles.startColor = allColors[spriteNum];
        particles.Play();

        // Set position
        rectTransform.anchoredPosition = startPoint;
        rectTransform.localScale = Vector3.one;

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
