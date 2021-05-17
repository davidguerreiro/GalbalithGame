using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : ScriptableObject
{
    public int gold;
    public int steps;

    [Header("Time played")]
    public int hours;
    public int minutes;
    public int seconds;

    [Header("Stadistics")]
    public int battles;
    public int victories;
    public int defeats;

    [Header("Settings")]
    public int maxGold;
    public int maxHours;

    /// <summary>
    /// Update gold.
    /// </summary>
    /// <parma name="amount">int - amount of gold to update</param>
    public void UpdateGold(int amount)
    {
        gold += amount;

        if (gold < 0) {
            gold = 0;
        }

        if (gold > maxGold) {
            gold = maxGold;
        }
    }

    /// <summary>
    /// Get time played displaye.
    /// </summary>
    /// <returns>string</returns>
    public string GetTimePlayed()
    {
        string parsedHours = (hours < 10) ? "0" + hours.ToString() : hours.ToString();
        string parsedMinutes = (minutes < 10) ? "0" + minutes.ToString() : minutes.ToString();
        string parsedSeconds = (seconds < 10) ? "0" + seconds.ToString() : seconds.ToString();

        return parsedHours + ":" + parsedMinutes + ":" + parsedSeconds;
    }

    /// <summary>
    /// Update game played time.
    /// </summary>
    public IEnumerator UpdateGameTimePlayed()
    {
        while(true) {
            yield return new WaitForSecondsRealtime(1f);
            seconds++;

            if (seconds >= 60) {
                seconds = 0;
                minutes++;
            }

            if (minutes >= 60) {
                minutes = 0;
                hours++;
            }

            if (hours >= maxHours) {
                hours = maxHours;
                yield break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// Reset basic player data.
    /// </summary>
    public void ResetBasicPlayerData()
    {
        gold = 0;
        hours = 0;
        minutes = 0;
        seconds = 0;
    }

    /// <summary>
    /// Reset player data.
    /// </summary>
    public void ResetAllPlayerData()
    {
        gold = 0;
        hours = 0;
        minutes = 0;
        seconds = 0;
        battles = 0;
        victories = 0;
        defeats = 0;
        steps = 0;
    }
}
