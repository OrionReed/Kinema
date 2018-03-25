using UnityEngine;

public class Player_Installer : MonoBehaviour
{
    public static Character currentCharacter { get; private set; }

    [SerializeField]
    private Transform PlayerRoot;
    [SerializeField]
    private Character Character;
    private void Awake()
    {
        if (Character == null || PlayerRoot == null)
            Debug.LogError("Player_Installer not set up.");
        currentCharacter = Character;
        currentCharacter.Init(PlayerRoot);
        foreach (TreeNode<CharacterNode> node in currentCharacter.tree.nodeList)
            node.Data.transform.gameObject.AddComponent<Node_Collision>().node = node;
    }
    private void Start()
    {
        _LevelState.OnPlay += delegate { currentCharacter.ResetNodeData(); };
    }
}
