using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcultarParede : MonoBehaviour
{
    public bool ocultar;

    void Start()
    {
        ocultar = false;
    }

    void Update()
    {
        ShadowParede();
    }

    void ShadowParede()
    {
        if(ocultar == true)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else if(ocultar == false)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ShadowParede"))
        {
            ocultar = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ShadowParede"))
        {
            ocultar = false;
        }
    }
}
