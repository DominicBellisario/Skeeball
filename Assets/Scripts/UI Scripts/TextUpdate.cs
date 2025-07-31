using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextUpdate : MonoBehaviour
{
    TextMeshProUGUI TextField { get { return GetComponent<TextMeshProUGUI>(); } }

    public void UpdateText(string newText)
    {
        StopAllCoroutines(); // Stop any ongoing fade effects
        TextField.color = new Color(TextField.color.r, TextField.color.g, TextField.color.b, 1f); // Reset alpha to fully opaque
        TextField.text = newText;
        StartCoroutine(FadeText(1f, 0.5f));
    }

    IEnumerator FadeText(float waitTime, float fadeDuration)
    {
        Color originalColor = TextField.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        yield return new WaitForSeconds(waitTime);
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            TextField.color = Color.Lerp(originalColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        TextField.color = targetColor; // Ensure final color is set
    }
}
