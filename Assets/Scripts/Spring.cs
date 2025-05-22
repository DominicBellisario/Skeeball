using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] GameObject springBoard;
    [SerializeField] GameObject pole;
    [SerializeField] int forceMultiplier;
    [SerializeField] float deactivateTime;

    public int ForceMultiplier { get { return forceMultiplier; } }

    public void ActivateSpring()
    {
        springBoard.transform.position += transform.up * 0.5f;
        pole.transform.position += transform.up * 0.5f;
        springBoard.GetComponent<BoxCollider>().enabled = false;
        //pole.SetActive(true);
        StartCoroutine(DeactivateSpring());
    }

    IEnumerator DeactivateSpring()
    {
        yield return new WaitForSeconds(deactivateTime);
        springBoard.transform.position -= transform.up * 0.5f;
        pole.transform.position -= transform.up * 0.5f;
        springBoard.GetComponent<BoxCollider>().enabled = true;
        //pole.SetActive(false);
    }
}
