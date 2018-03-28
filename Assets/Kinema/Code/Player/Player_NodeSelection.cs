using System.Collections.Generic;
using UnityEngine;
using System;

/// Sets and stores selection state of player nodes
public class Player_NodeSelection : MonoBehaviour
{
    public event Action OnSelectionUpdate = delegate { };
    public event Action OnModeUpdate = delegate { };
    public List<TreeNode<CharacterNode>> nodeList { get; private set; } = new List<TreeNode<CharacterNode>>();
    public List<TreeNode<CharacterNode>> chain { get; private set; } = new List<TreeNode<CharacterNode>>();
    public List<TreeNode<CharacterNode>> chainMirror { get; private set; } = new List<TreeNode<CharacterNode>>();
    public List<TreeNode<CharacterNode>> chainFollow { get; private set; } = new List<TreeNode<CharacterNode>>();

    public SelectionModeEnum selectionMode;
    public enum SelectionModeEnum { None, Chain, Mirror, Follow, MAX };
    private Character character;

    private void Start()
    {
        character = FindObjectOfType<Character_Installer>().currentCharacter;
        nodeList = FindObjectOfType<Character_Installer>().currentCharacter.tree.nodeList;
        _Input.OnKeySelectionMode += UpdateSelectionMode;
        _Input.OnClickLeft += SelectObject;
    }
    private void OnDisable()
    {
        _Input.OnKeySelectionMode -= UpdateSelectionMode;
        _Input.OnClickLeft -= SelectObject;
    }
    private void UpdateSelectionMode()
    {
        selectionMode += 1; if (selectionMode == SelectionModeEnum.MAX) selectionMode = 0; OnModeUpdate();
    }

    private void UpdateSelection(TreeNode<CharacterNode> node)
    {
        if (node.Data.selected && node == chain[0])
        {
            DeselectAll();
            OnSelectionUpdate();
            return;
        }

        DeselectAll();
        TreeNode<CharacterNode> oppositeNode = character.tree.GetOpposite(node);
        chain.Add(node);
        switch (selectionMode)
        {
            case SelectionModeEnum.Chain:
                chain.AddRange(_CharacterTree<CharacterNode>.GetChildren(node));
                break;

            case SelectionModeEnum.Follow:
                chainFollow.Add(oppositeNode);
                chain.AddRange(_CharacterTree<CharacterNode>.GetChildren(node));
                chainFollow.AddRange(_CharacterTree<CharacterNode>.GetChildren(chainFollow[0]));
                break;

            case SelectionModeEnum.Mirror:
                chainMirror.Add(oppositeNode);
                chain.AddRange(_CharacterTree<CharacterNode>.GetChildren(node));
                chainMirror.AddRange(_CharacterTree<CharacterNode>.GetChildren(chainMirror[0]));
                break;

        }
        foreach (TreeNode<CharacterNode> n in chain)
            n.Data.selected = true;
        foreach (TreeNode<CharacterNode> n in chainMirror)
            n.Data.selected = true;
        foreach (TreeNode<CharacterNode> n in chainFollow)
            n.Data.selected = true;
        OnSelectionUpdate();
    }

    private void SelectObject()
    {
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
        TreeNode<CharacterNode> hitNode = character.ContainsTransform(hit.transform);
        if (hitNode != null)
        {
            UpdateSelection(character.ContainsTransform(hit.transform));

        }
    }

    private void DeselectAll()
    {
        foreach (TreeNode<CharacterNode> n in chain)
            n.Data.selected = false;
        foreach (TreeNode<CharacterNode> n in chainMirror)
            n.Data.selected = false;
        foreach (TreeNode<CharacterNode> n in chainFollow)
            n.Data.selected = false;
        chain.Clear();
        chainMirror.Clear();
        chainFollow.Clear();
        OnSelectionUpdate();
    }
}
