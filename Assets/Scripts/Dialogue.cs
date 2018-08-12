using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour 
{
    public SceneChanger sceneManager;
    public TMP_Text dialogueText;
    public TMP_Text characterNameDialogue;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public GameObject continueButton;

    private void Start()
    {
        StartCoroutine(Type());
    }

    public void Update()
    {
        if (dialogueText.text == sentences[index])
        {
            continueButton.gameObject.SetActive(true);
        }
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        continueButton.gameObject.SetActive(false);
        if (index < sentences.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Type());
        }
        else
        {
            Debug.Log("fdasfda");
            dialogueText.text = "";
            continueButton.gameObject.SetActive(false);
            sceneManager.ChangeSceneToGamePlay();
        }
    }
}
