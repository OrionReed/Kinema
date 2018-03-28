using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Player_Color : MonoBehaviour
{
    [SerializeField]
    private Color ColorNone = Color.white;
    [SerializeField]
    private Color ColorRoot = Color.white;
    [SerializeField]
    private Color ColorChild = Color.white;
    [SerializeField]
    private Color ColorRootFollow = Color.white;
    [SerializeField]
    private Color ColorChildFollow = Color.white;
    [SerializeField]
    private float ColorLerpTime = 0.1f;
    [SerializeField]
    private Color Hurt = Color.red;

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
            ColorLerpTime,
            Color.Lerp(ColorNone, Hurt, node.damage),
            node.renderer.material);
        StartCoroutine(coroutine);
    }

    private void UpdateSelectionColor()
    {
        Color selectionColor = Color.magenta;
        for (int i = 0; i < selection.nodeList.Count; i++)
        {
            if (selection.nodeList[i].Data.selected == false)
            {
                selectionColor = ColorNone;
                IEnumerator coroutine = LerpToColor(ColorLerpTime, selectionColor, selection.nodeList[i].Data.renderer.material);
                StartCoroutine(coroutine);
            }
        }

        for (int i = 0; i < selection.chain.Count; i++)
        {
            IEnumerator coroutine = LerpToColor(
                ColorLerpTime,
                selectionColor = i == 0 ? ColorRoot : ColorChild,
                selection.chain[i].Data.renderer.material);
            StartCoroutine(coroutine);
        }
        for (int i = 0; i < selection.chainFollow.Count; i++)
        {
            IEnumerator coroutine = LerpToColor(
                ColorLerpTime,
                selectionColor = i == 0 ? ColorRootFollow : ColorChildFollow,
                selection.chainFollow[i].Data.renderer.material);
            StartCoroutine(coroutine);
        }
        for (int i = 0; i < selection.chainMirror.Count; i++)
        {
            IEnumerator coroutine = LerpToColor(
                ColorLerpTime,
                selectionColor = i == 0 ? ColorRoot : ColorChild,
                selection.chainMirror[i].Data.renderer.material);
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
