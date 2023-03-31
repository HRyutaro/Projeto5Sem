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
    public float distanciaMin;
    public float distanciaMax;
    private float distMax;
    float distanciaDoAlvo;
    public Transform alvo;
    private NavMeshAgent navMeshAgent;
    public float speed;

    [Header("Animação")]
    public Animator Anim;

    [Header("Dano")]
    public Color corDano;
    public Color corNormal;
    public Color corGelado;
    private bool tomouDano = false;

    [Header("ataque")]
    public Collider atack;
    public float CdAtack;
    public float speedAtaque;
    private float nextattackTime;
    private bool isAttacking = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = distanciaMin;
        navMeshAgent.speed = speed;
        distMax = distanciaMax;
    }


    void Update()
    {
        atacar();
    }
    void FixedUpdate()
    {
        move();
    }
    void move()
    {
        distanciaDoAlvo = Vector3.Distance(transform.position, alvo.position);
        if (distanciaDoAlvo > distMax)
        {
            navMeshAgent.SetDestination(transform.position);
        }
        else
        {
            navMeshAgent.SetDestination(alvo.position);
        }
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
        if(Time.time >= nextattackTime)
        {
            if (!isAttacking && Vector3.Distance(transform.position, alvo.position) <= distanciaMin)
            {
                StartCoroutine("AtackGarra");
            }
            nextattackTime = Time.time + CdAtack;
        }

    }

    void Congelado()
    {
        StartCoroutine("EfeitoGelo");
        if(Vida >= 1)
        {
            --Vida;
        }
        else if(Vida <= 0)
        {
            Destroy(gameObject, 1f);
        }
    }

    IEnumerator CDTomarDano()
    {
        tomouDano = true;
        yield return new WaitForSeconds(0.3f);
        tomouDano = false;
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
        yield return new WaitForSeconds(1);
        isAttacking = false;
        Anim.SetFloat("Atack", 0);
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
    IEnumerator inSmoke()
    {
        yield return new WaitForSeconds(1f);
        distMax = 1;
        yield return new WaitForSeconds(2f);
        distMax = distanciaMax;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("espada") && !tomouDano)
        {
            StartCoroutine("CDTomarDano");
            tomarDano(1);
        }
        if(other.gameObject.tag == "gelo" && !tomouDano)
        {
            StartCoroutine("CDTomarDano");
            Congelado();
            GetComponent<Renderer>().material.color = corGelado;
        }
        if(other.gameObject.tag == "fogo" && !tomouDano)
        {
            StartCoroutine("CDTomarDano");
            tomarDanoFogo();
        }
        if (other.gameObject.tag == "fumaca")
        {
            StartCoroutine(inSmoke());
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,distanciaMax);
    }

}
