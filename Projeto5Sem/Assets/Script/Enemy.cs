using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("vida")]
    public int vida;
    public int vidaAtual;
    public Collider col;
    public GameObject corpo;
    public GameObject radiacao;
    public Slider Life;
    private bool isDead;
    public Rigidbody rb;

    [Header("Perseguição")]
    public float speed;
    public float speedAtual;
    private float distMax;
    float distanciaDoAlvo;
    public Transform alvo;
    public float distanciaMin;
    public float distanciaMax;
    private NavMeshAgent navMeshAgent;
    public bool stop;

    [Header("Animação")]
    public Animator Anim;

    [Header("Dano")]
    public Color corDano;
    public Color corNormal;
    public Color corGelado;
    private bool tomouDano = false;
    public MeshRenderer mesh;
    public GameObject deBuff;
    public GameObject danoFrio;
    public GameObject raioeffect;
    public GameObject raioArea;
    private bool tomouRaio;

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

    [Header("Sons")]
    public AudioSource DanoAudio;
    public AudioSource MorteAudio;
    public AudioSource AtackAudio;

    void Start()
    {
        numeroDrops = Random.Range(1, 2);
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = distanciaMin;
        navMeshAgent.speed = speedAtual;
        distMax = distanciaMax;
        atack.enabled = false;
        Life.maxValue = vida;
        vidaAtual = vida;
        isDead = false;
        speedAtual = speed;
    }


    void Update()
    {
        Life.value = vidaAtual;
        atacar();
        ControleVida();
    }

    void FixedUpdate()
    {
        move();
    }
    void move()
    {
        if (stop == false)
        {
            gameObject.transform.LookAt(alvo);
            float distance = Vector3.Distance(transform.position, alvo.position);
            Vector3 direction = alvo.position - transform.position;
            direction.Normalize();
            if (distance < distanciaMin)
            {
                rb.velocity = Vector3.zero;
            }
            else if (distance > distMax)
            {
                rb.velocity = Vector3.zero;
            }
            else
            {
                Vector3 velocity = direction * speedAtual;
                rb.velocity = velocity;
            }

        }
    }

    void ControleVida()
    {
        if (vidaAtual <= 0 && isDead == false)
        {
            if (especialDrop == true)
            {
                MorteAudio.Play();
                isDead = true;
                Anim.SetFloat("Atack", 0);
                col.enabled =  false;
                corpo.SetActive(false);
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
                MorteAudio.Play();
                isDead = true;
                Anim.SetFloat("Atack", 0);
                col.enabled = false;
                corpo.SetActive(false);
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
                    AtackAudio.Play();
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


    void tomarDano(int dano)
    {
        StartCoroutine(DanoCorCD());
        vidaAtual -= dano;
        Anim.SetFloat("Atack", 0);
        DanoAudio.Play();
    }
    IEnumerator CDTomarDano()
    {
        tomouDano = true;
        yield return new WaitForSeconds(0.3f);
        tomouDano = false;
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
        --vidaAtual;
        navMeshAgent.isStopped = true;
        GetComponent<Renderer>().material.color = corDano;
        yield return new WaitForSeconds(0.3f);
        navMeshAgent.isStopped = false;
        yield return new WaitForSeconds(0.3f);

        --vidaAtual;
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(0.3f);
        navMeshAgent.isStopped = false;
        yield return new WaitForSeconds(0.3f);

        --vidaAtual;
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
        --vidaAtual;
        danoFrio.SetActive(true);
        stop = true;
        yield return new WaitForSeconds(1f);
        --vidaAtual;
        yield return new WaitForSeconds(1f);
        danoFrio.SetActive(false);
        stop = false;
        mesh.material.color = corNormal;

    }


    IEnumerator inSmoke()
    {
        --vidaAtual;
        Anim.speed = 0.5f;
        deBuff.SetActive(true);
        speedAtual = speed / 2;
        yield return new WaitForSeconds(3f);
        --vidaAtual;
        DanoAudio.Play();
        Anim.speed = 1;
        deBuff.SetActive(false);
        speedAtual = speed;
    }

    IEnumerator inShock()
    {
        tomouRaio = true;
        stop = true;
        yield return new WaitForSeconds(1f);
        stop = false;
        raioeffect.SetActive(true);
        raioArea.SetActive(true);
        --vidaAtual;
        DanoAudio.Play();
        yield return new WaitForSeconds(4f);
        --vidaAtual;
        DanoAudio.Play();
        raioeffect.SetActive(false);
        raioArea.SetActive(false);
        tomouRaio = false;
    }

    IEnumerator InShock2()
    {
        raioeffect.SetActive(true);
        stop = true;
        yield return new WaitForSeconds(1f);
        raioeffect.SetActive(false);
        stop = false;
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
            Congelado();
            mesh.material.color = corGelado;
            DanoAudio.Play();
        }
        if(other.gameObject.tag == "fogo" && !tomouDano)
        {
            StartCoroutine(CDTomarDano());
            tomarDanoFogo();
            DanoAudio.Play();
        }
        if (other.gameObject.tag == "fumaca")
        {
            StartCoroutine(CDTomarDano());
            StartCoroutine(inSmoke());
            DanoAudio.Play();
        }
        if(other.gameObject.tag == "Raio")
        {
            StartCoroutine(inShock());
            DanoAudio.Play();
        }
        if (other.gameObject.tag == "AreaRaio")
        {
            if (tomouRaio == false)
            {
                StartCoroutine(InShock2());
                --vidaAtual;
                DanoAudio.Play();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,distanciaMax);
    }

}
