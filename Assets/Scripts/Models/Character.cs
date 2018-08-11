using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Character : MonoBehaviour, IHealth
{
    [SerializeField] protected Image HealthBar;
    [SerializeField] protected float MaxHP;
    protected float CurrentHP;
    [SerializeField] protected int Damage;
    [SerializeField] protected Sprite FullHPSprite, HalfHPSprite, ZeroHPSprite, FinishSprite;
    protected Animator animator;
    protected SpriteRenderer sr;
    protected AudioSource characterAudio;

    public void PlayHurtAnimation()
    {
        //TO DO: Play sound and hurt animation
    }

    public void TakeDamage(int damage)
    {
        float oldHP = CurrentHP;
        CurrentHP -= damage;
        if (CurrentHP <= MaxHP * 0.6f)
        {
            sr.sprite = HalfHPSprite;
        }
        else if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            sr.sprite = ZeroHPSprite;
            Time.timeScale = 0;
        }
        DOTween.To(() => HealthBar.fillAmount, x => HealthBar.fillAmount = x, CurrentHP / MaxHP, 0.5f);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        characterAudio = GetComponent<AudioSource>();
        CurrentHP = MaxHP;
        sr.sprite = FullHPSprite;
    }
}
