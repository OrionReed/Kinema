using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node_Health : MonoBehaviour
{
    public event Action OnImpact = delegate { };
    [SerializeField]
    private AnimationCurve impactForce;
    private Node_Map map;

    private void Start()
    {
        map = GetComponent<Node_Map>();
        for (int i = 0; i < map.nodes.Count; i++)
            map.nodes[i].Data.gameObj.AddComponent<NodeHealth_CollisionDetection>().health = this;
    }

    public void Collided(Collision collision, GameObject node)
    {
        map.GetFromTransform(node.transform).Data.currentImpactForce = 1;
        OnImpact();
    }
}
