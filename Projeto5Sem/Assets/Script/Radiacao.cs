using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radiacao : MonoBehaviour
{
    public float timeDestroy;
    public int valorMin;
    public int valorMax;
    private int valor;

    void Start()
    {
        valor = Random.Range(valorMin, valorMax);
        Destroy(gameObject, timeDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.instance.Radiacao += valor;
            Destroy(gameObject);
        }
    }
}
