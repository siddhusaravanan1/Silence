using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletDamage : MonoBehaviour
{
    Vector3 dest;

    void Start()
    {
        transform.eulerAngles = new Vector3(-90f, transform.eulerAngles.y, transform.eulerAngles.z);
        dest = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)) + Camera.main.transform.forward * 500f;
        destroyMe(1);
    }
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, dest, 300f * Time.deltaTime);
        //transform.Translate(Vector3.down * 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Enemy")
        {
            other.GetComponent<AI_Controller>().doDamage();
            destroyMe(0);
        }

        if(other.tag=="Player")
        {
            other.GetComponent<PlayerController>().doDamage(50f);
        }
    }

    void destroyMe(float t)
    {
        Destroy(gameObject, t);
    }
}
