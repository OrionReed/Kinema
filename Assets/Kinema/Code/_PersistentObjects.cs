using System.Collections.Generic;
using UnityEngine;

public class _ReinitializeObjects : MonoBehaviour
{
    private static _ReinitializeObjects instance;

    private IList<IInitializeOnReload> reinitializeList;

    void Awake()
    {
        if (instance)
        {
            Debug.LogWarning("There can only be one instance of _ReinitializeObjects");
            DestroyImmediate(gameObject);
        }
        else
            instance = this;
    }
    void Start()
    {
        reinitializeList = InterfaceFinder.FindObjects<IInitializeOnReload>();
    }
    public static void Reinitialize()
    {
        foreach (IInitializeOnReload init in instance.reinitializeList)
            init.Reinitialize();
    }
}
