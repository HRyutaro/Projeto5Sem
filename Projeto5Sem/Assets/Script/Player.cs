using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Combat")]
    public float Vida;
    public GameObject espadaAtack;
    public GameObject espadaCosta;
    public GameObject espada;
    public float CDAtack;
    private bool isCdAtack;
    private bool stop;

    [Header("pocao")]
    public GameObject pocaoPrefab;
    public float forcaArremesso;
    public Transform pocaoRespawn;


    [Header("Movimento")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5;
    [SerializeField] private float turnSpeed = 360;
    private Vector3 input;


    public Animator Anim;


    void Start()
    {
        rb.GetComponent<Rigidbody>();
    }
    void Update()
    {
        GatherInput();
        Look();
        AtackMelee();
        AtackRange();
    }

    private void FixedUpdate()
    {
        Move();
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
        if (Input.GetButtonDown("Fire2"))
        {
            //Vector3 direcao = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            //direcao.z = 0;

            GameObject pocao = Instantiate(pocaoPrefab, pocaoRespawn.position, Quaternion.identity);

            //pocao.GetComponent<Rigidbody>().AddForce(direcao.normalized * forcaArremesso, ForceMode.Impulse);
        }
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
    
}
