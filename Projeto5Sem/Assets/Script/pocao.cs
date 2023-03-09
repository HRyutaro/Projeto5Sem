using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pocao : MonoBehaviour
{
    public float speed;
    public float timeDestroy;

    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
