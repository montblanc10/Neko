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
            Debug.LogError("JSONファイルが見つかりません: " + path);
        }
    }
}


// 複数のノーツデータを格納するためのラッパークラス
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
    public float beat; // 開始拍
    public float time; // 実際のノーツ出現時間（計算される）
    public NoteType noteType; // ノーツの種類
    public FlickDirection flickDirection; // フリックの方向（フリックノーツ用）

    public float endBeat; // ホールドノーツの終了拍
    public float duration; // ホールドノーツの長さ（秒単位）
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
