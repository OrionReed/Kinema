using System;
using UnityEngine;

[Serializable]
public class PlayerNode
{
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

    public GameObject gameObj;
    public float spring = 30;
    public float damper = 5;
    public Vector3 initialVelocity;
    public float maxImpactForce = 10;
    public float currentImpactForce = 0;

    public void SetCoords(int CoordX, int CoordY)
    {
        coordX = CoordX;
        coordY = CoordY;
    }
}

public enum NodeState { None, Root, RootChild, MirrorRoot, MirrorChild, FollowRoot, FollowChild }