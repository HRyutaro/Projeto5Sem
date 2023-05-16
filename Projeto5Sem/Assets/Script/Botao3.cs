using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botao3 : MonoBehaviour
{
    private bool passouCartao;
    private bool PodePassarCartao;
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
            botao.material.color = Color.red;
            passouCartao = false;
            FimDeJogo.instance.desligouReator = true;
        }
        if (PodePassarCartao == true)
        {
            if (Input.GetButtonDown("Interacao") && Player.tipoDeControle == 1)
            {
                if (GameController.numeroPortais >= 7)
                {
                    passouCartao = true;
                }
                else if (GameController.numeroPortais < 7)
                {
                    GameController.instance.ShowInformacao("Preciso da chave");
                    Cartao2.saber = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.E) && Player.tipoDeControle == 0)
            {
                if (GameController.numeroPortais >= 7)
                {
                    passouCartao = true;
                }
                else if (GameController.numeroPortais < 7)
                {
                    GameController.instance.ShowInformacao("Preciso da chave");
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
