using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundManager : Singleton<SoundManager>
{
    public AudioSource AufxClick;
    public AudioSource AufxRain;
    public AudioSource AufxBackground;
    public AudioClip rain;
    public AudioClip arrowRain;
    public AudioClip buttonClip;
    public AudioClip music;
    public AudioClip buyOrUpgrade;
    public AudioClip victory;
    public AudioClip defeat;
    public AudioClip swordAttack;
    public AudioClip arrowAttack;
    public AudioClip typeKeyBoard;
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
    }
    public void Rain()
    {
        AufxRain.clip = rain;
        AufxRain.Play();
    }
    public void ButtonSound()
    {
        AufxClick.PlayOneShot(buttonClip);
    }
    public void BuyOrUpgrade()
    {
        AufxClick.PlayOneShot(buyOrUpgrade);
    }
    public void Victory()
    {
        AufxClick.PlayOneShot(victory);
    }
    public void Defeat()
    {
        AufxClick.PlayOneShot(defeat);
    }
    public void SwordAttack()
    {
        AufxClick.PlayOneShot(swordAttack);
    }
    public void ArrowAttack()
    {
        AufxClick.PlayOneShot(arrowAttack);
    } 
    public void ArrowRainAttack()
    {
        AufxClick.PlayOneShot(arrowRain);
    } 
    public void TypeKeyBoard()
    {
        AufxClick.PlayOneShot(typeKeyBoard);
    }

}
