using UnityEngine;
using System.Linq;

public class Player_Gizmos : MonoBehaviour
{
    [SerializeField]
    private Transform ghostRoot;

    private Character character;
    private TargetCharacter ghostCharacter = new TargetCharacter();
    private Player_NodeSelection selection;
    private Color gizmoColor = Color.blue;

    private void Awake()
    {
        ghostCharacter.Init(ghostRoot);
    }
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
        /*
        for (int i = 0; i < selection.nodeList.Count; i++)
        {
            if (selection.nodeList[i].Parent != null)
            {
                Quaternion rootTargetRotation = 
                    Quaternion.Inverse(ghostCharacter.nodeList[i].Parent.Data.transform.rotation) * 
                    ghostCharacter.nodeList[i].Data.transform.rotation * 
                    _Input.inputRotation;

                ghostCharacter.nodeList[i].Data.renderer.material.color =
                    selection.nodeList[i].Data.selectionState == NodeSelectionState.None ?
                    Color.clear : ghostColor;

                ghostCharacter.nodeList[i].Data.transform.position = character.nodeList[i].Data.transform.position;
                ghostCharacter.nodeList[i].Data.transform.rotation = character.nodeList[i].Data.transform.rotation;

                ghostCharacter.nodeList[i].Data.transform.rotation = rootTargetRotation;
                ghostCharacter.nodeList[i].Data.transform.position = ghostCharacter.GetNode(i).transform.TransformPoint(character.nodeList[i].Data.joint.anchor);
                ghostCharacter.nodeList[i].Data.transform.Translate(Vector3.up * Vector3.Distance(
                                                ghostCharacter.nodeList[i].Data.transform.TransformPoint(character.nodeList[i].Data.joint.anchor),
                                                ghostCharacter.nodeList[i].Data.transform.position
                                                ),
                                                Space.Self);
            }
        }
        */
    }
}
