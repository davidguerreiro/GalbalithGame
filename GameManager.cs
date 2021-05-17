using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Level Components")]
    public LevelMusicManager levelMusicManager;                 // Manages level music logic.

    [Header("UI Components")]
    public OverWorldUI overWorldUI;                             // Manages overworld gameplay UI.
    public GameMenu gameMenu;                                   // Manges main game menu UI.
    public PauseUI pauseUI;                                     // Manages game pause and UI.

    [Header("Game Status")]
    public bool inGameplay = true;                              // Controls wheter we are playing the game or wheter we are in event.
    public bool paused = false;                                 // Controls wheter the game is paused or not.

    void Awake() {
        if ( instance == null ) {
            instance = this;
        }
    }

    /// <summary>
    /// Block game menu.
    /// </summary>
    public void LockGameMenu()
    {
        gameMenu.menuBlocked = true;
    }

    /// <summary>
    /// Unlock game menu.
    /// </summary>
    public void UnlockGameMenu()
    {
        gameMenu.menuBlocked = false;
    }
}
