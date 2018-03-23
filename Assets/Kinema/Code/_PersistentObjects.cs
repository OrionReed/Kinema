using System.Collections.Generic;
using UnityEngine;

public class _ReinitializeObjects : MonoBehaviour
{
    private static _ReinitializeObjects Instance;

    private IList<IInitializeOnReload> reinitializeList;

    void Awake()
    {
        if (Instance)
        {
            Debug.LogWarning("There can only be one instance of _ReinitializeObjects");
            DestroyImmediate(gameObject);
        }
        else
            Instance = this;
    }
    void Start()
    {
        reinitializeList = InterfaceFinder.FindObjects<IInitializeOnReload>();
    }
    public static void Reinitialize()
    {
        foreach (IInitializeOnReload init in Instance.reinitializeList)
            init.Reinitialize();
    }
}
