using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour {
    // components
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    
    // state
    private List<Color> _spriteRenderersDefaultColors = new List<Color>();
    private float _tempColorTimer;
    private Color _tempColor;
    
    private float _flashTimer;
    private Color _flashColor;
    private float _flashPeriod;

    protected virtual void Start() {
        for (int i = 0; i < spriteRenderers.Count; i++) {
            var defaultColor = spriteRenderers[i].color;
            _spriteRenderersDefaultColors.Add(defaultColor);
        }
    }

    protected virtual void Update() {
        // count down timers
        if (_tempColorTimer > 0) {
            _tempColorTimer -= Time.deltaTime;
        }
        if (_flashTimer > 0) {
            _flashTimer -= Time.deltaTime;
        }
        
        // choose color 
        if (_tempColorTimer > 0 || _tempColorTimer == -1) {
            SetColor(_tempColor);
        }
        else if (_flashTimer > 0 || _flashTimer == -1) {
            float t = Mathf.Repeat(Time.time, _flashPeriod);
            if (t < 0.5f * _flashPeriod) {
                SetDefaultColors();
            }
            else {
                SetColor(_flashColor);
            }
        }
        else {
            SetDefaultColors();
        }
    }

    private void SetDefaultColors() {
        for (var i = 0; i < spriteRenderers.Count; i++) {
            spriteRenderers[i].color = _spriteRenderersDefaultColors[i];
        }
    }

    private void SetColor(Color color) {
        foreach (var spriteRenderer in spriteRenderers) {
            spriteRenderer.color = color;
        }
    }

    protected void SetTemporaryColor(Color temporaryColor, float time) {
        SetColor(temporaryColor);
        _tempColor = temporaryColor;
        _tempColorTimer = time;
    }
    protected void StopTemporaryColor() {
        _tempColorTimer = 0;
    }

    protected void SetFlashing(Color flashColor, float period, float time) {
        _flashColor = flashColor;
        _flashPeriod = period;
        _flashTimer = time;
    }
    protected void StopFlashing() {
        _flashTimer = 0;
    }
}