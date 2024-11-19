using DG.Tweening;
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
    public float sliderLerpTime;
    public float disappearTime;
    
    // state
    private float _previousValue = -1;
    private float _disappearTimer;

    private void OnEnable() {
        slider.value = GetSliderValue();
    }

    private void Update() {
        if (_previousValue != currentValue.Value) {
            LerpSliderValue();
            _previousValue = currentValue.Value;
            _disappearTimer = 0;
        }
        else if (currentValue.Value == minValue.Value) {
            if (_disappearTimer > disappearTime) {
                Disappear();
            }
            else {
                _disappearTimer += Time.deltaTime;
            }
        }
    }

    private void LerpSliderValue() {
        canvasGroup.alpha = 1;
        
        float targetValue = GetSliderValue();
        if (sliderLerpTime > 0) slider.DOValue(targetValue, sliderLerpTime);
        slider.value = targetValue;
    }

    private void Disappear() {
        canvasGroup.alpha = 0;
    }

    private float GetSliderValue() {
        return Mathf.Clamp01(Mathf.InverseLerp(minValue.Value, maxValue.Value, currentValue.Value));
    }
}