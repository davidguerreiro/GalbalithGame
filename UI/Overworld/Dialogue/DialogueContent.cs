using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueContent : ScriptableObject
{
    public bool useLocalVars;
    
    [Serializable]
    public struct Dialogue 
    {
        public string speaker;                  // Speaker name.
        public Color color;                     // Speaker box background color.
        [TextArea(5, 30)]
        public string content;                  // Dialogue content.
    }
    
    // public string[] parsedHtml;                 // HTML words parsed array.
    public Dialogue[] dialogue;                 // Dialogue reference.

    [HideInInspector]
    public bool[] localVars;

    /// <summary>
    /// Set local vars.
    /// </summary>
    public void SetLocalVars()
    {
        localVars = new bool[dialogue.Length];
    }

    /// <summary>
    /// Reset local vars.
    /// </summary>
    public void ResetLocalVars()
    {
        for (int i = 0; i < localVars.Length; i++) {
            localVars[i] = false;
        }
    }
}
