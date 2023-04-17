using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDialogo : MonoBehaviour
{
    public Sprite[] profile;
    public string[] speechtxt;
    public string[] nome;

    public GameObject interecao;

    public LayerMask playerLayer;
    public float width;
    public float height;
    public float depth;

    private DialogoControl dc;

    private bool podeInteragir = false;
    private bool interagiu = false;
    public static bool podeSeguir;

    public int proxSprite = 0;

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
        if (podeInteragir == true)
        {
            if (interagiu == false)
            {
                GameController.instance.isPause = true;
                dc.Speech(profile[0], speechtxt, nome[0]);
                Player.instance.isPaused = true;
                Player.instance.stop = true;
                interagiu = true;
            }


        }
        if (interagiu == true && podeSeguir == true)
        {
            if (Input.GetButtonDown("Submit"))
            {
                ++proxSprite;
                dc.NextSentence(profile[proxSprite], nome[proxSprite]);
                podeSeguir = false;
            }
        }
    }

    public void Interacao()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, new Vector3(width, height, depth) / 2f, Quaternion.identity, playerLayer);
        if (hits.Length > 0)
        {
            podeInteragir = true;
            DialogoControl.bossDialago = true;
        }
        else
        {
            podeInteragir = false;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position, new Vector3(width, height, depth));
    }
}
