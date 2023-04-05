using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParedeTransparente : MonoBehaviour
{
    public Transform alvo;
    public RaycastHit hitPoint = new RaycastHit();

    void Start()
    {
        
    }

    void Update()
    {
        if(Physics.Linecast(transform.position, alvo.transform.position, out hitPoint))
        {
            Debug.DrawLine(transform.position, alvo.transform.position);
        }
    }
}
