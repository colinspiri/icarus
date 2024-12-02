using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;

public class TextDisplayInt : MonoBehaviour {
    public TextMeshProUGUI text;
    public IntReference intReference;
    public string stringBeforeInt;
    public string stringAfterInt;
    
    // state
    private int _previousValue = -1;

    private void Start() {
        UpdateText();
    }

    private void Update() {
        if (_previousValue != intReference.Value) {
            UpdateText();
        }
    }

    private void UpdateText() {
        text.text = stringBeforeInt + (intReference.Value) + stringAfterInt;
        _previousValue = intReference.Value;
    }
}