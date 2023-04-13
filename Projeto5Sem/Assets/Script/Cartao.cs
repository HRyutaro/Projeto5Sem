using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartao : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim.SetBool("Ativar", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Player.instance.TemCartao = true;
            Destroy(gameObject);

        }
    }
}
