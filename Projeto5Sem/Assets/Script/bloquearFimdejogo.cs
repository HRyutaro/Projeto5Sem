using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloquearFimdejogo : MonoBehaviour
{

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameController.instance.ShowInformacao("Sr.Montimer: Voc� precisa fechar todos os portais");
        }
    }

}
