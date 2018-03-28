using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKeyframeNode
{
    KeyframeNode GetNodeKeyframe();
    void SetNodeKeyframe(KeyframeNode keyframe);
}
