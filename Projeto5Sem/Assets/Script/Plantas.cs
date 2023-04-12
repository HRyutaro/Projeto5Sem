using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plantas : MonoBehaviour
{
    public int planta;
    public GameObject textoInte;
    public GameObject quantosPegoutext;
    private int quantidadeDePlantas;
    public Text quantosPegou;
    private bool podePegar;
    public Color[] cor;

    void Start()
    {
        quantidadeDePlantas = Random.Range(1, 4);
        planta = Random.Range(1, 6);
        quantosPegou.text = quantidadeDePlantas.ToString();
        tipoDePlanta();
    }


    void Update()
    {
        PegarPlanta();
    }
    void PegarPlanta()
    {
        if (Input.GetButtonDown("Interacao") && podePegar == true || Input.GetKeyDown(KeyCode.E) && podePegar == true)
        {
            if (planta == 1)
            {
                Player.instance.temPlantaCura += quantidadeDePlantas;
                StartCoroutine(PegouPlanta());
                StartCoroutine(PegouPlanta2());
                textoInte.SetActive(false);
                Destroy(gameObject, 1);
            }
            else if (planta == 2)
            {
                Player.instance.temPlantaMana += quantidadeDePlantas;
                StartCoroutine(PegouPlanta());
                StartCoroutine(PegouPlanta2());
                textoInte.SetActive(false);
                Destroy(gameObject, 1);
            }
            else if (planta == 3)
            {
                Player.instance.temPlantaGelo += quantidadeDePlantas;
                StartCoroutine(PegouPlanta());
                StartCoroutine(PegouPlanta2());
                textoInte.SetActive(false);
                Destroy(gameObject, 1);
            }
            else if (planta == 4)
            {
                Player.instance.temPlantaFumaca += quantidadeDePlantas;
                StartCoroutine(PegouPlanta());
                StartCoroutine(PegouPlanta2());
                textoInte.SetActive(false);
                Destroy(gameObject, 1);
            }
            else if (planta == 5)
            {
                Player.instance.temPlantaFogo += quantidadeDePlantas;
                StartCoroutine(PegouPlanta());
                StartCoroutine(PegouPlanta2());
                textoInte.SetActive(false);
                Destroy(gameObject, 1);
            }
        }
    }

    void tipoDePlanta()
    {
        if(planta == 1)
        {
            GetComponent<Renderer>().material.color = cor[0];
        }
        if (planta == 2)
        {
            GetComponent<Renderer>().material.color = cor[1];
        }
        if (planta == 3)
        {
            GetComponent<Renderer>().material.color = cor[2];
        }
        if (planta == 4)
        {
            GetComponent<Renderer>().material.color = cor[3];
        }
        if (planta == 5)
        {
            GetComponent<Renderer>().material.color = cor[4];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            textoInte.SetActive(true);
            Player.instance.blockByInt = true;
            podePegar = true;

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            textoInte.SetActive(false);
            Player.instance.blockByInt = false;
            podePegar = false;
        }
    }
    IEnumerator PegouPlanta()
    {
        quantosPegou.enabled = true;
        yield return new WaitForSeconds(0.5f);
        quantosPegou.enabled = false;
    }
    IEnumerator PegouPlanta2()
    {
        quantosPegoutext.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        quantosPegoutext.SetActive(false);
    }
}