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
    public AudioSource portao;

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
            passouCartao = false;
            portao.Play();
        }
        if (PodePassarCartao == true)
        {
            if (Input.GetButtonDown("Interacao") && Player.tipoDeControle == 1)
            {
                if (Player.instance.TemCartao2 == true)
                {
                    passouCartao = true;
                }
                else if (Player.instance.TemCartao2 == false)
                {
                    GameController.instance.ShowInformacao("Preciso de uma joia Vermelha");
                    Cartao2.saber = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.E) && Player.tipoDeControle == 0)
            {
                if (Player.instance.TemCartao2 == true)
                {
                    passouCartao = true;
                }
                else if (Player.instance.TemCartao2 == false)
                {
                    GameController.instance.ShowInformacao("Preciso de uma joia Vermelha");
                    Cartao2.saber = true;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && passouCartao == false)
        {
            PodePassarCartao = true;
            GameController.instance.interacaoNatela = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PodePassarCartao = false;
            GameController.instance.interacaoNatela = false;
        }
    }
}
