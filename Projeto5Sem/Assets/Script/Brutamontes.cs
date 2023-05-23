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
    public Rigidbody rb;
    public GameObject Corpo;

    [Header("Perseguição")]
    public float speed;
    public float speedAtual;
    private float distMax;
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
    public GameObject corpo;
    public MeshRenderer mesh;
    public GameObject deBuff;
    public GameObject danoFrio;
    public GameObject danoPedra;
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
        distMax = distanciaMax;
        atack.enabled = false;
        life.maxValue = vida;
        vidaAtual = vida;
        isDead = false;
        speedAtual = speed;
    }


    void Update()
    {
        life.value = vidaAtual;
        atacar();
    }

    void FixedUpdate()
    {
        move();
        ControleVida();
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
                GetComponent<Collider>().enabled = false;
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
                corpo.SetActive(false);
                GetComponent<Collider>().enabled = false;
                Destroy(gameObject, 1f);
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
        }
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

    void atacar()
    {
        if(isDead == false)
        {
            if (Time.time >= nextattackTime && tomouDano == false)
            {
            
                if (!isAttacking && Vector3.Distance(transform.position, alvo.position) <= distanciaMin)
                {
                    gameObject.transform.LookAt(alvo);
                    StartCoroutine(AtackGarra());
                    AtackAudio.Play();
                }
                nextattackTime = Time.time + CdAtack;
            }
        }
    }
    IEnumerator AtackGarra()
    {
        Anim.SetFloat("golpe", 1);
        atack.enabled = true;
        yield return new WaitForSeconds(1);
        atack.enabled = false;
        Anim.SetFloat("golpe", 0);

    }


    void tomarDano(int dano)
    {
        StartCoroutine(DanoCorCD());
        vidaAtual -= dano;
        Anim.SetFloat("golpe", 0);
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
        mesh.material.color = Color.red;
        stop = true;
        yield return new WaitForSeconds(1f);
        stop = false;
        mesh.material.color = corNormal;
    }


    void tomarDanoFogo()
    {
        StartCoroutine("DanoCorCDFogo");
    }
    IEnumerator DanoCorCDFogo()
    {
        --vidaAtual;
        stop = true;
        mesh.material.color = corDano;
        yield return new WaitForSeconds(0.3f);
        stop = false;
        yield return new WaitForSeconds(0.3f);

        --vidaAtual;
        stop = true;
        yield return new WaitForSeconds(0.3f);
        stop = false;
        yield return new WaitForSeconds(0.3f);

        --vidaAtual;
        stop = true;
        yield return new WaitForSeconds(0.3f);
        stop = false;
        mesh.material.color = corNormal;
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
        mesh.material.color = corGelado;
        yield return new WaitForSeconds(1f);
        --vidaAtual;
        DanoAudio.Play();
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
        danoPedra.SetActive(true);
        yield return new WaitForSeconds(3f);
        DanoAudio.Play();
        danoPedra.SetActive(false);
        --vidaAtual;
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
        if (other.gameObject.CompareTag("espada") && !tomouDano)
        {
            StartCoroutine(CDTomarDano());
            tomarDano(1);
        }
        if (other.gameObject.tag == "fogo" && !tomouDano)
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
        if (other.gameObject.tag == "Raio")
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
        Gizmos.DrawWireSphere(transform.position, distanciaMax);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanciaMin);

    }
}
