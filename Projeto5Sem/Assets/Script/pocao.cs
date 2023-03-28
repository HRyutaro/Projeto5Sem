using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class pocao : MonoBehaviour
{
    public float forcaLancamento;
    public float timeDestroy;
    public Rigidbody rb;
    public GameObject[] tipo;
    public static int tipoDapocao = 0;
    public Collider colisor;

    [Header("Cores")]
    public Renderer CorPocao;
    public Color corFogo;
    public Color corGelo;
    public Color corFumaca;


    void Start()
    {
        if (tipoDapocao == 2)
        {
            CorPocao.material.color = corFogo;
        }
        if (tipoDapocao == 3)
        {
            CorPocao.material.color = corGelo;

        }
        if (tipoDapocao == 4)
        {
            CorPocao.material.color = corFumaca;
        }
        Destroy(gameObject, timeDestroy);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "piso" )
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y -= 0.2f;
            if(tipoDapocao == 2)
            {
                Instantiate(tipo[0],spawnPosition , Quaternion.identity);
            }
            if(tipoDapocao == 3)
            {
                Instantiate(tipo[1], spawnPosition, Quaternion.identity);
            }
            if(tipoDapocao == 4)
            {
                Instantiate(tipo[2], spawnPosition, Quaternion.identity);
            }
            colisor.enabled = false;
        }
        if(collision.gameObject.tag == "player")
        {
            colisor.enabled = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            colisor.enabled = true;
        }
    }

}
