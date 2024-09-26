using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class CameraZoom : MonoBehaviour {
    [SerializeField] private Camera cameraComponent;
    [SerializeField] private FloatReference heat;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float lerpSpeed;

    // Update is called once per frame
    void Update() {
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, heat.Value);
        cameraComponent.orthographicSize = Mathf.Lerp(cameraComponent.orthographicSize, targetZoom, lerpSpeed * Time.deltaTime);
    }
}
