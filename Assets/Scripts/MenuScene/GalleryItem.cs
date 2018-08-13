using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName = "Gallery Item")]
[System.Serializable]
public class GalleryItem {
    public int charId;
    [Header("Gallery")]
    public string charName;
    public string charBirthday;
    public string charSize;
    public string charInterest;
    public List<Sprite> pics; //anh suu tap

    //game scene
    [Header("For Boss only!")]
    public string[] hurtSound, attackSound;
    public float MaxHP;
    public float Damage;
    public AnimatorController charAnimator;
    public float AttackTime;
    public Sprite halfImage, finishImage;
    public GameObject[] AttackEffect;
}
