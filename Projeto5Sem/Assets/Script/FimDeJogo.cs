using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FimDeJogo : MonoBehaviour
{
    public static FimDeJogo instance;
    public bool desligouReator;
    public GameObject effects;

    void Start()
    {
        effects.SetActive(false);
    }


    void Update()
    {
        if(desligouReator == true)
        {
            effects.SetActive(true);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && desligouReator == true)
        {
            SceneManager.LoadScene("FimDeJogo");
        }
    }
}
