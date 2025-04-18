using UnityEngine;

[CreateAssetMenu(fileName = "UIConstants", menuName = "Scriptable Objects/UIConstants")]
public class UIConstants : ScriptableObject
{
    [Header("Menu Option Animation")] 
    public float scaleOnSelect;
    public float selectTime;
    public Color selectedColor;
    public float selectedFontSize;

    [Header("Menu Screen Animation")] 
    public float offscreenDistance;
    public float menuScreenTransitionTime;
    
    // TODO: add colors here but make it an editor function so that the UI designer can edit colors just from the SO and the UI elements automatically update on editor refresh
}