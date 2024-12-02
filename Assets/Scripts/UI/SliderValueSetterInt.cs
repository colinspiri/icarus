using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueSetterInt : MonoBehaviour
{
    // components
    public Slider slider;
    public CanvasGroup canvasGroup;
    public IntReference currentValue;
    public IntReference maxValue;
    public IntReference minValue;

    // constants
    public float sliderLerpTime;
    public float disappearTime = -1;
    
    // state
    private int _previousValue = -1;
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
        else if (disappearTime != -1) {
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
        else slider.value = targetValue;
    }

    private void Disappear() {
        canvasGroup.alpha = 0;
    }

    private float GetSliderValue() {
        return Mathf.Clamp01(Mathf.InverseLerp(minValue.Value, maxValue.Value, currentValue.Value));
    }
}