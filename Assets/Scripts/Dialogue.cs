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
    public string characterName;
    public string bossName;
    public string[] characterSentences;
    public string[] bossSentences;
    private int index=0;
    private int bossIndex =0;
    public float typingSpeed;
    public GameObject continueButton;

    private void Start()
    {
        StartCoroutine(Type());
        index++;
    }


    IEnumerator Type()
    {
        dialogueText.text = "";
        foreach (char letter in characterSentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator BossType()
    {
        dialogueText.text = "";
        foreach (char bossletter in bossSentences[bossIndex].ToCharArray())
        {
            dialogueText.text += bossletter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        dialogueText.text = "";
        if (bossIndex == index-1)
        {
            characterNameDialogue.text = bossName;
            StartCoroutine(BossType());
            bossIndex++;
        }
        else if (index < characterSentences.Length)
        {
            characterNameDialogue.text = characterName;
            StartCoroutine(Type());
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
