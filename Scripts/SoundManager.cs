using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource BgSource;
    public AudioClip BgClip;
    public AudioClip StartStage;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        BgSource.Stop();
        if (arg0.name != "Intro")
        {
            SFXPlay("StartStage", StartStage);
            BgSoundPlay(BgClip, 0.1f);
        }
    }

    private void BgSoundPlay(AudioClip bgm,float volume)
    {
        BgSource.clip = bgm;
        BgSource.loop = true;
        BgSource.volume = volume;
        BgSource.Play();
    }

    public void SFXPlay(string Name, AudioClip clip)
    {
        GameObject obj = new GameObject(Name + "Sound");
        AudioSource audiosource = obj.AddComponent<AudioSource>();
        audiosource.clip = clip;
        audiosource.Play();

        Destroy(obj, clip.length);
    }


}
