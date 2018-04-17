using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CmdConsole
{
    [RequireComponent(typeof(Console))]
    public class ConsoleAnimator : MonoBehaviour
    {
        public bool visible { get; private set; } = true;
        [SerializeField] private KeyCode showConsole;
        [SerializeField] private KeyCode hideConsole;
        [SerializeField] private float transitionSpeed = 0.15f;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] TMP_InputField inputField;

        private IEnumerator CoTransition;

        private void Update()
        {
            if (Input.GetKeyDown(showConsole))
                SetVisibility(true);
            if (Input.GetKeyDown(hideConsole))
                SetVisibility(false);
        }

        public void SetVisibility(bool show)
        {
            if (show) Transition(transitionSpeed, true);
            else Transition(transitionSpeed, false);
        }
        private void Transition(float timeForEffect, bool setVisibility)
        {
            canvasGroup.interactable = setVisibility;
            if (setVisibility == true) inputField.ActivateInputField();

            if (CoTransition != null) StopCoroutine(CoTransition);
            CoTransition = FadeCanvas(
                setVisibility ? 1 : 0,
                timeForEffect,
                canvasGroup);



            StartCoroutine(CoTransition);
            visible = setVisibility;
            _Input.ConsoleInput = setVisibility;
        }
        private IEnumerator FadeCanvas(float targetAlpha, float time, CanvasGroup canvas)
        {
            float startAlpha = canvas.alpha;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / Time.timeScale / time)
            {
                canvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                yield return null;
            }
            canvas.alpha = targetAlpha;
        }
    }
}
