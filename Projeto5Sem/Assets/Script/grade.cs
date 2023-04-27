using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grade : MonoBehaviour
{
    public Rigidbody rb;
    public bool queda;
    public GameObject spawn;

    void Start()
    {

    }


    void Update()
    {
        Queda();
    }

    void Queda()
    {
        if (queda == true)
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
        else if (queda == false)
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionY;
        }
    }

    IEnumerator startQueda()
    {
        yield return new WaitForSeconds(0.5f);
        queda = true;
    }
    IEnumerator Return()
    {
        yield return new WaitForSeconds(4);
        queda = false;
        gameObject.transform.position = spawn.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(startQueda());
        }
        if(collision.gameObject.tag == "pisoFora")
        {
            StartCoroutine(Return());
        }
    }
}
