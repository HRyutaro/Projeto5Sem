using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    [Header("Saude")]
    public int VidaTotal;
    public static int VidaAtual;
    public ParticleSystem curar;
    public ParticleSystem curarMana;
    public MeshRenderer playerMesh;
    public GameObject playerMeshCorpo;
    

    [Header("Movimento")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5;
    private float speedAtual;
    [SerializeField] private float turnSpeed = 360;
    private Vector3 input;
    private Vector3 input2;
    [SerializeField] public bool isPaused;
    [SerializeField] public bool stop;

    [Header("Combat")]
    public GameObject espadaCosta;
    public GameObject espada;
    public GameObject efeitoEspada;
    public float CDAtack;
    public float CDRange2;
    private bool isCdAtack;
    public int isDashing = 4;
    public float forcaDash;
    public float CDDash;
    public float timeInDashing;
    public TrailRenderer rastroDash;
    private bool isAiming = false;
    private bool isAtacking = false;

    [Header("Pocao")]
    public GameObject pocaoPrefab;
    public float forcaArremesso;
    public Transform pocaoRespawn;
    public float CDRange;
    private bool isCdRange;

    [Header("Inventario")]
    public int Radiacao;
    public int temPocaoFogo;
    public int temPocaoGelo;
    public int temPocaoFumaca;
    public int temPocaoMana;
    public int temPocaoCura;
    public int temPlantaFogo;
    public int temPlantaGelo;
    public int temPlantaFumaca;
    public int temPlantaMana;
    public int temPlantaCura;
    public bool TemCartao;
    public bool TemCartao2;

    [Header("Magia")]
    public int manaTotal;
    public int manaAtual;
    public GameObject feiticoPrefab;
    public float forcaArremessoFeitico;
    public Transform feiticoRespawn;


    [Header("Dano")]
    public float forcaEmpurrao;
    public float tempoEmpurrado = 0.5f;
    public Animator Anim;
    public float CdTomarDano;
    private bool dashing;
    private bool TomouDano = false;
    public Color corNormal;

    //controles
    public static int tipoDeControle;

    public GameObject Interacao;

    void Start()
    {
        instance = this;
        VidaAtual = VidaTotal;
        StartNumeroPocao();
        speedAtual = speed;
    }
    void Update()
    {
        if (isPaused == false && stop == false)
        {
            AtackMelee();
            GatherInput();
            Look();
            Vida();
            trocarPocao();
            atackPocao();
        }
    }

    private void FixedUpdate()
    {
        if(isPaused == false && stop == false)
        {
            Move();
            Dash();
        }
    }

    void GatherInput()
    {
        if(tipoDeControle == 1)
        {
            input = new Vector3(Input.GetAxisRaw("HorizontalJoystick"), 0, Input.GetAxisRaw("VerticalJoystick"));
            input2 = new Vector3(Input.GetAxisRaw("Horizontal2"), 0, Input.GetAxisRaw("Vertical2"));
        }
        if (tipoDeControle == 0)
        {
            input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }
    }
    void Look()
    {
        if( input != Vector3.zero)
        {
            if(isAtacking == false )
            {
                var relative = (transform.position + input.toIso()) - transform.position;
                var rot = Quaternion.LookRotation(relative, Vector3.up);
     
                transform.rotation = Quaternion.RotateTowards(transform.rotation,rot,turnSpeed); //rotação mais demorada (...turnSpeed * Time.deltatime)

            }
        }
    }

    void Move()//movimento cm velocidade
    {
        if (isAiming == false && isAtacking == false && stop == false)
        {
            rb.velocity = input.toIso() * speedAtual;
            if (Input.GetAxisRaw("Horizontal") != 0 && speedAtual >= 1 && tipoDeControle == 0 || Input.GetAxisRaw("Vertical") != 0 && speedAtual >= 1 && tipoDeControle == 0)
            {
                Anim.SetFloat("isRun", 1);
            }
            else
            {
                Anim.SetFloat("isRun", 0);
            }
        }
        
    }
    void Move2()
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
    } //movimento cm transform

    void AtackMelee()
    {
        
        if (tipoDeControle == 1)
        {
            if (Input.GetButtonDown("R1") && isCdAtack == false)
            {
                Anim.SetFloat("isRun", 0);
                StartCoroutine("AtackMeleeTime");
                StartCoroutine("CDAtackMelee");
                Vector3 atackingfoward = transform.forward;
                rb.AddForce(4.5f * atackingfoward, ForceMode.Impulse);
            }
        }
        else if (tipoDeControle == 0)
        {
            if (Input.GetButtonDown("mouse0") && isCdAtack == false)
            {
                Anim.SetFloat("isRun", 0);
                StartCoroutine("AtackMeleeTime");
                StartCoroutine("CDAtackMelee");
                Vector3 atackingfoward = transform.forward;
                rb.AddForce(4.5f * atackingfoward, ForceMode.Impulse);
            }
        }
    }

    void AtackRange()
    {
        if(Input.GetButton("L1") || Input.GetButton("mouse1"))
        {
            
            if(manaAtual >= 1)
            {
                if (isCdRange == false)
                {
                    Anim.SetFloat("Feitico", 1);
                    Anim.SetFloat("isRun", 0);
                    stop = true;
                    isAiming = true;
                }
            }

        }
        if (Input.GetButtonUp("L1") || Input.GetButtonUp("mouse1") )
        {
            if(manaAtual >= 1)
            {
                if(isCdRange == false)
                {
                    if(dashing == false)
                    {
                        stop = false;
                        manaAtual -= 1;
                        Anim.SetFloat("Feitico", 0);
                        StartCoroutine(TimetoAiming());
                        StartCoroutine("CDAtackRange2");
                        Vector3 direcao = transform.forward;
                        GameObject magia = Instantiate(feiticoPrefab, feiticoRespawn.position, feiticoRespawn.rotation);
                        magia.GetComponent<Rigidbody>().AddForce(direcao * forcaArremessoFeitico, ForceMode.Impulse);

                    }
                }

            }
        }
    }

    void atackPocao()
    {
        if(Input.GetButtonDown("L1") && tipoDeControle == 1)
        {
            if(isCdRange == false)
            {
                if (pocao.tipoDapocao == 0)
                {
                    if (temPocaoCura > 0)
                    {
                        if (VidaAtual < VidaTotal)
                        {
                            StartCoroutine("Cura");
                            VidaAtual += 3;
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
                } // pocao cura
                if (pocao.tipoDapocao == 1)
                {
                    if (temPocaoMana > 0)
                    {
                        if (manaAtual < manaTotal)
                        {
                            StartCoroutine("ManaCura");
                            manaAtual += 3;
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

                } // pocao mana
            }
        }
        if (Input.GetButtonDown("mouse1") && tipoDeControle == 0)
        {
            if (isCdRange == false)
            {
                if (pocao.tipoDapocao == 0)
                {
                    if (temPocaoCura > 0)
                    {
                        if (VidaAtual < VidaTotal)
                        {
                            StartCoroutine("Cura");
                            VidaAtual += 3;
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
                } // pocao cura
                if (pocao.tipoDapocao == 1)
                {
                    if (temPocaoMana > 0)
                    {
                        if (manaAtual < manaTotal)
                        {
                            StartCoroutine("ManaCura");
                            manaAtual += 3;
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

                } // pocao mana
            }
        }

        if (Input.GetButton("L1") && tipoDeControle == 1)
        {
            if(isCdRange == false)
            {
                if (pocao.tipoDapocao == 2)
                {
                    if (temPocaoFogo > 0)
                    {
                        isAiming = true;
                        Anim.SetFloat("Pocao", 1);
                        Anim.SetFloat("isRun", 0);
                    }
                } // pocao Fogo
                if (pocao.tipoDapocao == 3)
                {
                    if (temPocaoGelo > 0)
                    {
                        isAiming = true;
                        Anim.SetFloat("Pocao", 1);
                        Anim.SetFloat("isRun", 0);
                    }
                } // pocao Gelo
                if (pocao.tipoDapocao == 4)
                {
                    if (temPocaoFumaca > 0)
                    {
                        isAiming = true;
                        Anim.SetFloat("Pocao", 1);
                        Anim.SetFloat("isRun", 0);
                    }
                } // pocao fumaça
            }
        }
        if (Input.GetButton("mouse1") && tipoDeControle == 0)
        {
            if (isCdRange == false)
            {
                if (pocao.tipoDapocao == 2)
                {
                    if (temPocaoFogo > 0)
                    {
                        isAiming = true;
                        Anim.SetFloat("Pocao", 1);
                        Anim.SetFloat("isRun", 0);
                    }
                } // pocao Fogo
                if (pocao.tipoDapocao == 3)
                {
                    if (temPocaoGelo > 0)
                    {
                        isAiming = true;
                        Anim.SetFloat("Pocao", 1);
                        Anim.SetFloat("isRun", 0);
                    }
                } // pocao Gelo
                if (pocao.tipoDapocao == 4)
                {
                    if (temPocaoFumaca > 0)
                    {
                        isAiming = true;
                        Anim.SetFloat("Pocao", 1);
                        Anim.SetFloat("isRun", 0);
                    }
                } // pocao fumaça
            }
        }

        if (Input.GetButtonUp("L1") && tipoDeControle == 1)
        {
            if (pocao.tipoDapocao == 2)
            {
                if (temPocaoFogo > 0)
                {
                    if(isCdRange == false)
                    {
                        --temPocaoFogo;
                        Anim.SetFloat("Pocao", 0);
                        StartCoroutine("CDAtackRange");
                        StartCoroutine(TimetoAiming());
                        Vector3 direcao = transform.forward;
                        GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                        pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                        GameController.numeroPocoesAtual = temPocaoFogo;

                    }
                }
            } // pocao Fogo
            if (pocao.tipoDapocao == 3)
            {
                if (temPocaoGelo > 0)
                {
                    if (isCdRange == false)
                    {
                        --temPocaoGelo;
                        Anim.SetFloat("Pocao", 0);  
                        StartCoroutine("CDAtackRange");
                        StartCoroutine(TimetoAiming());
                        Vector3 direcao = transform.forward;
                        GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                        pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                        GameController.numeroPocoesAtual = temPocaoGelo;
                    }
                }
            } // pocao Gelo
            if (pocao.tipoDapocao == 4)
            {
                if (temPocaoFumaca > 0)
                {
                    if(isCdRange == false)
                    {
                        --temPocaoFumaca;
                        Anim.SetFloat("Pocao", 0);
                        StartCoroutine("CDAtackRange");
                        StartCoroutine(TimetoAiming());
                        Vector3 direcao = transform.forward;
                        GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                        pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                        GameController.numeroPocoesAtual = temPocaoFumaca;
                    }
                }
            } // pocao fumaça
        }
        if (Input.GetButtonUp("mouse1") && tipoDeControle == 0)
        {
            if (pocao.tipoDapocao == 2)
            {
                if (temPocaoFogo > 0)
                {
                    if (isCdRange == false)
                    {
                        --temPocaoFogo;
                        Anim.SetFloat("Pocao", 0);
                        StartCoroutine("CDAtackRange");
                        StartCoroutine(TimetoAiming());
                        Vector3 direcao = transform.forward;
                        GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                        pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                        GameController.numeroPocoesAtual = temPocaoFogo;

                    }
                }
            } // pocao Fogo
            if (pocao.tipoDapocao == 3)
            {
                if (temPocaoGelo > 0)
                {
                    if (isCdRange == false)
                    {
                        --temPocaoGelo;
                        Anim.SetFloat("Pocao", 0);
                        StartCoroutine("CDAtackRange");
                        StartCoroutine(TimetoAiming());
                        Vector3 direcao = transform.forward;
                        GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                        pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                        GameController.numeroPocoesAtual = temPocaoGelo;
                    }
                }
            } // pocao Gelo
            if (pocao.tipoDapocao == 4)
            {
                if (temPocaoFumaca > 0)
                {
                    if (isCdRange == false)
                    {
                        --temPocaoFumaca;
                        Anim.SetFloat("Pocao", 0);
                        StartCoroutine("CDAtackRange");
                        StartCoroutine(TimetoAiming());
                        Vector3 direcao = transform.forward;
                        GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                        pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                        GameController.numeroPocoesAtual = temPocaoFumaca;
                    }
                }
            } // pocao fumaça
        }
    }
    
    void Vida()
    {
        if(GameController.almasAtual <= 0)
        {
            gameObject.SetActive(false);

        }
        else if(VidaAtual <= 0)
        {
            GameController.almasAtual --;
            Reiniciar();
        }
    }

    void Reiniciar()
    {
        VidaAtual = VidaTotal;
        checkpoint.renasceu = true;
        gameObject.transform.position = GameController.instance.checkPoint[GameController.checkpointNumber].transform.position;
    }

    public void TomarDano(int Dano,float Empurrao)
    {
        if(GameController.instance.modoDeus == false)
        {
            if(dashing == false)
            {
                
                StartCoroutine("CDTomarDano");
                VidaAtual -= Dano;
                forcaEmpurrao = Empurrao;

                Vector3 empurrar = -transform.forward;

                rb.AddForce(empurrar * forcaEmpurrao, ForceMode.Impulse);
                GameController.instance.DorGato();

            }
        }
    }

    void Dash()
    {
        if(isPaused == false && stop == false )
        {
           if(Input.GetButton("bolinha") && isDashing == 4 && tipoDeControle == 1 && stop == false)
           {
                isDashing = 0;
                Vector3 dashing = transform.forward;
                rb.AddForce(dashing * forcaDash, ForceMode.Impulse);
                StartCoroutine("CdDash");
           }
           else if (Input.GetKeyDown(KeyCode.LeftShift) && isDashing == 4 && tipoDeControle == 0)
           {
                isDashing = 0;
                Vector3 dashing = transform.forward;
                rb.AddForce(dashing * forcaDash, ForceMode.Impulse);
                StartCoroutine("CdDash");
           }

        }
    }

    void trocarPocao()
    {

        if (Input.GetButtonDown("triangulo") && tipoDeControle == 1)
        {
            if(pocao.tipoDapocao == 0)
            {
                GameController.numeroPocoesAtual = temPocaoMana;
                pocao.tipoDapocao = 1;
                print("pocao Raio");
            }
            else if(pocao.tipoDapocao == 1)
            {
                GameController.numeroPocoesAtual = temPocaoFogo;
                pocao.tipoDapocao = 2;
                print("pocao Fogo");
            }
            else if (pocao.tipoDapocao == 2)
            {
                GameController.numeroPocoesAtual = temPocaoGelo;
                pocao.tipoDapocao = 3;
                print("pocao gelo");
            }
            else if (pocao.tipoDapocao == 3)
            {
                GameController.numeroPocoesAtual = temPocaoFumaca;
                pocao.tipoDapocao = 4;
                print("pocao fumaca");
            }
            else if (pocao.tipoDapocao == 4)
            {
                GameController.numeroPocoesAtual = temPocaoCura;
                pocao.tipoDapocao = 0;
                print("pocao Cura");
            }
        }
        if (Input.GetKeyDown(KeyCode.Q) && tipoDeControle == 0)
        {
            if (pocao.tipoDapocao == 0)
            {
                GameController.numeroPocoesAtual = temPocaoMana;
                pocao.tipoDapocao = 1;
                print("pocao Raio");
            }
            else if (pocao.tipoDapocao == 1)
            {
                GameController.numeroPocoesAtual = temPocaoFogo;
                pocao.tipoDapocao = 2;
                print("pocao Fogo");
            }
            else if (pocao.tipoDapocao == 2)
            {
                GameController.numeroPocoesAtual = temPocaoGelo;
                pocao.tipoDapocao = 3;
                print("pocao gelo");
            }
            else if (pocao.tipoDapocao == 3)
            {
                GameController.numeroPocoesAtual = temPocaoFumaca;
                pocao.tipoDapocao = 4;
                print("pocao fumaca");
            }
            else if (pocao.tipoDapocao == 4)
            {
                GameController.numeroPocoesAtual = temPocaoCura;
                pocao.tipoDapocao = 0;
                print("pocao Cura");
            }
        }
    }

    public void StartNumeroPocao()
    {
        if (pocao.tipoDapocao == 0)
        {
            GameController.numeroPocoesAtual = temPocaoCura;
            print("pocao Cura");
        }
        else if (pocao.tipoDapocao == 1)
        {
            GameController.numeroPocoesAtual = temPocaoMana; 
            print("pocao Raio");
        }
        else if (pocao.tipoDapocao == 2)
        {
            GameController.numeroPocoesAtual = temPocaoFogo;
            print("pocao Fogo");
        }
        else if (pocao.tipoDapocao == 3)
        {
            GameController.numeroPocoesAtual = temPocaoGelo;
            print("pocao Gelo");
        }
        else if (pocao.tipoDapocao == 4)
        {
            GameController.numeroPocoesAtual = temPocaoFumaca;
            print("pocao fumaça");
        }
    }

    IEnumerator TimetoAiming()
    {
        yield return new WaitForSeconds(0.35f);
        isAiming = false;
    }
    IEnumerator CdDash()
    {
        dashing = true;
        speedAtual = speedAtual + 5;
        playerMeshCorpo.SetActive(false);
        espadaCosta.SetActive(false);
        rastroDash.emitting = true;
        yield return new WaitForSeconds(timeInDashing);
        dashing = false;
        speedAtual = speed;
        playerMeshCorpo.SetActive(true);
        rastroDash.emitting = false;
        espadaCosta.SetActive(true);
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
        isAtacking = true;
        espada.SetActive(true);
        efeitoEspada.SetActive(true);
        Anim.SetFloat("Atack", 1);
        espadaCosta.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        stop = false;
        isAtacking = false;
        espada.SetActive(false);
        efeitoEspada.SetActive(false);
        Anim.SetFloat("Atack", 0);
        espadaCosta.SetActive(true);
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
    IEnumerator CDAtackRange2()
    {
        isCdRange = true;
        yield return new WaitForSeconds(CDRange2);
        isCdRange = false;
    }

    IEnumerator CDTomarDano()
    {
        stop = true;
        TomouDano = true;
        playerMesh.material.color = Color.red;
        yield return new WaitForSeconds(CdTomarDano);
        playerMesh.material.color = corNormal;
        TomouDano = false;
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
        if (other.gameObject.tag == "garra" && TomouDano == false)
        {
            TomarDano(1,7);
        }

        if (other.gameObject.CompareTag("worktable"))
        {
            GameController.instance.interacaoNatela = true;
            GameController.instance.pertoDaTable = true;
        }
        if(other.gameObject.tag == "Brutamontes" && TomouDano == false)
        {
            TomarDano(3,10);
        }

        if (other.gameObject.tag == "Guspe" && TomouDano == false)
        {
            TomarDano(1, 5);
        }
        if (other.gameObject.tag == "CobraBoss" && TomouDano == false)
        {
            TomarDano(1, 10);
        }
        if(other.gameObject.tag == "BossInvestida" && TomouDano == false)
        {
            TomarDano(3, 15);
        }
        if (other.gameObject.tag == "BossUrso" && TomouDano == false)
        {
            TomarDano(0, 5);
        }

        if (GameController.pularTutorial == false)
        {
            if (other.gameObject.tag == "pisoTutorialCraft")
            {
                GameController.instance.inPisoTutorialCraft = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("worktable"))
        {
            GameController.instance.interacaoNatela = false;
            GameController.instance.pertoDaTable = false;
        }
        if(GameController.pularTutorial == false)
        {
            if(other.gameObject.CompareTag("pisoTutorial"))
            {
                GameController.instance.outPisoTutorial = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("caixa"))
        {
            Anim.SetFloat("Empurrar", 1);
            Anim.SetFloat("isRun", 1.1f);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("caixa"))
        {
            Anim.SetFloat("Empurrar", 0);
            Anim.SetFloat("isRun", 0);
        }
    }
}
