using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField]
    private Text textLoad;

    private _SceneManager sceneManager;

    private void Start()
    {
        sceneManager = FindObjectOfType<_SceneManager>();
        sceneManager.OnLoadScene += LoadScreen;
    }
    private void OnDisable()
    {
        sceneManager.OnLoadScene -= LoadScreen;
    }

    private void Update()
    {
        if (textLoad.text != "")
            textLoad.color = new Color(textLoad.color.r, textLoad.color.g, textLoad.color.b, Mathf.PingPong(Time.time, 2));
    }

    // Update is called once per frame
    private void LoadScreen(SceneField loadingScene)
    {
        textLoad.text = "Loading Scene: " + loadingScene.SceneName.Substring(loadingScene.SceneName.LastIndexOf("_") + 1);
    }
}
