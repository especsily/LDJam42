using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionControll : MonoBehaviour {

	public void ChangeToDisplayImage()
    {
        GalleryController.Instance.galleryNameImage.SetActive(false);
        GalleryController.Instance.avaSelectPanel.SetActive(false);
        GalleryController.Instance.infoListPanel.SetActive(true);
        GalleryController.Instance.pictureList.SetActive(true);
        GalleryController.Instance.manageButton.gameObject.SetActive(true);
        GalleryController.Instance.backButton.gameObject.SetActive(true);
        GalleryController.Instance.prevButton.gameObject.SetActive(false);

    }

    public void NextButton()
    {

    }
}
