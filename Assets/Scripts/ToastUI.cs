using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ToastUI : MonoBehaviour {
    public static ToastUI Instance;
    
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI toastText;

    [SerializeField] private float fadeInTime; // 0.2
    [SerializeField] private float remainTime; // 1
    [SerializeField] private float fadeOutTime; // 0.5

    private bool _toastPlaying;
    private List<string> _toastQueue = new List<string>();

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        HideToastUI();
    }

    private void HideToastUI() {
        canvasGroup.alpha = 0;
        toastText.text = "";
    }

    public void QueueToast(string text) {
        if (_toastPlaying) {
            _toastQueue.Add(text);
        }
        else {
            StartCoroutine(ToastCoroutine(text));
        }
    }

    private void PopToast() {
        if (_toastQueue.Count == 0) return;
        
        string poppedToast = _toastQueue[0];
        _toastQueue.RemoveAt(0);

        StartCoroutine(ToastCoroutine(poppedToast));
    }

    private IEnumerator ToastCoroutine(string text) {
        _toastPlaying = true;

        canvasGroup.alpha = 0;
        toastText.text = text;
        
        // fade in
        Tween fadeInTween = canvasGroup.DOFade(1, fadeInTime);
        yield return fadeInTween.WaitForCompletion();
        
        // remain
        yield return new WaitForSeconds(remainTime);

        // fade out
        Tween fadeOutTween = canvasGroup.DOFade(0, fadeOutTime);
        yield return fadeOutTween.WaitForCompletion();

        _toastPlaying = false;
        
        // play next toast
        if(_toastQueue.Count > 0) PopToast();
    }
}