using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plantas : MonoBehaviour
{
    public int planta;
    private int quantidadeDePlantas;
    public Text quantosPegou;
    private bool podePegar;
    public Color[] cor;
    public GameObject[] tipoDaMesh;
    public Image[] imagemPlanta;

    void Start()
    {
        quantidadeDePlantas = Random.Range(1, 3);
        planta = Random.Range(0, 5);
        quantosPegou.text = quantidadeDePlantas.ToString();
        tipoDePlanta();
    }


    void Update()
    {
        PegarPlanta();
    }
    void PegarPlanta()
    {
        if (Input.GetButtonDown("Interacao") && podePegar == true && Player.tipoDeControle == 1 || Input.GetKeyDown(KeyCode.E) && podePegar == true && Player.tipoDeControle == 0)
        {
            if (planta == 0)
            {
                Player.instance.temPlantaCura += quantidadeDePlantas;
                quantosPegou.text = ("Pegou " + quantidadeDePlantas + " Raiz da Vida");
                StartCoroutine(PegouPlanta());
                GameController.instance.interacaoNatela = false;
                Player.instance.AnimPegar();
                Destroy(gameObject, 1);
            }
            else if (planta == 1)
            {
                Player.instance.temPlantaRaio += quantidadeDePlantas;
                quantosPegou.text = ("Pegou " + quantidadeDePlantas + " Raiz Elétrica");
                StartCoroutine(PegouPlanta());
                GameController.instance.interacaoNatela = false;
                Player.instance.AnimPegar();
                Destroy(gameObject, 1);
            }
            else if (planta == 2)
            {
                Player.instance.temPlantaGelo += quantidadeDePlantas;
                quantosPegou.text = ("Pegou " + quantidadeDePlantas + " Cogumelos Glacial");
                StartCoroutine(PegouPlanta());
                GameController.instance.interacaoNatela = false;
                Player.instance.AnimPegar();
                Destroy(gameObject, 1);
            }
            else if (planta == 3)
            {
                Player.instance.temPlantaFumaca += quantidadeDePlantas;
                quantosPegou.text = ("Pegou " + quantidadeDePlantas + " Pinhas Cósmicas");
                StartCoroutine(PegouPlanta());
                GameController.instance.interacaoNatela = false;
                Player.instance.AnimPegar();
                Destroy(gameObject, 1);
            }
            else if (planta == 4)
            {
                Player.instance.temPlantaFogo += quantidadeDePlantas;
                quantosPegou.text = ("Pegou " + quantidadeDePlantas + " Tentáculos Vulcânicos");
                StartCoroutine(PegouPlanta());
                GameController.instance.interacaoNatela = false;
                Player.instance.AnimPegar();
                Destroy(gameObject, 1);
            }
        }
    }

    void tipoDePlanta()
    {
        if(planta == 0)
        {
            GetComponent<Renderer>().material.color = cor[0];
            tipoDaMesh[0].SetActive(true);
        }
        if (planta == 1)
        {
            GetComponent<Renderer>().material.color = cor[1];
            tipoDaMesh[1].SetActive(true);
        }
        if (planta == 2)
        {
            GetComponent<Renderer>().material.color = cor[2];
            tipoDaMesh[2].SetActive(true);
        }
        if (planta == 3)
        {
            GetComponent<Renderer>().material.color = cor[3];
            tipoDaMesh[3].SetActive(true);
        }
        if (planta == 4)
        {
            GetComponent<Renderer>().material.color = cor[4];
            tipoDaMesh[4].SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameController.instance.interacaoNatela = true;
            podePegar = true;

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameController.instance.interacaoNatela = false;
            podePegar = false;
        }
    }
    IEnumerator PegouPlanta()
    {
        quantosPegou.enabled = true;
        imagemPlanta[planta].enabled = true;
        yield return new WaitForSeconds(0.8f);
        imagemPlanta[planta].enabled = false;
        quantosPegou.enabled = false;
    }
}
