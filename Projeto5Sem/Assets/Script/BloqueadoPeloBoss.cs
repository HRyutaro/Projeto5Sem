using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloqueadoPeloBoss : MonoBehaviour
{
    public Collider porta;
    public int tipoDeBoss;
    public Collider bloqueador;
    public Animator anim;
    public static bool bossUrsoisDead = false;
    public static bool bossCobraisDead = false;
    private bool podeInteragir;
    void Start()
    {
        
    }

    void Update()
    {
        bloquear();
        ShowInformacao();
    }
    void ShowInformacao()
    {
        if(podeInteragir == true)
        {
            if (Player.tipoDeControle == 1 && Input.GetButtonDown("Interacao") || Player.tipoDeControle == 0 && Input.GetKeyDown(KeyCode.E))
            {
                if (tipoDeBoss == 0)
                {
                    GameController.instance.ShowInformacao("Preciso derrotar o urso primeiro");
                }
                if (tipoDeBoss == 1)
                {
                    GameController.instance.ShowInformacao("Preciso derrotar a cobra primeiro");
                }

            }
        }
    }
    void bloquear()
    {
        if (bossUrsoisDead == true)
        {
            porta.enabled = true;
            bloqueador.enabled = false;
            anim.SetFloat("Aberta", 1);
        }
        else if (tipoDeBoss == 0 && BossUrso.startBoss == true)
        {
            porta.enabled = false;
            bloqueador.enabled = true;
            anim.SetFloat("Aberta", 0);
        }
        if(bossCobraisDead == true)
        {
            anim.SetFloat("Abrir", 1);
        }
        else if (tipoDeBoss == 1 && BossCobra.startBossBattle == true)
        {
            anim.SetFloat("Abrir", 0);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameController.instance.interacaoNatela = true;
            podeInteragir = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            podeInteragir = false;
            GameController.instance.interacaoNatela = false;
        }
    }
}
