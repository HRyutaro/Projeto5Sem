using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    [Header("Combat")]
    public int VidaTotal;
    public static int VidaAtual;
    public GameObject espadaAtack;
    public GameObject espadaCosta;
    public GameObject espada;
    public float CDAtack;
    private bool isCdAtack;
    private bool stop;
    public int isDashing = 4;
    public float forcaDash;
    public float CDDash;

    [Header("pocao")]
    public GameObject pocaoPrefab;
    public float forcaArremesso;
    public Transform pocaoRespawn;
    public float CDRange;
    private bool isCdRange;
    public int temPocaoFogo;
    public int temPocaoGelo;
    public int temPocaoFumaca;
    public int temPocaoMana;
    public int temPocaoCura;

    [Header("magia")]
    public int manaTotal;
    public int manaAtual;
    public GameObject feiticoPrefab;
    public float forcaArremessoFeitico;
    public Transform feiticoRespawn;

    [Header("Movimento")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5;
    [SerializeField] private float turnSpeed = 360;
    private Vector3 input;

    [Header("Dano")]
    public float forcaEmpurrao;
    public float tempoEmpurrado = 0.5f;
    public Animator Anim;
    public float CdTomarDano;
    private bool dashing;

    public ParticleSystem curar;
    public ParticleSystem curarMana;

    void Start()
    {
        instance = this;
        rb.GetComponent<Rigidbody>();
        VidaAtual = VidaTotal;
        startNumeroPocao();
    }
    void Update()
    {
        GatherInput();
        Look();
        AtackMelee();
        AtackRange();
        Vida();
        trocarPocao();
        atackPocao();
    }

    private void FixedUpdate()
    {
        Move();
        Dash();
    }

    void GatherInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
    }
    void Look()
    {
        if( input != Vector3.zero & stop == false)
        {
            var relative = (transform.position + input.toIso()) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);
     
            transform.rotation = Quaternion.RotateTowards(transform.rotation,rot,turnSpeed); //rotação mais demorada (...turnSpeed * Time.deltatime)
        }
        

    }

    void Move()
    {
        if(stop == false)
        {
            rb.MovePosition(transform.position + (transform.forward * input.magnitude)* speed * Time.deltaTime);
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                Anim.SetFloat("isRun", 1);
            }
            else
            {
                Anim.SetFloat("isRun", 0);
            }

        }
    }

    void AtackMelee()
    {
        if(Input.GetButtonDown("Fire1") & isCdAtack == false)
        {
            StartCoroutine("AtackMeleeTime");
            StartCoroutine("CDAtackMelee");
        }
    }

    void AtackRange()
    {
        if(Input.GetButtonDown("L1")|| Input.GetKeyDown(KeyCode.E) && isCdRange == false && manaAtual >= 1)
        {
            Vector3 direcao = transform.forward;

            GameObject magia = Instantiate(feiticoPrefab, feiticoRespawn.position, feiticoRespawn.rotation);
            magia.GetComponent<Rigidbody>().AddForce(direcao * forcaArremessoFeitico, ForceMode.Impulse);
            StartCoroutine("CDAtackRange");
            manaAtual -= 1;
        }
    }

    void atackPocao()
    {
        if (Input.GetButtonDown("Fire2") && isCdRange == false)
        {
            if (pocao.tipoDapocao == 0)
            {
                if (temPocaoFogo > 0)
                {
                    Vector3 direcao = transform.forward;

                    GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                    pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                    StartCoroutine("CDAtackRange");
                    --temPocaoFogo;
                    GameController.numeroPocoesAtual = temPocaoFogo;


                }
            }
            if (pocao.tipoDapocao == 1)
            {
                if (temPocaoGelo > 0)
                {
                    Vector3 direcao = transform.forward;

                    GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                    pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                    StartCoroutine("CDAtackRange");
                    --temPocaoGelo;
                    GameController.numeroPocoesAtual = temPocaoGelo;
                }
            }
            if (pocao.tipoDapocao == 2)
            {
                if (temPocaoCura > 0)
                {
                    if (VidaAtual < VidaTotal)
                    {
                        StartCoroutine("Cura");
                        ++VidaAtual;
                        --temPocaoCura;
                        GameController.numeroPocoesAtual = temPocaoCura;
                    }
                    else
                    {
                        StartCoroutine("Cura");
                        --temPocaoCura;
                        GameController.numeroPocoesAtual = temPocaoCura;
                    }
                }
            }
            if (pocao.tipoDapocao == 3)
            {
                if (temPocaoFumaca > 0)
                {
                    Vector3 direcao = transform.forward;

                    GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                    pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                    StartCoroutine("CDAtackRange");
                    --temPocaoFumaca;
                    GameController.numeroPocoesAtual = temPocaoFumaca;
                }
            }
            if (pocao.tipoDapocao == 4)
            {
                if (temPocaoMana > 0)
                {
                    if (manaAtual < manaTotal)
                    {
                        StartCoroutine("ManaCura");
                        ++manaAtual;
                        --temPocaoMana;
                        GameController.numeroPocoesAtual = temPocaoMana;
                    }
                    else
                    {
                        StartCoroutine("ManaCura");
                        --temPocaoMana;
                        GameController.numeroPocoesAtual = temPocaoMana;
                    }
                }
            }
        }
    }

    void Vida()
    {
        if(VidaAtual <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    
    public void TomarDano(int Dano)
    {
        if(dashing == false)
        {
            StartCoroutine("CDTomarDano");
            VidaAtual -= Dano;

            Vector3 empurrar = -transform.forward;

            rb.AddForce(empurrar * forcaEmpurrao, ForceMode.Impulse);
        }
    }

    void Dash()
    {
        if(Input.GetButton("Fire3") && isDashing == 4)
        {
            isDashing = 0;
            Vector3 dashing = transform.forward;
            rb.AddForce(dashing * forcaDash, ForceMode.Impulse);
            StartCoroutine("CdDash");
        }
    }

    void trocarPocao()
    {

        if (Input.GetButtonDown("R1") || Input.GetKeyDown(KeyCode.Q))
        {
            if(pocao.tipoDapocao == 0)
            {
                GameController.numeroPocoesAtual = temPocaoGelo;
                pocao.tipoDapocao = 1;
                print("pocao Gelo");
            }
            else if(pocao.tipoDapocao == 1)
            {
                GameController.numeroPocoesAtual = temPocaoCura;
                pocao.tipoDapocao = 2;
                print("pocao Cura");
            }
            else if (pocao.tipoDapocao == 2)
            {
                GameController.numeroPocoesAtual = temPocaoFumaca;
                pocao.tipoDapocao = 3;
                print("pocao fumaca");
            }
            else if (pocao.tipoDapocao == 3)
            {
                GameController.numeroPocoesAtual = temPocaoMana;
                pocao.tipoDapocao = 4;
                print("pocao mana");
            }
            else if (pocao.tipoDapocao == 4)
            {
                GameController.numeroPocoesAtual = temPocaoFogo;
                pocao.tipoDapocao = 0;
                print("pocao fogo");
            }
        }
    }

    void startNumeroPocao()
    {
        if (pocao.tipoDapocao == 0)
        {
            GameController.numeroPocoesAtual = temPocaoFogo;
            print("pocao Fogo");
        }
        else if (pocao.tipoDapocao == 1)
        {
            GameController.numeroPocoesAtual = temPocaoGelo;
            print("pocao Gelo");
        }
        else if (pocao.tipoDapocao == 2)
        {
            GameController.numeroPocoesAtual = temPocaoCura;
            print("pocao Cura");
        }
        else if (pocao.tipoDapocao == 3)
        {
            GameController.numeroPocoesAtual = temPocaoFumaca;
            print("pocao Fumaca");
        }
        else if (pocao.tipoDapocao == 4)
        {
            GameController.numeroPocoesAtual = temPocaoMana;
            print("pocao Mana");
        }
    }

    IEnumerator CdDash()
    {
        dashing = true;
        yield return new WaitForSeconds(CDDash);
        dashing = false;
        isDashing += 1;
        yield return new WaitForSeconds(CDDash);
        isDashing += 1;
        yield return new WaitForSeconds(CDDash);
        isDashing += 1;
        yield return new WaitForSeconds(CDDash);
        isDashing += 1;
        print("Dash inCD " + isDashing);

    }

    IEnumerator AtackMeleeTime()
    {
        stop = true; 
        espada.SetActive(true);
        Anim.SetFloat("Atack", 1);
        espadaAtack.SetActive(true);
        espadaCosta.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        stop = false;
        espada.SetActive(false);
        Anim.SetFloat("Atack", 0);
        espadaCosta.SetActive(true);
        espadaAtack.SetActive(false);
    }

    IEnumerator CDAtackMelee()
    {
        isCdAtack = true;
        yield return new WaitForSeconds(CDAtack);
        isCdAtack = false;
    }

    IEnumerator CDAtackRange()
    {
        isCdRange = true;
        yield return new WaitForSeconds(CDRange);
        isCdRange = false;
    }

    IEnumerator CDTomarDano()
    {
        stop = true;
        yield return new WaitForSeconds(CdTomarDano);
        stop = false;

    }

    IEnumerator Cura()
    {
        curar.Play(true);
        yield return new WaitForSeconds(0.5f);
        curar.Play(false);

    }
    IEnumerator ManaCura()
    {
        curarMana.Play(true);
        yield return new WaitForSeconds(0.5f);
        curarMana.Play(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "garra")
        {
            TomarDano(1);
        }
    }
}
