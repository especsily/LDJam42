using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameLogic : MonoBehaviour, IInputReceiver, IInputGiveup
{
    public ICanvasOutputReceiver canvasOutputReceiver;
    public ICanvasInfo canvasInfo;
    public IAudioOutputReceiver audioOutputReceiver;
    public IHeartRateOutputReceiver heartRateOutputReceiver;
    public IAudioInfo audioInfo;
    public IGenerator generator;

    [Header("Game controller")]
    [SerializeField] private GameObject activator;
    [SerializeField] private float missRange;
    private List<GameObject> listStackNote;
    private Queue<GameObject> listCurrentNote;
    private int comboStack;
    private float noteSpeed;

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
    }

    private void Update()
    {
        songPos = audioInfo.GetSongPosition() - offset;

        //start the song
        if (songPos >= 0)
        {
            if (!hasOffsetAdjusted)
            {
                hasOffsetAdjusted = true;
                // heartRateOutputReceiver.SetBlipDuration(beatDuration);
                // heartRateOutputReceiver.StartBlip();
            }
            else
                canvasOutputReceiver.DisplaySongTime(audioInfo.GetSong().length - songPos);
        }

        //test beat
        if (songPos >= (lastBeat + beatDuration / 2) && !hasNoteCreated)
        {
            hasNoteCreated = true;
            var newNote = generator.GenerateNote(new Vector3(canvasInfo.GetHalfInputPanelWidth(), 0, 0), noteSpeed, listCurrentNote, canvasInfo.GetInputTransform());
            newNote.GetComponent<Note>().gameController = this;
        }
        if (songPos >= (lastBeat + beatDuration))
        {
            lastBeat += beatDuration;
            Debug.Log("Beat!!!!!!");
            var newNote = generator.GenerateNote(new Vector3(canvasInfo.GetHalfInputPanelWidth(), 0, 0), noteSpeed, listCurrentNote, canvasInfo.GetInputTransform());
            newNote.GetComponent<Note>().gameController = this;
            hasNoteCreated = false;
        }
    }

    private List<ComboConfig> GetAvailableCombos(List<Vector2Int> currentInputs, List<ComboConfig> listCombo)
    {
        return listCombo.FindAll(line =>
        {
            for (int i = 0; i < currentInputs.Count; i++)
            {
                if (currentInputs[i] != line.arrows[i]) return false;
                if (currentInputs.Count > line.arrows.Count) return false;
            }
            return true;
        });
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
        heartRateOutputReceiver.PerformBlip(result);
        return result;
    }

    private float GetDistanceClosestNote(GameObject activator, GameObject closestNote)
    {
        return Mathf.Abs(((closestNote.transform as RectTransform).anchoredPosition - (activator.transform as RectTransform).anchoredPosition).magnitude);
    }

    // ------------------------------------ Input interface methods -------------------------------------
    public void OnUserFinish()
    {
    }

    public void OnUserInput(string key)
    {
        if (key == "") return;
        GameObject closestNote = listCurrentNote.ToList()[0];
        float distance = GetDistanceClosestNote(activator, closestNote);
        if (distance <= missRange)
        {
            if (IsInActivatorRange(activator, closestNote, distance) && closestNote.GetComponent<Note>().GetKey() == key)
            {
                //TO DO: ANIMATION fade out
                closestNote.gameObject.SetActive(false);
                listCurrentNote.Dequeue();
                comboStack++;
                canvasOutputReceiver.DisplayCombo(comboStack);
            }
            else
            {
                // comboStack = 0;
                // canvasOutputReceiver.DisplayCombo(comboStack);
                ResetStackList(listStackNote);
            }
        }
        else
        {
            return;
        }
    }

    private bool IsInActivatorRange(GameObject activator, GameObject closestNote, float distance)
    {
        Debug.Log((closestNote.transform as RectTransform).rect.width / 2 + (activator.transform as RectTransform).rect.width / 2);
        if (distance <= (closestNote.transform as RectTransform).rect.width / 2 + (activator.transform as RectTransform).rect.width / 2)
            return true;
        return false;
    }

    private void ResetStackList(List<GameObject> listStackNote)
    {
        listStackNote.Clear();
        canvasOutputReceiver.RemoveAllStackPanel();
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
}
