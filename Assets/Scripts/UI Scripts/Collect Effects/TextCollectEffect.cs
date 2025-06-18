using System.Collections;
using UnityEngine;
using TMPro;

public class TextCollectEffect : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] TextMeshProUGUI textMesh;

    /// <summary>
    /// beginning of lerp
    /// </summary>
    Vector3 startPoint;
    /// <summary>
    /// end of lerp
    /// </summary>
    Vector3 endPoint;

    /// <summary>
    /// how long before the object begins to move
    /// </summary>
    [SerializeField] float upDistance;
    /// <summary>
    /// how long the lerp takes
    /// </summary>
    [SerializeField] float lerpTime;

    string functionToCall;

    public void SetValuesAndStart(Canvas canvas, Camera worldCam, Camera uiCam, Vector3 worldStartPoint, Vector3 screenEndPoint, float verticalOffset, Color textColor, string text, string functionToCall)
    {
        this.functionToCall = functionToCall;

        // Convert world point to screen point using the world camera
        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(worldCam, worldStartPoint);

        // Convert screen point to local point in canvas space using the UI camera
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 localStart;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, uiCam, out localStart);

        // Apply vertical offset (in UI units)
        localStart.y += verticalOffset;
        startPoint = localStart;

        // Convert screen-space end point to canvas-local point using the UI camera
        Vector2 localEnd;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenEndPoint, uiCam, out localEnd);
        endPoint = localEnd;

        // Set up text
        textMesh.text = text;
        textMesh.color = textColor;

        // Position and reset scale
        rectTransform.anchoredPosition = startPoint;
        rectTransform.localScale = Vector3.one;

        // Start animation
        StartCoroutine(LerpToTarget());
    }

    private IEnumerator LerpToTarget()
    {
        Vector3 currentEndPoint = startPoint + new Vector3(0, upDistance, 0);
        float t = 0;
        //lerp up and fade in
        while (t < lerpTime)
        {
            t += Time.deltaTime;
            rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(startPoint.x, currentEndPoint.x, t / lerpTime), Mathf.Lerp(startPoint.y, currentEndPoint.y, t / lerpTime));
            textMesh.alpha = t * 2;

            yield return new WaitForEndOfFrame();
        }

        startPoint = rectTransform.anchoredPosition;
        float startFontSize = textMesh.fontSize;
        t = 0;

        //lerp to target and shrink
        while (t < lerpTime)
        {
            t += Time.deltaTime;
            rectTransform.anchoredPosition = new Vector2(Mathf.SmoothStep(startPoint.x, endPoint.x, t / lerpTime), Mathf.SmoothStep(startPoint.y, endPoint.y, t / lerpTime));
            textMesh.fontSize = startFontSize * (1 - t / lerpTime);
            yield return new WaitForEndOfFrame();
        }

        //once it reaches:
        //update the function needed
        LevelUILogic.Instance.Invoke(functionToCall, 0);
        //play score update sound with a random pitch
        SoundManager.Instance.PlaySound(3, 15, 1, 0.75f + Helper.Instance.RandomInt(1, 50) * 0.01f);

        //then, destroy it
        Destroy(gameObject);
    }
}
