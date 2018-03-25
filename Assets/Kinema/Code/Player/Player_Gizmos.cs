using UnityEngine;

public class Player_Gizmos : MonoBehaviour
{
    [SerializeField]
    private Transform ghostRoot;

    private Player_NodeSelection selection;
    private Color gizmoColor = Color.blue;
    private Color ghostColor;
    private Ghost ghostCharacter = new Ghost();

    private void Awake()
    {
        selection = FindObjectOfType<Player_NodeSelection>();
        ghostCharacter.Init(ghostRoot);
        ghostColor = ghostCharacter.tree.nodeList[0].Data.renderer.material.color;
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
        Gizmos.DrawSphere(Utilities_Character.GetCenterOfMass(Player_Installer.currentCharacter), 0.1f);
        if (selection.rootFollow != null && selection.rootFollow.Parent != null)
        {
            CharacterNode root = selection.rootFollow.Data;
            CharacterNode parent = selection.rootFollow.Parent.Data;

            Vector3 pivotPointWorld = root.transform.TransformPoint(root.joint.anchor);

            Gizmos.color = gizmoColor;
            Gizmos.DrawRay(pivotPointWorld, (Quaternion.Euler(root.joint.axis) * parent.transform.forward).normalized * 0.3f);
            Gizmos.DrawRay(pivotPointWorld, (Quaternion.Euler(root.joint.axis) * parent.transform.up).normalized * 1);

            Gizmos.DrawSphere(pivotPointWorld, 0.03f);
            Gizmos.DrawRay(transform.position, transform.forward * 0.5f);
            Gizmos.DrawRay(pivotPointWorld, transform.up * 2);
        }
    }
    private void DrawGhost()
    {
        Color clearColor = Color.clear;
        for (int i = 0; i < Player_Installer.currentCharacter.tree.nodeList.Count; i++)
        {
            if (ghostCharacter.GetTreeNode(i).Parent != null)
            {

                Quaternion rootTargetRotation = Quaternion.Inverse(ghostCharacter.GetTreeNode(i).Parent.Data.transform.rotation) * (ghostCharacter.GetNode(i).transform.rotation * _Input.inputRotation);
                ghostCharacter.GetNode(i).renderer.material.color =
                    Player_Installer.currentCharacter.GetNode(i).selectionState == NodeSelectionState.None ?
                    clearColor : ghostColor;

                ghostCharacter.GetNode(i).transform.position = Player_Installer.currentCharacter.GetNode(i).transform.position;
                ghostCharacter.GetNode(i).transform.rotation = Player_Installer.currentCharacter.GetNode(i).transform.rotation;

                /*ghostCharacter.GetNode(i).transform.rotation = rootTargetRotation;
                ghostCharacter.GetNode(i).transform.position = ghostCharacter.GetNode(i).transform.TransformPoint(Player_Installer.currentCharacter.GetNode(i).joint.anchor);
                ghostCharacter.GetNode(i).transform.Translate(Vector3.up * Vector3.Distance(
                                                ghostCharacter.GetNode(i).transform.TransformPoint(Player_Installer.currentCharacter.GetNode(i).joint.anchor),
                                                ghostCharacter.GetNode(i).transform.position
                                                ),
                                                Space.Self);*/
            }
        }
    }
}
