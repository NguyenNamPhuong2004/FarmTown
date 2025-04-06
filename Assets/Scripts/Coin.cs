using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    [SerializeField] private Text text;
    private void Start()
    {
        UpdateView();
    }
    private void UpdateView()
    {
        text.text = DataPlayer.GetCoin().ToString();
    }
    private void OnEnable()
    {
        DataPlayer.AddListenerUpdateCoinEvent(UpdateView);
    }
    private void OnDisable()
    {
        DataPlayer.RemoveListenerUpdateCoinEvent(UpdateView);
    }
}
