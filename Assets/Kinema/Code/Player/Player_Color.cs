using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Player_Color : MonoBehaviour
{
    [SerializeField]
    private Color colorNone = Color.white;
    [SerializeField]
    private Color colorRoot = Color.white;
    [SerializeField]
    private Color colorChild = Color.white;
    [SerializeField]
    private Color colorRootFollow = Color.white;
    [SerializeField]
    private Color colorChildFollow = Color.white;
    [SerializeField]
    private Color colorHurt = Color.red;
    [SerializeField]
    private float colorLerpTime = 0.1f;
    [SerializeField]
    private float pulseFadeIn = 0.05f;
    [SerializeField]
    private float pulseWait = 0.1f;
    [SerializeField]
    private float pulseFadeOut = 0.9f;
    [SerializeField]
    private Material playerMaterial;

    private Character playerCharacter;
    private Player_NodeSelection selection;

    private void Start()
    {
        _LevelState.OnPlay += UpdateSelectionColor;
        GetComponent<Player_Health>().OnNodeDamaged += UpdateHealthColor;
        GetComponent<Player_NodeSelection>().OnNodeSelection += UpdateSelectionColor;
        selection = FindObjectOfType<Player_NodeSelection>();
        playerCharacter = FindObjectOfType<Player_Character>().PlayerCharacter;
        playerMaterial.EnableKeyword("_EMISSION");
    }

    private void UpdateHealthColor(CharacterNode node)
    {
        IEnumerator coroutine = PulseToColorHDR(
            pulseFadeIn, pulseWait, pulseFadeOut,
            colorHurt,
            2,
            node.Renderer.material);
        StartCoroutine(coroutine);
    }

    private void UpdateSelectionColor()
    {
        Color selectionColor = Color.magenta;
        foreach (TreeNode<CharacterNode> node in playerCharacter.CharacterTree.NodeList)
        {
            selectionColor = colorNone;
            IEnumerator coroutine = LerpToColor(colorLerpTime, selectionColor, node.Data.Renderer.material);
            StartCoroutine(coroutine);
        }

        for (int i = 0; i < selection.ChainSelected.Count; i++)
        {
            IEnumerator coroutine = LerpToColor(
                colorLerpTime,
                selectionColor = i == 0 ? colorRoot : colorChild,
                selection.ChainSelected[i].Data.Renderer.material);
            StartCoroutine(coroutine);
        }
        for (int i = 0; i < selection.ChainFollow.Count; i++)
        {
            IEnumerator coroutine = LerpToColor(
                colorLerpTime,
                selectionColor = i == 0 ? colorRootFollow : colorChildFollow,
                selection.ChainFollow[i].Data.Renderer.material);
            StartCoroutine(coroutine);
        }
        for (int i = 0; i < selection.ChainMirror.Count; i++)
        {
            IEnumerator coroutine = LerpToColor(
                colorLerpTime,
                selectionColor = i == 0 ? colorRoot : colorChild,
                selection.ChainMirror[i].Data.Renderer.material);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator LerpToColor(float timeForEffect, Color endColor, Material materialInstance)
    {
        Color startColor = materialInstance.color;
        float elapsed = 0f;

        while (elapsed < timeForEffect)
        {
            materialInstance.color = Color.Lerp(startColor, endColor, elapsed / timeForEffect);
            elapsed += Time.deltaTime / Time.timeScale;
            yield return null;
        }

        materialInstance.color = endColor;
    }
    private IEnumerator PulseToColorHDR(float fadeIn, float wait, float fadeOut, Color pulseToColor, float intensity, Material materialInstance)
    {
        Color startColor = materialInstance.color;
        Vector4 HDRColor = pulseToColor * 2;
        float stageElapsed = 0;

        while (stageElapsed < fadeIn)
        {
            materialInstance.SetColor("_EmissionColor", Color.Lerp(startColor, HDRColor, stageElapsed / fadeIn));
            stageElapsed += Time.deltaTime / Time.timeScale;
            yield return null;
        }
        stageElapsed = 0;
        yield return new WaitForSeconds(wait);
        while (stageElapsed < fadeIn)
        {
            materialInstance.SetColor("_EmissionColor", Color.Lerp(HDRColor, startColor, stageElapsed / fadeIn));
            stageElapsed += Time.deltaTime / Time.timeScale;
            yield return null;
        }
    }
}
