using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Porta : MonoBehaviour
{
    public Animator anim;
    private bool abriu;
    private bool podeAbrir;
    public bool portaBloqueada;
    public bool podeInteragir;
    public Text notificao;

    void Start()
    {
        abriu = false;
        podeAbrir = false;

    }

    // Update is called once per frame
    void Update()
    {

        if(podeAbrir == true && portaBloqueada == false)
        {
            if (Player.tipoDeControle == 0 && Input.GetButtonDown("Interacao") && abriu == false ||
                Player.tipoDeControle == 1 && Input.GetKeyDown(KeyCode.E) && abriu == false)
            {
                anim.SetFloat("Aberta", 1);
                abriu = true;
            }
            else if (Player.tipoDeControle == 0 && Input.GetButtonDown("Interacao") && abriu == true ||
                Player.tipoDeControle == 1 && Input.GetKeyDown(KeyCode.E) && abriu == true)
            {

                anim.SetFloat("Aberta", 0);
                abriu = false;

            }
        }
        else if( portaBloqueada == true)
        {
            if(podeInteragir== true)
            {
                if (Player.tipoDeControle == 0 && Input.GetButtonDown("Interacao") && abriu == false ||
                        Player.tipoDeControle == 1 && Input.GetKeyDown(KeyCode.E) && abriu == false)
                {
                    StartCoroutine(Notificao());
                    notificao.text = "Essa porta parece emperrada";
                }
            }
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
            notificao.enabled = false;
            podeInteragir = false;
        }
    }
    IEnumerator Notificao()
    {
        notificao.enabled = true;
        GameController.instance.interacaoNatela = false;
        yield return new WaitForSeconds(3f);
        notificao.enabled = false;
    }
}
