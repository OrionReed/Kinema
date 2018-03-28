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
    private Color ghostColor;

    private void Awake()
    {
        ghostCharacter.Init(ghostRoot);
        ghostColor = ghostCharacter.tree.nodeList[0].Data.renderer.material.color;
    }
    private void Start()
    {
        selection = FindObjectOfType<Player_NodeSelection>();
        character = FindObjectOfType<Character_Installer>().currentCharacter;
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
        Gizmos.DrawSphere(character.GetCenterOfMass(), 0.1f);
        if (selection.chain.Any() == true)
        {
            foreach (TreeNode<CharacterNode> node in selection.chain)
            {
                Vector3 pivotPointWorld = node.Data.transform.TransformPoint(node.Data.joint.anchor);
                Gizmos.color = gizmoColor;
                Gizmos.DrawRay(pivotPointWorld, (Quaternion.Euler(node.Data.joint.axis) * node.Parent.Data.transform.forward).normalized * 0.3f);
                Gizmos.DrawRay(pivotPointWorld, (Quaternion.Euler(node.Data.joint.axis) * node.Parent.Data.transform.up).normalized * 1);
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

                ghostCharacter.nodeList[i].Data.transform.position = character.tree.nodeList[i].Data.transform.position;
                ghostCharacter.nodeList[i].Data.transform.rotation = character.tree.nodeList[i].Data.transform.rotation;

                ghostCharacter.nodeList[i].Data.transform.rotation = rootTargetRotation;
                ghostCharacter.nodeList[i].Data.transform.position = ghostCharacter.GetNode(i).transform.TransformPoint(character.tree.nodeList[i].Data.joint.anchor);
                ghostCharacter.nodeList[i].Data.transform.Translate(Vector3.up * Vector3.Distance(
                                                ghostCharacter.nodeList[i].Data.transform.TransformPoint(character.tree.nodeList[i].Data.joint.anchor),
                                                ghostCharacter.nodeList[i].Data.transform.position
                                                ),
                                                Space.Self);
            }
        }
        */
    }
}
