using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartRateMonitor : MonoBehaviour, IHeartRateOutputReceiver
{
    private enum Result { Perfect, Good, Cool, Bad, Miss };

    [SerializeField] private GameObject BlipPrefab;
    [SerializeField] private float BlipTrailStartSize;
    [SerializeField] private float BlipTrailEndSize;
    [SerializeField] private float RateMonitorWidth;
    [SerializeField] private float RateMonitorHeight;
    [SerializeField] private float BlipSize;
    [SerializeField] private float BlipDurationOffset;
    [SerializeField] private Material BlipMaterial;
    [SerializeField] private bool ShowBlip;
    [SerializeField] private int BaseBlipNum;

    private float BlipDuration;
    private float BlipSpeed;

    private float startPosY;
    [SerializeField] private int coolMultiplier, goodMultiplier, perMultiplier;
    private GameObject oldBlip, newBlip;
    private bool isStart = false;

    void Start()
    {
        startPosY = transform.position.y;
        BlipMaterial.SetColor("_TintColor", Color.white);
    }

    void Update()
    {
        if (newBlip != null && isStart)
        {
            newBlip.transform.localPosition += Vector3.right * BlipSpeed * Time.deltaTime;
            if (newBlip.transform.localPosition.x >= RateMonitorWidth / 2)
            {
                oldBlip = newBlip;
                oldBlip.GetComponent<MeshRenderer>().enabled = false;
                StartCoroutine(WaitThenDestroy(oldBlip, BlipDuration));
                newBlip = CreateBlip(false);
            }
        }
    }

    private GameObject CreateBlip(bool firstCreate)
    {
        var blip = Instantiate(BlipPrefab, Vector3.zero, Quaternion.identity, transform);
        if (firstCreate)
            blip.transform.localPosition = transform.position;
        else
            blip.transform.localPosition = new Vector3(newBlip.transform.localPosition.x - RateMonitorWidth, transform.position.y, transform.position.z);
        blip.transform.localScale = new Vector3(BlipSize, BlipSize, BlipSize);

        var trail = blip.GetComponentInChildren<TrailRenderer>();
        trail.startWidth = BlipTrailStartSize;
        trail.endWidth = BlipTrailStartSize;
        trail.time = BlipDuration - BlipDurationOffset;

        if (!ShowBlip)
        {
            blip.GetComponent<MeshRenderer>().enabled = false;
        }
        return blip;
    }

    private IEnumerator WaitThenDestroy(GameObject blip, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(blip);
    }

    private IEnumerator PerformBlip(GameObject blip, int multiplier)
    {
        blip.GetComponent<MeshRenderer>().enabled = false;
        int numOfBlip = BaseBlipNum + multiplier;
        int midNumber = Mathf.FloorToInt(numOfBlip / 2);
        int sign = -1;
        for (int i = 1; i < numOfBlip; i++)
        {
            sign = sign * (-1);

            blip.transform.localPosition = GetRandomBlipHeight(blip.transform.localPosition, multiplier * 0.2f, i * sign, midNumber);
            yield return new WaitForSeconds(float.MinValue);
            blip.transform.localPosition = new Vector3(blip.transform.localPosition.x, startPosY, blip.transform.localPosition.z);
            yield return new WaitForSeconds(Random.Range(0, 0.05f));
        }

        if(ShowBlip)
            blip.GetComponent<MeshRenderer>().enabled = true;
    }

    private Vector3 GetRandomBlipHeight(Vector3 pos, float multiplier, int index, int midNumber)
    {
        if (index == midNumber)
            return new Vector3(pos.x, index * RateMonitorHeight * multiplier, pos.z);
        return new Vector3(pos.x, Random.Range(0f, index) * RateMonitorHeight * multiplier, pos.z);
    }

    private void SetBlipColor(Color color)
    {
        BlipMaterial.SetColor("_TintColor", color);
    }

    private float RandomPositiveOrNegative()
    {
        return Random.Range(0, 2) * 2 - 1;
    }

    // ------------------------- Interface methods -----------------------------
    public void PerformBlip(string result)
    {
        switch (result)
        {
            case "Perfect":
                SetBlipColor(Color.magenta);
                StartCoroutine(PerformBlip(newBlip, perMultiplier));
                break;
            case "Good":
                SetBlipColor(Color.green);
                StartCoroutine(PerformBlip(newBlip, goodMultiplier));
                break;
            case "Cool":
                SetBlipColor(Color.cyan);
                StartCoroutine(PerformBlip(newBlip, coolMultiplier));
                break;
            case "Bad":
                SetBlipColor(Color.white);
                StartCoroutine(PerformBlip(newBlip, 1));
                break;
            case "Miss":
                SetBlipColor(Color.red);
                break;
        }
    }

    public void StartBlip()
    {
        isStart = true;
        newBlip = CreateBlip(isStart);
    }

    public void SetBlipDuration(float duration)
    {
        BlipDuration = duration;
        BlipSpeed = RateMonitorWidth / BlipDuration;
    }
}
