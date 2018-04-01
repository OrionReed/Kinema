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

    private Player_NodeSelection selection;

    private void Awake()
    {
        selection = FindObjectOfType<Player_NodeSelection>();
        GetComponent<Player_NodeSelection>().OnSelectionUpdate += UpdateSelectionColor;
        GetComponent<Player_Health>().OnNodeDamaged += UpdateHealthColor;
        _LevelState.OnPlay += UpdateSelectionColor;
    }

    private void UpdateHealthColor(CharacterNode node)
    {
        IEnumerator coroutine = LerpToColor(
            colorLerpTime,
            Color.Lerp(colorNone, colorHurt, node.Damage),
            node.Renderer.material);
        StartCoroutine(coroutine);
    }

    private void UpdateSelectionColor()
    {
        Color selectionColor = Color.magenta;
        for (int i = 0; i < selection.List.Count; i++)
        {
            if (selection.List[i].Data.Selected == false)
            {
                selectionColor = colorNone;
                IEnumerator coroutine = LerpToColor(colorLerpTime, selectionColor, selection.List[i].Data.Renderer.material);
                StartCoroutine(coroutine);
            }
        }

        for (int i = 0; i < selection.Chain.Count; i++)
        {
            IEnumerator coroutine = LerpToColor(
                colorLerpTime,
                selectionColor = i == 0 ? colorRoot : colorChild,
                selection.Chain[i].Data.Renderer.material);
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
}
