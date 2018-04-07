using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKeyframeNode
{
    CharacterKeyframeNode GetNodeKeyframe();
    void SetNodeKeyframe(CharacterKeyframeNode keyframe);
}
