using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGameSlider : SliderAbstract
{
    protected override void ResetValue()
    {
        base.ResetValue();
        slider.value = DataPlayer.GetMusic();
    }
    protected override void OnSliderValueChanged(float value)
    {
        DataPlayer.SetMusic(value);
    }
}
