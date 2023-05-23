using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public static float volumeMaster = 1;
    public static float volumeMusica = 1;
    public static float volumeSfx = 1;
    public Slider sliderMaster;
    public Slider sliderMusica;
    public Slider sliderSfx;

    void Start()
    {
        sliderMaster.value = PlayerPrefs.GetFloat("Master");
        sliderMusica.value = PlayerPrefs.GetFloat("Musica");
        sliderMusica.value = PlayerPrefs.GetFloat("Sfx");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VolumeMaster(float volume)
    {
        volumeMaster = volume;
        AudioListener.volume = volumeMaster;
        PlayerPrefs.SetFloat("Master", volumeMaster);
    }

    public void VolumeMusica(float volume)
    {
        volumeMusica = volume;
        GameObject[] Musicas = GameObject.FindGameObjectsWithTag("Musica");
        for (int i = 0; i < Musicas.Length; i++)
        {
            Musicas[i].GetComponent<AudioSource>().volume = volumeMusica;
        }
        PlayerPrefs.SetFloat("Musica", volumeMusica);
    }

    public void VolumeSfx(float volume)
    {
        volumeSfx = volume;
        GameObject[] sfx = GameObject.FindGameObjectsWithTag("Sfx");
        for (int i = 0; i < sfx.Length; i++)
        {
            sfx[i].GetComponent<AudioSource>().volume = volumeSfx;
        }
        PlayerPrefs.SetFloat("Sfx", volumeSfx);
    }
}
