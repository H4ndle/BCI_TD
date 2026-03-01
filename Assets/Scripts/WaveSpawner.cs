using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System.Reflection;

[Serializable]
public struct SpawnWave
{
    //NOTE: Make sure these arrays are the same length or you're gonna have a bad time.
    //I tried to find a way to enforce this, but it's difficult with a struct.
    public GameObject[] types;
    public int[] counts;
    public float[] rates;
    public float[] delays;
}

public class WaveSpawner : MonoBehaviour
{
    public SpawnWave[] waves;
    SpawnWave currentWave;

    public float[] spawnTimers;
    public int currentWaveCounter;
    

    public Transform enemyParent;

    private void OnMouseDown()
    {
        //For testing
        BeginWave();
    }

    public void BeginWave()
    {
        if (GameManager.instance.waveInProgress) return;

        if (currentWaveCounter < waves.Length)
        {
            currentWave = waves[currentWaveCounter];
            spawnTimers = new float[currentWave.rates.Length];
            Array.Copy(currentWave.rates, spawnTimers, currentWave.rates.Length);

            GameManager.instance.waveInProgress = true;
            StartCoroutine(SpawnerCoroutine(currentWave));
        }
        else
        {
            print("I don't have any more waves. That's all the waves. Stop asking.");
        }
    }

    IEnumerator SpawnerCoroutine(SpawnWave wave)
    {        
        while (GameManager.instance.waveInProgress)
        {


            for (int i = 0; i < spawnTimers.Length; i++)
            {
                if (wave.delays[i] > 0)
                {
                    wave.delays[i] -= Time.deltaTime;
                }

                if (wave.delays[i] <= 0)
                { 
                    //If the spawn timer hasn't expired, count down.
                    if (spawnTimers[i] > 0)
                    {
                        spawnTimers[i] -= Time.deltaTime;
                    }
                    //If it has, spawn the associated enemy, decrease the count for this wave, reset timer.
                    else
                    {
                        if (wave.counts[i] > 0)
                        {
                            Instantiate(wave.types[i], transform.position, Quaternion.identity, enemyParent);
                            wave.counts[i] -= 1;
                            spawnTimers[i] = wave.rates[i];
                        }
                    }
                }
            }

            //Default this placeholder boolean to true, and set it false if there are any unspawned dudes.
            bool spawningComplete = true;
            for (int i = 0; i < wave.counts.Length; i++)
            {
                if (wave.counts[i] > 0)
                {
                    spawningComplete = false;
                }
            }

            //If no more dudes to spawn and the field is clear, set up for the next wave.
            if (spawningComplete && enemyParent.childCount == 0)
            {
                currentWaveCounter++;
                GameManager.instance.waveInProgress = false;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}

[CustomEditor(typeof(WaveSpawner))]
public class WaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WaveSpawner waveSpawner = (WaveSpawner)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Spawn Wave"))
        {
            waveSpawner.BeginWave();
        }
    }
}


