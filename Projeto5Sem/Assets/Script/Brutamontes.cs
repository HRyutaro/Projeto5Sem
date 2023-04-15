using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Brutamontes : MonoBehaviour
{
    [Header("vida")]
    public int vida;
    public int vidaAtual;
    public Collider col;
    public GameObject radiacao;
    public Slider life;
    private bool isDead;

    [Header("Perseguição")]
    public float speed;
    private float distMax;
    float distanciaDoAlvo;
    public Transform alvo;
    public float distanciaMin;
    public float distanciaMax;
    private NavMeshAgent navMeshAgent;

    [Header("Animação")]
    public Animator Anim;

    [Header("Dano")]
    public Color corDano;
    public Color corNormal;
    public Color corGelado;
    private bool tomouDano = false;

    [Header("ataque")]
    public float CdAtack;
    public Collider atack;
    public Collider atack2;
    public float speedAtaque;
    private float nextattackTime;
    private bool isAttacking = false;

    [Header("Especial")]
    public GameObject prefabDrop;
    public bool especialDrop;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = distanciaMin;
        navMeshAgent.speed = speed;
        distMax = distanciaMax;
        atack.enabled = false;
        life.maxValue = vida;
        vidaAtual = vida;
        isDead = false;
    }


    void Update()
    {
        life.value = vidaAtual;
        atacar();
    }

    void FixedUpdate()
    {
        move();
    }
    void move()
    {
        if(isDead == false)
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
    }

    void tomarDano(float dano)
    {
        StartCoroutine("DanoCorCD");
        if (vidaAtual >= 1)
        {
            vidaAtual -= 1;
        }
        if (vidaAtual <= 0)
        {
            if(especialDrop == true)
            {
                isDead = true;
                Anim.SetFloat("golpe", 0);
                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                GetComponentInChildren<MeshRenderer>().enabled = false;
                Instantiate(radiacao, gameObject.transform.position, gameObject.transform.rotation);
                Instantiate(prefabDrop, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(gameObject, 1f);

            }
            else if(especialDrop == false)
            {
                isDead = true;
                Anim.SetFloat("golpe", 0);
                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                GetComponentInChildren<MeshRenderer>().enabled = false;
                Instantiate(radiacao, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(gameObject, 1f);

            }
        }
    }
    void tomarDanoFogo()
    {
        StartCoroutine("DanoCorCDFogo");
        if (vidaAtual >= 1)
        {
            --vidaAtual;
        }
        if (vidaAtual <= 0)
        {
            isDead = true;
            Anim.SetFloat("golpe", 0);
            Instantiate(radiacao, gameObject.transform.position, gameObject.transform.rotation);
            GetComponent<MeshRenderer>().enabled = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 1f);
        }
    }
    void atacar()
    {
        if(isDead == false)
        {
            if (Time.time >= nextattackTime && tomouDano == false)
            {
            
                if (!isAttacking && Vector3.Distance(transform.position, alvo.position) <= distanciaMin)
                {
                    gameObject.transform.LookAt(alvo);
                    StartCoroutine("AtackGarra");
                }
                nextattackTime = Time.time + CdAtack;
            }
        }
    }

    void Congelado()
    {
        StartCoroutine("EfeitoGelo");
        if (vidaAtual >= 1)
        {
            Anim.SetFloat("golpe", 0);
            --vidaAtual;
        }
        else if (vidaAtual <= 0)
        {
            isDead = true;
            Anim.SetFloat("golpe", 0);
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
            Instantiate(radiacao, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject, 1f);
        }
    }
    IEnumerator AtackGarra()
    {
        Anim.SetFloat("golpe", 1);
        atack.enabled = true;
        atack2.enabled = true;
        yield return new WaitForSeconds(2);
        atack.enabled = false;
        atack2.enabled = false;
        Anim.SetFloat("golpe", 0);

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
        Anim.speed = 0.5f;
        yield return new WaitForSeconds(3f);
        navMeshAgent.speed = speed;
        Anim.speed = 1;
        GetComponent<Renderer>().material.color = corNormal;

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
        if (other.gameObject.CompareTag("espada") && !tomouDano)
        {
            StartCoroutine("CDTomarDano");
            tomarDano(1);
        }
        if (other.gameObject.tag == "gelo" && !tomouDano)
        {
            StartCoroutine("CDTomarDano");
            Congelado();
            GetComponent<Renderer>().material.color = corGelado;
        }
        if (other.gameObject.tag == "fogo" && !tomouDano)
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
        Gizmos.DrawWireSphere(transform.position, distanciaMax);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanciaMin);

    }
}
