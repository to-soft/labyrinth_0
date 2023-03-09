using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicEngine : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource[] audioSources;
    private int labyrinthPathLength;
    private int playerCell;
    private int playerUnit;
    private int unit;

    private void Awake()
    {
        List<int> clips = Enumerable.Range(0, audioClips.Length).ToList();
        audioSources = new AudioSource[audioClips.Length];
        for (int i = 0; i < audioClips.Length; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            int j = Random.Range(0, clips.Count);
            source.clip = audioClips[clips[j]];
            clips.RemoveAt(j);
            audioSources[i] = source;
        }

        labyrinthPathLength = LabyrinthState.pathLength;
        unit = (int)Math.Floor((float)(labyrinthPathLength / audioClips.Length));
    }

    void Start()
    {
        foreach (var source in audioSources)
        {
            Debug.Log($"Source found with clip: {source.clip.name}");
        }
        CalculatePlayerUnit();
    }

    private void CalculatePlayerUnit()
    {
        if (Player.DistanceFromGoal == Int32.MaxValue)
        {
            playerCell = 0;
            playerUnit = 0;
            return;
        };
        playerCell = labyrinthPathLength - Player.DistanceFromGoal;
        playerUnit = (int)Math.Ceiling((float)(playerCell / unit));
        Debug.Log($"playerUnit: {playerUnit}");
    }

    private void Update()
    {
        CalculatePlayerUnit();
    }
}
