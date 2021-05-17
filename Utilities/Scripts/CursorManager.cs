using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public bool cursorEnabled;                                  // Wheter the cursor is enabled or disabled in the game.

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() 
    {
        CheckCursor();
    }

    /// <summary>
    /// Update cursor based on public
    /// attribute. This attribute is modified
    /// by game logic through the game.
    /// </summary>
    private void CheckCursor() 
    {
        if ( cursorEnabled ) {
            DisplayCursor();
        } else {
            HideCursor();
        }
    }

    /// <summary>
    /// Display cursor.
    /// </summary>
    public void DisplayCursor() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Hide cursor.
    /// </summary>
    public void HideCursor() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
