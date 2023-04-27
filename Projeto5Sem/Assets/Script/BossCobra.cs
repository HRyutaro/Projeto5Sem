using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCobra : MonoBehaviour
{
    [Header("Vida")]
    public int vida;
    public static int vidaAtual;
    public bool isDead;
    private bool tomouDano;
    public GameObject Corpo;
    public GameObject calda;

    [Header("Configuração")]
    private int spawnNumber;
    public Transform[] spawn;
    public bool trocar;
    public Rigidbody rb;
    public float speed;
    public float speedAtual;
    public Color corNormal;
    public Color corOrange;
    public Color corGelado;
    public Animator anim;
    public GameObject drop;
    public int trocou;
    public Collider col;
    public MeshRenderer mesh;
    public MeshRenderer mesh2;
    public MeshRenderer mesh3;
    public static bool startBossBattle;
    public GameObject olhoD;
    public GameObject olhoE;

    [Header("Ataque")]
    public GameObject prefabGuspe;
    public Transform[] spawnprefab;
    public float cdAtackGuspe;
    public float cdTrocaDePosicao;
    public float forcaGuspe;
    private bool guspiu;
    private float primeiroAtack;

    public GameObject frioEffect;
    void Start()
    {
        vidaAtual = vida;
        speedAtual = speed;
        primeiroAtack = 3;
    }

    void Update()
    {
        if(startBossBattle == true)
        {
            if(isDead == false)
            {
                controleVida();
                trocarDePosicao();
                Atacar();
            }
        }
    }

    void controleVida()
    {
        if(vidaAtual <= 4)
        {
            anim.speed = 1;
            olhoD.SetActive(true);
            olhoE.SetActive(true);
        }
        else if(vidaAtual <= 0)
        {
            BloqueadoPeloBoss.bossCobraisDead = true;
            isDead = true;
            col.enabled = false;
            mesh.enabled = false;
            mesh2.enabled = false;
            mesh3.enabled = false;
            Instantiate(drop, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject, 1);
        }
    }

    void Atacar()
    {
        if(spawnNumber == 0)
        {
            gameObject.transform.position = spawn[0].position;
            gameObject.transform.rotation = spawn[0].rotation;
            gameObject.tag = "BossParado";
            
            if (guspiu == false)
            {
                guspiu = true;
                StartCoroutine(atacarGuspe());
            }
        }
        else if(spawnNumber > 0)
        {
            gameObject.tag = "CobraBoss";
            Andar();
        }

    }
    void AtaqueGuspe()
    {
        GameObject guspe = Instantiate(prefabGuspe, spawnprefab[0].position, spawnprefab[0].rotation);
        GameObject guspe1 = Instantiate(prefabGuspe, spawnprefab[1].position, spawnprefab[1].rotation);
        GameObject guspe2 = Instantiate(prefabGuspe, spawnprefab[2].position, spawnprefab[2].rotation);
        GameObject guspe3 = Instantiate(prefabGuspe, spawnprefab[3].position, spawnprefab[3].rotation);
        GameObject guspe4 = Instantiate(prefabGuspe, spawnprefab[4].position, spawnprefab[4].rotation);
    }
    IEnumerator atacarGuspe()
    {
        yield return new WaitForSeconds(primeiroAtack);
        AtaqueGuspe();
        yield return new WaitForSeconds(cdAtackGuspe);
        AtaqueGuspe();
        yield return new WaitForSeconds(cdAtackGuspe);
        guspiu = false;
        trocar = true;
        trocou = 0;
        primeiroAtack = 1;
    }


    void Andar()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    void trocarDePosicao()
    {
        if(trocou == 4)
        {
            spawnNumber = 0;
        }
        else if (trocou < 4)
        {
            if(trocar == true)
            {
                trocar = false;
                spawnNumber = Random.Range(1, 5);
                print("numero Random " + spawnNumber);
                print(trocou);
                gameObject.transform.position = spawn[spawnNumber].position;
                gameObject.transform.rotation = spawn[spawnNumber].rotation;
                StartCoroutine(TrocarDeposicao());
            }
        }
    }
    IEnumerator TrocarDeposicao()
    {
        yield return new WaitForSeconds(cdTrocaDePosicao);
        ++ trocou;
        trocar = true;
    }


    void TomarDano(float dano)
    {
        StartCoroutine(DanoCorCD());
        --vidaAtual;
    }
    IEnumerator DanoCorCD()
    {
        mesh.material.color = Color.red;
        mesh2.material.color = Color.red;
        mesh3.material.color = Color.red;
        yield return new WaitForSeconds(1f);
        mesh.material.color = corNormal;
        mesh2.material.color = corNormal;
        mesh3.material.color = corNormal;
    }


    IEnumerator CDTomarDano()
    {
        tomouDano = true;
        yield return new WaitForSeconds(0.3f);
        tomouDano = false;
    }


    void Congelado()
    {
        StartCoroutine(EfeitoGelo());
    }
    IEnumerator EfeitoGelo()
    {
        --vidaAtual;
        frioEffect.SetActive(true);
        mesh.material.color = corGelado;
        mesh2.material.color = corGelado;
        mesh3.material.color = corGelado;
        yield return new WaitForSeconds(1f);
        --vidaAtual;
        yield return new WaitForSeconds(1f);
        frioEffect.SetActive(false);
        mesh.material.color = corNormal;
        mesh2.material.color = corNormal;
        mesh3.material.color = corNormal;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("espada") && !tomouDano)
        {
            StartCoroutine(CDTomarDano());
            TomarDano(1);
        }
        if (other.gameObject.tag == "gelo" && !tomouDano)
        {
            StartCoroutine(CDTomarDano());
            Congelado();
        }

    }

}
