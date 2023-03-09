using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class efeitoPocao : MonoBehaviour
{
    public float duracao;
    void Start()
    {
        Destroy(gameObject, duracao);
    }

}
