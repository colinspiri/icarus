using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class ScrollingTexture : MonoBehaviour {
    [SerializeField] private RawImage rawImage;
    [Space]
    [SerializeField] private bool useHeatForSpeed;
    [SerializeField] private FloatReference heat;
    [SerializeField] private float minHeatSpeed;
    [SerializeField] private float maxHeatSpeed;
    [Space]    
    [SerializeField] private float defaultScrollSpeed;

    // Update is called once per frame
    void Update() {
        float speed = defaultScrollSpeed;
        
        if (useHeatForSpeed) {
            speed = Mathf.Lerp(minHeatSpeed, maxHeatSpeed, heat.Value);
        }
        
        Vector2 position = rawImage.uvRect.position + new Vector2(0, speed * Time.deltaTime);
        rawImage.uvRect = new Rect(position, rawImage.uvRect.size);
    }
}