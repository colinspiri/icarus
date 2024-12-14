using UnityEngine;

public class HUDManager : MonoBehaviour {
    public static HUDManager Instance;
    private PlayerInputActions _playerInputActions;
    
    // components
    public CanvasGroup canvasGroup;

    // state
    public bool HUDEnabled { get; private set; }

    private void Awake() {
        Instance = this;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
    }

    // Start is called before the first frame update
    void Start() {
        SetHUDEnabled(true);
    }

    public void SetHUDEnabled(bool value) {
        HUDEnabled = value;

        canvasGroup.alpha = HUDEnabled ? 1 : 0;
    }
}