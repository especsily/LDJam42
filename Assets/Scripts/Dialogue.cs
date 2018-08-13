using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour 
{
    public SceneChanger sceneManager;
    public Text dialogueText;
    public Text characterNameDialogue;
    public string characterName;
    public string bossName;
    public string[] characterSentences;
    public string[] bossSentences;
    private int index=0;
    private int bossIndex =0;
    public float typingSpeed;
    public GameObject continueButton;

    public Coroutine dialogCoroutine;
    private void Start()
    {
        Type(characterSentences[index]);
        index++;
    }


    void Type(string text)
    {
        if (dialogCoroutine != null) StopCoroutine(dialogCoroutine);
        dialogCoroutine = StartCoroutine(PlayDialog(text));
    }

    IEnumerator PlayDialog(string text) {
        dialogueText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        dialogCoroutine = null;
    }

    public void NextSentence()
    {
        dialogueText.text = "";
        if (bossIndex == index-1)
        {
            characterNameDialogue.text = bossName;
            Type(bossSentences[bossIndex]);
            bossIndex++;
        }
        else if (index < characterSentences.Length)
        {
            characterNameDialogue.text = characterName;
            Type(characterSentences[index]);
            index++;
        }
        else
        {
            dialogueText.text = "";
            continueButton.gameObject.SetActive(false);
            sceneManager.ChangeSceneToGamePlay();
        }
    }
}
