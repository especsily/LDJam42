using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryController : MonoBehaviour {

    public static GalleryController Instance { get; private set; }

    public GalleryList list;

    public GameObject characterSelect;
    public GameObject parent;

    public Text nameText;
    public Text stat1;
    public Text stat2;
    public Text stat3;
    public Text stat4;


    public GameObject avaSelectPanel;
    public GameObject galleryNameImage;
    public GameObject infoListPanel;
    public GameObject pictureList;
    public Image[] picList;


    public Button nextButton;
    public Button prevButton;
    public Button backButton;
    public GameObject manageButton;


    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        for (int i = 0; i < list.listItem.Count; i++)
        {
            GameObject sampleListItem = Instantiate(characterSelect, parent.transform);
            sampleListItem.GetComponent<CharacterScript>().nameText = list.listItem[i].Name;
            sampleListItem.GetComponent<CharacterScript>().stat1 = list.listItem[i].Stat1;
            sampleListItem.GetComponent<CharacterScript>().stat2 = list.listItem[i].Stat2;
            sampleListItem.GetComponent<CharacterScript>().stat3 = list.listItem[i].Stat3;
            sampleListItem.GetComponent<CharacterScript>().stat4 = list.listItem[i].Stat4;

            sampleListItem.GetComponent<CharacterScript>().pic1 = list.listItem[i].pics[0];
            sampleListItem.GetComponent<CharacterScript>().pic2 = list.listItem[i].pics[1];
            sampleListItem.GetComponent<CharacterScript>().pic3 = list.listItem[i].pics[2];
        }
        //for (int i=0; i<item.Length;i++)
        //{
        //    GameObject sampleItem = Instantiate(characterSelect, parent.transform);
        //    sampleItem.GetComponent<CharacterScript>().nameText = item[i].Name;
        //    sampleItem.GetComponent<CharacterScript>().stat1 = item[i].Stat1;
        //    sampleItem.GetComponent<CharacterScript>().stat2 = item[i].Stat2;
        //    sampleItem.GetComponent<CharacterScript>().stat3 = item[i].Stat3;
        //    sampleItem.GetComponent<CharacterScript>().stat4 = item[i].Stat4;

        //    sampleItem.GetComponent<CharacterScript>().pic1 = item[i].pic1;
        //    sampleItem.GetComponent<CharacterScript>().pic2 = item[i].pic2;
        //    sampleItem.GetComponent<CharacterScript>().pic3 = item[i].pic3;

        //}
    }

    public void BackButton()
    {
        galleryNameImage.SetActive(true);
        avaSelectPanel.SetActive(true);
        infoListPanel.SetActive(false);
        pictureList.SetActive(false);
        manageButton.SetActive(false);
        backButton.gameObject.SetActive(false);
        picList[0].gameObject.SetActive(true);
        for (int i = 1; i < picList.Length; i++)
        {
            picList[i].gameObject.SetActive(false);
        }
    }

    public void NextButton()
    {
        picList[0].gameObject.SetActive(false);
        infoListPanel.SetActive(false);
        for (int i=1;i<picList.Length;i++)
        {
            picList[i].gameObject.SetActive(true);
        }
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(true);
    }

    public void PrevButton()
    {
        picList[0].gameObject.SetActive(true);
        for (int i = 1; i < picList.Length; i++)
        {
            picList[i].gameObject.SetActive(false);
        }
        nextButton.gameObject.SetActive(true);
        prevButton.gameObject.SetActive(false);

        infoListPanel.SetActive(true);
    }


    
}
