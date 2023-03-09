using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pocao : MonoBehaviour
{
    public float forcaLancamento;
    public float timeDestroy;
    public Rigidbody rb;
    public GameObject[] tipo;
    public static int tipoDapocao = 0;
    public Collider colisor;


    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "piso" )
        {
            if(tipoDapocao == 0)
            {
                Instantiate(tipo[0],gameObject.transform.position, Quaternion.identity);
            }
            if(tipoDapocao == 1)
            {
                Instantiate(tipo[1],gameObject.transform.position, Quaternion.identity);
            }
                colisor.enabled = false;
        }
    }
}
