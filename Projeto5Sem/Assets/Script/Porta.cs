using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Porta : MonoBehaviour
{
    public Animator anim;
    public bool abriu;
    private bool podeAbrir;
    public bool portaBloqueada;
    public bool podeInteragir;
    public bool abriuFechouPorta;

    void Start()
    {
        abriu = false;
        podeAbrir = false;

    }

    // Update is called once per frame
    void Update()
    {
        AbrirFecharPorta();
        if (podeAbrir == true && portaBloqueada == false)
        {
            if (Player.tipoDeControle == 1 && Input.GetButtonDown("Interacao") && abriu == false ||
                Player.tipoDeControle == 0 && Input.GetKeyDown(KeyCode.E) && abriu == false)
            {  
                abriu = true;
            }
            else if (Player.tipoDeControle == 1 && Input.GetButtonDown("Interacao") && abriu == true ||
                Player.tipoDeControle == 0 && Input.GetKeyDown(KeyCode.E) && abriu == true)
            {
                abriu = false;
            }
        }
        else if( portaBloqueada == true)
        {
            if(podeInteragir== true)
            {
                if (Player.tipoDeControle == 1 && Input.GetButtonDown("Interacao") && abriu == false ||
                        Player.tipoDeControle == 0 && Input.GetKeyDown(KeyCode.E) && abriu == false)
                {
                    GameController.instance.ShowInformacao("Parece que a porta esta emperrada");
                }
            }
        }
    }

    void AbrirFecharPorta()
    {
        if(abriu == true)
        {
            anim.SetFloat("Aberta", 1);
        }
        if(abriu == false)
        {
            anim.SetFloat("Aberta", 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameController.instance.interacaoNatela = true;
            podeAbrir = true;
            podeInteragir = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameController.instance.interacaoNatela = false;
            podeAbrir = false;
            podeInteragir = false;
        }
    }
}
