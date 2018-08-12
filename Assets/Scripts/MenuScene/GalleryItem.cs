using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName = "Gallery Item")]
[System.Serializable]
public class GalleryItem {

    public string Name;

    public string Stat1;
    public string Stat2;
    public string Stat3;
    public string Stat4;
    public List<Sprite> pics;

    public int characterId;

}
