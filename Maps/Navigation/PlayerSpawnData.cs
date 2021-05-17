using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnData : ScriptableObject
{
    public enum Orientation
    {
        up,
        down,
        left,
        right,
    }

    [Serializable]
    public struct SpawnData
    {
        public string name;
        public string gameObjectName;
        public Orientation orientation; 
    }

    public SpawnData[] spawnData;

    public SpawnData activeSpawn;                      // Spawn to be used after player moves through door or any othe part of the map.

    /// <sumamry>
    /// Set up active spawn data.
    /// </summary>
    /// <param name="key">int - spawn array key</param>
    public void SetActiveSpawn(int key)
    {
        activeSpawn = spawnData[key];
    }
}
