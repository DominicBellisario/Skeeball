using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FlashFade : MonoBehaviour
{
    [SerializeField] float fadeDuration;
    Image image;


    void OnEnable()
    {
        image = GetComponent<Image>();
        StartCoroutine(LerpFade(fadeDuration, 0)); // Start with fade in
    } 
    
    IEnumerator LerpFade(float duration, float targetAlpha)
    {
        float elapsed = 0f;
        float initialAlpha = image.color.a;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(initialAlpha, targetAlpha, t));
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, targetAlpha); // Ensure final alpha is set
    }
}
