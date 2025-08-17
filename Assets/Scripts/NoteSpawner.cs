using UnityEngine;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{
    public GameObject normalNotePrefab;
    public GameObject holdNotePrefab;
    public GameObject flickNotePrefab;
    public GameObject endNotePrefab;

    public Transform spawnPoint;
    public float startDelay = 3f;
    public float bpm = 120f;

    private float songTimer = 0f;
    private bool started = false;

    public List<NoteData> notes = new List<NoteData>();
    private int nextNoteIndex = 0;

    void Start()
    {
        LoadNotesFromJSON("song1");
        Invoke(nameof(StartSpawning), startDelay);
    }

    void StartSpawning()
    {
        started = true;
    }

    void Update()
    {
        if (!started) return;

        songTimer += Time.deltaTime;

        while (nextNoteIndex < notes.Count && songTimer >= notes[nextNoteIndex].time)
        {
            SpawnNote(notes[nextNoteIndex]);
            nextNoteIndex++;
        }
    }

    void SpawnNote(NoteData data)
    {
        GameObject prefab = null;

        switch (data.noteType)
        {
            case NoteType.Normal:
                prefab = normalNotePrefab;
                break;
            case NoteType.Hold:
                prefab = holdNotePrefab;
                break;
            case NoteType.Flick:
                prefab = flickNotePrefab;
                break;
            case NoteType.End:
                prefab = endNotePrefab;
                break;
        }

        if (prefab != null)
        {
            GameObject note = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        }
    }

    void LoadNotesFromJSON(string path)
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>(path);

        if (jsonAsset != null)
        {
            NoteChartData chart = JsonUtility.FromJson<NoteChartData>(jsonAsset.text);
            this.bpm = chart.bpm;
            this.startDelay = chart.startDelay;
            this.notes = chart.notes;

            foreach (var note in notes)
            {
                note.time = note.beat * (60f / bpm);
                if (note.noteType == NoteType.Hold)
                {
                    note.duration = (note.endBeat - note.beat) * (60f / bpm);
                }
            }
        }
        else
        {
            Debug.LogError("JSON�t�@�C����������܂���: " + path);
        }
    }
}


// �����̃m�[�c�f�[�^���i�[���邽�߂̃��b�p�[�N���X
[System.Serializable]
public class NoteChartData
{
    public float bpm;
    public float startDelay;
    public List<NoteData> notes;
}


[System.Serializable]
public class NoteData
{
    public float beat; // �J�n��
    public float time; // ���ۂ̃m�[�c�o�����ԁi�v�Z�����j
    public NoteType noteType; // �m�[�c�̎��
    public FlickDirection flickDirection; // �t���b�N�̕����i�t���b�N�m�[�c�p�j

    public float endBeat; // �z�[���h�m�[�c�̏I����
    public float duration; // �z�[���h�m�[�c�̒����i�b�P�ʁj
}

public enum NoteType
{
    Normal, // = 0
    Hold,   // = 1
    Flick,   // = 2
    End
}

public enum FlickDirection
{
    Up,
    Down,
    Left,
    Right
}
