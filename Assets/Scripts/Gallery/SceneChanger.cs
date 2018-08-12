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
        SceneManager.LoadScene(galleryScene);
    }

    public void ChangeSceneToGamePlay()
    {
        SceneManager.LoadScene(gamePlayScene);
    }

    public void ChangeSceneToStartScene()
    {
        SceneManager.LoadScene(startScene);
    }
}
