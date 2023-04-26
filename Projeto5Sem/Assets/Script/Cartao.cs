using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartao : MonoBehaviour
{
    public Animator anim;
    public GameObject itemtodo;
    void Start()
    {
        anim.SetBool("Ativar", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Player.instance.TemCartao = true;
            Destroy(itemtodo);
            Destroy(gameObject);
        }
    }
}
