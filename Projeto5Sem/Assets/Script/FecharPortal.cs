using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FecharPortal : MonoBehaviour
{
    private bool podeFechar;
    public GameObject[] inimigo;


    void Start()
    {
        podeFechar = false;
    }

    void Update()
    {
        if(podeFechar == true && Input.GetKeyDown(KeyCode.E) || podeFechar == true && Input.GetButtonDown("Interacao"))
        {
            bool todosInimigosMortos = true;

            for (int i = 0; i < inimigo.Length; i++)
            {
                if (inimigo[i] != null)
                {
                    todosInimigosMortos = false;
                    break;
                }
            }

            if (todosInimigosMortos)
            {
                Destroy(gameObject, 0);
                GameController.instance.interacaoNatela = false;
                GameController.numeroPortais++;
            }
            else
            {
                    GameController.instance.ShowInformacao("Preciso matar todos os inimigos antes");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameController.instance.interacaoNatela = true;
            podeFechar = true;

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameController.instance.interacaoNatela = false;
            podeFechar = false;
        }
    }


}
