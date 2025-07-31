using System.Collections;
using UnityEngine;

public class ButtonLerpEffect : MonoBehaviour
{
    [SerializeField] float scaleWaitTime;
    [SerializeField] float scaleDuration;
    [SerializeField] float targetScale;
    [SerializeField] bool scaleOnEnable; // If true, the effect will play when the button is enabled
    [SerializeField] bool playSound;

    RectTransform ButtonRect { get { return GetComponent<RectTransform>(); } }

    void Start()
    {
        // do the fade in animation only when game starts
        if (Manager.Instance.GameJustStarted && !scaleOnEnable)
        {
            ButtonRect.localScale = Vector3.zero; // Start with scale at zero
            StartCoroutine(LerpScale());
        }
    }

    void OnEnable()
    {
        if (scaleOnEnable)
        {
            ButtonRect.localScale = Vector3.zero; // Start with scale at zero
            StartCoroutine(LerpScale());
        }
    }

    //change the object's size over a period of time
    IEnumerator LerpScale()
    {
        float elapsed = 0f;
        Vector3 initialScale = ButtonRect.localScale;

        yield return new WaitForSeconds(scaleWaitTime); // Wait before starting the scale effect
        while (elapsed < scaleDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / scaleDuration);
            float currentScale = Mathf.Lerp(initialScale.x, targetScale, t);
            ButtonRect.localScale = new Vector3(currentScale, currentScale, currentScale);
            yield return null;
        }
        if (playSound)
        {
            GetComponent<AudioSource>().Play(); // Play the sound effect
        }

        ButtonRect.localScale = new Vector3(targetScale, targetScale, targetScale); // Ensure final scale is set
    }
}
