using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogoControl : MonoBehaviour
{
    [Header("Componentes")]
    public GameObject canvasDialogo;
    public Image perfilGato;
    public Text speechText;
    public Text nomeText;
    static public int dialogo = 0;
    public bool onDialogo;
    public bool podeApertar = true;
    public GameObject passarText;
    public GameObject passarImagem;

    [Header("Configurações")]
    public float typingSpeed;
    private string[] sentences;
    public int proxSprite;
    private int index;
    public static bool pularDialogoControl;

    [Header("Dialogo1")]
    public Sprite[] profile;
    public string[] speechtxt;
    public string[] nome;

    [Header("DialogoBoss")]
    public static bool bossDialagoCobra = false;
    public static bool bossDialagoUrso = false;


    private void Update()
    {
        if(pularDialogoControl == false)
        {
            if(dialogo == 1)
            {
                DialogoInicio();
                onDialogo = true;
            }
            if(onDialogo == true)
            {
                if (Input.GetButtonDown("Submit"))
                {
                    if(podeApertar == true)
                    {
                        ++proxSprite;
                        NextSentence(profile[proxSprite], nome[proxSprite]);
                        podeApertar = false;
                    }
                }
            }
            if(Player.tipoDeControle == 1)
            {
                passarText.SetActive(false);
                passarImagem.SetActive(true);

            }
            if (Player.tipoDeControle == 0)
            {
                passarText.SetActive(true);
                passarImagem.SetActive(false);
            }

        }
    }

    public void Speech(Sprite p, string[] txt,string nomePerfil)
    {
        canvasDialogo.SetActive(true);
        perfilGato.sprite = p;
        sentences = txt;
        nomeText.text = nomePerfil;
        StartCoroutine(TypeSentence());
        if(Player.tipoDeControle == 0)
        {
            passarImagem.SetActive(true);
            passarText.SetActive(false);
        }
        else if(Player.tipoDeControle == 1)
        {
            passarImagem.SetActive(false);
            passarText.SetActive(true);
        }
    }

    IEnumerator TypeSentence()
    {
        foreach(char letter in sentences[index].ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        podeApertar = true;
        Dialogo.podeSeguir = true;
        EnterDialogo.podeSeguir = true;
    }

    public void NextSentence(Sprite p,string newname)
    {
        if(speechText.text == sentences[index])
        {
            if(index < sentences.Length - 1)
            {
                index++;
                speechText.text = "";
                perfilGato.sprite = p;
                nomeText.text = newname;
                StartCoroutine(TypeSentence());
            }
            else
            {
                index = 0; 
                proxSprite = 0;
                onDialogo = false;
                speechText.text = "";
                Player.instance.stop = false;
                canvasDialogo.SetActive(false);
                Player.instance.isPaused = false;
                GameController.instance.isPause = false;
                podeApertar = false;
                Dialogo.podeSeguir = false;
                if(bossDialagoCobra == true)
                {
                    StartCoroutine(StartBossBattleCobra());
                    bossDialagoCobra = false;
                }
                if (bossDialagoUrso == true)
                {
                    StartCoroutine(StartBossBattleUrso());
                    bossDialagoUrso = true;
                }
                if (checkpoint.renasceu == true)
                {
                    checkpoint.JaLeu = true;
                }
            }
            
        }
    }

    void DialogoInicio()
    {
        dialogo = 0;
        canvasDialogo.SetActive(true);
        GameController.instance.isPause = true;
        Speech(profile[0], speechtxt, nome[0]);
        Player.instance.isPaused = true;
        Player.instance.stop = true;
    }

    IEnumerator StartBossBattleCobra()
    {
        Player.instance.stop = false;
        yield return new WaitForSeconds(2f);
        BossCobra.startBossBattle = true;
    }
    IEnumerator StartBossBattleUrso()
    {
        Player.instance.stop = false;
        yield return new WaitForSeconds(2f);
        BossUrso.startBoss = true;
    }
}
