using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "List gallery", menuName = "List gallery")]
public class GalleryList : ScriptableObject {
    public List<GalleryItem> listItem;
}
