using UnityEngine;

public class Character_Installer : MonoBehaviour
{
    public Character CurrentCharacter { get; private set; }

    [SerializeField]
    private Transform playerRoot;
    [SerializeField]
    private Character character;
    private void Awake()
    {
        if (character == null || playerRoot == null)
            Debug.LogError("Player_Installer not set up.");
        CurrentCharacter = character;
        CurrentCharacter.Init(playerRoot);
        foreach (TreeNode<CharacterNode> node in CurrentCharacter.List)
            node.Data.Transform.gameObject.AddComponent<Node_Collision>().Node = node;
    }
}
