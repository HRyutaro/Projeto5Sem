using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;

    [Range(0,1)]
    public float suavidade;
    public Transform player;

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, suavidade);
    }
}
