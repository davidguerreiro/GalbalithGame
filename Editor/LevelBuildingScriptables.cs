using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu( menuName = "Scriptables" )]
public class LevelBuildingScriptables
{
    [MenuItem( "Assets/Create/LevelBuilding/PlayerSpawns")]
    /// <summary>
    /// Add Player spawns scriptable item
    /// in the scriptables menu.
    /// </summary>
    public static void AddPlayerSpwnsScriptableObject() {

        var asset = ScriptableObject.CreateInstance<PlayerSpawnData>();

        // if needs preconfiguration, add here.

        var path = AssetDatabase.GetAssetPath( Selection.activeObject );
        path += "/NewPlayerSpawn.asset";

        ProjectWindowUtil.CreateAsset( asset, path );
    }
}
