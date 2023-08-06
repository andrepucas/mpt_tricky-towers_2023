using UnityEngine;
using System.Collections;

/// <summary>
/// Handles opening and closing behaviours of UI panels.
/// </summary>
public abstract class UIPanelAbstract : MonoBehaviour
{
    // V A R I A B L E S

    [Tooltip("Canvas group of this panel.")]
    [SerializeField] private CanvasGroup _canvasGroup;

    private float _elapsedTime;

    // M E T H O D S

    protected void Open(float p_transition)
    {
        StopAllCoroutines();

        // If transition time is 0.
        if (p_transition == 0)
        {
            // Directly reveals and enables everything in canvas group.
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        // Otherwise, reveal it over time.
        else StartCoroutine(RevealOverTime(p_transition));
    }

    protected void Close(float p_transition)
    {
        StopAllCoroutines();

        // Stops blocking raycasts right away.
        _canvasGroup.blocksRaycasts = false;

        // If transition time is 0.
        if (p_transition == 0)
        {
            // Directly hides and disables everything in canvas group.
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
        }

        // Otherwise, hides and disables over time.
        else StartCoroutine(HideOverTime(p_transition));
    }

    //  C O R O U T I N E S

    private IEnumerator RevealOverTime(float p_transition)
    {
        _elapsedTime = 0;

        // While the canvas isn't fully revealed.
        while (_canvasGroup.alpha < 1)
        {
            // Lerps the canvas alpha value from 0 to 1.
            _canvasGroup.alpha = Mathf.Lerp(0, 1, _elapsedTime/p_transition);

            // Updates elapsed time based on unscaled delta time.
            _elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        // Fully reveals and enables canvas group elements.
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    private IEnumerator HideOverTime(float p_transition)
    {
        // Sets elapsed time to 0.
        float m_elapsedTime = 0;

        // While the canvas isn't fully hidden.
        while (_canvasGroup.alpha > 0)
        {
            // Lerps the canvas alpha value from 1 to 0.
            _canvasGroup.alpha = Mathf.Lerp(1, 0, m_elapsedTime/p_transition);

            // Updates elapsed time based on unscaled delta time.
            m_elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        // Fully hides and disables canvas group elements.
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
    }
}
