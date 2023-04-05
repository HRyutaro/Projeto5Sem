using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("HUD")]
    public GameObject[] Pocoes;
    public static int numeroPocoesAtual;
    public Text numeroPocoes;
    public Text numeroRadiacao;
    public Slider vida;
    public Slider mana;
    public Slider dash;
    public GameObject[] FaceGato;

    [Header("Pause")]
    public GameObject Pause;
    private bool isPause = false;
    public GameObject[] sairCtz;

    [Header("Botões")]
    private int PaginasMenu;
    public GameObject[] menuButtons;
    public GameObject control;
    public GameObject[] controls;
    public GameObject sound;
    public GameObject[] sounds;

    [Header("config")]
    public Slider controlSlide1;
    public Slider controlSlide2;

    [Header("Sons")]
    public Slider musica;
    public Slider son;

    [Header("Inventario")]
    [SerializeField] public bool pertoDaTable = false;
    private bool inInventario = false;
    private bool inInventario2 = false;
    public GameObject Inventario;
    public GameObject[] inventButtons;
    public GameObject[] inventPocaoTela;
    public Text[] pocoesInvent;
    public Text[] plantasInvent;



    void Start()
    {
        instance = this;
        vida.maxValue = Player.instance.VidaTotal;
        mana.maxValue = Player.instance.manaTotal;
        Cursor.lockState = CursorLockMode.Locked;
        pertoDaTable = false;
        musica.value = GameControllerMenu.musicaValor;
        son.value = GameControllerMenu.musicaValor;
    }

    void Update()
    {
        VidaHud();
        PocaoHud();
        dashHud();
        updateNumerosHud();
        ManaHud();
        showPause();
        ControlePause();
        ShowInventario();
        ShowInventario2();
        controleInventario();
        updateInventario();
        checkcontroles();
    }

    void checkcontroles()
    {
        if(Player.op == 1)
        {
            controlSlide1.value = 0;
            controlSlide2.value = 1;
        }
        else if(Player.op == 2)
        {
            controlSlide1.value = 1;
            controlSlide2.value = 0;
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
                sairCtz[0].SetActive(false);
                controls[0].SetActive(false);
                controls[1].SetActive(false);
                menuButtons[0].SetActive(true);
                menuButtons[1].SetActive(true);
                menuButtons[2].SetActive(true);
                if (Input.GetButtonDown("Cancel"))
                {
                    isPause = false;
                    Time.timeScale = 1;
                    Pause.SetActive(false);
                    Cursor.visible = true;
                    StartCoroutine(SairMenu());
                    Cursor.lockState = CursorLockMode.Locked;
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
                controls[0].SetActive(false);
                controls[1].SetActive(false);
                menuButtons[0].SetActive(false);
                menuButtons[1].SetActive(false);
                menuButtons[2].SetActive(false);
            
                if (Input.GetButtonDown("Cancel"))
                {
                    PaginasMenu = 0;
                    EventSystem.current.SetSelectedGameObject(menuButtons[0]);
                }
            }
            else if (PaginasMenu == 2) // Controls
            {
            
                sound.SetActive(false);
                control.SetActive(true);
                sounds[0].SetActive(false);
                sounds[1].SetActive(false);
                controls[0].SetActive(true);
                controls[1].SetActive(true);
                menuButtons[0].SetActive(false);
                menuButtons[1].SetActive(false);
                menuButtons[2].SetActive(false);
                if (Input.GetButtonDown("Cancel"))
                {
                    PaginasMenu = 1;
                    EventSystem.current.SetSelectedGameObject(control);
                }
            }
            else if(PaginasMenu == 3) // Sounds
            {
            
                sound.SetActive(true);
                control.SetActive(false);
                sounds[0].SetActive(true);
                sounds[1].SetActive(true);
                controls[0].SetActive(false);
                controls[1].SetActive(false);
                menuButtons[0].SetActive(false);
                menuButtons[1].SetActive(false);
                menuButtons[2].SetActive(false);
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
                sairCtz[0].SetActive(true);
                controls[0].SetActive(false);
                controls[1].SetActive(false);
                menuButtons[0].SetActive(true);
                menuButtons[1].SetActive(true);
                menuButtons[2].SetActive(true);
                if (Input.GetButtonDown("Cancel"))
                {
                    PaginasMenu = 0;
                    EventSystem.current.SetSelectedGameObject(menuButtons[0]);
                }
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
    public void VoltarSairCtz()
    {
        PaginasMenu = 0;
        EventSystem.current.SetSelectedGameObject(menuButtons[0]);
    }
    public void controleConfig()
    {
        if (controlSlide1.value == 0)
        {
            Player.op = 1;
            controlSlide2.value = 1;
        }
        else if(controlSlide1.value == 1)
        {
            Player.op = 2;
            controlSlide2.value = 0;
        }
        if(controlSlide2.value == 0)
        {
            Player.op = 2;
            controlSlide1.value = 1;
        }
        else if (controlSlide1.value == 1)
        {
            Player.op = 1;
            controlSlide2.value = 0;
        }
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
            }
            if (EventSystem.current.currentSelectedGameObject == inventButtons[1])
            {
                inventPocaoTela[0].SetActive(false);
                inventPocaoTela[1].SetActive(true);
                inventPocaoTela[2].SetActive(false);
                inventPocaoTela[3].SetActive(false);
                inventPocaoTela[4].SetActive(false);
            }
            if (EventSystem.current.currentSelectedGameObject == inventButtons[2])
            {
                inventPocaoTela[0].SetActive(false);
                inventPocaoTela[1].SetActive(false);
                inventPocaoTela[2].SetActive(true);
                inventPocaoTela[3].SetActive(false);
                inventPocaoTela[4].SetActive(false);
            }
            if (EventSystem.current.currentSelectedGameObject == inventButtons[3])
            {
                inventPocaoTela[0].SetActive(false);
                inventPocaoTela[1].SetActive(false);
                inventPocaoTela[2].SetActive(false);
                inventPocaoTela[3].SetActive(true);
                inventPocaoTela[4].SetActive(false);
            }
            if (EventSystem.current.currentSelectedGameObject == inventButtons[4])
            {
                inventPocaoTela[0].SetActive(false);
                inventPocaoTela[1].SetActive(false);
                inventPocaoTela[2].SetActive(false);
                inventPocaoTela[3].SetActive(false);
                inventPocaoTela[4].SetActive(true);
            }
        }
        if(Input.GetButtonDown("Cancel"))
        {
            Inventario.SetActive(false);
            Player.instance.stop = false;
            StartCoroutine(SairMenu());
            inInventario = false;
        }
    }

    public void fabricarPocaoCura()
    {
        if(Player.instance.Radiacao >= 2 && Player.instance.temPocaoCura <= 3 && Player.instance.temPlantaCura >= 1 && inInventario2 == false)
        {
            Player.instance.temPocaoCura += 1;
            Player.instance.Radiacao -= 2;
            Player.instance.temPlantaCura -= 1;
        }
    }
    public void fabricarPocaoMana()
    {
        if (Player.instance.Radiacao >= 2 && Player.instance.temPocaoMana <= 3 && Player.instance.temPlantaMana >= 1 && inInventario2 == false)
        {
            Player.instance.temPocaoMana += 1;
            Player.instance.Radiacao -= 2;
            Player.instance.temPlantaMana -= 1;
        }
    }
    public void fabricarPocaoGelo()
    {
        if (Player.instance.Radiacao >= 3 && Player.instance.temPocaoGelo <= 3 && Player.instance.temPlantaGelo >= 1 && inInventario2 == false)
        {
            Player.instance.temPocaoGelo += 1;
            Player.instance.Radiacao -= 3;
            Player.instance.temPlantaGelo -= 1;
        }
    }
    public void fabricarPocaoFumaca()
    {
        if (Player.instance.Radiacao >= 3 && Player.instance.temPocaoFumaca <= 3 && Player.instance.temPlantaFumaca >= 1 && inInventario2 == false)
        {
            Player.instance.temPocaoFumaca += 1;
            Player.instance.Radiacao -= 3;
            Player.instance.temPlantaFumaca -= 1;
        }
    }
    public void fabricarPocaofogo()
    {
        if (Player.instance.Radiacao >= 5 && Player.instance.temPocaoFogo <= 3 && Player.instance.temPlantaFogo >= 1 && inInventario2 == false)
        {
            Player.instance.temPocaoFogo += 1;
            Player.instance.Radiacao -= 5;
            Player.instance.temPlantaFogo-= 1;
        }
    }

    void showPause()
    {
        if(Input.GetButtonDown("Pause"))
        {
            if(isPause == false)// pause game
            {
                isPause = true;
                Time.timeScale = 0;
                Pause.SetActive(true);
                Player.instance.isPaused = true;
                PaginasMenu = 0;
                EventSystem.current.SetSelectedGameObject(menuButtons[0]);
            }
            else if (isPause == true) // resume game
            {
                isPause = false;
                Time.timeScale = 1;
                Pause.SetActive(false);
                Cursor.visible = true;
                StartCoroutine(SairMenu());
                Cursor.lockState = CursorLockMode.Locked;
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

    public void ShowInventario2()
    {
        if (Input.GetButtonDown("Inventario") && isPause == false || Input.GetKeyDown(KeyCode.Tab) && isPause == false)
        {
            if (inInventario == false)
            {
                Inventario.SetActive(true);
                Player.instance.stop = true;
                Player.instance.isPaused = true;
                inInventario = true;
                EventSystem.current.SetSelectedGameObject(inventButtons[0]);
                inInventario2 = true;
            }
            else if (inInventario == true)
            {
                Inventario.SetActive(false);
                Player.instance.stop = false;
                StartCoroutine(SairMenu());
                inInventario = false;
                inInventario2 = false;
            }
        }
    }

    public void ShowInventario()
    {
        if (Input.GetButtonDown("Interacao") && pertoDaTable == true && isPause == false || Input.GetKeyDown(KeyCode.E) && pertoDaTable == true && isPause == false)
        {
            if (inInventario == false)
            {
                Inventario.SetActive(true);
                Player.instance.stop = true;
                Player.instance.isPaused = true;
                inInventario = true;
                EventSystem.current.SetSelectedGameObject(inventButtons[0]);
            }
            else if (inInventario == true)
            {
                Inventario.SetActive(false);
                Player.instance.stop = false;
                StartCoroutine(SairMenu());
                inInventario = false;
            }
        }
    }
    void updateInventario()
    {
        pocoesInvent[0].text = Player.instance.temPocaoCura.ToString();
        pocoesInvent[1].text = Player.instance.temPocaoMana.ToString();
        pocoesInvent[2].text = Player.instance.temPocaoGelo.ToString();
        pocoesInvent[3].text = Player.instance.temPocaoFumaca.ToString();
        pocoesInvent[4].text = Player.instance.temPocaoFogo.ToString();
        pocoesInvent[5].text = Player.instance.Radiacao.ToString();

        plantasInvent[0].text = Player.instance.temPlantaCura.ToString();
        plantasInvent[1].text = Player.instance.temPlantaMana.ToString();
        plantasInvent[2].text = Player.instance.temPlantaGelo.ToString();
        plantasInvent[3].text = Player.instance.temPlantaFumaca.ToString();
        plantasInvent[4].text = Player.instance.temPlantaFogo.ToString();

    }

    public void voltarInvetario()
    {
        Inventario.SetActive(false);
        Player.instance.stop = false;
        Player.instance.isPaused = false;
        inInventario = false;
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
}
