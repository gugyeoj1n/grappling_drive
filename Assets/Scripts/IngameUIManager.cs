using UnityEngine;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    public static IngameUIManager instance;

    public Slider boostGaugeSlider;

    void Awake( )
    {
        instance = this;
    }

    public void SetBoostGauge( float value )
    {
        boostGaugeSlider.value = value;
    }
}