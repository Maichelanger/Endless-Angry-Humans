using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float playTime;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    internal void RecordPlayingTime()
    {
        playTime = Time.timeSinceLevelLoad;
    }

    internal void DestroyGameManager()
    {
        Destroy(gameObject);
    }
}
