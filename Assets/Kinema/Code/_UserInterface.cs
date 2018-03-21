using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

/// Displays the user interface.
public class _UserInterface : MonoBehaviour
{
    private _Camera cam;
    private K_Selection selection;
    private K_Movement movement;
    private _TimeControl timeControl;

    [SerializeField]
    private Text SelectionModeText;
    [SerializeField]
    private Text ForceModeText;
    [SerializeField]
    private Text CameraModeText;
    [SerializeField]
    private Text TimeScaleText;
    [SerializeField]
    private Color TextColor = Color.black;
    [SerializeField]
    private float TextSolidTime = 1;
    [SerializeField]
    private float TextFadeTime = 1;

    IEnumerator co1;
    IEnumerator co2;
    IEnumerator co3;
    IEnumerator co4;

    private void Awake()
    {
        CameraModeText.text = "";
        ForceModeText.text = "";
        SelectionModeText.text = "";
        TimeScaleText.text = "";

        CameraModeText.color = Color.clear;
        SelectionModeText.color = Color.clear;
        ForceModeText.color = Color.clear;
        TimeScaleText.color = Color.clear;

        co1 = ShowText(CameraModeText, "", "");
        co2 = ShowText(ForceModeText, "", "");
        co3 = ShowText(SelectionModeText, "", "");
        co4 = ShowText(TimeScaleText, "", "");

        StartCoroutine(co1);
        StartCoroutine(co2);
        StartCoroutine(co3);
        StartCoroutine(co4);
    }

    private void Start()
    {
        cam = FindObjectOfType<_Camera>();
        cam.OnModeUpdate += DisplayCameraState;
        selection = FindObjectOfType<K_Selection>();
        selection.OnModeUpdate += DisplaySelectionState;
        movement = FindObjectOfType<K_Movement>();
        movement.OnModeUpdate += DisplayForceState;
        timeControl = FindObjectOfType<_TimeControl>();
        timeControl.OnSpeedChange += DisplayTimeScale;
    }

    private void OnDisable()
    {
        if (cam != null)
            cam.OnModeUpdate -= DisplayCameraState;
        if (selection != null)
            selection.OnModeUpdate -= DisplaySelectionState;
        if (movement != null)
            movement.OnModeUpdate -= DisplayForceState;
        if (timeControl != null)
            timeControl.OnSpeedChange -= DisplayTimeScale;
    }

    private void DisplayCameraState()
    {
        StopCoroutine(co1);
        co1 = ShowText(CameraModeText, "CAMERA: ", cam.cameraMode.ToString());
        StartCoroutine(co1);
    }
    private void DisplayForceState()
    {
        StopCoroutine(co2);
        co2 = ShowText(ForceModeText, "FORCE: ", movement.forceMode.ToString());
        StartCoroutine(co2);
    }
    private void DisplaySelectionState()
    {
        StopCoroutine(co3);
        co3 = ShowText(SelectionModeText, "SELECTION: ", selection.selectionMode.ToString());
        StartCoroutine(co3);
    }
    private void DisplayTimeScale()
    {
        StopCoroutine(co4);
        co4 = ShowText(TimeScaleText, "TIME: 1 / ", timeControl.timeFraction.ToString());
        StartCoroutine(co4);
    }
    private IEnumerator ShowText(Text uiText, string textPrefix, string text)
    {
        uiText.text = textPrefix + text;
        float solidElapsed = 0;
        float fadeElapseded = 0;
        uiText.color = TextColor;
        Color endColor = uiText.color;
        endColor.a = 0;

        while (solidElapsed < TextSolidTime)
        {
            yield return null;
            solidElapsed += Time.deltaTime / Time.timeScale;
        }
        while (fadeElapseded < TextFadeTime)
        {
            uiText.color = Color.Lerp(TextColor, endColor, fadeElapseded / TextFadeTime);
            fadeElapseded += Time.deltaTime / Time.timeScale;
            yield return null;
        }

        uiText.color = endColor;
    }
}