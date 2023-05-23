using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    public static checkpoint instance;

    public Collider col;
    public GameObject primeiroRenascimento;
    public static bool renasceu;
    public bool Jarenasceu = false;
    public static bool JaLeu;
    public GameObject ef1;
    public GameObject ef2;
    public GameObject ef3;
    public GameObject ef4;

    void Start()
    {
        ef1.SetActive(false);
        ef2.SetActive(false);
        ef3.SetActive(false);
        ef4.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if( JaLeu == true)
        {
            primeiroRenascimento.SetActive(false);
        }
        else if(Jarenasceu == false && renasceu == true)
        {
            StartCoroutine(playEffc());
            primeiroRenascimento.SetActive(true);
            Jarenasceu = true;
        }

    }

    public void PlayEffects()
    {
        StartCoroutine(playEffc());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            col.enabled = false;
            GameController.checkpointNumber ++;
            print("checkpoint " + GameController.checkpointNumber);
        }
    }
    IEnumerator playEffc()
    {
        ef1.SetActive(true);
        ef2.SetActive(true);
        ef3.SetActive(true);
        ef4.SetActive(true);
        yield return new WaitForSeconds(2f);
        ef1.SetActive(false);
        ef2.SetActive(false);
        ef3.SetActive(false);
        ef4.SetActive(false);
    }
}
