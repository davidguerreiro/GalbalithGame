using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu( menuName = "Scriptables" )]
public static class EventsScriptables
{
    [MenuItem( "Assets/Create/Events/Dialogue")]
    /// <summary>
    /// Add Events dialogue scriptable item
    /// in the scriptables menu.
    /// </summary>
    public static void AddDialogueScriptableObject() {

        var asset = ScriptableObject.CreateInstance<DialogueContent>();

        // if needs preconfiguration, add here.

        var path = AssetDatabase.GetAssetPath( Selection.activeObject );
        path += "/NewDialogue.asset";

        ProjectWindowUtil.CreateAsset( asset, path );
    }
}
