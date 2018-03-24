using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class _PostProcessing : MonoBehaviour
{

    private PostProcessVolume globalVolume;
    [SerializeField]
    private PostProcessProfile playProfile;
    [SerializeField]
    private PostProcessProfile deadProfile;

    private void Awake() { globalVolume = GetComponent<PostProcessVolume>(); }
    void Start()
    {
        _LevelState.OnPlay += delegate { globalVolume.profile = playProfile; };
        _LevelState.OnDead += delegate { globalVolume.profile = deadProfile; };
    }
}
