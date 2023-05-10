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
    public GameObject telaInicial;
    public GameObject telaInicial1;
    public GameObject voltar;

    [Header("Opcao")]
    public GameObject[] controles;
    public GameObject[] sons;
    public GameObject pularTutorialButoon;
    public GameObject pularDialogoButoon;
    public GameObject eventSytem;
    public GameObject eventSytem1;

    [Header("Controles")]
    public Slider controlSlide;

    [Header("Sons")]
    public Slider[] sonSlides;
    public static float somValor = 1;
    public static float musicaValor = 1;

    public Slider pularTutorialSlide;
    public Slider slideDialogo;

    void Start()
    {
        tela = 0;
        sonSlides[0].value = somValor;
        sonSlides[1].value = musicaValor;
    }

    void Update()
    {
        controleTela();
        checkcontroles();
        controleEventeSystem();
    }

    void controleEventeSystem()
    {
        if (Player.tipoDeControle == 1)
        {
            eventSytem.SetActive(true);
            eventSytem1.SetActive(false);
        }
        if (Player.tipoDeControle == 0)
        {
            eventSytem.SetActive(false);
            eventSytem1.SetActive(true);
        }
    }

    void checkcontroles()
    {
        if (Player.tipoDeControle == 1)
        {
            controlSlide.value = 0;
        }
        else if (Player.tipoDeControle == 0)
        {
            controlSlide.value = 1;

        }
    }
    void controleTela()
    {
        if (tela == 0)// tela menu
        {
            telaInicial.SetActive(true);
            telaInicial1.SetActive(false);
            sairCtz[0].SetActive(false);
            novoJogo.SetActive(true);
            opcoes.SetActive(true);
            sair.SetActive(true);

            controles[0].SetActive(false);
            sons[0].SetActive(false);
            pularTutorialButoon.SetActive(false);
            pularDialogoButoon.SetActive(false);
            controles[1].SetActive(false);
            sons[1].SetActive(false);
            sons[2].SetActive(false);
            sons[3].SetActive(false);
            voltar.SetActive(false);

        }
        else if(tela == 1) // tela opcoes
        {
            telaInicial.SetActive(false);
            telaInicial1.SetActive(true);
            sairCtz[0].SetActive(false);
            novoJogo.SetActive(false);
            opcoes.SetActive(false);
            sair.SetActive(false);

            controles[0].SetActive(true);
            sons[0].SetActive(true);
            pularTutorialButoon.SetActive(true);
            pularDialogoButoon.SetActive(true);
            controles[1].SetActive(false);
            sons[1].SetActive(false);
            sons[2].SetActive(false);
            sons[3].SetActive(false);
            voltar.SetActive(true);

            if (Input.GetButtonDown("Cancel"))
            {
                tela = 0;
                EventSystem.current.SetSelectedGameObject(novoJogo);
            }
        }
        else if(tela == 2)// tela controles
        {
            telaInicial.SetActive(false);
            telaInicial1.SetActive(true);
            sairCtz[0].SetActive(false);
            novoJogo.SetActive(false);
            opcoes.SetActive(false);
            sair.SetActive(false);

            controles[0].SetActive(false);
            sons[0].SetActive(false);
            pularTutorialButoon.SetActive(false);
            pularDialogoButoon.SetActive(false);
            controles[1].SetActive(true);
            sons[1].SetActive(false);
            sons[2].SetActive(false);
            sons[3].SetActive(false);
            voltar.SetActive(true);

            if (Input.GetButtonDown("Cancel"))
            {
                tela = 1;
                EventSystem.current.SetSelectedGameObject(controles[0]);
            }
        }
        else if (tela == 3)// tela Sons
        {
            telaInicial.SetActive(false);
            telaInicial1.SetActive(true);
            sairCtz[0].SetActive(false);
            novoJogo.SetActive(false);
            opcoes.SetActive(false);
            sair.SetActive(false);

            controles[0].SetActive(false);
            sons[0].SetActive(false);
            pularTutorialButoon.SetActive(false);
            pularDialogoButoon.SetActive(false);
            controles[1].SetActive(false);
            sons[1].SetActive(true);
            sons[2].SetActive(true);
            sons[3].SetActive(true);
            voltar.SetActive(true);

            if (Input.GetButtonDown("Cancel"))
            {
                tela = 1;
                EventSystem.current.SetSelectedGameObject(controles[0]);
            }
        }
        else if (tela == -1)// tela Sair Ctz
        {
            telaInicial.SetActive(true);
            telaInicial1.SetActive(false);
            sairCtz[0].SetActive(true);
            novoJogo.SetActive(true);
            opcoes.SetActive(true);
            sair.SetActive(true);

            controles[0].SetActive(false);
            sons[0].SetActive(false);
            pularTutorialButoon.SetActive(false);
            pularDialogoButoon.SetActive(false);
            controles[1].SetActive(false);
            sons[1].SetActive(false);
            sons[2].SetActive(false);
            sons[3].SetActive(false);
            voltar.SetActive(true);

            if (Input.GetButtonDown("Cancel"))
            {
                tela = 0;
                EventSystem.current.SetSelectedGameObject(novoJogo);
            }
        }
    }

     public void PularTutorial()
    {
        if(pularTutorialSlide.value == 1)
        {
            GameController.pularTutorialSlideValue = 1;
            GameController.pularTutorial = true;
        }
        else if(pularTutorialSlide.value == 0)
        {
            GameController.pularTutorialSlideValue = 0;
            GameController.pularTutorial = false;
        }
    }

    public void ControleDialogoPular()
    {
        if (slideDialogo.value == 1)
        {
            Dialogo.pularDialogo = true;
            DialogoControl.pularDialogoControl = true;
            GameController.pularDialogoSlideValue = 1;
        }
        else if (slideDialogo.value == 0)
        {
            Dialogo.pularDialogo = false;
            DialogoControl.pularDialogoControl = false;
            GameController.pularDialogoSlideValue = 0;
        }
    }
    public void Voltar()
    {
        if (tela == 1) // tela opcoes
        {
            tela = 0;
            EventSystem.current.SetSelectedGameObject(novoJogo);

        }
        else if (tela == 2)// tela controles
        {
            tela = 1;
            EventSystem.current.SetSelectedGameObject(controles[0]);
        }
        else if (tela == 3)// tela Sons
        {
            tela = 1;
            EventSystem.current.SetSelectedGameObject(controles[0]);
        }
        else if (tela == -1)// tela Sair Ctz
        {
            tela = 0;
            EventSystem.current.SetSelectedGameObject(novoJogo);
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
        if (controlSlide.value == 1)
        {
            Player.tipoDeControle = 0;
        }
        else if (controlSlide.value == 0)
        {
            Player.tipoDeControle = 1;
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
