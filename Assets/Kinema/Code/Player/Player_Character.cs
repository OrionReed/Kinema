using UnityEngine;

public class Player_Character : MonoBehaviour
{
    public Character PlayerCharacter { get; private set; }

    [SerializeField]
    private Transform characterRoot;
    [SerializeField]
    private Character character;
    private void Awake()
    {
        PlayerCharacter = character;
        PlayerCharacter.Init(characterRoot);
        foreach (TreeNode<CharacterNode> node in PlayerCharacter.CharacterTree.NodeList)
            node.Data.Transform.gameObject.AddComponent<Node_Collision>().Node = node;
    }
}
