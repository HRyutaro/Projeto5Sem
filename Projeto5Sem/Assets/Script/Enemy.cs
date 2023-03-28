using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("vida")]
    public int Vida;
    public Collider col;

    [Header("Perseguição")]
    public float distanciaMinima;
    public Transform alvo;
    private NavMeshAgent navMeshAgent;
    public float speed;

    [Header("Animação")]
    public Animator Anim;

    [Header("Dano")]
    public Color corDano;
    public Color corNormal;
    public Color corGelado;

    [Header("ataque")]
    public Collider atack;
    public float CdAtack;
    public float tempoDoAtaque;
    private bool isAttacking = false;

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


    void tomarDano(float dano)
    {
        StartCoroutine("DanoCorCD");
        if(Vida >= 1)
        {
            Vida -= 1;
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
        if (isAttacking == false && Vector3.Distance(transform.position, alvo.position) <= distanciaMinima)
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
        atack.enabled = true;
        Anim.SetFloat("Atack", 1);
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(tempoDoAtaque);
        atack.enabled = false;
        Anim.SetFloat("Atack", 0);
        yield return new WaitForSeconds(CdAtack);
        navMeshAgent.isStopped = false;
        isAttacking = false;
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
    IEnumerator inSmoke()
    {
        yield return new WaitForSeconds(1f);
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(2f);
        navMeshAgent.isStopped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "espada")
        {
            tomarDano(1);
            col.enabled = false;
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
        if (other.gameObject.tag == "fumaca")
        {
            StartCoroutine(inSmoke());
        }

    }
    private void OnTriggerExit(Collider other)
    {
        col.enabled = true;
    }

}
