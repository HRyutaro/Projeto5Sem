using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radiacao : MonoBehaviour
{
    public float timeDestroy;
    public int valor;

    void Start()
    {
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
