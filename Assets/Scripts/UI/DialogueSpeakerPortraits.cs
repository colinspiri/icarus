using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class DialogueSpeakerPortraits : MonoBehaviour {
    // components
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private Color nonSpeakerColor;
    
    // portrait images
    [Header("Portraits")]
    [SerializeField] private Image silviaLeft;
    [SerializeField] private Image silviaRight;
    [Space]
    [SerializeField] private Image belliniLeft;
    [SerializeField] private Image belliniRight;
    [Space]
    [SerializeField] private Image leLeft;
    [SerializeField] private Image leRight;

    // state
    private string _currentSpeakerText;

    // names
    private readonly List<string> _silviaNames = new() { "Silvia" };
    private readonly List<string> _belliniNames = new() { "Bellini", "Engel" };
    private readonly List<string> _leNames = new() { "Le", "Karina" };

    public enum StagePosition {
        Left,
        Right
    };

    private void Awake() {
        dialogueRunner.AddCommandHandler<string, string>(
            "enter",
            EnterCharacter
        );
        dialogueRunner.AddCommandHandler<string>(
            "exit",
            ExitCharacter
        );
        dialogueRunner.AddCommandHandler<string>(
            "speaker",
            SetSpeaker
        );

        speakerText.text = "";
    }

    // Start is called before the first frame update
    void Start() {
        HideAllPortraits();
        dialogueRunner.onDialogueComplete.AddListener(ResetSpeakerAndPortraits);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueRunner.IsDialogueRunning && speakerText.text != _currentSpeakerText) {
            SetSpeaker(speakerText.text);
            _currentSpeakerText = speakerText.text;
        }
    }

    private void ResetSpeakerAndPortraits() {
        speakerText.text = "";
        _currentSpeakerText = "";
        HideAllPortraits();
    }

    private void HideAllPortraits() {
        silviaLeft.gameObject.SetActive(false);
        silviaRight.gameObject.SetActive(false);
        belliniLeft.gameObject.SetActive(false);
        belliniRight.gameObject.SetActive(false);
        leLeft.gameObject.SetActive(false);
        leRight.gameObject.SetActive(false);
    }

    private void EnterCharacter(string characterName, string positionName) {
        Character character = GetCharacterFromString(characterName);
        StagePosition position = GetStagePositionFromString(positionName);
        EnterCharacter(character, position);
    }
    private void EnterCharacter(Character character, StagePosition position) {
        Image portraitToEnter = character switch {
            Character.Silvia when position == StagePosition.Left => silviaLeft,
            Character.Silvia when position == StagePosition.Right => silviaRight,
            Character.Bellini when position == StagePosition.Left => belliniLeft,
            Character.Bellini when position == StagePosition.Right => belliniRight,
            Character.Le when position == StagePosition.Left => leLeft,
            Character.Le when position == StagePosition.Right => leRight,
            _ => throw new ArgumentOutOfRangeException(nameof(character), character, null)
        };
        portraitToEnter.gameObject.SetActive(true);
        portraitToEnter.color = nonSpeakerColor;
    }

    private void ExitCharacter(string characterName) {
        Character character = GetCharacterFromString(characterName);
        ExitCharacter(character);
    }
    private void ExitCharacter(Character character) {
        if (character == Character.Silvia) {
            silviaLeft.gameObject.SetActive(false);
            silviaRight.gameObject.SetActive(false);
        }
        else if (character == Character.Bellini) {
            belliniLeft.gameObject.SetActive(false);
            belliniRight.gameObject.SetActive(false);
        }
        else if (character == Character.Le) {
            leLeft.gameObject.SetActive(false);
            leRight.gameObject.SetActive(false);
        }
    }

    private void SetSpeaker(string characterName) {
        Character character = GetCharacterFromString(characterName);
        SetSpeaker(character); 
    }
    private void SetSpeaker(Character character) {
        silviaLeft.color = nonSpeakerColor;
        silviaRight.color = nonSpeakerColor;
        belliniLeft.color = nonSpeakerColor;
        belliniRight.color = nonSpeakerColor;
        leLeft.color = nonSpeakerColor;
        leRight.color = nonSpeakerColor;

        Color speakerColor = Color.white;
        if (character == Character.Silvia) {
            silviaLeft.color = speakerColor;
            silviaRight.color = speakerColor;
        }
        else if (character == Character.Bellini) {
            belliniLeft.color = speakerColor;
            belliniRight.color = speakerColor;
        }
        else if (character == Character.Le) {
            leLeft.color = speakerColor;
            leRight.color = speakerColor;
        }
    }

    private Character GetCharacterFromString(string characterName) {
        if (_silviaNames.Any(characterName.Contains)) {
            return Character.Silvia;
        }
        if (_belliniNames.Any(characterName.Contains)) {
            return Character.Bellini;
        }
        if (_leNames.Any(characterName.Contains)) {
            return Character.Le;
        }

        Debug.LogError("No character name found in speaker '" + characterName + "'");
        return Character.Silvia;
    }

    private StagePosition GetStagePositionFromString(string positionName) {
        if (positionName.ToLower().Contains("left")) {
            return StagePosition.Left;
        }
        if (positionName.ToLower().Contains("right")) {
            return StagePosition.Right;
        }
        
        Debug.LogError("No valid stage position found in string '" + positionName + "'");
        return StagePosition.Left;
    }
}

public enum Character { Silvia, Bellini, Le }
