using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogo : MonoBehaviour
{
    public Sprite[] profile;
    public string[] speechtxt;
    public string[] nome;

    public LayerMask playerLayer;
    public float radious;

    private DialogoControl dc;

    private bool podeInteragir;
    private bool interagiu;
    public static bool podeSeguir;

    public int proxSprite = 0;

    public bool forcarInte;
    private bool forcar;
    private bool isForced = false;

    private void Start()
    {
        dc = FindObjectOfType<DialogoControl>();
    }
    private void FixedUpdate()
    {
        Interacao();
    }
    private void Update()
    {
        Semforce();
        Forcar();
    }

    void Semforce()
    {
        if (Input.GetButtonDown("Interacao") && podeInteragir == true && Player.tipoDeControle == 0)
        {
            if (interagiu == false)
            {
                GameController.instance.isPause = true;
                dc.Speech(profile[0], speechtxt, nome[0]);
                Player.instance.isPaused = true;
                Player.instance.stop = true;
                interagiu = true;
            }


        }
        else if (Input.GetKeyDown(KeyCode.E) && podeInteragir == true && Player.tipoDeControle == 1)
        {
            if (interagiu == false)
            {
                GameController.instance.isPause = true;
                dc.Speech(profile[0], speechtxt, nome[0]);
                Player.instance.isPaused = true;
                Player.instance.stop = true;
                interagiu = true;
            }


        }

        if (interagiu == true && podeSeguir == true)
        {
            if (Input.GetButtonDown("Submit"))
            {
                ++proxSprite;
                dc.NextSentence(profile[proxSprite], nome[proxSprite]);
                podeSeguir = false;
            }
        }
    }

    void Forcar()
    {
        
        if(forcar && !isForced)
        {
            GameController.instance.isPause = true;
            dc.Speech(profile[0], speechtxt, nome[0]);
            Player.instance.isPaused = true;
            Player.instance.stop = true;
            forcar = false;
            isForced = true;
        }
        if (isForced == true && podeSeguir == true)
        {
            if (Input.GetButtonDown("Submit"))
            {
                ++proxSprite;
                dc.NextSentence(profile[proxSprite], nome[proxSprite]);
                podeSeguir = false;
            }
        }
    }

    public void Interacao()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radious, playerLayer);
        if(forcarInte == false)
        {
            if (hits.Length > 0)
            {
                podeInteragir = true;
                GameController.instance.interacaoNatela = true;
            }
            else
            {
                podeInteragir = false;
                GameController.instance.interacaoNatela = false;
            }
        }
        else if(forcarInte == true)
        {
            if (hits.Length > 0)
            {
                forcar = true;
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radious);
    }
}
