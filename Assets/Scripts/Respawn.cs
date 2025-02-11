using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform[] spwanPoints;
    [SerializeField] private GameObject fade;
    public static int checkPointCount;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            fade.SetActive(true);
            //fade.GetComponent<Animator>().enabled = true;
            fade.GetComponent<Animator>().Play("FadeIn");
            StartCoroutine(Respwan(collider));
        }
    }

    private void Start()
    {
        checkPointCount = 0;
    }

    IEnumerator Respwan(Collider collider)
    {
        yield return new WaitForSeconds(1.7f);
        collider.transform.position = spwanPoints[checkPointCount].transform.position;
        yield return new WaitForSeconds(1f);
        fade.GetComponent<Animator>().Play("FadeOut");

        yield return new WaitForSeconds(2f);
        fade.SetActive(false);
    }
}
