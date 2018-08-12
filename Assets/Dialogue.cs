using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    public Text dialogueText;
    public Text characterNameDialogue;
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
            dialogueText.text = "";
            continueButton.gameObject.SetActive(false);
        }
    }
}
