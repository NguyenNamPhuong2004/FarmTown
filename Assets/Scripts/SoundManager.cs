using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundManager : Singleton<SoundManager>
{
    public AudioSource AufxClick;
    public AudioSource AufxBackground;
    public AudioClip buttonClip;
    public AudioClip music;
    public AudioClip collectCoin;
    public AudioClip proceed;
    public AudioClip plant;
    public AudioClip harvest;
    private void OnValidate()
    {
        if (AufxClick == null)
        {
            AufxClick = gameObject.AddComponent<AudioSource>();
            AufxClick.playOnAwake = false;
        }
    }
    protected override void Awake()
    {
        MakeSingleton(true);
        Music();
    }
    private void FixedUpdate()
    {
        SoundVolume();
        MusicVolume();
    }
    private void SoundVolume()
    {
        AufxClick.volume = DataPlayer.GetSound();
    }
    private void MusicVolume()
    {
        AufxBackground.volume = DataPlayer.GetMusic();
    }
    public void Music()
    {
        AufxBackground.clip = music;
        AufxBackground.Play();
        AufxBackground.playOnAwake = true;
        AufxBackground.loop = true;
    }
    public void ButtonSound()
    {
        AufxClick.PlayOneShot(buttonClip);
    }
    public void CollectCoin()
    {
        AufxClick.PlayOneShot(collectCoin);
    } 
    public void Proceed()
    {
        AufxClick.PlayOneShot(proceed);
    } 
    public void Plant()
    {
        AufxClick.PlayOneShot(plant);
    } 
    public void Harvest()
    {
        AufxClick.PlayOneShot(harvest);
    }
    
}
