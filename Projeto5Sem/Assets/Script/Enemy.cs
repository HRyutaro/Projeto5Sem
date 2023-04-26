using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("vida")]
    public int Vida;
    public int VidaAtual;
    public Collider col;
    public GameObject radiacao;
    public Slider Life;
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
    public GameObject deBuff;
    public GameObject danoFrio;

    [Header("ataque")]
    public float CdAtack;
    public Collider atack;
    public float speedAtaque;
    private float nextattackTime;
    private bool isAttacking = false;

    [Header("Especial")]
    public GameObject prefabDrop;
    public bool especialDrop;
    private int numeroDrops;

    void Start()
    {
        numeroDrops = Random.Range(0, 2);
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = distanciaMin;
        navMeshAgent.speed = speed;
        distMax = distanciaMax;
        atack.enabled = false;
        Life.maxValue = Vida;
        VidaAtual = Vida;
        isDead = false;
    }


    void Update()
    {
        Life.value = VidaAtual;
        atacar();
        ControleVida();
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

    void ControleVida()
    {
        if (VidaAtual <= 0 && isDead == false)
        {
            if (especialDrop == true)
            {
                isDead = true;
                Anim.SetFloat("Atack", 0);
                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                Destroy(gameObject, 1f);
                Instantiate(prefabDrop, gameObject.transform.position + new Vector3(0, 0.5f, 0), gameObject.transform.rotation);
                if (numeroDrops == 1)
                {
                    Instantiate(radiacao, gameObject.transform.position, gameObject.transform.rotation);
                }
                else if (numeroDrops == 2)
                {
                    Instantiate(radiacao, gameObject.transform.position, gameObject.transform.rotation);
                    Instantiate(radiacao, gameObject.transform.position, gameObject.transform.rotation);
                }
            }
            else if (especialDrop == false)
            {
                isDead = true;
                Anim.SetFloat("Atack", 0);
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
                Destroy(gameObject, 1f);
                if(numeroDrops == 1)
                {
                    Instantiate(radiacao, gameObject.transform.position, gameObject.transform.rotation);
                }
                else if(numeroDrops == 2)
                {
                    Instantiate(radiacao, gameObject.transform.position, gameObject.transform.rotation);
                    Instantiate(radiacao, gameObject.transform.position, gameObject.transform.rotation);
                }
            }
        }
    }

    void atacar()
    {
        if(isDead == false)
        {
            if(Time.time >= nextattackTime && tomouDano == false)
            {
                gameObject.transform.LookAt(alvo);
                if (!isAttacking && Vector3.Distance(transform.position, alvo.position) <= distanciaMin)
                {
                    StartCoroutine(AtackGarra());
                }
                nextattackTime = Time.time + CdAtack;
            }
        }
    }
    IEnumerator AtackGarra()
    {
        isAttacking = true;
        atack.enabled = true;
        Anim.SetFloat("Atack", 1);
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(1);
        isAttacking = false;
        atack.enabled = false;
        Anim.SetFloat("Atack", 0);
        navMeshAgent.isStopped = false;
    }
    IEnumerator CDTomarDano()
    {
        tomouDano = true;
        yield return new WaitForSeconds(0.3f);
        tomouDano = false;
    }

    void tomarDano(int dano)
    {
        StartCoroutine(DanoCorCD());
        VidaAtual -= dano;
        Anim.SetFloat("Atack", 0);
    }
    IEnumerator DanoCorCD()
    {
        navMeshAgent.isStopped = true;
        GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(1f);
        navMeshAgent.isStopped = false;
        GetComponent<Renderer>().material.color = corNormal;
    }

    void tomarDanoFogo()
    {
        StartCoroutine(DanoCorCDFogo());
    }
    IEnumerator DanoCorCDFogo()
    {
        --VidaAtual;
        navMeshAgent.isStopped = true;
        GetComponent<Renderer>().material.color = corDano;
        yield return new WaitForSeconds(0.3f);
        navMeshAgent.isStopped = false;
        yield return new WaitForSeconds(0.3f);

        --VidaAtual;
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(0.3f);
        navMeshAgent.isStopped = false;
        yield return new WaitForSeconds(0.3f);

        --VidaAtual;
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(0.3f);
        navMeshAgent.isStopped = false;
        GetComponent<Renderer>().material.color = corNormal;
    }


    void Congelado()
    {
        StartCoroutine(EfeitoGelo());
    }

    IEnumerator EfeitoGelo()
    {
        --VidaAtual;
        danoFrio.SetActive(true);
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(1f);
        --VidaAtual;
        yield return new WaitForSeconds(1f);
        danoFrio.SetActive(false);
        navMeshAgent.isStopped = false;
        GetComponent<Renderer>().material.color = corNormal;

    }


    IEnumerator inSmoke()
    {
        --VidaAtual;
        Anim.speed = 0.5f;
        deBuff.SetActive(true);
        navMeshAgent.speed = 1.5f;
        yield return new WaitForSeconds(3f);
        --VidaAtual;
        Anim.speed = 1;
        deBuff.SetActive(false);
        navMeshAgent.speed = speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("espada") && !tomouDano)
        {
            StartCoroutine(CDTomarDano());
            tomarDano(1);
        }
        if(other.gameObject.tag == "gelo" && !tomouDano)
        {
            StartCoroutine(CDTomarDano());
            Congelado();
            GetComponent<Renderer>().material.color = corGelado;
        }
        if(other.gameObject.tag == "fogo" && !tomouDano)
        {
            StartCoroutine(CDTomarDano());
            tomarDanoFogo();
        }
        if (other.gameObject.tag == "fumaca")
        {
            StartCoroutine(CDTomarDano());
            StartCoroutine(inSmoke());
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,distanciaMax);
    }

}
