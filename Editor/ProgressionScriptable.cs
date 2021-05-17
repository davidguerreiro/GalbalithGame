using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ProgressionScriptable
{
    [MenuItem( "Assets/Create/Progresion/LocalVars")]
    /// <summary>
    /// Add Events dialogue scriptable item
    /// in the scriptables menu.
    /// </summary>
    public static void AddLocalVarsScriptableObject() {

        var asset = ScriptableObject.CreateInstance<LocalVars>();

        // if needs preconfiguration, add here.

        var path = AssetDatabase.GetAssetPath( Selection.activeObject );
        path += "/NewLocalVars.asset";

        ProjectWindowUtil.CreateAsset( asset, path );
    }

    [MenuItem( "Assets/Create/Progresion/PlayerData")]
    /// <summary>
    /// Add Player data scriptable item
    /// in the scriptables menu.
    /// </summary>
    public static void AddPlayerDataScriptableObject() {

        var asset = ScriptableObject.CreateInstance<PlayerData>();

        // if needs preconfiguration, add here.

        var path = AssetDatabase.GetAssetPath( Selection.activeObject );
        path += "/NewPlayerData.asset";

        ProjectWindowUtil.CreateAsset( asset, path );
    }
}
