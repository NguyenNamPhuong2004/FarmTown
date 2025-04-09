using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : Singleton<GameUIManager>
{
    public GameObject Shop;
    public GameObject Setting;
    public GameObject Inventory;
    public GameObject Grinder;
    public GameObject ListGrinderRecipe;
    public GameObject GrinderRecipe;
    public GameObject PlantType;
    public GameObject Ads;
    public GameObject Level;
    protected override void Awake()
    {
        MakeSingleton(false);
    }
    public void OpenLevel()
    {
        if (Level.activeSelf) CloseLevel();
        else Level.SetActive(true);
        SoundManager.Ins.ButtonSound();
    }
    public void CloseLevel()
    {
        Level.SetActive(false);
    }
    public void OpenShop()
    {
        Shop.SetActive(true);
        SoundManager.Ins.ButtonSound();
    }
    public void CloseShop()
    {
        Shop.SetActive(false);
        SoundManager.Ins.ButtonSound();
    }
    public void OpenInventory()
    {
        Inventory.SetActive(true);
        SoundManager.Ins.ButtonSound();
    }
    public void CloseInventory()
    {
        Inventory.SetActive(false);
        SoundManager.Ins.ButtonSound();
    }
    public void OpenSetting()
    {
        Setting.SetActive(true);
        SoundManager.Ins.ButtonSound();
    }
    public void CloseSetting()
    {
        Setting.SetActive(false);
        SoundManager.Ins.ButtonSound();
    }
    public void OpenGrinder()
    {
        Grinder.SetActive(true);
        SoundManager.Ins.ButtonSound();
    }
    public void CloseGrinder()
    {
        GrinderRecipe.SetActive(false);
        ListGrinderRecipe.SetActive(false);
        Grinder.SetActive(false);
        SoundManager.Ins.ButtonSound();
    }
    public void OpenListGrinderRecipe()
    {
        ListGrinderRecipe.SetActive(true);
        SoundManager.Ins.ButtonSound();
    } 
    public void CloseListGrinderRecipe()
    {
        ListGrinderRecipe.SetActive(false);
        SoundManager.Ins.ButtonSound();
    }
    public void OpenGrinderRecipe()
    {
        GrinderRecipe.SetActive(true);
        SoundManager.Ins.ButtonSound();
    } 
    public void CloseGrinderRecipe()
    {
        GrinderRecipe.SetActive(false);
        SoundManager.Ins.ButtonSound();
    } 
    public void OpenPlantType()
    {
        PlantType.SetActive(true);
      //  SoundManager.Ins.ButtonSound();
    } 
    public void ClosePlantType()
    {
        PlantType.SetActive(false);
        SoundManager.Ins.ButtonSound();
    }
    public void OpenAds()
    {
        Ads.SetActive(true);
        SoundManager.Ins.ButtonSound();
    }
    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

