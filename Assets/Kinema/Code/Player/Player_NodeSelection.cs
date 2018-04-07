﻿using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// Sets and stores selection state of player nodes
public class Player_NodeSelection : MonoBehaviour
{
    public event Action OnNodeSelection = delegate { };
    public event Action OnModeUpdate = delegate { };
    public List<TreeNode<CharacterNode>> ChainSelected { get; private set; } = new List<TreeNode<CharacterNode>>();
    public List<TreeNode<CharacterNode>> ChainMirror { get; private set; } = new List<TreeNode<CharacterNode>>();
    public List<TreeNode<CharacterNode>> ChainFollow { get; private set; } = new List<TreeNode<CharacterNode>>();
    public List<TreeNode<CharacterNode>> AllSelected { get; private set; } = new List<TreeNode<CharacterNode>>();
    public enum SelectionModeEnum { None, Chain, Mirror, Follow, MAX };
    public SelectionModeEnum SelectionMode;

    private Character character;

    private void Start()
    {
        character = FindObjectOfType<Player_Character>().PlayerCharacter;
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
        if (ChainSelected.Count > 0 && node == ChainSelected[0])
        {
            DeselectAll();
            OnNodeSelection();
            return;
        }

        DeselectAll();
        TreeNode<CharacterNode> oppositeNode = character.CharacterTree.GetOpposite(node);
        ChainSelected.Add(node);
        switch (SelectionMode)
        {
            case SelectionModeEnum.Chain:
                ChainSelected.AddRange(_CharacterTree<CharacterNode>.GetChildren(node));
                break;

            case SelectionModeEnum.Follow:
                ChainFollow.Add(oppositeNode);
                ChainSelected.AddRange(_CharacterTree<CharacterNode>.GetChildren(node));
                ChainFollow.AddRange(_CharacterTree<CharacterNode>.GetChildren(ChainFollow[0]));
                break;

            case SelectionModeEnum.Mirror:
                ChainMirror.Add(oppositeNode);
                ChainSelected.AddRange(_CharacterTree<CharacterNode>.GetChildren(node));
                ChainMirror.AddRange(_CharacterTree<CharacterNode>.GetChildren(ChainMirror[0]));
                break;

        }
        AllSelected.Clear();
        AllSelected.AddRange(ChainSelected);
        AllSelected.AddRange(ChainMirror);
        AllSelected.AddRange(ChainFollow);
        OnNodeSelection();
    }

    private void SelectObject()
    {
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
        TreeNode<CharacterNode> hitNode = character.ContainsTransform(hit.transform);
        if (hitNode != null)
            UpdateSelection(character.ContainsTransform(hit.transform));
    }

    private void DeselectAll()
    {
        AllSelected.Clear();
        ChainSelected.Clear();
        ChainMirror.Clear();
        ChainFollow.Clear();
        OnNodeSelection();
    }
}
