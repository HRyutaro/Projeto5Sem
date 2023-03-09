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
    public float speed;

    [Header("Animação")]
    public Animator Anim;

    [Header("Dano")]
    public SkinnedMeshRenderer skin;
    public Color corDano;
    public Color corNormal;
    public Color corGelado;

    [Header("ataque")]
    public Collider atack;
    public Collider atack2;
    public float CdAtack;
    public float tempoDoAtaque;
    private bool isAttacking = false;

    private bool OnGelo;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = distanciaMinima;
        navMeshAgent.speed = speed;
    }


    void Update()
    {
        navMeshAgent.SetDestination(alvo.position);
        atacar();
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
    void tomarDanoFogo()
    {
        StartCoroutine("DanoCorCDFogo");
        if (Vida >= 1)
        {
            --Vida;
        }
        if (Vida <= 0)
        {
            Destroy(gameObject, 1f);
        }
    }
    void atacar()
    {
        if (!isAttacking && Vector3.Distance(transform.position, alvo.position) <= distanciaMinima)
        {
            StartCoroutine("AtackGarra");
        }
    }

    void Congelado()
    {
        StartCoroutine("EfeitoGelo");
    }
    IEnumerator EfeitoGelo()
    {
        navMeshAgent.speed = 1.5f;
        yield return new WaitForSeconds(3f);
        navMeshAgent.speed = speed;
        GetComponent<Renderer>().material.color = corNormal;

    }

    IEnumerator AtackGarra()
    {
        isAttacking = true;
        Anim.SetFloat("Atack", 1);
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(tempoDoAtaque);
        atack.enabled = true;
        atack2.enabled = true;
        yield return new WaitForSeconds(0.1f);
        atack.enabled = false;
        atack2.enabled = false;
        Anim.SetFloat("Atack", 0);
        yield return new WaitForSeconds(CdAtack);
        isAttacking = false;
        navMeshAgent.isStopped = false;

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
    IEnumerator DanoCorCDFogo()
    {
        GetComponent<Renderer>().material.color = corDano;
        //skin.material.color = corDano;
        navMeshAgent.isStopped = true;
        //Anim.SetFloat("isRun", 0);
        yield return new WaitForSeconds(0.3f);
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
        if(other.gameObject.tag == "gelo")
        {
            Congelado();
            GetComponent<Renderer>().material.color = corGelado;
        }
        if(other.gameObject.tag == "fogo")
        {
            tomarDanoFogo();
        }

    }


}
