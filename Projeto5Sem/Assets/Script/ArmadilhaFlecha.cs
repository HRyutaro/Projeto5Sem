using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilhaFlecha : MonoBehaviour
{
    public GameObject flechaprefab;
    public float tempo;
    public float tempoDeMovimento;
    private bool isRunning = false;
   

    void Start()
    {
        StartCoroutine(flecha());
    }
    void Update()
    {

    }

    IEnumerator flecha()
    {
        while (true) 
        {
            yield return new WaitForSeconds(tempo);

         
            if (!isRunning)
            {
                isRunning = true; 

                Vector3 initialPosition = transform.position; 

                GameObject flecha = Instantiate(flechaprefab, initialPosition, gameObject.transform.rotation);

                Vector3 targetPosition = initialPosition + transform.forward * 10f;

                float elapsedTime = 0f; 

                while (elapsedTime < tempoDeMovimento)
                {
                    flecha.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / tempoDeMovimento);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                Destroy(flecha);

                isRunning = false;
            }
        }
    }
}
