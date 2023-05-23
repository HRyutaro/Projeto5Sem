using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botao3 : MonoBehaviour
{
    public bool passouCartao;
    private bool PodePassarCartao;
    public MeshRenderer botao;

    void Start()
    {
        passouCartao = false;
    }

    void Update()
    {
        if (passouCartao == true)
        {
            botao.material.color = Color.red;
            FimDeJogo.instance.desligouReator = true;
        }
        if (PodePassarCartao == true)
        {
            if (Input.GetButtonDown("Interacao") && Player.tipoDeControle == 1)
            {
                if (GameController.numeroPortais >= 7)
                {
                    passouCartao = true;
                    FimDeJogo.instance.desligouReator = true;
                }
                else if (GameController.numeroPortais < 7)
                {
                    GameController.instance.ShowInformacao("Preciso da chave com o senhor Montimer");
                }
            }
            if (Input.GetKeyDown(KeyCode.E) && Player.tipoDeControle == 0)
            {
                if (GameController.numeroPortais >= 7)
                {
                    passouCartao = true;
                    FimDeJogo.instance.desligouReator = true;
                }
                else if (GameController.numeroPortais < 7)
                {
                    GameController.instance.ShowInformacao("Preciso da chave com o senhor Montimer");
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
