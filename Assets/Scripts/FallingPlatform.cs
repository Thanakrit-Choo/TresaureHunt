using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float delayFallingTime;
    public float respawnTime;
    // Start is called before the first frame update
    float timer;
    float resTimer;
    Rigidbody rb;
    bool isStepOn = false;
    bool respawn = false;
    Vector3 localPosition;
    void Start()
    {
        if (rb == null)
        {
            if (!TryGetComponent<Rigidbody>(out rb))
            {
                Debug.Log("Rigidbody is missing");
            }
        }
        localPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            rb.mass = 100;
            isStepOn = true;
            timer = delayFallingTime;
            respawn = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(isStepOn)
        {
            timer -= Time.deltaTime;
            if(timer <= 0 && transform.position.y > -30f)
            {
                rb.velocity = new Vector3(0, -20f, 0);
            }
            else if(transform.position.y < -30f)
            {
                rb.velocity = Vector3.zero;
                isStepOn = false;
                respawn = true;
                resTimer = respawnTime;
            }
        }
        if(respawn)
        {
            rb.mass = 99999f;
            resTimer -= Time.deltaTime;
            if(resTimer <= 0)
            {
                this.transform.position = Vector3.MoveTowards(transform.position, localPosition, 3f * Time.deltaTime);
                isStepOn = false;
            }
            if(transform.position == localPosition)
            {
                respawn = false;
            }
            
        }
    }
}
