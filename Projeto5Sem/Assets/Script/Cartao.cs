using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartao : MonoBehaviour
{
    public Animator anim;
    public GameObject itemtodo;

    public static bool saber;
    void Start()
    {
        anim.SetBool("Ativar", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(saber == true)
            {
                GameController.instance.ShowInformacao("Deve ser essa Joia");
            }
            else
            {
                GameController.instance.ShowInformacao("Uma pedra brilhante? Pra que serve?");
            }
            Player.instance.TemCartao = true;
            Destroy(itemtodo);
            Destroy(gameObject);
        }
    }
}
