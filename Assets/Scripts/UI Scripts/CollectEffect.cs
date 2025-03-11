using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectEffect : MonoBehaviour
{
    Camera cam;
    [SerializeField] RectTransform rectTransform;
    /// <summary>
    /// beginning of lerp
    /// </summary>
    Vector3 startPoint;
    /// <summary>
    /// end of lerp
    /// </summary>
    Vector3 endPoint;
    bool ready = false;
    [SerializeField] Sprite[] allSprites;
    /// <summary>
    /// how long before the object begins to move
    /// </summary>
    [SerializeField] float timeBeforeLerp;
    /// <summary>
    /// how long the lerp takes
    /// </summary>
    [SerializeField] float lerpTime;

    float startTime;

    private void Update()
    {
        //once time is up, move the object to its destination
        if (ready)
        {
            float t = (Time.time - startTime + 0.001f) / lerpTime;
            
            rectTransform.anchoredPosition = new Vector2(Mathf.SmoothStep(startPoint.x, endPoint.x, t), Mathf.SmoothStep(startPoint.y, endPoint.y, t));
        }

        //destroy it once it reaches
        if (Vector3.Distance(rectTransform.anchoredPosition, endPoint) < 1)
        {
            Destroy(gameObject);
        }
    }

    public void SetValuesAndStart(Camera _cam, Vector3 _worldStartPoint, Vector3 _screenEndPoint, int _spriteNum)
    {
        cam = _cam;

        //calculate the position of the object on the UI
        Vector3 startPointUnscaled = cam.WorldToViewportPoint(_worldStartPoint);
        startPoint = new Vector3((startPointUnscaled.x * Screen.width) - (Screen.width / 2), (startPointUnscaled.y * Screen.height) - (Screen.height / 2), 0);

        endPoint = _screenEndPoint;
        GetComponent<Image>().sprite = allSprites[_spriteNum];

        //set its position
        rectTransform.anchoredPosition = startPoint;

        //begin the lerp after a bit
        StartCoroutine(WaitUntilLerp());
    }

    private IEnumerator WaitUntilLerp()
    {
        yield return new WaitForSeconds(timeBeforeLerp);
        startTime = Time.time;
        ready = true;
    }
}
