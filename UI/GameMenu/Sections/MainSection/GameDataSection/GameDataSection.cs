using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataSection : MonoBehaviour
{

    [Header("Components")]
    public TextComponent gold;
    public TextComponent time;

    // Update is called once per frame
    void Update()
    {
        UpdateGoldText();
        UpdateGameTimeText();
    }

    /// <summary>
    /// Update gold in menu screen.
    /// </summary>
    private void UpdateGoldText()
    {
        gold.UpdateContent(Player.instance.data.gold.ToString());
    }

    /// <summary>
    /// Update game time in menu screen.
    /// </summary>
    private void UpdateGameTimeText()
    {
        time.UpdateContent(Player.instance.data.GetTimePlayed());
    }
}
