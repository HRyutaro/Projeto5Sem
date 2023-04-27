using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyRanged : MonoBehaviour
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
    public float RotationSpeed;

    [Header("Animação")]
    public Animator Anim;

    [Header("Dano")]
    public Color corDano;
    public Color corNormal;
    public Color corGelado;
    private bool tomouDano = false;

    [Header("ataque")]
    public float CdAtack;
    public float speedAtaque;
    private float nextattackTime;
    private bool isAttacking = false;
    public GameObject atackprefab;
    public Transform atackRespawn;
    public float forcaAtack;

    [Header("Especial")]
    public GameObject prefabDrop;
    public bool especialDrop;
    private int numeroDrops;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = distanciaMin;
        navMeshAgent.speed = speed;
        distMax = distanciaMax;
        life.maxValue = vida;
        vidaAtual = vida;
        isDead = false;
    }


    void Update()
    {
        life.value = vidaAtual;
        atacar();
        ControleVida();
    }

    void FixedUpdate()
    {
        move();
    }

    void ControleVida()
    {
        if (vidaAtual <= 0 && isDead == false)
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

    void TomarDano(float dano)
    {
        StartCoroutine("DanoCorCD");
        if (vidaAtual >= 1)
        {
            Anim.SetFloat("Guspindo", 0);
            vidaAtual -= 1;
            tomouDano = true;
        }
        if (vidaAtual <= 0)
        {
            isDead = true;
            Anim.SetFloat("Guspindo", 0);
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            Instantiate(radiacao, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject, 1f);
        }
    }
    void TomarDanoFogo()
    {
        StartCoroutine("DanoCorCDFogo");
        if (vidaAtual >= 1)
        {
            tomouDano = true;
            --vidaAtual;
        }
        if (vidaAtual <= 0)
        {
            isDead = true;
            Anim.SetFloat("Guspindo", 0);
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            Instantiate(radiacao, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject, 1f);
        }
    }
    void atacar()
    {
        if(isDead == false)
        {
            if (Time.time >= nextattackTime && tomouDano == false)
            {
                gameObject.transform.LookAt(alvo);
                if (!isAttacking && Vector3.Distance(transform.position, alvo.position) <= distanciaMin)
                {
                    Vector3 direcao = transform.forward;
                    GameObject guspe = Instantiate(atackprefab, atackRespawn.position, atackRespawn.rotation);
                    guspe.GetComponent<Rigidbody>().AddForce(direcao * forcaAtack, ForceMode.Impulse);
                    StartCoroutine(AnimacaoGuspe());
                }
                nextattackTime = Time.time + CdAtack;
                gameObject.transform.LookAt(alvo);
            }
        }
    }

    void Congelado()
    {
        StartCoroutine("EfeitoGelo");
        if (vidaAtual >= 1)
        {
            Anim.SetFloat("Guspindo", 0);
            --vidaAtual;
        }
        else if (vidaAtual <= 0)
        {
            isDead = true;
            Anim.SetFloat("Guspindo", 0);
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            Instantiate(radiacao, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject, 1f);
        }
    }
    IEnumerator AnimacaoGuspe()
    {
        Anim.SetFloat("Guspindo", 1);
        yield return new WaitForSeconds(0.3f);
        Anim.SetFloat("Guspindo", 0);
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
            TomarDano(1);
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
            TomarDanoFogo();
        }
        if (other.gameObject.tag == "fumaca")
        {
            StartCoroutine(inSmoke());
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanciaMax);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaMin);
    }
}
