using System;
using System.Collections.Generic;
using UnityEngine;

public class FlashSprite : MonoBehaviour {
    // constants
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashTime;

    // state
    private List<Color> _spriteRenderersOriginalColors = new List<Color>();
    private bool _flashing;
    private float _flashTimer;

    private void Start() {
        for (int i = 0; i < spriteRenderers.Count; i++) {
            var originalColor = spriteRenderers[i].color;
            _spriteRenderersOriginalColors.Add(originalColor);
        }
    }

    private void Update() {
        if (_flashing) {
            _flashTimer -= Time.deltaTime;
            if (_flashTimer <= 0) {
                _flashing = false;
                EndFlash();
            }
        }
    }

    private void EndFlash() {
        for (int i = 0; i < spriteRenderers.Count; i++) {
            spriteRenderers[i].color = _spriteRenderersOriginalColors[i];
        }
    }

    public void Flash() {
        foreach (var spriteRenderer in spriteRenderers) {
            spriteRenderer.color = flashColor;
        }
        _flashing = true;
        _flashTimer = flashTime;
    }
}