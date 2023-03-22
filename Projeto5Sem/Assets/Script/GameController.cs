using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("HUD")]
    public GameObject[] Pocoes;
    public static int numeroPocoesAtual;
    public Text numeroPocoes;
    public Slider vida;
    public Slider mana;
    public Slider dash;

    void Start()
    {
        vida.maxValue = Player.instance.VidaTotal;
        vida.maxValue = Player.instance.manaTotal;
        instance = this;
        
    }

    void Update()
    {
        VidaHud();
        PocaoHud();
        dashHud();
        updateNumeroPocoes();
        ManaHud();
    }

    void VidaHud()
    {
        vida.maxValue = Player.instance.VidaTotal;
        vida.value = Player.VidaAtual;
    }
    void ManaHud()
    {
        mana.maxValue = Player.instance.manaTotal;
        mana.value = Player.instance.manaAtual;
    }

    void PocaoHud()
    {
        if(pocao.tipoDapocao == 0)
        {
            Pocoes[0].SetActive(true);
            Pocoes[1].SetActive(false);
            Pocoes[2].SetActive(false);
            Pocoes[3].SetActive(false);
            Pocoes[4].SetActive(false);
        }
        if (pocao.tipoDapocao == 1)
        {
            Pocoes[0].SetActive(false);
            Pocoes[1].SetActive(true);
            Pocoes[2].SetActive(false);
            Pocoes[3].SetActive(false);
            Pocoes[4].SetActive(false);
        }
        if (pocao.tipoDapocao == 2)
        {
            Pocoes[0].SetActive(false);
            Pocoes[1].SetActive(false);
            Pocoes[2].SetActive(true);
            Pocoes[3].SetActive(false);
            Pocoes[4].SetActive(false);
        }
        if (pocao.tipoDapocao == 3)
        {
            Pocoes[0].SetActive(false);
            Pocoes[1].SetActive(false);
            Pocoes[2].SetActive(false);
            Pocoes[3].SetActive(true);
            Pocoes[4].SetActive(false);
        }
        if (pocao.tipoDapocao == 4)
        {
            Pocoes[0].SetActive(false);
            Pocoes[1].SetActive(false);
            Pocoes[2].SetActive(false);
            Pocoes[3].SetActive(false);
            Pocoes[4].SetActive(true);
        }
    }

    void dashHud()
    {
        dash.value = Player.instance.isDashing;

    }

    public void updateNumeroPocoes()
    {
        numeroPocoes.text = numeroPocoesAtual.ToString();
    }
}
