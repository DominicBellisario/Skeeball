using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class TitleImageEffects : MonoBehaviour
{
    [SerializeField] float scaleDuration;
    [SerializeField] float scaleMax;
    [SerializeField] float scaleMin;
    [SerializeField] GameObject flash;

    RectTransform ImageRect { get { return GetComponent<RectTransform>(); } }

    void Start()
    {
        // do the fade in animation only when game starts
        if (Manager.Instance.GameJustStarted)
        {
            ImageRect.localScale = Vector3.zero;
            StartCoroutine(LerpScale(scaleDuration + 0.4f, scaleMax + 0.3f));
        }
        // otherwise, only do the bounce effect
        else
        {
            StartCoroutine(LerpScale(scaleDuration, scaleMin));
        }
    }

    //change the object's size over a period of time
    IEnumerator LerpScale(float duration, float targetScale)
    {
        float elapsed = 0f;
        Vector3 initialScale = ImageRect.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float currentScale = Mathf.SmoothStep(initialScale.x, targetScale, t);
            ImageRect.localScale = new Vector3(currentScale, currentScale, currentScale);
            yield return null;
        }

        ImageRect.localScale = new Vector3(targetScale, targetScale, targetScale); // Ensure final scale is set

        // After scaling, start the bounce effect
        // do other effects if the image is larger than the max size
        if (ImageRect.localScale.x > scaleMax)
        {
            StartCoroutine(LerpScale(scaleDuration, scaleMin));
            yield return new WaitForSeconds(scaleDuration - 0.2f);
            flash.SetActive(true); // Activate the flash effect
        }
        else if (ImageRect.localScale.x == scaleMax) { StartCoroutine(LerpScale(scaleDuration, scaleMin)); }
        //make it larger
        else { StartCoroutine(LerpScale(scaleDuration, scaleMax)); }
    }
}
