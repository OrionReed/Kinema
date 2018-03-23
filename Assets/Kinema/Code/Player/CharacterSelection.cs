using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField]
    private Transform PlayerRoot;
    [SerializeField]
    private Character Character;
    public static Character currentCharacter { get; private set; }
    private void Awake()
    {
        currentCharacter = Character;
        currentCharacter.Init(PlayerRoot);
    }
}
