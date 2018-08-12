using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScript : MonoBehaviour {

    public string nameText;
    public string stat1;
    public string stat2;
    public string stat3;
    public string stat4;

    public Sprite pic1;
    public Sprite pic2;
    public Sprite pic3;

    public void OnClick()
    {
        GalleryController.Instance.nameText.text = nameText;
        GalleryController.Instance.stat1.text = stat1;
        GalleryController.Instance.stat2.text = stat2;
        GalleryController.Instance.stat3.text = stat3;
        GalleryController.Instance.stat4.text = stat4;

        GalleryController.Instance.picList[0].sprite = pic1;
        GalleryController.Instance.picList[1].sprite = pic2;
        GalleryController.Instance.picList[2].sprite = pic3;
    }

    public void ChangeToDisplayImage()
    {
        GalleryController.Instance.galleryNameImage.SetActive(false);
        GalleryController.Instance.avaSelectPanel.SetActive(false);
        GalleryController.Instance.infoListPanel.SetActive(true);
        GalleryController.Instance.pictureList.SetActive(true);
        GalleryController.Instance.manageButton.gameObject.SetActive(true);
        GalleryController.Instance.backButton.gameObject.SetActive(true);
        GalleryController.Instance.prevButton.gameObject.SetActive(false);
        GalleryController.Instance.nextButton.gameObject.SetActive(true);
    }
}
