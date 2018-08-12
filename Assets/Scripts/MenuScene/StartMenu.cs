using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Image rapier;
    [SerializeField] private Image setTop;
    [SerializeField] private Image setUnder;
    [SerializeField] private Image hLazer;
    [SerializeField] private Image vLazer;
    [SerializeField] private Image logo;
    [SerializeField] private Image tkLogo;
    [SerializeField] private Image VLazerFinal;
    [SerializeField] private Image SetTopFinal;
    [SerializeField] private RectTransform startButtonPanel;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        StartCoroutine(StartSceneAnimation());
    }

    private IEnumerator StartSceneAnimation()
    {
        yield return new WaitForSeconds(0.8f);
        rapier.gameObject.SetActive(true);
        rapier.transform.DOMoveX(-600f, 1f).From();
        yield return new WaitForSeconds(1f);

        hLazer.gameObject.SetActive(true);
        // yield return new WaitForSeconds(hLazer.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForSeconds(0.4f);
        vLazer.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        setTop.gameObject.SetActive(true);
        yield return new WaitForSeconds(setTop.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        setUnder.gameObject.SetActive(true);
        yield return new WaitForSeconds(setUnder.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForSeconds(0.4f);

        logo.gameObject.SetActive(true);
        logo.DOColor(Utilities.ChangeColorAlpha(logo.color, 0f), 0.3f).From();

        yield return new WaitForSeconds(0.9f);
        tkLogo.gameObject.SetActive(true);
        tkLogo.DOColor(Utilities.ChangeColorAlpha(tkLogo.color, 0), 0.3f).From();

		VLazerFinal.gameObject.SetActive(true);
        VLazerFinal.DOColor(Utilities.ChangeColorAlpha(VLazerFinal.color, 0), 0.3f).From();

		SetTopFinal.gameObject.SetActive(true);
        SetTopFinal.DOColor(Utilities.ChangeColorAlpha(SetTopFinal.color, 0), 0.3f).From();

		startButtonPanel.gameObject.SetActive(true);
		foreach (Transform child in startButtonPanel)
		{
			child.gameObject.GetComponent<Image>().DOColor(Utilities.ChangeColorAlpha(child.gameObject.GetComponent<Image>().color, 0), 0.3f).From();
		}
    }
}

