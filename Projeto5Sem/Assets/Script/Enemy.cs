using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("vida")]
    public int Vida;

    [Header("Perseguição")]
    public float distanciaMinima;
    public Transform alvo;
    private NavMeshAgent navMeshAgent;

    [Header("Animação")]
    public Animator Anim;

    [Header("Dano")]
    public SkinnedMeshRenderer skin;
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
        //skin.material.color = corDano;
        navMeshAgent.isStopped = true;
        //Anim.SetFloat("isRun", 0);
        yield return new WaitForSeconds(1f);
        //Anim.SetFloat("isRun", 1);
        navMeshAgent.isStopped = false;
        //skin.material.color = corNormal;
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
