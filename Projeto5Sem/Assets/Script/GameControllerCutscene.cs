using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameControllerCutscene : MonoBehaviour
{
    public bool canSkip = false;
    public GameObject space;
    public GameObject x;
    public VideoPlayer video;
    public bool fimdejogo;

    void Start()
    {
        StartCoroutine(continuar());
    }

    void Update()
    {
        if(Input.anyKey)
        {
            StartCoroutine(showSkip());
            if(canSkip == true)
            {
                if(Input.GetButtonDown("Submit"))
                {
                    if(fimdejogo == true)
                    {
                        SceneManager.LoadScene("Menu");
                        Time.timeScale = 1;
                    }
                    else
                    {
                        SceneManager.LoadScene("fase");
                        Time.timeScale = 1;
                    }
                }
            }
        }

    }

    IEnumerator continuar()
    {
        yield return new WaitForSeconds(20f);
        if (Player.tipoDeControle == 0)
        {
            space.SetActive(true);
            canSkip = true;
        }
        if (Player.tipoDeControle == 1)
        {
            x.SetActive(true);
            canSkip = true;
        }
    }
    IEnumerator showSkip()
    {
        
        if(Player.tipoDeControle == 0)
        {
            space.SetActive(true);
        }
        if(Player.tipoDeControle == 1)
        {
            x.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        canSkip = true;
        yield return new WaitForSeconds(4.5f);
        space.SetActive(false);
        x.SetActive(false);
        canSkip = false;
    }
}
