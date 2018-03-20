using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// Sets and stores selection state of player nodes
public class K_Selection : MonoBehaviour
{
    public event Action OnSelectionUpdate = delegate { };
    public event Action OnModeUpdate = delegate { };
    public TreeNode<PlayerNode> root { get; private set; }
    public TreeNode<PlayerNode> rootMirror { get; private set; }
    public TreeNode<PlayerNode> rootFollow { get; private set; }
    public List<TreeNode<PlayerNode>> childrenChain { get; private set; }
    public List<TreeNode<PlayerNode>> childrenMirror { get; private set; }
    public List<TreeNode<PlayerNode>> childrenFollow { get; private set; }

    public SelectionModeEnum selectionMode;
    public enum SelectionModeEnum { None, Chain, Mirror, Follow, MAX };
    private K_NodeMap nodeMap;
    private _Input input;

    private void Awake()
    {
        nodeMap = GetComponent<K_NodeMap>();
        selectionMode = 0;
    }

    private void Start()
    {
        input = FindObjectOfType<_Input>();
        input.OnSelectionMode += UpdateSelectionMode;
        input.OnClickLeft += SelectObject;
    }
    private void OnDisable()
    {
        if (input != null)
        {
            input.OnSelectionMode -= UpdateSelectionMode;
            input.OnClickLeft -= SelectObject;
        }
    }

    private void UpdateSelectionMode()
    {
        selectionMode += 1;
        if (selectionMode == SelectionModeEnum.MAX)
            selectionMode = 0;
        OnModeUpdate();
    }

    private void UpdateSelection(TreeNode<PlayerNode> node)
    {
        if (node.Data.selectionState == NodeState.Root)
        {
            DeselectAll();
            OnSelectionUpdate();
            return;
        }
        DeselectAll();
        TreeNode<PlayerNode> oppositeNode = nodeMap.GetOpposite(node);
        switch (selectionMode)
        {
            case SelectionModeEnum.None:
                root = node;

                root.Data.selectionState = NodeState.Root;
                break;

            case SelectionModeEnum.Chain:
                root = node;
                childrenChain = nodeMap.GetChildren(node);

                root.Data.selectionState = NodeState.Root;
                SetSelection(childrenChain, NodeState.RootChild);
                break;

            case SelectionModeEnum.Mirror:
                root = node;
                rootMirror = oppositeNode;
                childrenChain = nodeMap.GetChildren(node);
                childrenMirror = nodeMap.GetChildren(rootMirror);

                root.Data.selectionState = NodeState.Root;
                SetSelection(childrenChain, NodeState.RootChild);
                rootMirror.Data.selectionState = NodeState.MirrorRoot;
                SetSelection(childrenMirror, NodeState.MirrorChild);
                break;

            case SelectionModeEnum.Follow:
                root = node;
                rootFollow = oppositeNode;
                childrenChain = nodeMap.GetChildren(node);
                childrenFollow = nodeMap.GetChildren(rootMirror);

                root.Data.selectionState = NodeState.Root;
                SetSelection(childrenChain, NodeState.RootChild);
                rootFollow.Data.selectionState = NodeState.FollowRoot;
                SetSelection(childrenFollow, NodeState.FollowChild);
                break;
        }
        OnSelectionUpdate();
    }

    private void SelectObject()
    {
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
        TreeNode<PlayerNode> node = nodeMap.GetFromTransform(hit.transform);
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
        SetSelection(nodeMap.nodes, NodeState.None);
        OnSelectionUpdate();
    }

    private void SetSelection(List<TreeNode<PlayerNode>> NodeList, NodeState SelectionState)
    {
        for (int i = 0; i < NodeList.Count; i++)
        {
            NodeList[i].Data.selectionState = SelectionState;
        }
    }
}
