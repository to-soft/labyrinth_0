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
            audioSources[i].Play();
            audioSources[i].mute = true;
            audioSources[i].loop = true;
        }

    }

    void Start()
    {
        foreach (var source in audioSources)
        {
            source.Play();
            source.loop = true;
            source.mute = true;
        }
        labyrinthPathLength = LabyrinthState.pathLength;
        Debug.Log($"labyrinth length: {labyrinthPathLength} audio clips length: {audioClips.Length}");
        unit = (int)Math.Floor((float)(labyrinthPathLength / audioClips.Length));
        Debug.Log($"unit: {unit}");

        CalculatePlayerUnit();
        Debug.Log($"playerUnit is {playerUnit}");
    }

    private void AddressChoir()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            AudioSource source = audioSources[i];
            if (i < playerUnit)
            {
                source.mute = false;
            }
            else
            {
                source.mute = true;
            }
        }
    }

    private void CalculatePlayerUnit()
    {
        if (Player.DistanceFromGoal > labyrinthPathLength)
        {
            playerCell = -1;
            playerUnit = -1;
            return;
        }
        playerCell = labyrinthPathLength - Player.DistanceFromGoal;
        float _playerCellFloat = playerCell;
        double tmpRaw = _playerCellFloat / unit;
        double tmp = Math.Ceiling(tmpRaw);
        int newUnit = (int)tmp;
        if (newUnit != playerUnit)
        {
            Debug.Log($"playerCell: {playerCell}");
            // Debug.Log($"playerCellFloat: {_playerCellFloat}");
            // Debug.Log($"tmpRaw: {tmpRaw}");
            // Debug.Log($"tmp: {tmp}");
            // Debug.Log($"newUnit: {newUnit}");
            playerUnit = newUnit;
            Debug.Log($"New playerUnit: {playerUnit}");
            AddressChoir();
        }
    }

    private void Update()
    {
        CalculatePlayerUnit();
    }
}
