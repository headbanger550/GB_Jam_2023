using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private WaveSystem[] waveSystem;

    // Start is called before the first frame update
    void Start()
    {
        waveSystem = GetComponentsInChildren<WaveSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < waveSystem.Length; i++)
        {
            if(waveSystem[i].endedWaves == waveSystem.Length)
            {
                Debug.Log("Wave ended :D");
            }
        }
    }
}
