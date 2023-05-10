using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossUrso : MonoBehaviour
{
    [Header("vida")]
    public int Vida;
    public int VidaAtual;
    public Collider col;
    public GameObject radiacao;
    public Slider Life;
    private bool isDead;
    public Rigidbody rb;

    [Header("Perseguição")]
    private bool stop;
    public float speed;
    private float speedAtual;
    private float distMax;
    float distanciaDoAlvo;
    public Transform alvo;
    public float distanciaMin;
    public float distanciaMax;
    private NavMeshAgent navMeshAgent;

    [Header("Animação")]
    public Animator Anim;

    [Header("Dano")]
    public MeshRenderer mesh;
    public Color corDano;
    public Color corNormal;
    public Color corGelado;
    private bool tomouDano = false;
    public GameObject deBuff;
    public GameObject danoFrio;
    public GameObject raioeffect;
    public GameObject raioArea;

    [Header("ataque")]
    public float CdAtack;
    public Collider atack;
    public float speedAtaque;
    private float nextattackTime;
    private bool isAttacking = false;

    [Header("ATAQUE ESPECIAL")]
    public bool podeInvestida;
    public float CdInvestida;
    public float primeiraInvestida;
    public float forcaInvestida;
    public GameObject areaInvestida;

    [Header("Especial")]
    public GameObject prefabDrop;
    public bool especialDrop;
    private int numeroDrops;
    public  static bool startBoss = false;

    void Start()
    {
        numeroDrops = Random.Range(1, 2);
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = distanciaMin;
        distMax = distanciaMax;
        atack.enabled = false;
        Life.maxValue = Vida;
        VidaAtual = Vida;
        speedAtual = speed;
        isDead = false;
    }


    void Update()
    {
        Life.value = VidaAtual;

        if(startBoss == true)
        {
            atacar();
            ControleVida();
            
        }
    }

    void FixedUpdate()
    {
        
        if (startBoss == true)
        {
            move();
            investida();

        }
        
    }


    void investida()
    {
        if(podeInvestida == true)
        {
            Anim.SetFloat("Atack", 0);
            stop = true;
            gameObject.transform.LookAt(alvo);
            StartCoroutine(Investida());
            gameObject.tag = "BossInvestida";
            rb.velocity = Vector3.zero;
            col.isTrigger = true;
        }
    }

    IEnumerator Investida()
    {
        podeInvestida = false;
        areaInvestida.SetActive(true);
        Anim.SetFloat("PreparandoInvestida", 1);
        yield return new WaitForSeconds(3);
        Anim.SetFloat("PreparandoInvestida", 0);
        Anim.SetFloat("Investida", 1);
        Vector3 empurrar = transform.forward;
        rb.AddForce(empurrar * forcaInvestida, ForceMode.Impulse);
        areaInvestida.SetActive(false);
        yield return new WaitForSeconds(1);
        Anim.SetFloat("Investida", 0);
        gameObject.tag = "BossUrso";
        yield return new WaitForSeconds(3);
        stop = false;
        col.isTrigger = false;
        yield return new WaitForSeconds(CdInvestida);
        podeInvestida = true;

    }

    void move()
    {
        if(stop == false)
        {
            gameObject.transform.LookAt(alvo);
            float distance = Vector3.Distance(transform.position, alvo.position);
            Vector3 direction = alvo.position - transform.position;
            direction.Normalize();
            if(distance < distanciaMin)
            {
                rb.velocity = Vector3.zero;
            }
            else if( distance > distMax)
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
        if (VidaAtual <= 0 && isDead == false)
        {
            if (especialDrop == true)
            {
                BloqueadoPeloBoss.bossUrsoisDead = true;
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
                BloqueadoPeloBoss.bossUrsoisDead = true;
                isDead = true;
                Anim.SetFloat("Atack", 0);
                GetComponent<MeshRenderer>().enabled = false;
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

    void atacar()
    {
        if (isDead == false)
        {
            if (Time.time >= nextattackTime && tomouDano == false)
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
        stop = true;
        yield return new WaitForSeconds(1);
        isAttacking = false;
        atack.enabled = false;
        Anim.SetFloat("Atack", 0);
        stop = false;
    }


    void tomarDano(int dano)
    {
        StartCoroutine(DanoCorCD());
        VidaAtual -= dano;
        Anim.SetFloat("Atack", 0);
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
        yield return new WaitForSeconds(1f);
        mesh.material.color = corNormal;
    }

    void tomarDanoFogo()
    {
        StartCoroutine(DanoCorCDFogo());
    }
    IEnumerator DanoCorCDFogo()
    {
        --VidaAtual;
        stop = true;
        mesh.material.color = corDano;
        yield return new WaitForSeconds(0.3f);
        stop = false;
        yield return new WaitForSeconds(0.3f);

        --VidaAtual;
        yield return new WaitForSeconds(0.3f);
        navMeshAgent.isStopped = false;
        yield return new WaitForSeconds(0.3f);

        --VidaAtual;
        yield return new WaitForSeconds(0.3f);
        mesh.material.color = corNormal;
    }


    void Congelado()
    {
        StartCoroutine(EfeitoGelo());
    }

    IEnumerator EfeitoGelo()
    {
        tomouDano = true;
        --VidaAtual;
        danoFrio.SetActive(true);
        stop = true;
        mesh.material.color = corGelado;
        yield return new WaitForSeconds(0.5f);
        --VidaAtual;
        yield return new WaitForSeconds(0.5f);
        danoFrio.SetActive(false);
        stop = false;
        mesh.material.color = corNormal;
        tomouDano = false;
    }


    IEnumerator inSmoke()
    {
        --VidaAtual;
        Anim.speed = 0.5f;
        deBuff.SetActive(true);
        speedAtual = 1.5f;
        yield return new WaitForSeconds(3f);
        --VidaAtual;
        Anim.speed = 1;
        deBuff.SetActive(false);
        speedAtual = speed;
    }

    IEnumerator inShock()
    {
        stop = true;
        yield return new WaitForSeconds(1f);
        stop = false;
        raioeffect.SetActive(true);
        raioArea.SetActive(true);
        --VidaAtual;
        yield return new WaitForSeconds(4f);
        --VidaAtual;
        raioeffect.SetActive(false);
        raioArea.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("espada") && !tomouDano)
        {
            StartCoroutine(CDTomarDano());
            tomarDano(1);
        }
        if (other.gameObject.tag == "gelo" && !tomouDano)
        {
            StartCoroutine(CDTomarDano());
            Congelado();
            
        }
        if (other.gameObject.tag == "fogo" && !tomouDano)
        {
            StartCoroutine(CDTomarDano());
            tomarDanoFogo();
        }
        if (other.gameObject.tag == "fumaca")
        {
            StartCoroutine(CDTomarDano());
            StartCoroutine(inSmoke());
        }
        if (other.gameObject.tag == "Raio")
        {
            StartCoroutine(inShock());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaMax);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, distanciaMin);
    }

}
