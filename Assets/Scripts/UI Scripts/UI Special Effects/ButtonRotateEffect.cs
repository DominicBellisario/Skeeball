using System.Collections;
using UnityEngine;

public class ButtonRotateEffect : MonoBehaviour
{
    
    [SerializeField] float rotateWaitTime;
    [SerializeField] float rotateDuration = 0.5f;
    [SerializeField] float rotateAmount = 270;

    RectTransform ButtonRect { get { return GetComponent<RectTransform>(); } }

    private void OnEnable()
    {
        ButtonRect.rotation = Quaternion.Euler(new Vector3(ButtonRect.rotation.x, 90, ButtonRect.rotation.z));
        StartCoroutine(Rotate());
    }

    //change the object's size over a period of time
    IEnumerator Rotate()
    {
        float elapsed = 0f;
        Vector3 initialRotation = ButtonRect.rotation.eulerAngles;

        yield return new WaitForSeconds(rotateWaitTime); // Wait before starting the scale effect
        GetComponent<AudioSource>().Play(); // Play the sound effect
        while (elapsed < rotateDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotateDuration);
            float currentRotation = Mathf.Lerp(initialRotation.y, initialRotation.y + rotateAmount, t);
            ButtonRect.rotation = Quaternion.Euler(new Vector3(initialRotation.x, currentRotation, initialRotation.z));
            yield return null;
        }

        ButtonRect.rotation = Quaternion.Euler(new Vector3(initialRotation.x, initialRotation.y + rotateAmount, initialRotation.z)); // Ensure final scale is set
    }
}
