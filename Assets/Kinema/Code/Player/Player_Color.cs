using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Player_Color : MonoBehaviour
{
    [Header("Selection")]
    [SerializeField]
    private Color colorDeselected = Color.white;
    [SerializeField]
    private Gradient gradientSelected;
    [SerializeField]
    private Gradient gradientMirror;
    [SerializeField]
    private Gradient gradientFollow;
    [SerializeField]
    private float selectionLerpTime = 0.1f;

    [Header("Damage")]
    [SerializeField]
    private Gradient gradientDamage;
    [SerializeField]
    private float damageMinVisible = 3f;
    [SerializeField]
    private float damageFadeIn = 0.05f;
    [SerializeField]
    private float damageWait = 0.1f;
    [SerializeField]
    private float damageFadeOut = 0.9f;
    [SerializeField]
    private float damageIntensity = 2f;

    private Character playerCharacter;
    private Player_NodeSelection selection;

    private void Start()
    {
        _LevelState.OnPlay += UpdateSelectionColor;
        GetComponent<PlayerHealth_Damage>().OnNodeDamaged += UpdateDamageColor;
        GetComponent<Player_NodeSelection>().OnNodeSelection += UpdateSelectionColor;
        selection = FindObjectOfType<Player_NodeSelection>();
        playerCharacter = FindObjectOfType<Player_Character>().PlayerCharacter;
        foreach (TreeNode<CharacterNode> node in playerCharacter.CharacterTree.NodeList)
        {
            node.Data.Renderer.material.EnableKeyword("_EMISSION");
            node.Data.Renderer.material.SetColor("_EmissionColor", Color.clear);
        }
    }

    private void UpdateDamageColor(CharacterNode node)
    {
        if (node.DamageCurrent > damageMinVisible)
        {
            StartCoroutine(PulseDamageColor(node));
        }
    }

    private void UpdateSelectionColor()
    {
        foreach (TreeNode<CharacterNode> node in playerCharacter.CharacterTree.NodeList)
        {
            IEnumerator coroutine = LerpSelectionColor(selectionLerpTime, colorDeselected, node.Data.Renderer.material);
            StartCoroutine(coroutine);
        }

        for (int i = 0; i < selection.ChainSelected.Count; i++)
        {
            IEnumerator coroutine = LerpSelectionColor(
                selectionLerpTime,
                gradientSelected.Evaluate(i == 0 ? 0 : 0.5f),
                selection.ChainSelected[i].Data.Renderer.material);
            StartCoroutine(coroutine);
        }
        for (int i = 0; i < selection.ChainFollow.Count; i++)
        {
            IEnumerator coroutine = LerpSelectionColor(
                selectionLerpTime,
                gradientFollow.Evaluate(i == 0 ? 0 : 0.5f),
                selection.ChainFollow[i].Data.Renderer.material);
            StartCoroutine(coroutine);
        }
        for (int i = 0; i < selection.ChainMirror.Count; i++)
        {
            IEnumerator coroutine = LerpSelectionColor(
                selectionLerpTime,
                gradientMirror.Evaluate(i == 0 ? 0 : 0.5f),
                selection.ChainMirror[i].Data.Renderer.material);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator LerpSelectionColor(float timeForEffect, Color endColor, Material materialInstance)
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
    private IEnumerator PulseDamageColor(CharacterNode node)
    {
        float stageElapsed = 0;

        Color startColor = node.Renderer.material.GetColor("_EmissionColor");
        if (startColor == Color.clear)
        {
            Color colorDamage = gradientDamage.Evaluate(node.DamageCurrent / node.DamageMax) * damageIntensity;
            while (stageElapsed < damageFadeIn)
            {
                node.Renderer.material.SetColor(
                    "_EmissionColor",
                    Color.Lerp(Color.clear, colorDamage, stageElapsed / damageFadeIn));

                stageElapsed += Time.deltaTime / Time.timeScale;
                yield return null;
            }
        }

        yield return new WaitForSeconds(damageWait);
        stageElapsed = 0;

        Color currentColor = node.Renderer.material.GetColor("_EmissionColor");
        while (stageElapsed < damageFadeOut)
        {
            node.Renderer.material.SetColor(
                "_EmissionColor",
                Color.Lerp(currentColor, Color.clear, stageElapsed / damageFadeOut));

            stageElapsed += Time.deltaTime / Time.timeScale;
            yield return null;
        }
        node.Renderer.material.SetColor("_EmissionColor", Color.clear);
    }
}
