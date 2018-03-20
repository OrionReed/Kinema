using System;
using UnityEngine;

[Serializable]
public class PlayerNode
{
    public GameObject gameObj;
    [HideInInspector]
    public Transform transform;
    [HideInInspector]
    public int coordX;
    [HideInInspector]
    public int coordY;
    [HideInInspector]
    public NodeState selectionState = NodeState.None;
    [HideInInspector]
    public Renderer renderer;
    [HideInInspector]
    public Rigidbody rigidbody;
    [HideInInspector]
    public ConfigurableJoint joint;
    [HideInInspector]
    public Quaternion originalRotation;

    public JointDrive driveMode;

    public float spring = 30;
    public float damper = 5;
    public Vector3 initialVelocity;

    public void SetCoords(int CoordX, int CoordY)
    {
        coordX = CoordX;
        coordY = CoordY;
    }
}

public enum NodeState { None, Root, RootChild, MirrorRoot, MirrorChild, FollowRoot, FollowChild }