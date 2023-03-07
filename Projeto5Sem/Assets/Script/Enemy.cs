using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public int Vida;

    public float distanciaMinima = 2;
    public Transform alvo;
    private NavMeshAgent navMeshAgent;


    public Color corDano;
    public Color corNormal;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = distanciaMinima;
    }


    void Update()
    {
        navMeshAgent.SetDestination(alvo.position);
    }

    void tomarDano()
    {
        StartCoroutine("DanoCorCD");
        if(Vida >= 1)
        {
            --Vida;
        }
        if(Vida <= 0)
        {
            Destroy(gameObject,1f);
        }
    }

    IEnumerator DanoCorCD()
    {
        GetComponent<Renderer>().material.color = corDano;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Renderer>().material.color = corNormal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "espada")
        {
            tomarDano();
        }
    }
}
