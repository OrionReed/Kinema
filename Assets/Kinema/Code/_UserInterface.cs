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
    private Player_NodeSelection selection;
    private Player_Movement movement;
    private _TimeControl timeControl;

    [SerializeField]
    private Text textSelectionMode;
    [SerializeField]
    private Text textForceMode;
    [SerializeField]
    private Text textCameraMode;
    [SerializeField]
    private Text textTimescale;
    [SerializeField]
    private Color uiColor = Color.black;
    [SerializeField]
    private float solidTime = 1;
    [SerializeField]
    private float fadeTime = 1;

    IEnumerator co1;
    IEnumerator co2;
    IEnumerator co3;
    IEnumerator co4;

    private void Awake()
    {
        textCameraMode.text = "";
        textForceMode.text = "";
        textSelectionMode.text = "";
        textTimescale.text = "";

        textCameraMode.color = Color.clear;
        textSelectionMode.color = Color.clear;
        textForceMode.color = Color.clear;
        textTimescale.color = Color.clear;

        co1 = ShowText(textCameraMode, "", "");
        co2 = ShowText(textForceMode, "", "");
        co3 = ShowText(textSelectionMode, "", "");
        co4 = ShowText(textTimescale, "", "");

        StartCoroutine(co1);
        StartCoroutine(co2);
        StartCoroutine(co3);
        StartCoroutine(co4);
    }

    private void Start()
    {
        cam = FindObjectOfType<_Camera>();
        cam.OnModeUpdate += DisplayCameraState;
        selection = FindObjectOfType<Player_NodeSelection>();
        selection.OnModeUpdate += DisplaySelectionState;
        movement = FindObjectOfType<Player_Movement>();
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
        co1 = ShowText(textCameraMode, "CAMERA: ", cam.CameraMode.ToString());
        StartCoroutine(co1);
    }
    private void DisplayForceState()
    {
        StopCoroutine(co2);
        co2 = ShowText(textForceMode, "FORCE: ", movement.ForceMode.ToString());
        StartCoroutine(co2);
    }
    private void DisplaySelectionState()
    {
        StopCoroutine(co3);
        co3 = ShowText(textSelectionMode, "SELECTION: ", selection.SelectionMode.ToString());
        StartCoroutine(co3);
    }
    private void DisplayTimeScale()
    {
        StopCoroutine(co4);
        co4 = ShowText(textTimescale, "TIME: 1 / ", timeControl.TimeFraction.ToString());
        StartCoroutine(co4);
    }
    private IEnumerator ShowText(Text uiText, string textPrefix, string text)
    {
        uiText.text = textPrefix + text;
        float solidElapsed = 0;
        float fadeElapseded = 0;
        uiText.color = uiColor;
        Color endColor = uiText.color;
        endColor.a = 0;

        while (solidElapsed < solidTime)
        {
            yield return null;
            solidElapsed += Time.deltaTime / Time.timeScale;
        }
        while (fadeElapseded < fadeTime)
        {
            uiText.color = Color.Lerp(uiColor, endColor, fadeElapseded / fadeTime);
            fadeElapseded += Time.deltaTime / Time.timeScale;
            yield return null;
        }

        uiText.color = endColor;
    }
}