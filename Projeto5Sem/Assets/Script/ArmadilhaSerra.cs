using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilhaSerra : MonoBehaviour
{
    public float speed = 5f; // Velocidade do objeto
    public float distance = 10f; // Dist�ncia que o objeto percorrer�
    private Vector3 initialPosition; // Posi��o inicial do objeto
    private Vector3 targetPosition; // Posi��o de destino do objeto
    public bool Horizontal;

    void Start()
    {
        initialPosition = transform.position;
        if(Horizontal == true)
        {
            targetPosition = initialPosition + new Vector3(0f, 0f, distance);
        }
        else
        {
            targetPosition = initialPosition + new Vector3(distance, 0f, 0f);
        }
    }

    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            // Inverte a posi��o de destino para fazer o objeto andar em loop
            if (Horizontal == true)
            {
                targetPosition = (targetPosition == initialPosition) ? initialPosition + new Vector3(0f, 0f, distance) : initialPosition;

            }
            else
            {
                targetPosition = (targetPosition == initialPosition) ? initialPosition + new Vector3(distance, 0f, 0f) : initialPosition;
            }
        }
    }
}
