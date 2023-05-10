using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("CheckPoint")]
    public int almasTotal;
    public static int almasAtual;
    public GameObject[] checkPoint;
    public static int checkpointNumber;

    [Header("HUD")]
    public GameObject[] Pocoes;
    public static int numeroPocoesAtual;
    public Text numeroPocoes;
    public Text numeroRadiacao;
    public Slider vida;
    //public Slider mana;
    public Slider dash;
    public GameObject[] FaceGato;
    public Image interacao;
    public Image interacao2;
    public bool interacaoNatela;
    public Text notificacao;
    public GameObject[] almasImagens;
    public Slider vidaBoss;
    public GameObject vidaBossObject;

    [Header("Pause")]
    public GameObject Pause;
    [HideInInspector] public bool isPause = false;
    public GameObject[] sairCtz;
    public GameObject voltar;

    [Header("Botões")]
    private int PaginasMenu;
    public GameObject[] menuButtons;
    public GameObject control;
    public GameObject[] controls;
    public GameObject sound;
    public GameObject[] sounds;
    public GameObject pularTutorialBotao;
    public GameObject pularDialogoBotao;

    [Header("config")]
    public Slider controlSlide;
    public Slider slideTutorial;
    public bool modoDeus;
    public GameObject eventSytem;
    public GameObject eventSytem1;
    public Slider slideDialogo;
    public static int pularDialogoSlideValue;

    [Header("Sons")]
    public Slider musica;
    public Slider son;

    [Header("Inventario")]
    [SerializeField] public bool pertoDaTable = false;
    private bool inInventario = false;
    private bool inInventario2 = false;
    public GameObject InventarioGameObject;
    public GameObject[] inventButtons;
    public GameObject[] inventPocaoTela;
    public Text[] pocoesInvent;
    public Text[] plantasInvent;
    public Text naoTem;
    public GameObject inventarioBancada;
    public GameObject inventario;
    public GameObject[] cartaoKey;
    public GameObject[] infocartaoKey;
    public GameObject botaoFabricar;
    public Image imageInteracaoInventario;
    public Image textoInteracaoInventario;


    [SerializeField]
    [Header("GameOver")]
    private bool isGameOver;
    public GameObject gameOver;
    public GameObject gameOverPanel;
    public GameObject gameOverPanel1;
    public GameObject reiniciarButton;
    public GameObject sairCtzGameOver;
    public GameObject sairButtonGameOver;
    private bool selecionarReiniciar;

    [Header("Tutorial")]
    public GameObject tutorialHud;
    private bool inTutorial;
    public static bool pularTutorial;
    public static int pularTutorialSlideValue;
    public Image imageInteracaotutorial;
    public Image imageInteracaotutorial2;
    public Collider pisoTutorial;
    public GameObject tutorialBasico;
    public GameObject tutorialBasico2;
    [HideInInspector]public bool outPisoTutorial;
    public Collider pisoTutorialCraft;
    public GameObject tutorialCrafting;
    public GameObject tutorialCrafting2;
    [HideInInspector]public bool inPisoTutorialCraft;
    public GameObject tutorialArremesso;
    public GameObject tutorialArremesso2;
    public int pagTutorial;

    void Start()
    {
        DialogoControl.dialogo = 1;
        almasAtual = almasTotal;
        instance = this;
        slideTutorial.value = pularTutorialSlideValue;
        slideDialogo.value = pularDialogoSlideValue;
        Time.timeScale = 1;
        vida.maxValue = Player.instance.VidaTotal;
        musica.value = GameControllerMenu.musicaValor;
        son.value = GameControllerMenu.somValor;
        pertoDaTable = false;
        outPisoTutorial = false;
        inTutorial = false;
    }

    void Update()
    {
        VidaHud();
        PocaoHud();
        dashHud();
        updateNumerosHud();
        showPause();
        ControlePause();
        ShowInventario();
        ShowInventario2();
        controleInventario();
        UpdateInventario();
        checkcontroles();
        ShowGameOver();
        ShowTutorial();
        controleTutorial();
        ControleKeys();
        ShowInteracao();
        controleEventeSystem();
        AlmasHud();
    }

    void Cheat()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Player.instance.Radiacao++;
        }
    }

    void controleEventeSystem()
    {
        if(Player.tipoDeControle == 1)
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
        if(Player.tipoDeControle == 1)
        {
            controlSlide.value = 0;
        }
        if(Player.tipoDeControle == 0)
        {
            controlSlide.value = 1;
        }
    }
    public void ControlePause()
    {
        if(isPause == true)
        {
            if (PaginasMenu == 0)//Menu
            {
                sound.SetActive(false);
                control.SetActive(false);
                sounds[0].SetActive(false);
                sounds[1].SetActive(false);
                sounds[2].SetActive(false);
                sairCtz[0].SetActive(false);
                controls[0].SetActive(false);
                menuButtons[0].SetActive(true);
                menuButtons[1].SetActive(true);
                menuButtons[2].SetActive(true);
                pularTutorialBotao.SetActive(false);
                pularDialogoBotao.SetActive(false);
                voltar.SetActive(false);

                if (Input.GetButtonDown("Cancel"))
                {
                    isPause = false;
                    Time.timeScale = 1;
                    Pause.SetActive(false);
                    StartCoroutine(SairMenu());
                    if (inInventario == true)
                    {
                        EventSystem.current.SetSelectedGameObject(inventButtons[0]);
                    }
                }
            }
            else if(PaginasMenu == 1) // options
            {
                sound.SetActive(true);
                control.SetActive(true);
                sounds[0].SetActive(false);
                sounds[1].SetActive(false);
                sounds[2].SetActive(false);
                controls[0].SetActive(false);
                menuButtons[0].SetActive(false);
                menuButtons[1].SetActive(false);
                menuButtons[2].SetActive(false);
                pularTutorialBotao.SetActive(true);
                pularDialogoBotao.SetActive(true);
                voltar.SetActive(true);

                if (Input.GetButtonDown("Cancel"))
                {
                    PaginasMenu = 0;
                    EventSystem.current.SetSelectedGameObject(menuButtons[0]);
                }
            }
            else if (PaginasMenu == 2) // Controls
            {
            
                sound.SetActive(false);
                control.SetActive(false);
                sounds[0].SetActive(false);
                sounds[1].SetActive(false);
                sounds[2].SetActive(false);
                controls[0].SetActive(true);
                menuButtons[0].SetActive(false);
                menuButtons[1].SetActive(false);
                menuButtons[2].SetActive(false);
                pularTutorialBotao.SetActive(false);
                pularDialogoBotao.SetActive(false);
                voltar.SetActive(true);

                if (Input.GetButtonDown("Cancel"))
                {
                    PaginasMenu = 1;
                    EventSystem.current.SetSelectedGameObject(control);
                }
            }
            else if(PaginasMenu == 3) // Sounds
            {
            
                sound.SetActive(false);
                control.SetActive(false);
                sounds[0].SetActive(true);
                sounds[1].SetActive(true);
                sounds[2].SetActive(true);
                controls[0].SetActive(false);
                menuButtons[0].SetActive(false);
                menuButtons[1].SetActive(false);
                menuButtons[2].SetActive(false);
                pularTutorialBotao.SetActive(false);
                pularDialogoBotao.SetActive(false);
                voltar.SetActive(true);

                if (Input.GetButtonDown("Cancel"))
                {
                    PaginasMenu = 1;
                    EventSystem.current.SetSelectedGameObject(control);
                }
            }
            if (PaginasMenu == -1) //sairCtz
            {
                sound.SetActive(false);
                control.SetActive(false);
                sounds[0].SetActive(false);
                sounds[1].SetActive(false);
                sounds[2].SetActive(false);
                sairCtz[0].SetActive(true);
                controls[0].SetActive(false);
                menuButtons[0].SetActive(false);
                menuButtons[1].SetActive(false);
                menuButtons[2].SetActive(false);
                pularTutorialBotao.SetActive(false);
                pularDialogoBotao.SetActive(false);
                voltar.SetActive(false);

                if (Input.GetButtonDown("Cancel"))
                {
                    PaginasMenu = 0;
                    EventSystem.current.SetSelectedGameObject(menuButtons[0]);
                }
            }

        }
    }
    
    public void Voltar()
    {
        if (isPause == true)
        {
            if (PaginasMenu == 1) // options
            {

                PaginasMenu = 0;
                EventSystem.current.SetSelectedGameObject(menuButtons[0]);

            }
            else if (PaginasMenu == 2) // Controls
            {
                PaginasMenu = 1;
                EventSystem.current.SetSelectedGameObject(control);

            }
            else if (PaginasMenu == 3) // Sounds
            {

               PaginasMenu = 1;
               EventSystem.current.SetSelectedGameObject(control);

            }
        }
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
    public void Options()
    {
        EventSystem.current.SetSelectedGameObject(control);
        PaginasMenu = 1;
    }
    public void Controls()
    {
        EventSystem.current.SetSelectedGameObject(controls[0]);
        PaginasMenu = 2;
    }
    public void Sounds()
    {
        PaginasMenu = 3;
        EventSystem.current.SetSelectedGameObject(sounds[0]);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void SairCtz()
    {
        PaginasMenu = -1;
        EventSystem.current.SetSelectedGameObject(sairCtz[1]);
    }
    public void SairCtzGameOver()
    {
        sairCtzGameOver.SetActive(true);
        EventSystem.current.SetSelectedGameObject(sairButtonGameOver);
    }

    public void VoltarSairCtz()
    {
        PaginasMenu = 0;
        EventSystem.current.SetSelectedGameObject(menuButtons[0]);
    }
    public void VoltarSairCtzGameOver()
    {
        sairCtzGameOver.SetActive(false);
        EventSystem.current.SetSelectedGameObject(reiniciarButton);
    }
    public void controleConfig()
    {
        if (controlSlide.value == 1)
        {
            Player.tipoDeControle = 0;
        }
        else if(controlSlide.value == 0)
        {
            Player.tipoDeControle = 1;
        }
    }
    public void Restart()
    {
        BossUrso.startBoss = false;
        BossCobra.startBossBattle = false;
        checkpointNumber = 0;
        Time.timeScale = 0;
        SceneManager.LoadScene("Fase");
    }

    public void controleInventario()
    {
        if(isPause == false)
        {
            if (EventSystem.current.currentSelectedGameObject == inventButtons[0])
            {
                inventPocaoTela[0].SetActive(true);
                inventPocaoTela[1].SetActive(false);
                inventPocaoTela[2].SetActive(false);
                inventPocaoTela[3].SetActive(false);
                inventPocaoTela[4].SetActive(false);
                infocartaoKey[0].SetActive(false);
                infocartaoKey[1].SetActive(false);
            }
            if (EventSystem.current.currentSelectedGameObject == inventButtons[1])
            {
                inventPocaoTela[0].SetActive(false);
                inventPocaoTela[1].SetActive(true);
                inventPocaoTela[2].SetActive(false);
                inventPocaoTela[3].SetActive(false);
                inventPocaoTela[4].SetActive(false);
                infocartaoKey[0].SetActive(false);
                infocartaoKey[1].SetActive(false);
            }
            if (EventSystem.current.currentSelectedGameObject == inventButtons[2])
            {
                inventPocaoTela[0].SetActive(false);
                inventPocaoTela[1].SetActive(false);
                inventPocaoTela[2].SetActive(true);
                inventPocaoTela[3].SetActive(false);
                inventPocaoTela[4].SetActive(false);
                infocartaoKey[0].SetActive(false);
                infocartaoKey[1].SetActive(false);
            }
            if (EventSystem.current.currentSelectedGameObject == inventButtons[3])
            {
                inventPocaoTela[0].SetActive(false);
                inventPocaoTela[1].SetActive(false);
                inventPocaoTela[2].SetActive(false);
                inventPocaoTela[3].SetActive(true);
                inventPocaoTela[4].SetActive(false);
                infocartaoKey[0].SetActive(false);
                infocartaoKey[1].SetActive(false);
            }
            if (EventSystem.current.currentSelectedGameObject == inventButtons[4])
            {
                inventPocaoTela[0].SetActive(false);
                inventPocaoTela[1].SetActive(false);
                inventPocaoTela[2].SetActive(false);
                inventPocaoTela[3].SetActive(false);
                inventPocaoTela[4].SetActive(true);
                infocartaoKey[0].SetActive(false);
                infocartaoKey[1].SetActive(false);
            }
            if (EventSystem.current.currentSelectedGameObject == inventButtons[5])
            {
                inventPocaoTela[0].SetActive(false);
                inventPocaoTela[1].SetActive(false);
                inventPocaoTela[2].SetActive(false);
                inventPocaoTela[3].SetActive(false);
                inventPocaoTela[4].SetActive(false);
                infocartaoKey[0].SetActive(true);
                infocartaoKey[1].SetActive(false);
            }
            if (EventSystem.current.currentSelectedGameObject == inventButtons[6])
            {
                inventPocaoTela[0].SetActive(false);
                inventPocaoTela[1].SetActive(false);
                inventPocaoTela[2].SetActive(false);
                inventPocaoTela[3].SetActive(false);
                inventPocaoTela[4].SetActive(false);
                infocartaoKey[0].SetActive(false);
                infocartaoKey[1].SetActive(true);
            }
        }
        if(Input.GetButtonDown("Cancel"))
        {
            inInventario = false;
            inInventario2 = false;
            InventarioGameObject.SetActive(true);
            inventarioBancada.SetActive(false);
            inventario.SetActive(false);
            StartCoroutine(SairMenu());
            Player.instance.stop = false;
            Player.instance.isPaused = false;
            imageInteracaoInventario.enabled = false;
            textoInteracaoInventario.enabled = false;
        }
    }

    public void fabricarPocaoCura()
    {
        if(Player.instance.Radiacao >= 2 && Player.instance.temPocaoCura < 5 && Player.instance.temPlantaCura >= 3 && inInventario2 == false)
        {
            Player.instance.temPocaoCura += 1;
            Player.instance.Radiacao -= 2;
            Player.instance.temPlantaCura -= 3;
            Player.instance.StartNumeroPocao();
        }
        else if(Player.instance.Radiacao < 2 && Player.instance.temPlantaCura < 3 && inInventario2 == false)//sem os dois
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "não tem Radiação e Elixir de Vida insuficiente";
        }
        else if(Player.instance.Radiacao >= 2 && Player.instance.temPlantaCura < 3 && inInventario2 == false)//raiz 
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "Não tem Raiz da vida suficiente";

        }
        else if (Player.instance.Radiacao < 2 && Player.instance.temPlantaCura >= 3 && inInventario2 == false) //radiacao
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "não tem Radiação suficiente";
        }
        else if(Player.instance.temPocaoCura == 5 && inInventario2 == false) //limete
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "limete de Elixir de Vida alcançado";
        }
    }
    public void fabricarPocaoMana()
    {
        if (Player.instance.Radiacao >= 3 && Player.instance.temPocaoRaio < 4 && Player.instance.temPlantaRaio >= 4 && inInventario2 == false)
        {
            Player.instance.temPocaoRaio += 1;
            Player.instance.Radiacao -= 3;
            Player.instance.temPlantaRaio -= 4;
            Player.instance.StartNumeroPocao();
        }
        else if(Player.instance.Radiacao < 3 && Player.instance.temPlantaRaio < 4 && inInventario2 == false)// os dois 
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "não tem Radiação e Raiz Elétrica";
        }
        else if(Player.instance.Radiacao >= 3 && Player.instance.temPlantaRaio < 4 && inInventario2 == false)// raiz 
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "Não tem Raiz Elétrica";
        }
        else if(Player.instance.Radiacao < 3 && Player.instance.temPlantaRaio >= 4 && inInventario2 == false)// radiação

        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "Não tem Radição suficiente";
        }
        else if (Player.instance.temPocaoRaio == 3 && inInventario2 == false)
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "Limite de Elixir Elétrico atingido";
        }
    }
    public void fabricarPocaoGelo()
    {
        if (Player.instance.Radiacao >= 2 && Player.instance.temPocaoGelo < 3 && Player.instance.temPlantaGelo >= 3 && inInventario2 == false)
        {
            Player.instance.temPocaoGelo += 1;
            Player.instance.Radiacao -= 2;
            Player.instance.temPlantaGelo -= 3;
            Player.instance.StartNumeroPocao();
        }
        else if(Player.instance.Radiacao < 2 && Player.instance.temPlantaGelo < 3 && inInventario2 == false)//os dois
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "não tem Radiação e Cogumelo Glacial suficiente";
        }
        else if(Player.instance.Radiacao  >= 2 && Player.instance.temPlantaGelo < 3 && inInventario2 == false)//cogumelo
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "não tem Cogumelo Glacial suficiente";
        }
        else if (Player.instance.Radiacao < 2 && Player.instance.temPlantaGelo >= 3 && inInventario2 == false)//radiacao
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "não tem Radiação suficiente";
        }
        else if(Player.instance.temPocaoGelo == 3 && inInventario2 == false)//limite
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "Limite de Frascos Glaciais atingido";
        }
    }
    public void fabricarPocaoFumaca()
    {
        if (Player.instance.Radiacao >= 2 && Player.instance.temPocaoFumaca < 3 && Player.instance.temPlantaFumaca >= 2 && inInventario2 == false)
        {
            Player.instance.temPocaoFumaca += 1;
            Player.instance.Radiacao -= 2;
            Player.instance.temPlantaFumaca -= 2;
            Player.instance.StartNumeroPocao();
        }
        else if(Player.instance.Radiacao < 2 && Player.instance.temPlantaFumaca < 2 && inInventario2 == false)// Os Dois
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "não tem Radiação e Pinhas Cósmicas  suficiente"; 
        }
        else if(Player.instance.Radiacao >= 2 && Player.instance.temPlantaFumaca < 2 && inInventario2 == false)//pinha
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "não tem Pinhas Cósmicas  suficiente";
        }
        else if(Player.instance.Radiacao < 2 && Player.instance.temPlantaFumaca >= 2 && inInventario2 == false)//radiacao
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "não tem Radiação suficiente";
        }
        else if(Player.instance.temPocaoFumaca == 4 && inInventario2 == false)//limite
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "Limite de Colisão Cósmica atingido";
        }
    }
    public void fabricarPocaofogo()
    {
        if (Player.instance.Radiacao >= 5 && Player.instance.temPocaoFogo < 3 && Player.instance.temPlantaFogo >= 4 && inInventario2 == false)
        {
            Player.instance.temPocaoFogo += 1;
            Player.instance.Radiacao -= 5;
            Player.instance.temPlantaFogo-= 4;
            Player.instance.StartNumeroPocao();
        }
        else if(Player.instance.Radiacao < 5 && Player.instance.temPlantaFogo < 4 && inInventario2 == false)// os dois
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "não tem Radiação e Tentáculos Vulcânicos suficiente";
        }
        else if (Player.instance.Radiacao >= 5 && Player.instance.temPlantaFogo < 4 && inInventario2 == false) // vuncanico
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "não tem Tentáculos Vulcânicos suficiente";
        }
        else if (Player.instance.Radiacao < 5 && Player.instance.temPlantaFogo >= 4 && inInventario2 == false) // radiacao
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "não tem Radiação suficiente";
        }
        else if (Player.instance.temPocaoFogo == 3 && inInventario2 == false) // limite
        {
            StartCoroutine(ShowTextInventario());
            naoTem.text = "Limite de Licor de Lava atingido ";
        }
    }

    public void ControleTutorialPular()
    {
        if(slideTutorial.value == 1)
        {
            pularTutorial = true;
            pularTutorialSlideValue = 1;
        }
        else if(slideTutorial.value == 0)
        {
            pularTutorial = false;
            pularTutorialSlideValue = 0;
        }
    }

    public void ControleDialogoPular()
    {
        if (slideDialogo.value == 1)
        {
            Dialogo.pularDialogo = true;
            DialogoControl.pularDialogoControl = true;
            pularDialogoSlideValue = 1;
        }
        else if (slideDialogo.value == 0)
        {
            Dialogo.pularDialogo = false;
            DialogoControl.pularDialogoControl = true;
            pularDialogoSlideValue = 0;
        }
    }

    void showPause()
    {
        if(Input.GetButtonDown("Pause") && isGameOver == false)
        {
            if(isPause == false && inInventario == false && inInventario2 == false && inTutorial == false)// pause game
            {
                isPause = true;
                Time.timeScale = 0;
                Pause.SetActive(true);
                Player.instance.isPaused = true;
                PaginasMenu = 0;
                EventSystem.current.SetSelectedGameObject(menuButtons[0]);
            }
            else if (isPause == true ) // resume game
            {
                isPause = false;
                Time.timeScale = 1;
                Pause.SetActive(false);
                StartCoroutine(SairMenu());
                if(inInventario == true)
                {
                    EventSystem.current.SetSelectedGameObject(inventButtons[0]);
                }
            }
        }

    }

    void VidaHud()
    {
        vida.maxValue = Player.instance.VidaTotal;
        vida.value = Player.VidaAtual;

    }

    void AlmasHud()
    {
        if(almasAtual == 3)
        {
            almasImagens[0].SetActive(true);
            almasImagens[1].SetActive(true);
            almasImagens[2].SetActive(true);
        }
        else if(almasAtual == 2)
        {
            almasImagens[0].SetActive(true);
            almasImagens[1].SetActive(true);
            almasImagens[2].SetActive(false);
        }
        else if (almasAtual == 1)
        {
            almasImagens[0].SetActive(true);
            almasImagens[1].SetActive(false);
            almasImagens[2].SetActive(false);
        }
        else if (almasAtual == 0)
        {
            almasImagens[0].SetActive(false);
            almasImagens[1].SetActive(false);
            almasImagens[2].SetActive(false);
        }
    }
    void ManaHud()
    {
        //mana.maxValue = Player.instance.manaTotal;
        //mana.value = Player.instance.manaAtual;
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

    void ShowGameOver()
    {
        if(almasAtual <= 0)
        {
            gameOver.SetActive(true);
            if(Input.GetButtonDown("Submit"))
            {
                gameOverPanel.SetActive(false);
                gameOverPanel1.SetActive(true);
                Time.timeScale = 0;
                isGameOver = true;
                EventSystem.current.SetSelectedGameObject(reiniciarButton);
            }
        }
    }

    public void ShowInventario2()
    {
        if (Input.GetButtonDown("Inventario") && isPause == false && Player.tipoDeControle == 1 && pertoDaTable == false)
        {
            if (inInventario2 == false)
            {
                inInventario2 = true;
                InventarioGameObject.SetActive(true);
                inventario.SetActive(true);
                Player.instance.stop = true;
                Player.instance.isPaused = true;
                EventSystem.current.SetSelectedGameObject(inventButtons[0]);
                imageInteracaoInventario.enabled = false;
                textoInteracaoInventario.enabled = false;
            }
            else if (inInventario2 == true)
            {
                inInventario2 = false;
                StartCoroutine(SairMenu());
                InventarioGameObject.SetActive(false);
                inventario.SetActive(false);
                Player.instance.stop = false;
                Player.instance.isPaused = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab) && isPause == false && Player.tipoDeControle == 0 && pertoDaTable == false)
        {
            if (inInventario2 == false)
            {
                inInventario2 = true;
                InventarioGameObject.SetActive(true);
                inventario.SetActive(true);
                Player.instance.stop = true;
                Player.instance.isPaused = true;
                EventSystem.current.SetSelectedGameObject(inventButtons[0]);
                imageInteracaoInventario.enabled = false;
                textoInteracaoInventario.enabled = false;
            }
            else if (inInventario2 == true)
            {
                inInventario = false;
                inInventario2 = false;
                StartCoroutine(SairMenu());
                inventario.SetActive(false);
                InventarioGameObject.SetActive(false);
                Player.instance.stop = false;
                Player.instance.isPaused = false;
            }
        }
    }

    public void ShowInventario()
    {
        if (Input.GetButtonDown("Interacao") && pertoDaTable == true && isPause == false && Player.tipoDeControle == 1)
        {
            if (inInventario == false)
            {
                inInventario = true;
                inInventario2 = false;
                InventarioGameObject.SetActive(true);
                inventarioBancada.SetActive(true);
                inventario.SetActive(false);
                Player.instance.stop = true;
                Player.instance.isPaused = true;
                EventSystem.current.SetSelectedGameObject(inventButtons[0]);
                imageInteracaoInventario.enabled = true;
                textoInteracaoInventario.enabled = false;
            }
            else if (inInventario == true)
            {
                inInventario = false;
                inInventario2 = false;
                InventarioGameObject.SetActive(true);
                inventarioBancada.SetActive(false);
                inventario.SetActive(false);
                StartCoroutine(SairMenu());
                Player.instance.stop = false;
                Player.instance.isPaused = false;
                imageInteracaoInventario.enabled = false;
                textoInteracaoInventario.enabled = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && pertoDaTable == true && isPause == false && Player.tipoDeControle == 0)
        {
            if (inInventario == false)
            {
                inInventario = true;
                inInventario2 = false;
                Player.instance.stop = true;
                Player.instance.isPaused = true;
                InventarioGameObject.SetActive(true);
                inventario.SetActive(false);
                inventarioBancada.SetActive(true);
                EventSystem.current.SetSelectedGameObject(inventButtons[0]);
                imageInteracaoInventario.enabled = false;
                textoInteracaoInventario.enabled = true;
            }
            else if (inInventario == true)
            {
                inInventario = false;
                inInventario2 = false;
                StartCoroutine(SairMenu());
                InventarioGameObject.SetActive(false);
                inventarioBancada.SetActive(false);
                inventario.SetActive(false);
                Player.instance.stop = false;
                Player.instance.isPaused = false;
                imageInteracaoInventario.enabled = false;
                textoInteracaoInventario.enabled = false;
            }
        }
    }

    void ControleKeys()
    {
        if(Player.instance.TemCartao == true)
        {
            cartaoKey[0].SetActive(true);
        }
        else if(Player.instance.TemCartao == false)
        {
            cartaoKey[0].SetActive(false);
        }
        if(Player.instance.TemCartao2 == true)
        {
            cartaoKey[1].SetActive(true);
        }
        else if (Player.instance.TemCartao2 == false)
        {
            cartaoKey[1].SetActive(false);
        }
    }

    void UpdateInventario()
    {
        pocoesInvent[0].text = Player.instance.temPocaoCura.ToString();
        pocoesInvent[1].text = Player.instance.temPocaoRaio.ToString();
        pocoesInvent[2].text = Player.instance.temPocaoGelo.ToString();
        pocoesInvent[3].text = Player.instance.temPocaoFumaca.ToString();
        pocoesInvent[4].text = Player.instance.temPocaoFogo.ToString();
        pocoesInvent[5].text = Player.instance.Radiacao.ToString();

        plantasInvent[0].text = Player.instance.temPlantaCura.ToString();
        plantasInvent[1].text = Player.instance.temPlantaRaio.ToString();
        plantasInvent[2].text = Player.instance.temPlantaGelo.ToString();
        plantasInvent[3].text = Player.instance.temPlantaFumaca.ToString();
        plantasInvent[4].text = Player.instance.temPlantaFogo.ToString();

        if(Player.tipoDeControle == 1 && inInventario == true)
        {
            imageInteracaoInventario.enabled = true;
            textoInteracaoInventario.enabled = false;
        }
        if (Player.tipoDeControle == 0 && inInventario == true)
        {
            imageInteracaoInventario.enabled = false;
            textoInteracaoInventario.enabled = true;
        }
    }

    void ShowInteracao()
    {
        if(interacaoNatela == true)
        {
            if(Player.tipoDeControle == 1)
            {
                interacao.enabled = true;
                interacao2.enabled = false;
            }
            if (Player.tipoDeControle == 0)
            {
                interacao.enabled = false;
                interacao2.enabled = true;
            }
        }
        else if(interacaoNatela == false)
        {
            interacao.enabled = false;
            interacao2.enabled = false;
        }
    }

    public void voltarInvetario()
    {
        inInventario = false;
        inInventario2 = false;
        StartCoroutine(SairMenu());
        InventarioGameObject.SetActive(false);
        inventarioBancada.SetActive(false);
        inventario.SetActive(false);
        Player.instance.stop = false;
        Player.instance.isPaused = false;
        imageInteracaoInventario.enabled = false;
        textoInteracaoInventario.enabled = false;
    }

    public void updateNumerosHud()
    {
        numeroPocoes.text = numeroPocoesAtual.ToString();
        numeroRadiacao.text = Player.instance.Radiacao.ToString();
    }

    public void DorGato()
    {
        StartCoroutine(GatoDor());
    }
    void ShowTutorial()
    {
        if(pularTutorial == false)
        {
            if (Player.tipoDeControle == 1)
            {
                imageInteracaotutorial.enabled = true;
                imageInteracaotutorial2.enabled = false;
                if (outPisoTutorial == true)
                {
                    inTutorial = true;
                    tutorialHud.SetActive(true);
                    tutorialBasico2.SetActive(true);

                    if (Input.GetButtonDown("Submit"))
                    {
                        inTutorial = false;
                        outPisoTutorial = false;
                        tutorialHud.SetActive(false);
                        pisoTutorial.enabled = false;
                        tutorialBasico2.SetActive(false);
                    }
                }
                if (inPisoTutorialCraft == true)
                {
                    if (pagTutorial == 0)
                    {
                        ++pagTutorial;
                        inTutorial = true;
                        tutorialHud.SetActive(true);
                        tutorialCrafting2.SetActive(true);

                    }
                    if (Input.GetButtonDown("Submit"))
                    {
                        if (pagTutorial == 1)
                        {
                            ++pagTutorial;
                            tutorialCrafting2.SetActive(false);
                            tutorialArremesso2.SetActive(true);
                        }
                        else if (pagTutorial == 2)
                        {
                            inTutorial = false;
                            inPisoTutorialCraft = false;
                            tutorialHud.SetActive(false);
                            tutorialCrafting2.SetActive(false);
                            pisoTutorialCraft.enabled = false;
                            tutorialArremesso2.SetActive(false);
                        }
                    }
                }
            }
            if (Player.tipoDeControle == 0)
            {
                imageInteracaotutorial.enabled = false;
                imageInteracaotutorial2.enabled = true;
                if (outPisoTutorial == true)
                {
                    inTutorial = true;
                    tutorialHud.SetActive(true);
                    tutorialBasico.SetActive(true);

                    if (Input.GetButtonDown("Submit"))
                    {
                        inTutorial = false;
                        outPisoTutorial = false;
                        tutorialHud.SetActive(false);
                        pisoTutorial.enabled = false;
                        tutorialBasico.SetActive(false);
                    }
                }
                if (inPisoTutorialCraft == true)
                {
                    if (pagTutorial == 0)
                    {
                        ++pagTutorial;
                        inTutorial = true;
                        tutorialHud.SetActive(true);
                        tutorialCrafting.SetActive(true);

                    }
                    if (Input.GetButtonDown("Submit"))
                    {
                        if (pagTutorial == 1)
                        {
                            ++pagTutorial;
                            tutorialCrafting.SetActive(false);
                            tutorialArremesso.SetActive(true);
                        }
                        else if (pagTutorial == 2)
                        {
                            inTutorial = false;
                            inPisoTutorialCraft = false;
                            tutorialHud.SetActive(false);
                            tutorialCrafting.SetActive(false);
                            pisoTutorialCraft.enabled = false;
                            tutorialArremesso.SetActive(false);
                        }
                    }
                }
            }
 
        }
    }
    void controleTutorial()
    {
        if(isPause == false && isGameOver == false && inInventario == false && inInventario2 == false)
        {
            if(inTutorial == true)
            {
                Time.timeScale = 0;
                Player.instance.stop = true;
                Player.instance.isPaused = true;
            }
            else if(inTutorial == false)
            {
                Time.timeScale = 1;
                Player.instance.stop = false;
                Player.instance.isPaused = false;
            }
        }
    }

    public IEnumerator GatoDor()
    {
        FaceGato[1].SetActive(true);
        FaceGato[0].SetActive(false);
        yield return new WaitForSeconds(1);
        FaceGato[0].SetActive(true);
        FaceGato[1].SetActive(false);
    }
    public IEnumerator SairMenu()
    {
        yield return new WaitForSeconds(0.3f);
        Player.instance.isPaused = false;
    }
    IEnumerator ShowTextInventario()
    {
        naoTem.enabled = true;
        botaoFabricar.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        naoTem.enabled = false;
        botaoFabricar.SetActive(true);
    }

    public void ShowInformacao(string x)
    {
        notificacao.text = x;
        StartCoroutine(showInformacao());
    }
     IEnumerator showInformacao()
    {
        notificacao.enabled = true;
        yield return new WaitForSeconds(3f);
        notificacao.enabled = false;
    }
}
