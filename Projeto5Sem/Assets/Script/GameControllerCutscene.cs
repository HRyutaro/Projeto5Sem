using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameControllerCutscene : MonoBehaviour
{
    public bool canSkip;
    public GameObject text;
    public GameObject image;
    public VideoPlayer video;

    void Start()
    {
        video.Play();
    }

    void Update()
    {
        if(Input.anyKeyDown)
        {
            StartCoroutine(showSkip());
            if(canSkip == true)
            {
                if(Input.GetButtonDown("Submit"))
                {
                    SceneManager.LoadScene("fase");
                    Time.timeScale = 1;
                }
            }
        }

    }

    IEnumerator showSkip()
    {
        canSkip = true;
        if(Player.tipoDeControle == 0)
        {
            text.SetActive(true);
        }
        if(Player.tipoDeControle == 1)
        {
            image.SetActive(true);
        }
        yield return new WaitForSeconds(3);
        text.SetActive(false);
        image.SetActive(false);
        canSkip = false;
    }
}
