using System.Collections;
using UnityEngine;
using TMPro;

public class TextCollectEffect : MonoBehaviour
{
    Camera cam;
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

    public void SetValuesAndStart(Camera _cam, Vector3 _worldStartPoint, Vector3 _screenEndPoint, float _verticalOffset, Color _textColor, string _text, string _functionToCall)
    {
        cam = _cam;
        functionToCall = _functionToCall;

        //calculate the position of the object on the UI
        Vector3 startPointUnscaled = cam.WorldToViewportPoint(_worldStartPoint);
        startPoint = new Vector3((startPointUnscaled.x * Screen.width) - (Screen.width / 2), (startPointUnscaled.y * Screen.height) - (Screen.height / 2) + _verticalOffset, 0);
        endPoint = _screenEndPoint;

        //set text
        textMesh.text = _text;

        //set color
        textMesh.color = _textColor;

        //set its position
        rectTransform.anchoredPosition = startPoint;

        //resize it to fit in the smaller UI
        rectTransform.localScale = Vector3.one;

        //perform movements
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
