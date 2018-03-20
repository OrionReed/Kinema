using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Gizmos : MonoBehaviour
{
    private K_NodeMap nodeMap;
    private K_Selection selection;
    [SerializeField]
    private Color PivotGizmoColor = Color.blue;
    [SerializeField]
    private Color LimbGizmoColor = Color.red;

    private void Awake()
    {
        nodeMap = GetComponent<K_NodeMap>();
        selection = GetComponent<K_Selection>();
    }

    private void OnDrawGizmos()
    {
        if (selection != null)
            DrawRuntimeGizmos();
    }

    void DrawRuntimeGizmos()
    {
        Gizmos.DrawSphere(nodeMap.CenterOfMass(), 0.1f);
        if (selection.rootFollow != null && selection.rootFollow.Parent != null)
        {
            PlayerNode root = selection.rootFollow.Data;
            PlayerNode parent = selection.rootFollow.Parent.Data;

            Vector3 pivotPointWorld = root.transform.TransformPoint(root.joint.anchor);

            Gizmos.color = PivotGizmoColor;
            Gizmos.DrawRay(pivotPointWorld, (Quaternion.Euler(root.joint.axis) * parent.transform.forward).normalized * 0.3f);
            Gizmos.DrawRay(pivotPointWorld, (Quaternion.Euler(root.joint.axis) * parent.transform.up).normalized * 1);

            Gizmos.color = LimbGizmoColor;
            Gizmos.DrawSphere(pivotPointWorld, 0.03f);
            Gizmos.DrawRay(transform.position, transform.forward * 0.5f);
            Gizmos.DrawRay(pivotPointWorld, transform.up * 2);
        }
    }
}
