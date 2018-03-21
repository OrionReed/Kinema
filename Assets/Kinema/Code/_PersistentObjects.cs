using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;

public class _PersistentObjects : MonoBehaviour
{
    private static _PersistentObjects Instance;

    [SerializeField]
    private GameObject[] spawnObjects;
    private IList<IInitializeOnReload> reinitializeOnLoad;

    void OnEnable() { SceneManager.sceneLoaded += OnLevelFinishedLoading; }
    void OnDisable() { SceneManager.sceneLoaded -= OnLevelFinishedLoading; }

    void Awake()
    {
        if (Instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;
            SpawnObjects();
        }
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < reinitializeOnLoad.Count; i++)
        {
            reinitializeOnLoad[i].InitOnSceneLoad();
        }
    }

    void SpawnObjects()
    {
        for (int i = 0; i < spawnObjects.Length; i++)
            Instantiate(spawnObjects[i], spawnObjects[i].transform.position, spawnObjects[i].transform.rotation);
        reinitializeOnLoad = InterfaceFinder.FindObjects<IInitializeOnReload>();
        Debug.Log("reinits: " + reinitializeOnLoad.Count);
    }
}
