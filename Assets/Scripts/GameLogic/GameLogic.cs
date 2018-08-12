using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using DG.Tweening;


public class GameLogic : MonoBehaviour, IInputReceiver, IInputGiveup, IEnemyAttackReceiver, ISetGameController
{
    // -------------- Interfaces --------------
    public ICanvasOutputReceiver canvasOutputReceiver;
    public ICanvasInfo canvasInfo;
    public IAudioInfo audioInfo;
    public IAudioReceiver audioController;
    public IGenerator generator;
    public IPlayerAttack player;

    [Header("Game controller")]
    [SerializeField] private GameObject activator;
    [SerializeField] private float missRange;
    private List<GameObject> listStackNote;
    private Queue<GameObject> listCurrentNote;
    private int comboStack;
    private float noteSpeed;
    [SerializeField] private int spaceLeft;
    [SerializeField] private float perMultiplier, goodMultiplier, coolMultiplier, badMultiplier;
    [SerializeField] private float pointPerCombo;
    [SerializeField] private int bossType;
    private bool isPauseGen = false;
    [SerializeField] private GameObject attack1Effect, attack2Effect, attack3Effect, enemyAttackEffect;

    [Header("Calculate beat!!!")]
    [SerializeField] private float missTime;
    [SerializeField] private float crotchetsPerSpace;
    [SerializeField] private float offset; //wait time before play song
    [SerializeField] private int startCrotchet;
    private float crotchet; //duration of a beat
    private float beatDuration; //duration of a space
    private float songPos; //current song pos
    private float lastBeat;
    private bool hasOffsetAdjusted = false;
    private bool hasNoteCreated = false;

    [Header("Generator")]
    [SerializeField] private float GenTime;
    private float timer = 0;
    // private int crotchetStep = 1;

    private void Start()
    {
        listStackNote = new List<GameObject>();
        listCurrentNote = new Queue<GameObject>();

        comboStack = 0;
        lastBeat = 0;
        crotchet = 60 / audioInfo.GetBpm();
        beatDuration = crotchet * crotchetsPerSpace;
        offset += startCrotchet * crotchet;

        noteSpeed = canvasInfo.GetHalfInputPanelWidth() / beatDuration;
        canvasOutputReceiver.DisplaySpaceLeft(true, spaceLeft);

        
    }

    private void Update()
    {
        songPos = audioInfo.GetSongPosition() - offset;

        if (songPos >= -4 && songPos < 0)
        {
            canvasOutputReceiver.DisplayCountDown(true, songPos);
        }

        //start the song
        if (songPos >= 0)
        {
            timer += Time.deltaTime;
            if (!hasOffsetAdjusted)
            {
                hasOffsetAdjusted = true;
                canvasOutputReceiver.MoveBall(canvasInfo.GetSpaceBarWidth() / beatDuration);
                canvasOutputReceiver.DisplayCountDown(false, songPos);
            }
            else
                canvasOutputReceiver.DisplaySongTime(audioInfo.GetSong().length - songPos);
        }

        if (timer >= GenTime && !isPauseGen)
        {
            timer = 0;
            var newNote = generator.GenerateNote(bossType, new Vector3(canvasInfo.GetHalfInputPanelWidth(), 0, 0), noteSpeed, listCurrentNote, canvasInfo.GetComingPanelTransform());
            newNote.GetComponent<Note>().gameController = this;
        }

        if (songPos >= (lastBeat + beatDuration))
        {
            lastBeat += beatDuration;
            canvasOutputReceiver.SpaceEffect();
            Debug.Log("Beat!!!!!!");
        }
    }

    private string CalculateResult(float songPos)
    {
        float timeOffset = 0;
        float currentTimeOffset = songPos - lastBeat;
        //offset in the current space period
        if (currentTimeOffset <= beatDuration / 2)
        {
            timeOffset = Mathf.Abs(songPos - lastBeat);
        }
        else
        {
            var nextBeat = lastBeat + beatDuration;
            timeOffset = Mathf.Abs(nextBeat - songPos);
        }

        Debug.Log(timeOffset);
        //return result
        string result = "";
        if (timeOffset >= missTime) result = "Miss";
        else if (timeOffset >= 0.15) result = "Bad";
        else if (timeOffset >= 0.1) result = "Cool";
        else if (timeOffset >= 0.05) result = "Good";
        else result = "Perfect";
        // heartRateOutputReceiver.PerformBlip(result);
        return result;
    }

    private bool IsInActivatorRange(GameObject activator, GameObject closestNote, float distance)
    {
        if (distance <= (closestNote.transform as RectTransform).rect.width / 2 + (activator.transform as RectTransform).rect.width / 2)
            return true;
        return false;
    }

    private void ResetStackList(List<GameObject> listStackNote)
    {
        listStackNote.Clear();
        canvasOutputReceiver.RemoveAllStackPanel();
    }

    private void ResetCurrentNoteList(Queue<GameObject> listCurrentNote)
    {
        canvasOutputReceiver.RemoveComingPanel();
        listCurrentNote.Clear();
    }

    private Color GetResultColor(string result)
    {
        if (result == "Perfect") return Color.magenta;
        else if (result == "Good") return Color.green;
        else if (result == "Cool") return Color.cyan;
        else if (result == "Bad") return Color.white;
        else return Color.red;
    }

    private float GetResultMultiplier(string result)
    {
        if (result == "Perfect") return perMultiplier;
        else if (result == "Good") return goodMultiplier;
        else if (result == "Cool") return coolMultiplier;
        else if (result == "Bad") return badMultiplier;
        else return 0;
    }

    private float GetDistanceClosestNote(GameObject activator, GameObject closestNote)
    {
        return Mathf.Abs(((closestNote.transform as RectTransform).anchoredPosition - (activator.transform as RectTransform).anchoredPosition).magnitude);
    }

    // ------------------------------------ Input interface methods -------------------------------------
    public void OnUserFinish()
    {
        if (hasOffsetAdjusted
        && !isPauseGen
        && lastBeat != 0)
        {
            if (spaceLeft > 0)
            {
                string result = CalculateResult(songPos);
                Color color = GetResultColor(result);
                float multiplier = GetResultMultiplier(result);

                int damage = Mathf.FloorToInt(multiplier * comboStack * pointPerCombo);
                player.PlayerAttack();

                //player attack
                canvasOutputReceiver.DisplayPlayerAttack(result, color, comboStack, damage);
                comboStack = 0;
                ResetStackList(listStackNote);
                ResetCurrentNoteList(listCurrentNote);
                isPauseGen = true;
                canvasOutputReceiver.SpaceResult(color);
                
                
                audioController.PlaySound("mage_aaa");

                spaceLeft--;
                canvasOutputReceiver.DisplaySpaceLeft(false, spaceLeft);
            }
            else
            {
                spaceLeft = 0;
                canvasOutputReceiver.DisplayRunningOut();
            }
        }
    }

    public void OnUserInput(string key)
    {
        if (key == "" || listCurrentNote.Count == 0) return;
        GameObject closestNote = listCurrentNote.ToList()[0];
        float distance = GetDistanceClosestNote(activator, closestNote);
        if (distance <= missRange)
        {
            if (IsInActivatorRange(activator, closestNote, distance) && closestNote.GetComponent<Note>().GetKey() == key)
            {
                ChangeButtonSprite(true, closestNote);
                if (spaceLeft > 0)
                {
                    comboStack++;
                    canvasOutputReceiver.DisplayCombo(comboStack);
                }
            }
            else
            {
                ChangeButtonSprite(false, closestNote);
                if (spaceLeft > 0)
                {
                    comboStack = 0;
                    canvasOutputReceiver.DisplayCombo(comboStack);
                }
                Camera.main.DOShakePosition(1f, 5f);
                // ResetStackList(listStackNote);
            }
            listCurrentNote.Dequeue();
            StartCoroutine(WaitForSeconds(0.25f, closestNote));
        }
        else
        {
            return;
        }
    }

    private void ChangeButtonSprite(bool isTrue, GameObject closestNote)
    {
        closestNote.GetComponent<Image>().sprite = closestNote.GetComponent<Note>().ReturnActiveNote();
        if (isTrue)
        {
            closestNote.GetComponent<Image>().DOColor(Utilities.ChangeColorAlpha(closestNote.GetComponent<Image>().color, 0), 0.25f);
        }
        else
        {
            closestNote.GetComponent<Image>().DOColor(Utilities.ChangeColorAlpha(Color.red, 0), 0.25f);
        }
    }

    private IEnumerator WaitForSeconds(float waitTime, GameObject closestNote)
    {
        yield return new WaitForSeconds(waitTime);
        closestNote.gameObject.SetActive(false);
    }

    // ------------------------------------ Note interface methods -------------------------------------
    public void AddToStackNote(GameObject note)
    {
        note.transform.SetParent(canvasInfo.GetStackTransform());
        listStackNote.Add(note);

        if (listStackNote.Count >= 5)
        {
            ResetStackList(listStackNote);
            comboStack = 0;
            canvasOutputReceiver.DisplayCombo(comboStack);
            Camera.main.DOShakePosition(1f, 5f);
        }
    }

    public float GetStackLine()
    {
        return -canvasInfo.GetHalfInputPanelWidth() + 64 * listStackNote.Count();
    }

    public float GetMissLine()
    {
        return missRange;
    }

    public void RemoveFromCurrentNote(GameObject note)
    {
        if (listCurrentNote.Contains(note))
            listCurrentNote.Dequeue();
    }


    // ----------------------- Enemy Attack ---------------------------
    public void ResetCombo()
    {
        ResetStackList(listStackNote);
        ResetCurrentNoteList(listCurrentNote);
        comboStack = 0;
    }

    public float GetSongPos()
    {
        return songPos;
    }

    public void DisplayDamage(int damage)
    {
        canvasOutputReceiver.DisplayEnemyAttack(damage);
    }

    // ----------------------- Game controller interface methods ------------------------
    public void SetDelayGenerator(bool isDelay)
    {
        isPauseGen = isDelay;
    }

    public GameObject GetPlayerAttackEffect(int damage)
    {
        if (damage >= 500000)
        {
            return attack3Effect;
        }
        else if (damage >= 100000)
        {
            return attack2Effect;
        }
        else
        {
            return attack1Effect;
        }
    }

    public GameObject GetEnemyAttackEffect()
    {
        return enemyAttackEffect;
    }
}
