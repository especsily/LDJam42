using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public string galleryScene;
    public string gamePlayScene;
    public string startScene;

    public void ChangeSceneToGallery()
    {
        TKSceneManager.ChangeScene(galleryScene);
    }

    public void ChangeSceneToGamePlay()
    {
        // TKSceneManager.ChangeScene(gamePlayScene);
        Camera.main.GetComponent<CameraMasker>().MaskChangeScene(gamePlayScene);
    }

    public void ChangeSceneToStartScene()
    {
        TKSceneManager.ChangeScene(startScene);
    }
}
