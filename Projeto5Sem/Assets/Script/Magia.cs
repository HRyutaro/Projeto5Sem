using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magia : MonoBehaviour
{
    public float timeDestroy;
    public Collider col;

    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "inimigo")
        {
            col.enabled = false;
            Destroy(gameObject, 0);
        }
        if (other.gameObject.tag == "Parede")
        {
            col.enabled = false;
            Destroy(gameObject, 0);
        }
        if (other.gameObject.tag == "Porta")
        {
            col.enabled = false;
            Destroy(gameObject, 0);
        }
        if (other.gameObject.tag == "Buraco")
        {
            col.enabled = false;
            Destroy(gameObject, 0);
        }
        if (other.gameObject.tag == "CobraBoss")
        {
            col.enabled = false;
            Destroy(gameObject, 0);
        }
        if (other.gameObject.tag == "BossParado")
        {
            col.enabled = false;
            Destroy(gameObject, 0);
        }
    }
}
