using System.Collections;
using UnityEngine;

public class Player_Color : MonoBehaviour
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
    [SerializeField]
    private Color Hurt = Color.red;

    private void Awake()
    {
        GetComponent<Player_NodeSelection>().OnSelectionUpdate += UpdateSelectionColor;
        GetComponent<Player_Health>().OnNodeImpact += UpdateHealthColor;
        _LevelState.OnPlay += UpdateSelectionColor;
    }

    private void UpdateHealthColor(CharacterNode node)
    {
        IEnumerator coroutine = LerpToColor(
            ColorLerpTime,
            Color.Lerp(SelectionNone, Hurt, node.damage),
            node.renderer.material);
        StartCoroutine(coroutine);
    }

    private void UpdateSelectionColor()
    {
        Color selectionColor = Color.magenta;

        for (int i = 0; i < CharacterSelection.currentCharacter.tree.nodeList.Count; i++)
        {
            switch (CharacterSelection.currentCharacter.tree.nodeList[i].Data.selectionState)
            {
                case NodeSelectionState.None:
                    selectionColor = SelectionNone;
                    break;
                case NodeSelectionState.Root:
                    selectionColor = SelectionRoot;
                    break;
                case NodeSelectionState.RootChild:
                    selectionColor = SelectionChild;
                    break;
                case NodeSelectionState.FollowRoot:
                    selectionColor = SelectionFollowRoot;
                    break;
                case NodeSelectionState.FollowChild:
                    selectionColor = SelectionFollowChild;
                    break;
                case NodeSelectionState.MirrorRoot:
                    selectionColor = SelectionRoot;
                    break;
                case NodeSelectionState.MirrorChild:
                    selectionColor = SelectionChild;
                    break;
            }
            IEnumerator coroutine = LerpToColor(ColorLerpTime, selectionColor, CharacterSelection.currentCharacter.tree.nodeList[i].Data.renderer.material);
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
