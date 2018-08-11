using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameLogic : MonoBehaviour, IInputReceiver, IInputGiveup, IEnemyAttackReceiver
{
    //Interfaces
    public ICanvasOutputReceiver canvasOutputReceiver;
    public ICanvasInfo canvasInfo;
    public IAudioOutputReceiver audioOutputReceiver;
    public IAudioInfo audioInfo;
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

    private void Start()
    {
        listStackNote = new List<GameObject>();
        listCurrentNote = new Queue<GameObject>();

        comboStack = 0;
        lastBeat = 0;
        crotchet = 60 / audioInfo.GetBpm();
        beatDuration = crotchet * crotchetsPerSpace;
        offset += startCrotchet * crotchet;

        noteSpeed = canvasInfo.GetHalfInputPanelWidth() / crotchet / 2;
        canvasOutputReceiver.DisplaySpaceLeft(true, spaceLeft);
    }

    private void Update()
    {
        songPos = audioInfo.GetSongPosition() - offset;
        isPauseGen = canvasInfo.isFinishAnimation();
        //start the song
        if (songPos >= 0)
        {
            if (!hasOffsetAdjusted)
            {
                hasOffsetAdjusted = true;
                canvasOutputReceiver.MoveBall(canvasInfo.GetSpaceBarWidth() / (beatDuration * 2));
                // heartRateOutputReceiver.SetBlipDuration(beatDuration);
                // heartRateOutputReceiver.StartBlip();
            }
            else
                canvasOutputReceiver.DisplaySongTime(audioInfo.GetSong().length - songPos);
        }

        //test beat
        if (songPos >= (lastBeat + crotchet) && !hasNoteCreated)
        {
            hasNoteCreated = true;

            //create note
            if (!isPauseGen)
            {
                var newNote = generator.GenerateNote(bossType, new Vector3(canvasInfo.GetHalfInputPanelWidth(), 0, 0), noteSpeed, listCurrentNote, canvasInfo.GetComingPanelTransform());
                newNote.GetComponent<Note>().gameController = this;
            }
        }

        if (songPos >= (lastBeat + beatDuration))
        {
            lastBeat += beatDuration;
            Debug.Log("Beat!!!!!!");

            //create note
            if (!isPauseGen)
            {
                var newNote = generator.GenerateNote(bossType, new Vector3(canvasInfo.GetHalfInputPanelWidth(), 0, 0), noteSpeed, listCurrentNote, canvasInfo.GetComingPanelTransform());
                newNote.GetComponent<Note>().gameController = this;
            }
            hasNoteCreated = false;
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

    private bool IsInActivatorRange(GameObject activator, GameObject closestNote, float distance)
    {
        if (distance <= (closestNote.transform as RectTransform).rect.width / 2 + (activator.transform as RectTransform).rect.width / 2)
            return true;
        return false;
    }

    private void ResetStackList(List<GameObject> listStackNote)
    {
        canvasOutputReceiver.RemoveAllStackPanel();
        listStackNote.Clear();
    }

    private void ResetCurrentNoteList(Queue<GameObject> listCurrentNote)
    {
        canvasOutputReceiver.RemoveComingPanel();
        listCurrentNote.Clear();
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
                player.AttackEnemy(damage);

                canvasOutputReceiver.DisplayResult(result, color, comboStack, damage);
                comboStack = 0;
                ResetStackList(listStackNote);
                ResetCurrentNoteList(listCurrentNote);
                isPauseGen = true;

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
                //TO DO: ANIMATION fade out
                comboStack++;
                canvasOutputReceiver.DisplayCombo(comboStack);
            }
            else
            {
                comboStack = 0;
                canvasOutputReceiver.DisplayCombo(comboStack);
                // ResetStackList(listStackNote);
            }
            listCurrentNote.Dequeue();
            closestNote.gameObject.SetActive(false);
        }
        else
        {
            return;
        }
    }

    // ------------------------------------ Note interface methods -------------------------------------
    public void AddToStackNote(GameObject note)
    {
        note.transform.SetParent(canvasInfo.GetStackTransform());
        listStackNote.Add(note);

        if (listStackNote.Count >= 5)
        {
            //TO DO: ANIMATION rung.
            ResetStackList(listStackNote);
            comboStack = 0;
            canvasOutputReceiver.DisplayCombo(comboStack);
        }
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
}
