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
    public GameObject ListKitchenRecipe;
    public GameObject KitchenRecipe;
    public GameObject PlantType;
    public GameObject Kitchen;
    public GameObject Delivery;
    public GameObject Mission;
    protected override void Awake()
    {
        MakeSingleton(false);
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
    public void OpenListKitchenRecipe()
    {
        ListKitchenRecipe.SetActive(true);
        SoundManager.Ins.ButtonSound();
    } 
    public void CloseListKitchenRecipe()
    {
        ListKitchenRecipe.SetActive(false);
        SoundManager.Ins.ButtonSound();
    }
    public void OpenKitchenRecipe()
    {
        KitchenRecipe.SetActive(true);
        SoundManager.Ins.ButtonSound();
    } 
    public void CloseKitchenRecipe()
    {
        KitchenRecipe.SetActive(false);
        SoundManager.Ins.ButtonSound();
    }
    public void OpenKitchen()
    {
        Kitchen.SetActive(true);
        SoundManager.Ins.ButtonSound();
    } 
    public void CloseKitchen()
    {
        Kitchen.SetActive(false);
        SoundManager.Ins.ButtonSound();
    } 
    public void OpenDelivery()
    {
        Delivery.SetActive(true);
        SoundManager.Ins.ButtonSound();
    } 
    public void CloseDelivery()
    {
        Delivery.SetActive(false);
        SoundManager.Ins.ButtonSound();
    } 
    public void OpenMission()
    {
        Mission.SetActive(true);
        SoundManager.Ins.ButtonSound();
    } 
    public void CloseMission()
    {
        Mission.SetActive(false);
        SoundManager.Ins.ButtonSound();
    } 
    public void OpenPlantType()
    {
        PlantType.SetActive(true);
        SoundManager.Ins.ButtonSound();
    } 
    public void ClosePlantType()
    {
        PlantType.SetActive(false);
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

