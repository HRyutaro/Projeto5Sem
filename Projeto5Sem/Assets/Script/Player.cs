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
    public ParticleSystem raioUtilizado;
    public SkinnedMeshRenderer playerMesh;
    public GameObject playerMeshCorpo;
    

    [Header("Movimento")]
    public Rigidbody rb;
    [SerializeField] private float speed = 5;
    private float speedAtual;
    [SerializeField] private float turnSpeed = 360;
    private Vector3 input;
    private Vector3 input2;
    [SerializeField] public bool isPaused;
    [SerializeField] public bool stop;
    public bool QuedaAtivada;
    public bool queda1;

    [Header("Combat")]
    public GameObject espadaCosta;
    public GameObject espada;
    public GameObject espadaGolpeRaio;
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
    public GameObject areaArremesso;

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
    public int temPocaoRaio;
    public int temPocaoCura;
    public int temPlantaFogo;
    public int temPlantaGelo;
    public int temPlantaFumaca;
    public int temPlantaRaio;
    public int temPlantaCura;
    public bool TemCartao;
    public bool TemCartao2;

    [Header("Magia")]
    public GameObject raioEffect;
    private bool espadaInRaio = false;

    [Header("Dano")]
    public float forcaEmpurrao;
    public float tempoEmpurrado = 0.5f;
    public Animator Anim;
    public float CdTomarDano;
    private bool dashing;
    private bool TomouDano = false;
    public Color corNormal;

    [Header("Audios")]
    public AudioSource passosAudio;
    public AudioSource dorAudio;
    public AudioSource golpeAudio;

    //controles
    public static int tipoDeControle;

    void Start()
    {

        dorAudio.enabled = false;
        instance = this;
        VidaAtual = VidaTotal;
        StartNumeroPocao();
        speedAtual = speed;
        QuedaAtivada = false;
        dorAudio.Play();
        passosAudio.Play();
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
            Queda();
        }
    }

    private void FixedUpdate()
    {
        if(isPaused == false && stop == false)
        {
            Move();
            Dash();
        }
        if (stop == true || isPaused == true)
        {
            rb.velocity = Vector3.zero;
            passosAudio.enabled = false;
            Anim.SetFloat("isRun", 0);
            Anim.SetFloat("Pocao", 0);
        }
    }

    void GatherInput()
    {
        if(tipoDeControle == 1)
        {
            input = new Vector3(Input.GetAxisRaw("HorizontalJoystick"), 0, Input.GetAxisRaw("VerticalJoystick"));
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
            if (Input.GetAxisRaw("Horizontal") != 0 && speedAtual >= 1 && tipoDeControle == 0 || Input.GetAxisRaw("Vertical") != 0 && speedAtual >= 1 && tipoDeControle == 0 ||
                Input.GetAxisRaw("HorizontalJoystick") != 0 && speedAtual >= 1 && tipoDeControle == 1 || Input.GetAxisRaw("VerticalJoystick") != 0 && speedAtual >= 1 && tipoDeControle == 0) 
            {
                passosAudio.enabled = true;
                Anim.SetFloat("isRun", 1);
                Anim.SetFloat("Pocao", 0);
            }
            else
            {   
                passosAudio.enabled = false;
                Anim.SetFloat("isRun", 0);
                Anim.SetFloat("Pocao", 0);
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

    void Queda()
    {
        if(queda1 == true)
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            rb.mass = 15;
            if (QuedaAtivada == true)
            {
                rb.AddForce(Vector3.down * 17, ForceMode.Impulse);
            }
        }
        else
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionY;
        }
    }

    void AtackMelee()
    {
        
        if (tipoDeControle == 1)
        {
            if (Input.GetButtonDown("R1") && isCdAtack == false)
            {
                Anim.SetFloat("isRun", 0);
                Anim.SetFloat("Pocao", 0);
                StartCoroutine("AtackMeleeTime");
                StartCoroutine("CDAtackMelee");
                rb.velocity = Vector3.zero;
                Vector3 atackingfoward = transform.forward;
                rb.AddForce(2f * atackingfoward, ForceMode.Impulse);
            }
        }
        else if (tipoDeControle == 0)
        {
            if (Input.GetButtonDown("mouse0") && isCdAtack == false)
            {
                Anim.SetFloat("isRun", 0);
                Anim.SetFloat("Pocao", 0);
                StartCoroutine("AtackMeleeTime");
                StartCoroutine("CDAtackMelee");
                rb.velocity = Vector3.zero;
                Vector3 atackingfoward = transform.forward;
                rb.AddForce(2f * atackingfoward, ForceMode.Impulse);
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
                    if(temPocaoRaio > 0)
                    {
                        --temPocaoRaio;
                        GameController.numeroPocoesAtual = temPocaoRaio;
                        StartCoroutine(PocaoDeRaio());
                        StartCoroutine(Raio());
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
                    if (temPocaoRaio > 0)
                    {
                        --temPocaoRaio;
                        GameController.numeroPocoesAtual = temPocaoRaio;
                        StartCoroutine(PocaoDeRaio());
                        StartCoroutine(Raio());
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
                        Anim.SetFloat("Pocao", 0.9f);
                        Anim.SetFloat("isRun", 0);
                        stop = true;
                        rb.velocity = Vector3.zero;
                        areaArremesso.SetActive(true);
                    }
                } // pocao Fogo
                if (pocao.tipoDapocao == 3)
                {
                    if (temPocaoGelo > 0)
                    {
                        isAiming = true;
                        Anim.SetFloat("Pocao", 0.9f);
                        Anim.SetFloat("isRun", 0);
                        stop = true;
                        rb.velocity = Vector3.zero;
                        areaArremesso.SetActive(true);
                    }
                } // pocao Gelo
                if (pocao.tipoDapocao == 4)
                {
                    if (temPocaoFumaca > 0)
                    {
                        isAiming = true;
                        Anim.SetFloat("Pocao", 0.9f);
                        Anim.SetFloat("isRun", 0);
                        stop = true;
                        rb.velocity = Vector3.zero;
                        areaArremesso.SetActive(true);
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
                        Anim.SetFloat("Pocao", 0.9f);
                        Anim.SetFloat("isRun", 0);
                        stop = true;
                        rb.velocity = Vector3.zero;
                        areaArremesso.SetActive(true);
                    }
                } // pocao Fogo
                if (pocao.tipoDapocao == 3)
                {
                    if (temPocaoGelo > 0)
                    {
                        isAiming = true;
                        Anim.SetFloat("Pocao", 0.9f);
                        Anim.SetFloat("isRun", 0);
                        stop = true;
                        rb.velocity = Vector3.zero;
                        areaArremesso.SetActive(true);
                    }
                } // pocao Gelo
                if (pocao.tipoDapocao == 4)
                {
                    if (temPocaoFumaca > 0)
                    {
                        isAiming = true;
                        Anim.SetFloat("Pocao", 0.9f);
                        Anim.SetFloat("isRun", 0);
                        stop = true;
                        rb.velocity = Vector3.zero;
                        areaArremesso.SetActive(true);
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
                        Anim.SetFloat("Pocao", 1);
                        StartCoroutine("CDAtackRange");
                        StartCoroutine(TimetoAiming());
                        Vector3 direcao = transform.forward;
                        GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                        pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                        GameController.numeroPocoesAtual = temPocaoFogo;
                        stop = false;
                        areaArremesso.SetActive(false);
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
                        Anim.SetFloat("Pocao", 1);  
                        StartCoroutine("CDAtackRange");
                        StartCoroutine(TimetoAiming());
                        Vector3 direcao = transform.forward;
                        GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                        pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                        GameController.numeroPocoesAtual = temPocaoGelo;
                        stop = false;
                        areaArremesso.SetActive(false);
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
                        Anim.SetFloat("Pocao", 1);
                        StartCoroutine("CDAtackRange");
                        StartCoroutine(TimetoAiming());
                        Vector3 direcao = transform.forward;
                        GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                        pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                        GameController.numeroPocoesAtual = temPocaoFumaca;
                        stop = false;
                        areaArremesso.SetActive(false);
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
                        Anim.SetFloat("Pocao", 1);
                        StartCoroutine("CDAtackRange");
                        StartCoroutine(TimetoAiming());
                        Vector3 direcao = transform.forward;
                        GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                        pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                        GameController.numeroPocoesAtual = temPocaoFogo;
                        stop = false;
                        areaArremesso.SetActive(false);
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
                        Anim.SetFloat("Pocao", 1);
                        StartCoroutine("CDAtackRange");
                        StartCoroutine(TimetoAiming());
                        Vector3 direcao = transform.forward;
                        GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                        pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                        GameController.numeroPocoesAtual = temPocaoGelo;
                        stop = false;
                        areaArremesso.SetActive(false);
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
                        Anim.SetFloat("Pocao", 1);
                        StartCoroutine("CDAtackRange");
                        StartCoroutine(TimetoAiming());
                        Vector3 direcao = transform.forward;
                        GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);
                        pocao.GetComponent<Rigidbody>().AddForce(direcao * forcaArremesso, ForceMode.Impulse);
                        GameController.numeroPocoesAtual = temPocaoFumaca;
                        stop = false;
                        areaArremesso.SetActive(false);
                    }
                }
            } // pocao fumaça
        }
    }
    
    void Vida()
    {
        if(GameController.almasAtual <= 0)
        {
            Anim.SetFloat("Morte", 1);
            stop = true;
            passosAudio.enabled = false;
        }
        else if(VidaAtual <= 0)
        {
            GameController.almasAtual --;
            StartCoroutine(ReiniciarAnim());
            VidaAtual = VidaTotal;
        }
    }

    void Reiniciar()
    {
        VidaAtual = VidaTotal;
        checkpoint.renasceu = true;
        Anim.SetFloat("Morte", 0);
        gameObject.transform.position = GameController.instance.checkPoint[GameController.checkpointNumber].transform.position;
        checkpoint.JaLeu = false;
    }
    IEnumerator ReiniciarAnim()
    {
        Anim.SetFloat("Morte", 1);
        espadaCosta.SetActive(false);
        yield return new WaitForSeconds(1.8f);
        espadaCosta.SetActive(true);
        Reiniciar();
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
           else if (Input.GetKey(KeyCode.LeftShift) && isDashing == 4 && tipoDeControle == 0)
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
                GameController.numeroPocoesAtual = temPocaoRaio;
                pocao.tipoDapocao = 1;
            }
            else if(pocao.tipoDapocao == 1)
            {
                GameController.numeroPocoesAtual = temPocaoFogo;
                pocao.tipoDapocao = 2;
            }
            else if (pocao.tipoDapocao == 2)
            {
                GameController.numeroPocoesAtual = temPocaoGelo;
                pocao.tipoDapocao = 3;
            }
            else if (pocao.tipoDapocao == 3)
            {
                GameController.numeroPocoesAtual = temPocaoFumaca;
                pocao.tipoDapocao = 4;
            }
            else if (pocao.tipoDapocao == 4)
            {
                GameController.numeroPocoesAtual = temPocaoCura;
                pocao.tipoDapocao = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q) && tipoDeControle == 0)
        {
            if (pocao.tipoDapocao == 0)
            {
                GameController.numeroPocoesAtual = temPocaoRaio;
                pocao.tipoDapocao = 1;
            }
            else if (pocao.tipoDapocao == 1)
            {
                GameController.numeroPocoesAtual = temPocaoFogo;
                pocao.tipoDapocao = 2;
            }
            else if (pocao.tipoDapocao == 2)
            {
                GameController.numeroPocoesAtual = temPocaoGelo;
                pocao.tipoDapocao = 3;
            }
            else if (pocao.tipoDapocao == 3)
            {
                GameController.numeroPocoesAtual = temPocaoFumaca;
                pocao.tipoDapocao = 4;
            }
            else if (pocao.tipoDapocao == 4)
            {
                GameController.numeroPocoesAtual = temPocaoCura;
                pocao.tipoDapocao = 0;
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
            GameController.numeroPocoesAtual = temPocaoRaio; 
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
        if(espadaInRaio == false)
        {
            efeitoEspada.SetActive(true);
            golpeAudio.enabled = true;
        }
        else if(espadaInRaio == true)
        {
            espadaGolpeRaio.SetActive(true);
        }
        stop = true; 
        isAtacking = true;
        espada.SetActive(true);
        Anim.SetFloat("Atack", 1);
        espadaCosta.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        stop = false;
        isAtacking = false;
        espada.SetActive(false);
        Anim.SetFloat("Atack", 0);
        golpeAudio.enabled = false;
        espadaCosta.SetActive(true);
        efeitoEspada.SetActive(false);
        espadaGolpeRaio.SetActive(false);
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
        dorAudio.enabled = true;
        playerMesh.material.color = Color.red;
        yield return new WaitForSeconds(CdTomarDano);
        playerMesh.material.color = corNormal;
        dorAudio.enabled = false;
        TomouDano = false;
        stop = false;

    }

    IEnumerator Cura()
    {
        curar.Play(true);
        Anim.SetFloat("Beber", 1);
        yield return new WaitForSeconds(1.5f);
        Anim.SetFloat("Beber", 0);
        curar.Play(false);

    }
    IEnumerator Raio()
    {
        raioUtilizado.Play(true);
        Anim.SetFloat("Beber", 1);
        yield return new WaitForSeconds(1.5f);
        Anim.SetFloat("Beber", 0);
        raioUtilizado.Play(false);
    }
    IEnumerator PocaoDeRaio()
    {
        espada.tag = "Raio";
        espadaInRaio = true;
        raioEffect.SetActive(true);
        yield return new WaitForSeconds(10f);
        raioEffect.SetActive(false);
        espadaInRaio = false;
        espada.tag = "espada";
    }
    
    public void AnimPegar()
    {
        StartCoroutine(AnimPegarPlanta());
    }

    IEnumerator AnimPegarPlanta()
    {
        Anim.SetFloat("Pegar", 1);
        yield return new WaitForSeconds(1.5f);
        Anim.SetFloat("Pegar", 0);
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
        if (other.gameObject.tag == "Serra" && TomouDano == false)
        {
            TomarDano(2, 10);
        }
        if (other.gameObject.tag == "Flecha" && TomouDano == false)
        {
            TomarDano(2, 10);
        }

        if (other.gameObject.tag == "AtivarQueda" && QuedaAtivada == false)
        {
            QuedaAtivada = true;
        }
        if(other.gameObject.tag == "DesativadorQueda" && queda1 == false)
        {
            queda1 = false;
            QuedaAtivada = false;
        }
        if (other.gameObject.tag == "Queda" && queda1 == false)
        {
            queda1 = true;
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
        if(collision.gameObject.tag == "pisoFora")
        {
            VidaAtual -= 2;
            checkpoint.renasceu = true;
            queda1 = false;
            QuedaAtivada = false;
            gameObject.transform.position = GameController.instance.checkPoint[GameController.checkpointNumber].transform.position;
        }
    }

}
