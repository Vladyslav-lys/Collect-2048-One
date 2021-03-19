using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : BaseManager<SoundManager>
{
    public AudioSource startSound;
    public AudioSource winSound;
    public AudioSource swipeSound;
    public AudioSource tapSound;
    public AudioSource collectSound;
    public AudioSource otherCubeHitSound;
    private UIManager _uiManager;
    
    protected override void Awake()
    {
        if (!instance)
            instance = this;
        base.Awake();
    }

    private void Start()
    {
        _uiManager = UIManager.instance;
        _uiManager.closeVibration.SetActive(!IsVibration());
        _uiManager.closeSound.SetActive(!IsSound());
    }

    public void OnOffVibration(int numEquals) => OnOffOption("Vibration", _uiManager.closeVibration);
    
    public void OnOffSound(int numEquals) => OnOffOption("Sound", _uiManager.closeSound);

    private void OnOffOption(string optionName, GameObject closeObject)
    {
        int option = PlayerPrefs.GetInt(optionName, 1) == 0 ? 1 : 0;
        PlayerPrefs.SetInt(optionName, option);
        _uiManager.OnOffClose(closeObject);
    }

    public bool IsVibration() => PlayerPrefs.GetInt("Vibration", 1) == 1;
    
    public bool IsSound() => PlayerPrefs.GetInt("Sound", 1) == 1;

    public void Vibrate()
    {
        if(!IsVibration())
            return;
        Handheld.Vibrate();
    }

    public void PlaySound(AudioSource sound)
    {
        if(!IsSound() || !sound)
            return;
        sound.Play();
    }

    public void PlayTapSound() => PlaySound(tapSound);
}
