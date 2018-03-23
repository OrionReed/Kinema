using System.Collections.Generic;
using UnityEngine;
using System;

/// Sets and stores selection state of player nodes
public class Node_Selection : MonoBehaviour
{
    public event Action OnSelectionUpdate = delegate { };
    public event Action OnModeUpdate = delegate { };
    public TreeNode<CharacterNode> root { get; private set; }
    public TreeNode<CharacterNode> rootMirror { get; private set; }
    public TreeNode<CharacterNode> rootFollow { get; private set; }
    public List<TreeNode<CharacterNode>> childrenChain { get; private set; }
    public List<TreeNode<CharacterNode>> childrenMirror { get; private set; }
    public List<TreeNode<CharacterNode>> childrenFollow { get; private set; }

    public SelectionModeEnum selectionMode;
    public enum SelectionModeEnum { None, Chain, Mirror, Follow, MAX };

    private void Awake()
    {
        selectionMode = 0;
    }

    private void Start()
    {
        _Input.OnKeySelectionMode += UpdateSelectionMode;
        _Input.OnKeyClickLeft += SelectObject;
    }
    private void OnDisable()
    {
        _Input.OnKeySelectionMode -= UpdateSelectionMode;
        _Input.OnKeyClickLeft -= SelectObject;
    }

    private void UpdateSelectionMode()
    {
        selectionMode += 1;
        if (selectionMode == SelectionModeEnum.MAX)
            selectionMode = 0;
        OnModeUpdate();
    }

    private void UpdateSelection(TreeNode<CharacterNode> node)
    {
        if (node.Data.selectionState == NodeSelectionState.Root)
        {
            DeselectAll();
            OnSelectionUpdate();
            return;
        }
        DeselectAll();
        TreeNode<CharacterNode> oppositeNode = CharacterUtils.GetOpposite(node);
        switch (selectionMode)
        {
            case SelectionModeEnum.None:
                root = node;

                root.Data.SetSelectionState(NodeSelectionState.Root);
                break;

            case SelectionModeEnum.Chain:
                root = node;
                childrenChain = CharacterUtils.GetChildren(node);

                root.Data.SetSelectionState(NodeSelectionState.Root);
                SetSelection(childrenChain, NodeSelectionState.RootChild);
                break;

            case SelectionModeEnum.Mirror:
                root = node;
                rootMirror = oppositeNode;
                childrenChain = CharacterUtils.GetChildren(node);
                childrenMirror = CharacterUtils.GetChildren(rootMirror);

                root.Data.SetSelectionState(NodeSelectionState.Root);
                SetSelection(childrenChain, NodeSelectionState.RootChild);
                rootMirror.Data.SetSelectionState(NodeSelectionState.MirrorRoot);
                SetSelection(childrenMirror, NodeSelectionState.MirrorChild);
                break;

            case SelectionModeEnum.Follow:
                root = node;
                rootFollow = oppositeNode;
                childrenChain = CharacterUtils.GetChildren(node);
                childrenFollow = CharacterUtils.GetChildren(rootMirror);

                root.Data.SetSelectionState(NodeSelectionState.Root);
                SetSelection(childrenChain, NodeSelectionState.RootChild);
                rootFollow.Data.SetSelectionState(NodeSelectionState.FollowRoot);
                SetSelection(childrenFollow, NodeSelectionState.FollowChild);
                break;
        }
        OnSelectionUpdate();
    }

    private void SelectObject()
    {
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
        TreeNode<CharacterNode> node = CharacterUtils.GetFromTransform(CharacterSelection.currentCharacter, hit.transform);
        if (node != null)
        {
            UpdateSelection(node);
        }
    }

    private void DeselectAll()
    {
        root = null;
        rootMirror = null;
        rootFollow = null;
        childrenChain = null;
        childrenMirror = null;
        childrenFollow = null;
        SetSelection(CharacterSelection.currentCharacter.nodeList, NodeSelectionState.None);
        OnSelectionUpdate();
    }

    private void SetSelection(List<TreeNode<CharacterNode>> NodeList, NodeSelectionState SelectionState)
    {
        for (int i = 0; i < NodeList.Count; i++)
        {
            NodeList[i].Data.SetSelectionState(SelectionState);
        }
    }
}
