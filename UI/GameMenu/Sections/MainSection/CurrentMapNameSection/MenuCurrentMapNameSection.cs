using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCurrentMapNameSection : MonoBehaviour
{
    private TextComponent _text;

    private void Awake() {
        if (_text == null) {
            _text = GetComponent<TextComponent>();
        }
        
        UpdateCurrentMapName();
    }

    /// <summary>
    /// Update current map in menu section.
    /// </summary>
    private void UpdateCurrentMapName()
    {
        if (LevelManager.instance != null) {
            _text.UpdateContent(LevelManager.instance.mapName);
        }
    }
}
