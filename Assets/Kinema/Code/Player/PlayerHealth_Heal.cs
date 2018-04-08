using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth_Heal : MonoBehaviour
{

    [SerializeField]
    private float healSpeed = 0.1f;

    private Character playerCharacter;
    private void Start()
    {
        playerCharacter = GetComponent<Player_Character>().PlayerCharacter;
        _LevelState.OnPlay += HealInstant;
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (TreeNode<CharacterNode> node in playerCharacter.CharacterTree.NodeList)
        {
            node.Data.SetDamage(node.Data.DamageCurrent - (Time.deltaTime * healSpeed));
        }
    }
    public void HealInstant()
    {
        foreach (TreeNode<CharacterNode> node in playerCharacter.CharacterTree.NodeList)
        {
            node.Data.SetDamage(0);
        }
    }

}
