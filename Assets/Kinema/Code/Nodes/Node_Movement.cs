using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node_Movement : MonoBehaviour
{
    public event Action OnModeUpdate = delegate { };
    public enum ForceModeEnum { Position, Speed, Contact, MAX };
    public ForceModeEnum forceMode;

    [SerializeField]
    private Material GhostMaterial;
    private _Input input;
    private Node_Selection selection;
    private Quaternion rootTargetRotation = Quaternion.identity;
    private GameObject ghostLimb;

    private void Awake()
    {
        ghostLimb = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ghostLimb.name = "Ghost_Limb";
        ghostLimb.transform.parent = this.transform;
        ghostLimb.GetComponent<Collider>().enabled = false;
        ghostLimb.transform.localScale = transform.localScale;
        ghostLimb.GetComponent<Renderer>().material = GhostMaterial;
    }

    private void Start()
    {
        input = FindObjectOfType<_Input>();
        selection = GetComponent<Node_Selection>();
        input.OnForceMode += UpdateForceMode;
    }
    private void OnDisable()
    {
        if (input != null)
            input.OnForceMode -= UpdateForceMode;
    }

    private void UpdateForceMode()
    {
        forceMode += 1;
        if (forceMode == ForceModeEnum.MAX)
            forceMode = 0;
        OnModeUpdate();
    }
    private void FixedUpdate()
    {
        TreeNode<PlayerNode> rootChain = selection.root;
        List<TreeNode<PlayerNode>> childrenChain = selection.childrenChain;
        if (rootChain != null)
            ChainMovement(rootChain, childrenChain);

        TreeNode<PlayerNode> rootMirror = selection.rootMirror;
        List<TreeNode<PlayerNode>> childrenMirror = selection.childrenMirror;
        if (rootMirror != null)
            MirrorMovement(rootMirror, childrenMirror);

        TreeNode<PlayerNode> rootFollow = selection.rootFollow;
        List<TreeNode<PlayerNode>> childrenFollow = selection.childrenFollow;
        if (rootFollow != null)
            FollowMovement(rootFollow, childrenFollow);
        if (rootChain != null)
            UpdateGhost(rootChain);
    }

    private void ChainMovement(TreeNode<PlayerNode> root, List<TreeNode<PlayerNode>> chain)
    {
        rootTargetRotation = Quaternion.Inverse(root.Parent.Data.transform.rotation) * (root.Data.transform.rotation * input.inputRotation);
        QuaternionUtils.SetTargetRotationLocal(root.Data.joint, rootTargetRotation, root.Data.originalRotation);

        if (chain != null)
            for (int i = 0; i < chain.Count; i++)
            {

            }
    }
    private void MirrorMovement(TreeNode<PlayerNode> root, List<TreeNode<PlayerNode>> chain)
    {

    }
    private void FollowMovement(TreeNode<PlayerNode> root, List<TreeNode<PlayerNode>> chain)
    {

    }
    private void UpdateGhost(TreeNode<PlayerNode> root)
    {
        ghostLimb.transform.localScale = root.Data.transform.localScale;
        ghostLimb.transform.rotation = rootTargetRotation;
        ghostLimb.transform.position = root.Data.transform.TransformPoint(root.Data.joint.anchor);
        ghostLimb.transform.Translate(Vector3.up * Vector3.Distance(
                                        root.Data.transform.TransformPoint(root.Data.joint.anchor),
                                        root.Data.transform.position
                                        ),
                                        Space.Self);
    }
}
