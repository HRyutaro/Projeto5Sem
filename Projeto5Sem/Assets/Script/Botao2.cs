using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Botao2 : MonoBehaviour
{
    private bool passouCartao;
    private bool PodePassarCartao;
    public Animator animPorta;
    public MeshRenderer botao;
    public Text textoInteracao;

    void Start()
    {
        passouCartao = false;
    }

    void Update()
    {
        if (passouCartao == true)
        {
            animPorta.SetFloat("Abrir", 1);
            botao.material.color = Color.red;
        }
        if (PodePassarCartao == true)
        {
            if (Input.GetButtonDown("Interacao") && Player.tipoDeControle == 0)
            {
                if (Player.instance.TemCartao2 == true)
                {
                    passouCartao = true;
                }
                else if (Player.instance.TemCartao2 == false)
                {
                    StartCoroutine(notificacao());
                    textoInteracao.text = "Preciso de uma joia Vermelha";
                }
            }
            if (Input.GetKeyDown(KeyCode.E) && Player.tipoDeControle == 1)
            {
                if (Player.instance.TemCartao2 == true)
                {
                    passouCartao = true;
                }
                else if (Player.instance.TemCartao2 == false)
                {
                    StartCoroutine(notificacao());
                    textoInteracao.text = "Preciso de uma joia Vermelha";
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && passouCartao == false)
        {
            PodePassarCartao = true;
            textoInteracao.enabled = true;
            GameController.instance.interacaoNatela = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PodePassarCartao = false;
            textoInteracao.enabled = false;
            GameController.instance.interacaoNatela = false;
        }
    }
    IEnumerator notificacao()
    {
        GameController.instance.interacaoNatela = false;
        textoInteracao.enabled = true;
        yield return new WaitForSeconds(2f);
        textoInteracao.enabled = false;
    }
}
