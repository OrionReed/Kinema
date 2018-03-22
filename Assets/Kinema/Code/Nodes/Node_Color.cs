using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Color : MonoBehaviour
{
    [SerializeField]
    private Color SelectionNone = Color.white;
    [SerializeField]
    private Color SelectionRoot = Color.white;
    [SerializeField]
    private Color SelectionChild = Color.white;
    [SerializeField]
    private Color SelectionFollowRoot = Color.white;
    [SerializeField]
    private Color SelectionFollowChild = Color.white;
    [SerializeField]
    private float ColorLerpTime = 0.1f;

    private Node_Map nodeMap;

    private void Awake()
    {
        nodeMap = GetComponent<Node_Map>();
        GetComponent<Node_Selection>().OnSelectionUpdate += UpdateSelectionColor;
        GetComponent<Node_Health>().OnImpact += UpdateHealthColor;
    }

    private void UpdateHealthColor()
    {
        for (int i = 0; i < nodeMap.nodes.Count; i++)
        {
            IEnumerator coroutine = LerpToColor(ColorLerpTime, Color.red, nodeMap.nodes[i].Data.renderer.material);
            if (nodeMap.nodes[i].Data.currentImpactForce == 1)
                StartCoroutine(coroutine);
        }
    }

    private void UpdateSelectionColor()
    {
        Color selectionColor = Color.magenta;

        for (int i = 0; i < nodeMap.nodes.Count; i++)
        {
            if (nodeMap.nodes[i].Data.selectionState == NodeState.None)
                selectionColor = SelectionNone;
            else if (nodeMap.nodes[i].Data.selectionState == NodeState.Root)
                selectionColor = SelectionRoot;
            else if (nodeMap.nodes[i].Data.selectionState == NodeState.RootChild)
                selectionColor = SelectionChild;
            else if (nodeMap.nodes[i].Data.selectionState == NodeState.MirrorRoot)
                selectionColor = SelectionRoot;
            else if (nodeMap.nodes[i].Data.selectionState == NodeState.MirrorChild)
                selectionColor = SelectionChild;
            else if (nodeMap.nodes[i].Data.selectionState == NodeState.FollowRoot)
                selectionColor = SelectionFollowRoot;
            else if (nodeMap.nodes[i].Data.selectionState == NodeState.FollowChild)
                selectionColor = SelectionFollowChild;
            IEnumerator coroutine = LerpToColor(ColorLerpTime, selectionColor, nodeMap.nodes[i].Data.renderer.material);
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
