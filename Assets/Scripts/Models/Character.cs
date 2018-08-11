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
	protected float animState = 0;

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
			animator.SetInteger("State", 1);
        }
        if (CurrentHP <= MaxHP * 0.2f)
        {
            // CurrentHP = 0;
			animator.SetInteger("State", 2);
            // Time.timeScale = 0;
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
