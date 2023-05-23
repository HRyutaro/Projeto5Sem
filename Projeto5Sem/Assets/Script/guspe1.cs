using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guspe1 : MonoBehaviour
{
    public float timeDestroy;
    public Collider col;

    public float speed;

    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            col.enabled = false;
            Destroy(gameObject, 0);
        }
        if (other.gameObject.tag == "Porta")
        {
            col.enabled = false;
            Destroy(gameObject, 0);
        }
        if (other.gameObject.tag == "Buraco")
        {
            col.enabled = false;
            Destroy(gameObject, 0);
        }
        if (other.gameObject.tag == "Parede2")
        {
            col.enabled = false;
            Destroy(gameObject, 0);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject, 0);
        }
        if (collision.gameObject.tag == "Porta")
        {
            Destroy(gameObject, 0);
        }
        if (collision.gameObject.tag == "Buraco")
        {
            Destroy(gameObject, 0);
        }
    }
}
