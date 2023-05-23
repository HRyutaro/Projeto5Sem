using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloquearFimdejogo : MonoBehaviour
{
    public GameObject dialogo;

    void Start()
    {
        
    }


    void Update()
    {
        if (GameController.numeroPortais >= 7)
        {
            dialogo.SetActive(true);
            Destroy(gameObject, 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameController.instance.ShowInformacao("Sr.Montimer: Você precisa fechar todos os portais");
        }
    }

}
