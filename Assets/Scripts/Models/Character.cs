using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Character : MonoBehaviour, IHealth
{
    //interfaces
    public IMenuReceiver menuController;
    public IAudioReceiver audioController;
    public IGameController gameController;
    public ICharacterCanvasOutput canvasOutput;
    public IHealth otherCharacter;

    [SerializeField] protected AudioClip AttackSounds, HurtSound, IdleSound;
    [SerializeField] protected float HurtAnimTime;
    [SerializeField] protected float MaxHP;
    protected float CurrentHP;
    [SerializeField] protected int Damage;
    [SerializeField] protected Sprite FullImage, HalfImage, FinishImage;
    protected Animator animator;

    private IEnumerator PlayHurtAnimation(Sprite CharacterSprite)
    {
        //TO DO: Play sound and hurt animation
        animator.SetBool("IsHit", true);
        yield return new WaitForSeconds(HurtAnimTime);
        //TO DO: EFFECT
        animator.SetBool("IsHit", false);

        StartCoroutine(ShowCharacterImage(CharacterSprite));
    }


    private IEnumerator ShowCharacterImage(Sprite CharacterSprite)
    {
        float waitTime = 0;
        if (CharacterSprite != null)
        {
            waitTime = canvasOutput.ShowCharacterImage(CharacterSprite);
        }
        yield return new WaitForSeconds(waitTime);
        gameController.SetDelayGenerator(false);
    }

    public void TakeDamage(int damage)
    {
        if (damage != 0)
        {
            CurrentHP -= damage;
            Sprite CharacterSprite = null;

            if (CurrentHP <= MaxHP * 0.6f)
            {
                animator.SetInteger("State", 1);
                CharacterSprite = HalfImage;
            }
            if (CurrentHP <= MaxHP * 0.2f)
            {
                animator.SetInteger("State", 2);
                CharacterSprite = FinishImage;
            }
            if (CurrentHP <= 0)
            {
                CurrentHP = 0;
                if (gameObject.GetComponent<Enemy>() != null)
                {
                    //enemy
                    Time.timeScale = 0;
                }
                else
                {
                    //player
                    menuController.DisplayLosePanel(gameController.GetCurrentDealedDamage(), otherCharacter.GetCurrentHP());
                    audioController.StopMainTheme();
                }
            }
            StartCoroutine(PlayHurtAnimation(CharacterSprite));

            GameObject effect = null;
            if (gameObject.GetComponent<Enemy>() != null)
            {
                canvasOutput.ShowEnemyHealth(CurrentHP / MaxHP);
                effect = gameController.GetPlayerAttackEffect(damage);
                var newEffect = Instantiate(effect, Vector3.zero, Quaternion.identity, gameObject.transform);
                newEffect.transform.localPosition = Vector3.zero;
                gameObject.GetComponent<Enemy>().ResetManaBar();
            }
            if (gameObject.GetComponent<Player>() != null)
            {
                canvasOutput.ShowPlayerHealth(CurrentHP / MaxHP);
                effect = gameController.GetEnemyAttackEffect();
                var newEffect = Instantiate(effect, Vector3.zero, Quaternion.identity, gameObject.transform);
                newEffect.transform.localPosition = new Vector3(0, -40, 0);
            }
        }
        else
        {
            gameController.SetDelayGenerator(false);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        CurrentHP = MaxHP;
    }

    public float GetCurrentHP()
    {
        return CurrentHP;
    }
}
