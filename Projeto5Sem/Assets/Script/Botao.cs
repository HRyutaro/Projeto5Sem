using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Botao : MonoBehaviour
{

    private Color Green;
    private bool passouCartao;
    private bool PodePassarCartao;
    public Animator animPorta;
    public GameObject textoInte;
    public MeshRenderer botao;
    public GameObject portao;
    public Text textoInteracao;

    void Start()
    {
        Green = Color.green;
        passouCartao = false;
    }

    void Update()
    {
        if(passouCartao == true)
        {
            animPorta.SetFloat("Abrir", 1);
            botao.material.color = Green;
        }
        if(PodePassarCartao == true)
        {
            if(Input.GetButtonDown("Interacao") || Input.GetKeyDown(KeyCode.E))
            {
                if(Player.instance.TemCartao == true)
                {
                    passouCartao = true;
                }
                else if(Player.instance.TemCartao == false)
                {
                    textoInteracao.text = "Preciso de um cartao";
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && passouCartao == false)
        {
            PodePassarCartao = true;
            textoInte.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PodePassarCartao = false;
            textoInte.SetActive(false);
            textoInteracao.text = "'E' / 'Quadrado' para interagir";
        }
    }
}
