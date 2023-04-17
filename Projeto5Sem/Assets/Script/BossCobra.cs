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

    [Header("Ataque")]
    public GameObject prefabGuspe;
    public Transform[] spawnprefab;
    public float cdAtackGuspe;
    public float cdTrocaDePosicao;
    public float forcaGuspe;
    private bool guspiu;

    void Start()
    {
        vidaAtual = vida;
        speedAtual = speed;
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
            anim.SetBool("dieing", true);
            Corpo.SetActive(false);
        }
        else if(vidaAtual <= 0)
        {
            isDead = true;
            Destroy(gameObject, 1);
        }
    }

    void Atacar()
    {
        if(spawnNumber == 0)
        {
            gameObject.transform.position = spawn[0].position;
            gameObject.transform.rotation = spawn[0].rotation;
            if (guspiu == false)
            {
                guspiu = true;
                StartCoroutine(atacarGuspe());
            }
        }
        else if(spawnNumber > 0)
        {
            Andar();
        }

    }
    void Andar()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    void Congelado()
    {
        StartCoroutine("EfeitoGelo");
        if (vidaAtual >= 1)
        {
            anim.speed = 0.5f;
            --vidaAtual;
        }
        else if (vidaAtual <= 0)
        {
            isDead = true;
            col.enabled = false;
            mesh.enabled = false;
            mesh2.enabled = false;
            mesh3.enabled = false;
            Instantiate(drop, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject, 1f);
        }
    }
    void TomarDano(float dano)
    {
        StartCoroutine("DanoCorCD");
        if (vidaAtual >= 1)
        {
            vidaAtual -= 1;
            tomouDano = true;
        }
        if (vidaAtual <= 0)
        {
            isDead = true;
            col.enabled = false;
            mesh.enabled = false;
            mesh2.enabled = false;
            mesh3.enabled = false;
            Instantiate(drop, gameObject.transform.position, gameObject.transform.rotation);
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
            col.enabled = false;
            mesh.enabled = false;
            mesh2.enabled = false;
            mesh3.enabled = false;
            Instantiate(drop, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject, 1f);
        }
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
        AtaqueGuspe();
        yield return new WaitForSeconds(cdAtackGuspe);
        AtaqueGuspe();
        yield return new WaitForSeconds(cdAtackGuspe);
        guspiu = false;
        trocar = true;
        trocou = 0;
    }

    IEnumerator TrocarDeposicao()
    {
        yield return new WaitForSeconds(cdTrocaDePosicao);
        ++ trocou;
        trocar = true;
    }
    IEnumerator CDTomarDano()
    {
        tomouDano = true;
        yield return new WaitForSeconds(0.3f);
        tomouDano = false;
    }
    IEnumerator EfeitoGelo()
    {
        speedAtual = 0.01f;
        yield return new WaitForSeconds(3f);
        speedAtual = speed;
        mesh.material.color = corNormal;
        mesh2.material.color = corNormal;
        mesh3.material.color = corNormal;

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
    IEnumerator DanoCorCDFogo()
    {
        mesh.material.color = corOrange;
        mesh2.material.color = corOrange;
        mesh3.material.color = corOrange;
        yield return new WaitForSeconds(0.3f);
        mesh.material.color = corNormal;
        mesh2.material.color = corNormal;
        mesh3.material.color = corNormal;
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

    }

}
