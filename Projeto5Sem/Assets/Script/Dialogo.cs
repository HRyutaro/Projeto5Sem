using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogo : MonoBehaviour
{
    public Sprite[] profile;
    public string[] speechtxt;
    public string nome;

    public GameObject interecao;

    public LayerMask playerLayer;
    public float radious;

    private DialogoControl dc;

    private bool podeInteragir;
    private bool interagiu;

    private void Start()
    {
        dc = FindObjectOfType<DialogoControl>();
    }
    private void FixedUpdate()
    {
        Interacao();
    }
    private void Update()
    {
        if(Input.GetButtonDown("Interacao") && podeInteragir == true || Input.GetKeyDown(KeyCode.E) && podeInteragir == true)
        {
            if (interagiu == false)
            {
                GameController.instance.isPause = true;
                dc.Speech(profile[0], speechtxt, nome);
                Player.instance.isPaused = true;
                Player.instance.stop = true;
                interagiu = true;
            }

            
        }
        if (interagiu == true)
        {
            if (Input.GetButtonDown("Submit"))
            {
                dc.NextSentence(profile[0],nome);
            }
        }
    }

    public void Interacao()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radious, playerLayer);
        if (hits.Length > 0)
        {
            podeInteragir = true;
            interecao.SetActive(true);
        }
        else
        {
            interecao.SetActive(false);
            podeInteragir = false;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radious);
    }
}
