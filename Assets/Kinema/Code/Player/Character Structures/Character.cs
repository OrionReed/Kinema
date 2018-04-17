using UnityEngine;

public class Character
{
    public _CharacterTree<CharacterNode> CharacterTree { get; private set; } = new _CharacterTree<CharacterNode>();

    public void Init(Transform rootTransform)
    {
        CharacterTree.Head.Init(this, 20, 0, 1, Vector3.left, Vector3.up, rootTransform.Find(CharacterTree.StringHead));
        CharacterTree.Chest.Init(this, 25, 0, 1, Vector3.left, Vector3.up, rootTransform.Find(CharacterTree.StringChest));
        CharacterTree.Stomach.Init(this, 50, 0, 0, Vector3.left, Vector3.up, rootTransform.Find(CharacterTree.StringStomach));

        CharacterTree.UpperArmLeft.Init(this, 50, -1, 1, Vector3.down, Vector3.left, rootTransform.Find(CharacterTree.StringUpperArmLeft));
        CharacterTree.LowerArmLeft.Init(this, 50, -1, 1, Vector3.up, Vector3.up, rootTransform.Find(CharacterTree.StringLowerArmLeft));
        CharacterTree.HandLeft.Init(this, 50, -1, 1, Vector3.down, Vector3.left, rootTransform.Find(CharacterTree.StringHandLeft));

        CharacterTree.UpperArmRight.Init(this, 50, 1, 1, Vector3.down, Vector3.left, rootTransform.Find(CharacterTree.StringUpperArmRight));
        CharacterTree.LowerArmRight.Init(this, 50, 1, 1, Vector3.down, Vector3.up, rootTransform.Find(CharacterTree.StringLowerArmRight));
        CharacterTree.HandRight.Init(this, 50, 1, 1, Vector3.down, Vector3.left, rootTransform.Find(CharacterTree.StringHandRight));

        CharacterTree.UpperLegLeft.Init(this, 50, -1, -1, Vector3.right, Vector3.up, rootTransform.Find(CharacterTree.StringUpperLegLeft));
        CharacterTree.LowerLegLeft.Init(this, 50, -1, -1, Vector3.right, Vector3.up, rootTransform.Find(CharacterTree.StringLowerLegLeft));
        CharacterTree.FootLeft.Init(this, 50, -1, -1, Vector3.right, Vector3.up, rootTransform.Find(CharacterTree.StringFootLeft));

        CharacterTree.UpperLegRight.Init(this, 50, 1, -1, Vector3.right, Vector3.up, rootTransform.Find(CharacterTree.StringUpperLegRight));
        CharacterTree.LowerLegRight.Init(this, 50, 1, -1, Vector3.right, Vector3.up, rootTransform.Find(CharacterTree.StringLowerLegRight));
        CharacterTree.FootRight.Init(this, 50, 1, -1, Vector3.right, Vector3.up, rootTransform.Find(CharacterTree.StringFootRight));

        for (int i = 0; i < CharacterTree.NodeList.Count; i++)
        {
            if (CharacterTree.NodeList[i].Parent != null)
            {
                CharacterNode root = CharacterTree.NodeList[i].Data;
                CharacterNode parent = CharacterTree.NodeList[i].Parent.Data;
                root.Joint.axis = root.ControlAxisPrimary;
                root.Joint.secondaryAxis = root.ControlAxisSecondary;
                root.Joint.connectedBody = parent.Rigidbody;
                JointDrive d = new JointDrive();
                d.positionSpring = root.Spring;
                d.positionDamper = root.Damper;
                d.maximumForce = Mathf.Infinity;
                root.Joint.slerpDrive = d;
                root.Joint.xMotion = ConfigurableJointMotion.Locked;
                root.Joint.yMotion = ConfigurableJointMotion.Locked;
                root.Joint.zMotion = ConfigurableJointMotion.Locked;
                root.Joint.rotationDriveMode = RotationDriveMode.Slerp;
            }
        }
    }

    // UTILITIES //

    /// Center of mass in world-space.
    public Vector3 GetCenterOfMass()
    {
        Vector3 CoM = Vector3.zero;
        float sumOfWeights = 0f;

        foreach (TreeNode<CharacterNode> node in CharacterTree.NodeList)
        {
            Rigidbody r = node.Data.Rigidbody;
            CoM += r.worldCenterOfMass * r.mass;
            sumOfWeights += r.mass;
        }
        CoM /= sumOfWeights;
        return CoM;
    }
    /// Returns node in character this transform belongs to.
    public TreeNode<CharacterNode> ContainsTransform(Transform transform)
    {
        for (int i = 0; i < CharacterTree.NodeList.Count; i++)
        {
            if (CharacterTree.NodeList[i].Data.Transform == transform)
                return CharacterTree.NodeList[i];
        }
        return null;
    }

    /// Applies a world-space keyframe to this character.
    public void SetKeyframe(CharacterKeyframe keyframe)
    {
        for (int i = 0; i < keyframe.Tree.NodeList.Count; i++)
            CharacterTree.NodeList[i].Data.SetNodeKeyframe(keyframe.Tree.NodeList[i].Data.GetNodeKeyframe());
    }

    /// Gets the current world-space keyframe of this character.
    public CharacterKeyframe GetKeyframe()
    {
        CharacterKeyframe currentKeyframe = new CharacterKeyframe();
        for (int i = 0; i < CharacterTree.NodeList.Count; i++)
            currentKeyframe.Tree.NodeList[i].Data.SetNodeKeyframe(CharacterTree.NodeList[i].Data.GetNodeKeyframe());

        return currentKeyframe;
    }
}