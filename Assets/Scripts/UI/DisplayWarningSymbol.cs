using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayWarningSymbol : MonoBehaviour
{
    [SerializeField] private BoolVariable isReadyToSpawnStaticObject;
    [SerializeField] private float displayTime = 2f;
    [SerializeField] private Image warningSymbol;
    [SerializeField] private FloatGameEvent displayWarningSymbol;

    private void OnEnable()
    {
        displayWarningSymbol.AddListener(DisplayWarning);
    }

    private void OnDisable()
    {
        displayWarningSymbol.RemoveListener(DisplayWarning);
    }
    private void Start()
    {
        isReadyToSpawnStaticObject.Value = false;
        warningSymbol.enabled = false;
    }

    public void DisplayWarning(float verticalPos)
    {
        StartCoroutine(DisplayWarningCoroutine(verticalPos));
    }

    private IEnumerator DisplayWarningCoroutine(float verticalPos)
    {
        Vector3 worldPos = new Vector3(0, verticalPos, 0);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        RectTransform canvasRect = warningSymbol.canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out Vector2 uiPos);

        Vector2 newAnchoredPos = warningSymbol.rectTransform.anchoredPosition;
        newAnchoredPos.y = uiPos.y;
        warningSymbol.rectTransform.anchoredPosition = newAnchoredPos;
        
        warningSymbol.enabled = true;
        yield return new WaitForSeconds(displayTime);
        warningSymbol.enabled = false;
        isReadyToSpawnStaticObject.Value = true;
    }
}
