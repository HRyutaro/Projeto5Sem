using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("HUD")]
    public GameObject[] Vidas;
    public GameObject[] Pocoes;
    public GameObject[] dash;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        VidaHud();
        PocaoHud();
        dashHud();
    }

    void VidaHud()
    {
        if(Player.VidaAtual == 4)
        {
            Vidas[0].SetActive(true);
            Vidas[1].SetActive(true);
            Vidas[2].SetActive(true);
            Vidas[3].SetActive(true);
        }
        if (Player.VidaAtual == 3)
        {
            Vidas[0].SetActive(true);
            Vidas[1].SetActive(true);
            Vidas[2].SetActive(true);
            Vidas[3].SetActive(false);
        }
        if (Player.VidaAtual == 2)
        {
            Vidas[0].SetActive(true);
            Vidas[1].SetActive(true);
            Vidas[2].SetActive(false);
            Vidas[3].SetActive(false);
        }
        if (Player.VidaAtual == 1)
        {
            Vidas[0].SetActive(true);
            Vidas[1].SetActive(false);
            Vidas[2].SetActive(false);
            Vidas[3].SetActive(false);
        }
        if (Player.VidaAtual == 0)
        {
            Vidas[0].SetActive(false);
            Vidas[1].SetActive(false);
            Vidas[2].SetActive(false);
            Vidas[3].SetActive(false);
        }
    }

    void PocaoHud()
    {
        if(pocao.tipoDapocao == 0)
        {
            Pocoes[0].SetActive(true);
            Pocoes[1].SetActive(false);
            Pocoes[2].SetActive(false);
        }
        if (pocao.tipoDapocao == 1)
        {
            Pocoes[0].SetActive(false);
            Pocoes[1].SetActive(true);
            Pocoes[2].SetActive(false);
        }
        if (pocao.tipoDapocao == 2)
        {
            Pocoes[0].SetActive(false);
            Pocoes[1].SetActive(false);
            Pocoes[2].SetActive(true);
        }
    }

    void dashHud()
    {
        if (Player.instance.isDashing == 4)
        {
            dash[0].SetActive(true);
            dash[1].SetActive(true);
            dash[2].SetActive(true);
            dash[3].SetActive(true);
        }
        if (Player.instance.isDashing == 3)
        {
            dash[0].SetActive(true);
            dash[1].SetActive(true);
            dash[2].SetActive(true);
            dash[3].SetActive(false);
        }
        if(Player.instance.isDashing == 2)
        {
            dash[0].SetActive(true);
            dash[1].SetActive(true);
            dash[2].SetActive(false);
            dash[3].SetActive(false);
        }
        if (Player.instance.isDashing == 1)
        {
            dash[0].SetActive(true);
            dash[1].SetActive(false);
            dash[2].SetActive(false);
            dash[3].SetActive(false);
        }
        if (Player.instance.isDashing == 0)
        {
            dash[0].SetActive(false);
            dash[1].SetActive(false);
            dash[2].SetActive(false);
            dash[3].SetActive(false);
        }
    }
}
