using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Botao : MonoBehaviour
{
    private bool passouCartao;
    private bool PodePassarCartao;
    public Animator animPorta;
    public MeshRenderer botao;

    void Start()
    {
        passouCartao = false;
    }

    void Update()
    {
        if(passouCartao == true)
        {
            animPorta.SetFloat("Aberta", 1);
            botao.material.color = Color.blue;
            passouCartao = false;
        }
        if(PodePassarCartao == true)
        {
            if(Input.GetButtonDown("Interacao") && Player.tipoDeControle == 1)
            {
                if(Player.instance.TemCartao == true)
                {
                    passouCartao = true;
                }
                else if(Player.instance.TemCartao == false)
                {
                    GameController.instance.ShowInformacao("Preciso de uma joia Azul");
                }
            }
            if (Input.GetKeyDown(KeyCode.E) && Player.tipoDeControle == 0)
            {
                if (Player.instance.TemCartao == true)
                {
                    passouCartao = true;
                }
                else if (Player.instance.TemCartao == false)
                {
                    GameController.instance.ShowInformacao("Preciso de uma joia Azul");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && passouCartao == false)
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
