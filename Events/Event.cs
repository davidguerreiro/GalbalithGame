using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour
{
    public int id;

    [Header("Settings")]
    public string eventName;
    public LocalVars localVars;
    public string actionDisplayed;
    public bool interactable;
    public bool inPlayerSight;
    public bool canBeTriggered;

    protected Coroutine _event; 

    /// <summary>
    /// Trigger event.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public abstract IEnumerator TriggerEvent();

    /// <summary>
    /// Update game UI with action.
    /// </summary>
    protected void UpdateGameUI()
    {
        
    }

    /// <summary>
    /// Check if variables to remove
    /// obstacle are set.
    /// </summary>
    /// <returns>
    protected bool CheckVars() 
    {
        bool readyToTrigger = true;
        foreach (LocalVars.LVars var in localVars.variables) {
            if (!var.value) {
                readyToTrigger = false;
                break;
            }
        }

        return readyToTrigger;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
