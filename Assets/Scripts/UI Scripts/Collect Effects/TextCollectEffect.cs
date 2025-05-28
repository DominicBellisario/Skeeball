using System.Collections;
using System.Collections.Generic;
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

        //perform movements
        StartCoroutine(LerpToTarget());
    }

    private IEnumerator LerpToTarget()
    {
        float startTime = Time.time;
        Vector3 currentEndPoint = startPoint + new Vector3(0, upDistance, 0);
        //lerp up and fade in
        while (Vector3.Distance(rectTransform.anchoredPosition, currentEndPoint) > 1)
        {
            float t = (Time.time - startTime) / lerpTime;
            rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(startPoint.x, currentEndPoint.x, t), Mathf.Lerp(startPoint.y, currentEndPoint.y, t));
            textMesh.alpha = t * 2;
            
            yield return new WaitForEndOfFrame();
        }

        startTime = Time.time;
        startPoint = rectTransform.anchoredPosition;
        float startFontSize = textMesh.fontSize;
        //lerp to target and shrink
        while (Vector3.Distance(rectTransform.anchoredPosition, endPoint) > 1)
        {
            float t = (Time.time - startTime) / lerpTime;
            rectTransform.anchoredPosition = new Vector2(Mathf.SmoothStep(startPoint.x, endPoint.x, t), Mathf.SmoothStep(startPoint.y, endPoint.y, t));
            textMesh.fontSize = startFontSize * (1 - t);
            yield return new WaitForEndOfFrame();
        }

        //once it reaches:
        //update the function needed
        LevelUILogic.Instance.Invoke(functionToCall, 0);
        //play score update sound with a random pitch
        SoundManager.Instance.PlaySound(3, 15, 0.75f, 0.75f + Helper.Instance.RandomInt(1, 50) * 0.01f);

        //then, destroy it
        Destroy(gameObject);
    }
}
