using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinText : TextAbstract
{
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
