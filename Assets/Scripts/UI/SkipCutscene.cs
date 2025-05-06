using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Yarn.Unity;

public class SkipCutscene : MonoBehaviour
{
    // components
    private PlayerInputActions _inputActions;
    [SerializeField] private Slider slider;
    private DialogueRunner _dialogueRunner;

    // constants
    public CanvasGroup skipPrompt;
    [SerializeField] private bool onlyShowForProgress;
    [SerializeField] private float skipPromptAlpha;
    [SerializeField] private float skipTime;
    [Header("On Skip")]
    [SerializeField] private bool useEvent;
    public UnityEvent nextEvent;
    [SerializeField] private bool useScene;
    [SerializeField] private SceneReference nextScene;
    public bool useYarnCallback;
    
    // state
    private bool _showSkipPrompt;
    private float _skipTimer;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        if (onlyShowForProgress)
        {
            skipPrompt.alpha = 0f;
        }
        else
        {
            _showSkipPrompt = true;
        }
        _skipTimer = 0f;
        _inputActions.Enable();
    }
    private void OnDisable()
    {
        _showSkipPrompt = false;
        _skipTimer = 0f;
        skipPrompt.alpha = 0f;
        UpdateSlider();
        _inputActions.Disable();
    }

    private void Start()
    {
        skipPrompt.gameObject.SetActive(false);
        
        _dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    private void Update()
    {
        if (_skipTimer >= skipTime) return;
        
        if (_showSkipPrompt)
        {
            skipPrompt.gameObject.SetActive(true);
            skipPrompt.alpha = skipPromptAlpha;
        }
        
        if (_inputActions.UI.SkipDialogue.ReadValue<float>() > 0)
        {
            _showSkipPrompt = true;
            skipPrompt.alpha = 1f;
            _skipTimer += Time.deltaTime;
            
            if (_skipTimer >= skipTime)
            {
                Skip();
            }
        }
        else
        {
            _skipTimer = 0f;
            _showSkipPrompt = false;
            if (onlyShowForProgress)
            {
                skipPrompt.alpha = 0f;
            }
        }

        if (slider)
        {
            UpdateSlider();
        }
    }

    private void Skip()
    {
        if (useEvent)
        {
            nextEvent.Invoke();
        }
        else if(useScene)
        {
            SceneManager.LoadScene(nextScene);
        }
        if (useYarnCallback)
        {
            _dialogueRunner.onNodeComplete.Invoke(_dialogueRunner.CurrentNodeName);
            _dialogueRunner.Stop();
        }
    }

    private void UpdateSlider()
    {
        slider.value = _skipTimer / skipTime;
    }
}
