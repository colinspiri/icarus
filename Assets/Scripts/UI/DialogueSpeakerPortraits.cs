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

    // speakers 
    [SerializeField] private Color speakerColor = Color.white;
    [SerializeField] private List<Character> possibleSpeakers;

    // state
    private string _currentSpeakerText;

    private enum StagePosition { Left, Right };

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
        
        HideAllPortraits();
        dialogueRunner.onDialogueComplete.AddListener(ResetSpeakerAndPortraits);
    }

    // Start is called before the first frame update
    void Start() {
        
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
        foreach (Character speaker in possibleSpeakers) {
            speaker.portraitLeft.gameObject.SetActive(false);
            speaker.portraitRight.gameObject.SetActive(false);
        }
    }

    private void EnterCharacter(string characterName, string positionName) {
        var character = GetCharacterFromNameString(characterName);
        StagePosition position = GetStagePositionFromString(positionName);

        if (character == null) return;
        Character validCharacter = (Character)character;
        
        Image portraitToEnter = position == StagePosition.Left ? validCharacter.portraitLeft : validCharacter.portraitRight;
        portraitToEnter.gameObject.SetActive(true);
        portraitToEnter.color = nonSpeakerColor;
    }

    private void ExitCharacter(string characterName) {
        var character = GetCharacterFromNameString(characterName);
        
        if (character == null) return;
        Character validCharacter = (Character)character;
        
        foreach (var possibleSpeaker in possibleSpeakers) {
            if (possibleSpeaker.Equals(validCharacter)) {
                validCharacter.portraitLeft.gameObject.SetActive(false);
                validCharacter.portraitRight.gameObject.SetActive(false);
            }
        }
    }

    private void SetSpeaker(string characterName) {
        var speaker = GetCharacterFromNameString(characterName);
        
        foreach (var possibleSpeaker in possibleSpeakers) {
            possibleSpeaker.portraitLeft.color = nonSpeakerColor;
            possibleSpeaker.portraitRight.color = nonSpeakerColor;
        }
        
        if (speaker != null) {
            Character validSpeaker = (Character)speaker;
            validSpeaker.portraitLeft.color = speakerColor;
            validSpeaker.portraitRight.color = speakerColor;
        }
    }

    private Character? GetCharacterFromNameString(string characterName) {
        foreach (var possibleSpeaker in possibleSpeakers) {
            if (possibleSpeaker.validNames.Any(characterName.Contains)) {
                return possibleSpeaker;
            }
        }

        Debug.Log("No character name found in '" + characterName + "'");
        return null;
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

[Serializable]
public struct Character {
    public List<string> validNames;
    public Image portraitLeft;
    public Image portraitRight;
}
