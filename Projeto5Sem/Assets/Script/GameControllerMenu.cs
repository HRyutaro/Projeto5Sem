using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameControllerMenu : MonoBehaviour
{
    private int tela;
    [Header("Menu")]
    public GameObject novoJogo;
    public GameObject opcoes;
    public GameObject sair;
    public GameObject[] sairCtz;

    [Header("Opcao")]
    public GameObject[] controles;
    public GameObject[] sons;
    public GameObject pularTutorialButoon;

    [Header("Controles")]
    public Slider[] controlSlide;

    [Header("Sons")]
    public Slider[] sonSlides;
    public static float somValor;
    public static float musicaValor;

    public Slider pularTutorialslide;
    void Start()
    {
        tela = 0;
        sonSlides[0].value = somValor;
        sonSlides[1].value = musicaValor;
        Player.op = 1;
    }

    void Update()
    {
        controleTela();
        checkcontroles();

    }

    void checkcontroles()
    {
        if (Player.op == 1)
        {
            controlSlide[0].value = 0;
            controlSlide[1].value = 0;
        }
        else if (Player.op == 2)
        {
            controlSlide[0].value = 0;
            controlSlide[1].value = 0;
        }
    }
    void controleTela()
    {
        if (tela == 0)// tela menu
        {
            sairCtz[0].SetActive(false);
            novoJogo.SetActive(true);
            opcoes.SetActive(true);
            sair.SetActive(true);

            controles[0].SetActive(false);
            sons[0].SetActive(false);
            pularTutorialButoon.SetActive(false);
            controles[1].SetActive(false);
            controles[2].SetActive(false);
            sons[1].SetActive(false);
            sons[2].SetActive(false);

        }
        else if(tela == 1) // tela opcoes
        {
            sairCtz[0].SetActive(false);
            novoJogo.SetActive(false);
            opcoes.SetActive(false);
            sair.SetActive(false);

            controles[0].SetActive(true);
            sons[0].SetActive(true);
            pularTutorialButoon.SetActive(true);
            controles[1].SetActive(false);
            controles[2].SetActive(false);
            sons[1].SetActive(false);
            sons[2].SetActive(false);
            if (Input.GetButtonDown("Cancel"))
            {
                tela = 0;
                EventSystem.current.SetSelectedGameObject(novoJogo);
            }
        }
        else if(tela == 2)// tela controles
        {
            sairCtz[0].SetActive(false);
            novoJogo.SetActive(false);
            opcoes.SetActive(false);
            sair.SetActive(false);

            controles[0].SetActive(false);
            sons[0].SetActive(false);
            pularTutorialButoon.SetActive(false);
            controles[1].SetActive(true);
            controles[2].SetActive(true);
            sons[1].SetActive(false);
            sons[2].SetActive(false);
            if (Input.GetButtonDown("Cancel"))
            {
                tela = 1;
                EventSystem.current.SetSelectedGameObject(controles[0]);
            }
        }
        else if (tela == 3)// tela Sons
        {

            sairCtz[0].SetActive(false);
            novoJogo.SetActive(false);
            opcoes.SetActive(false);
            sair.SetActive(false);

            controles[0].SetActive(false);
            sons[0].SetActive(false);
            pularTutorialButoon.SetActive(false);
            controles[1].SetActive(false);
            controles[2].SetActive(false);
            sons[1].SetActive(true);
            sons[2].SetActive(true);
            if (Input.GetButtonDown("Cancel"))
            {
                tela = 1;
                EventSystem.current.SetSelectedGameObject(controles[0]);
            }
        }
        else if (tela == -1)// tela Sair Ctz
        {
            sairCtz[0].SetActive(true);
            novoJogo.SetActive(true);
            opcoes.SetActive(true);
            sair.SetActive(true);

            controles[0].SetActive(false);
            sons[0].SetActive(false);
            pularTutorialButoon.SetActive(false);
            controles[1].SetActive(false);
            controles[2].SetActive(false);
            sons[1].SetActive(false);
            sons[2].SetActive(false);
            if (Input.GetButtonDown("Cancel"))
            {
                tela = 0;
                EventSystem.current.SetSelectedGameObject(novoJogo);
            }
        }
    }

     public void PularTutorial()
    {
        if(pularTutorialslide.value == 1)
        {
            GameController.pularTutorialSlideValue = 1;
            GameController.pularTutorial = true;
        }
        else if(pularTutorialslide.value == 0)
        {
            GameController.pularTutorialSlideValue = 0;
            GameController.pularTutorial = false;
        }
    }

    public void IniciarFase(string x)
    {
        SceneManager.LoadScene(x);
        Time.timeScale = 1;
        
    }
    public void Opcoes()
    {
        tela = 1;
        EventSystem.current.SetSelectedGameObject(controles[0]);
    }
    public void Controles()
    {
        tela = 2;
        EventSystem.current.SetSelectedGameObject(controles[1]);
    }
    public void Sons()
    {
        tela = 3;
        EventSystem.current.SetSelectedGameObject(sons[1]);
    }
    public void controleConfig()
    {
        if (controlSlide[0].value == 0)
        {
            Player.op = 1;
            controlSlide[1].value = 1;
        }
        else if (controlSlide[0].value == 1)
        {
            Player.op = 2;
            controlSlide[1].value = 0;
        }
        if (controlSlide[1].value == 0)
        {
            Player.op = 2;
            controlSlide[0].value = 1;
        }
        else if (controlSlide[1].value == 1)
        {
            Player.op = 1;
            controlSlide[0].value = 0;
        }
    }
    public void VoltarSairCtz()
    {
        tela = 0;
        EventSystem.current.SetSelectedGameObject(novoJogo);
    }
    public void SairCtz()
    {
        tela = -1;
        EventSystem.current.SetSelectedGameObject(sairCtz[1]);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
