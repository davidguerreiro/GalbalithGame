using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsLeavingController : MonoBehaviour
{
    [Header("Components")]
    public ClimbingStairs stairsEventTop;
    public ClimbingStairs stairsEventBottom;
    public string leavingFrom;

    private void OnTriggerStay(Collider other) {
        CheckStay(other, stairsEventTop, leavingFrom);
        CheckStay(other, stairsEventBottom, leavingFrom);
    }

    private void OnTriggerExit(Collider other) {
        CheckExit(other, stairsEventTop);
        CheckExit(other, stairsEventBottom);
    }

    /// <summary>
    /// Check wheter is possible to leave and from where.
    /// </summary>
    /// <param name="other">Collider - collider from trigger collision.</param>
    /// <param name="stairsEvent">ClimbingStairs - climbing stairs event.</param>
    /// <param name="leavingFrom">String - from which point the player is leaving the stairs</param>
    private void CheckStay(Collider other, ClimbingStairs stairsEvent, string leavingFrom)
    {
        if ( (stairsEventTop.climbing || stairsEventBottom.climbing ) && !stairsEvent.movingToPoint && other.gameObject.tag == "Player") {
            stairsEvent.readyToLeave = true;
            stairsEvent.leavingFrom = leavingFrom;
            
            Player.instance.playerUI.iconWrapper.Display();
        } else {
            stairsEvent.readyToLeave = false;
            Player.instance.playerUI.iconWrapper.Hide();
        }
    }

    /// <summary>
    /// Check if the player has exited the interaction zone
    /// for leaving.
    /// </summary>
    private void CheckExit(Collider other, ClimbingStairs stairsEvent)
    {
        if ((stairsEventTop.climbing || stairsEventBottom.climbing) && other.gameObject.tag == "Player") {
            stairsEvent.readyToLeave = false;
            Player.instance.playerUI.iconWrapper.Hide();
        }
    }
}
