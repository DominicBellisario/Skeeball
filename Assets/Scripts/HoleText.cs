using TMPro;
using UnityEngine;

public class HoleText : MonoBehaviour
{
    //the speed the text ascends
    [SerializeField] float speed;
    //how long the text stays opaque
    [SerializeField] float opaqueTime;
    //the rate the text turns transparent per second
    [SerializeField] float transparentRate;

    float timer = 0f;
    TextMeshPro textMesh;

    // Start is called before the first frame update
    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        //increment timer
        if (timer > -1)
        {
            timer += Time.deltaTime;
        }
        if (timer >= opaqueTime)
        {
            timer = -1;
        }
        //once time is up, start becoming transparent
        if (timer == -1)
        {
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, textMesh.color.a - (transparentRate * Time.deltaTime));
            //once transparent, destroy
            if (textMesh.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }

        //text ascends slowly
        transform.position = new Vector3(transform.position.x, transform.position.y + (speed * Time.deltaTime), transform.position.z);
    }

    //set the text to the score, and change its color if the ball had a power
    public void SetText(int score, bool goldBall, bool marked)
    {
        if (marked)
        {
            textMesh.color = Color.red;
        }
        if (goldBall)
        {
            textMesh.color = Color.yellow;
            score *= 2;
        }
        textMesh.text = score.ToString();
    }
}
