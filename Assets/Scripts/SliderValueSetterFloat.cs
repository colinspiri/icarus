using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueSetterFloat : MonoBehaviour
{
    // components
    public Slider slider;
    public CanvasGroup canvasGroup;
    public FloatReference currentValue;
    public FloatReference maxValue;
    public FloatReference minValue;

    // constants
    // public float sliderLerpTime;
    
    // state
    private float previousValue = -1;

    private void OnEnable() {
        slider.value = GetSliderValue();
    }

    private void Update() {
        if (previousValue != currentValue.Value) {
            LerpSliderValue();
            previousValue = currentValue.Value;
        }
        else Disappear();
    }

    private void LerpSliderValue() {
        canvasGroup.alpha = 1;
        
        float targetValue = GetSliderValue();
        // if (sliderLerpTime > 0) slider.DOValue(targetValue, sliderLerpTime);
        slider.value = targetValue;
    }

    private void Disappear() {
        canvasGroup.alpha = 0;
    }

    private float GetSliderValue() {
        return Mathf.Clamp01(Mathf.InverseLerp(minValue.Value, maxValue.Value, currentValue.Value));
    }
}