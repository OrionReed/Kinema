using System.Collections.Generic;
using UnityEngine;
using System;

/// Sets and stores selection state of player nodes
public class Player_NodeSelection : MonoBehaviour
{
    public event Action OnSelectionUpdate = delegate { };
    public event Action OnModeUpdate = delegate { };
    public List<TreeNode<CharacterNode>> List { get; private set; } = new List<TreeNode<CharacterNode>>();
    public List<TreeNode<CharacterNode>> Chain { get; private set; } = new List<TreeNode<CharacterNode>>();
    public List<TreeNode<CharacterNode>> ChainMirror { get; private set; } = new List<TreeNode<CharacterNode>>();
    public List<TreeNode<CharacterNode>> ChainFollow { get; private set; } = new List<TreeNode<CharacterNode>>();

    public enum SelectionModeEnum { None, Chain, Mirror, Follow, MAX };
    public SelectionModeEnum SelectionMode;
    private Character character;

    private void Start()
    {
        character = FindObjectOfType<Character_Installer>().CurrentCharacter;
        List = FindObjectOfType<Character_Installer>().CurrentCharacter.List;
        _Input.OnKeySelectionMode += UpdateSelectionMode;
        _Input.OnClickSelect += SelectObject;
    }
    private void OnDisable()
    {
        _Input.OnKeySelectionMode -= UpdateSelectionMode;
        _Input.OnClickSelect -= SelectObject;
    }
    private void UpdateSelectionMode()
    {
        SelectionMode += 1; if (SelectionMode == SelectionModeEnum.MAX) SelectionMode = 0; OnModeUpdate();
    }

    private void UpdateSelection(TreeNode<CharacterNode> node)
    {
        if (node.Data.Selected && node == Chain[0])
        {
            DeselectAll();
            OnSelectionUpdate();
            return;
        }

        DeselectAll();
        TreeNode<CharacterNode> oppositeNode = character.Tree.GetOpposite(node);
        Chain.Add(node);
        switch (SelectionMode)
        {
            case SelectionModeEnum.Chain:
                Chain.AddRange(_CharacterTree<CharacterNode>.GetChildren(node));
                break;

            case SelectionModeEnum.Follow:
                ChainFollow.Add(oppositeNode);
                Chain.AddRange(_CharacterTree<CharacterNode>.GetChildren(node));
                ChainFollow.AddRange(_CharacterTree<CharacterNode>.GetChildren(ChainFollow[0]));
                break;

            case SelectionModeEnum.Mirror:
                ChainMirror.Add(oppositeNode);
                Chain.AddRange(_CharacterTree<CharacterNode>.GetChildren(node));
                ChainMirror.AddRange(_CharacterTree<CharacterNode>.GetChildren(ChainMirror[0]));
                break;

        }
        foreach (TreeNode<CharacterNode> n in Chain)
            n.Data.Selected = true;
        foreach (TreeNode<CharacterNode> n in ChainMirror)
            n.Data.Selected = true;
        foreach (TreeNode<CharacterNode> n in ChainFollow)
            n.Data.Selected = true;
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
        foreach (TreeNode<CharacterNode> n in Chain)
            n.Data.Selected = false;
        foreach (TreeNode<CharacterNode> n in ChainMirror)
            n.Data.Selected = false;
        foreach (TreeNode<CharacterNode> n in ChainFollow)
            n.Data.Selected = false;
        Chain.Clear();
        ChainMirror.Clear();
        ChainFollow.Clear();
        OnSelectionUpdate();
    }
}
