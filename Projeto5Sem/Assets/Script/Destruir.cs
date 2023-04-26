using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruir : MonoBehaviour
{
    public float tempoParaDestruir;

    void Start()
    {
        Destroy(gameObject, tempoParaDestruir);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
