using UnityEngine;
using System.Linq;

public class Player_Gizmos : MonoBehaviour
{
    private Character character;
    private Player_NodeSelection selection;
    private Color gizmoColor = Color.blue;

    private void Start()
    {
        selection = FindObjectOfType<Player_NodeSelection>();
        character = FindObjectOfType<Player_Character>().PlayerCharacter;
    }
    private void Update()
    {
        if (selection != null)
        {
            DrawGhost();
        }
    }

    private void OnDrawGizmos()
    {
        if (selection != null)
        {
            DrawRuntimeGizmos();
        }
    }

    void DrawRuntimeGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(character.GetCenterOfMass(), 0.5f);
        if (selection.ChainSelected.Any() == true)
        {
            foreach (TreeNode<CharacterNode> node in selection.ChainSelected)
            {
                Vector3 pivotPointWorld = node.Data.Transform.TransformPoint(node.Data.Joint.anchor);
                Gizmos.color = gizmoColor;
                Gizmos.DrawRay(pivotPointWorld, (Quaternion.Euler(node.Data.Joint.axis) * node.Parent.Data.Transform.forward).normalized * 0.3f);
                Gizmos.DrawRay(pivotPointWorld, (Quaternion.Euler(node.Data.Joint.axis) * node.Parent.Data.Transform.up).normalized * 1);
                Gizmos.DrawSphere(pivotPointWorld, 0.03f);
                Gizmos.DrawRay(transform.position, transform.forward * 0.5f);
                Gizmos.DrawRay(pivotPointWorld, transform.up * 2);
            }
        }
    }
    private void DrawGhost()
    {

    }
}
