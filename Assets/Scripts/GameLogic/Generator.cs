using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Generator : MonoBehaviour, IGenerator
{
    [SerializeField] private List<GameObject> listNotePrefab;
    public GameObject GenerateNote(int type, Vector3 pos, float speed, Queue<GameObject> listCurrentNote, RectTransform parent)
    {
		List<GameObject> listPrefabBasedOnType = null;
		listPrefabBasedOnType = listNotePrefab.Where(x => x.GetComponent<Note>().GetNoteType() == 0).ToList();
        if (type != 0)
        {
            listPrefabBasedOnType = listNotePrefab.Where(x => x.GetComponent<Note>().GetNoteType() == 0 || x.GetComponent<Note>().GetNoteType() == type).ToList();
        }

        GameObject note = null;
		int rnd = Random.Range(0, listPrefabBasedOnType.Count);
        //pool
        bool needCreate = true;
        var listNote = listCurrentNote.ToList();
        for (int i = 0; i < listNote.Count; i++)
        {
            if (listNote[i].activeSelf)
            {
                continue;
            }
            else
            {
                needCreate = false;
                listNote[i] = listPrefabBasedOnType[rnd];
                listNote[i].SetActive(true);
                (listNote[i].transform as RectTransform).anchoredPosition = pos;
                note = listNote[i];
            }
        }

        if (needCreate)
        {
            var currentNoteConfig = listPrefabBasedOnType[rnd];
            note = Instantiate(currentNoteConfig, Vector3.zero, Quaternion.identity, parent);
            note.GetComponent<RectTransform>().anchoredPosition = pos;
            note.GetComponent<Note>().SetSpeed(speed);
            listCurrentNote.Enqueue(note);
        }
        return note;
    }
}
