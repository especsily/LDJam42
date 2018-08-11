using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameLogic : MonoBehaviour, IInputReceiver
{
    [SerializeField] private ListCombo listCombo;
    private List<Vector2Int> currentInputs;
    public ICanvasOutputReceiver canvasOutputReceiver;
    public IAudioOutputReceiver audioOutputReceiver;
    public IHeartRateOutputReceiver heartRateOutputReceiver;
    public IAudioInfo audioInfo;

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
    private bool haveFinished = false;
    private bool isMissed = false;


    private void Start()
    {
        currentInputs = new List<Vector2Int>();
        lastBeat = 0;
        crotchet = 60 / audioInfo.GetBpm();
        beatDuration = crotchet * crotchetsPerSpace;
        offset += startCrotchet * crotchet;
    }

    private void Update()
    {
        songPos = audioInfo.GetSongPosition() - offset;
        //start the song
        if (songPos >= 0 && !hasOffsetAdjusted)
        {
            hasOffsetAdjusted = true;
            heartRateOutputReceiver.SetBlipDuration(beatDuration);
            heartRateOutputReceiver.StartBlip();
        }

        //test beat
        if (songPos >= lastBeat + missTime 
        && !haveFinished //check if you have finish
        && !isMissed     //check if you missed the previous beat
        && lastBeat!= 0) //not in the first beat
        {
            isMissed = true;
            haveFinished = false;
            canvasOutputReceiver.DisplayFinish("Miss");
            heartRateOutputReceiver.PerformBlip("Miss");

            currentInputs = new List<Vector2Int>();
            canvasOutputReceiver.DisplaySuggestCombo(new List<Vector2Int>());
            canvasOutputReceiver.DisplayInputs(currentInputs);
        }

        if(songPos >= lastBeat + missTime + 0.1f && lastBeat != 0)
        {
            haveFinished = false;
        }

        if (songPos >= (lastBeat + beatDuration))
        {
            lastBeat += beatDuration;
            isMissed = false;
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

    // ------------------------------------ Input interface methods -------------------------------------
    public void OnUserFinish()
    {
        haveFinished = true;
        isMissed = false;
        var combos = GetAvailableCombos(currentInputs, listCombo.listComboConfig);
        var currentCombo = combos.Where(x => x.arrows.Count == currentInputs.Count).FirstOrDefault();
        if (currentCombo == null)
        {
            canvasOutputReceiver.DisplayFinish("Miss");
            heartRateOutputReceiver.PerformBlip("Miss");
        }
        else
        {
            canvasOutputReceiver.DisplayFinish(CalculateResult(songPos), currentCombo);
        }
        currentInputs = new List<Vector2Int>();
        canvasOutputReceiver.DisplaySuggestCombo(new List<Vector2Int>());
        canvasOutputReceiver.DisplayInputs(currentInputs);
    }

    public void OnUserInput(Vector2Int arrow)
    {
        if (arrow == Vector2Int.up
        || arrow == Vector2Int.right
        || arrow == Vector2Int.down
        || arrow == Vector2Int.left)
        {
            currentInputs.Add(arrow);
        }

        var combos = GetAvailableCombos(currentInputs, listCombo.listComboConfig);
        if (combos.Count == 0)
        {
            currentInputs = new List<Vector2Int>();
            canvasOutputReceiver.DisplaySuggestCombo(new List<Vector2Int>());
        }
        else
        {
            canvasOutputReceiver.DisplaySuggestCombo(combos.FirstOrDefault().arrows);
        }
        canvasOutputReceiver.DisplayInputs(currentInputs);
    }
}
