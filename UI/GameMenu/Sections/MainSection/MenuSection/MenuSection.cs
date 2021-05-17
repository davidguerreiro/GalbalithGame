using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSection : MonoBehaviour
{
    public GameMenuButton[] selectables;

    /// <summary>
    /// Set menu section as active section.
    /// </summary>
    /// <param name="id">string - button id to set as focused</param>
    public void MakeNavegable(string id = null)
    {
        if (id == null) {
            selectables[0].onFocus = true;
            selectables[0].cursor.SetActive(true);
            selectables[0].infoText.UpdateContent(selectables[0].info);     
        } else {
            for (int i = 0; i < selectables.Length; i++){
                if (selectables[i].id == id) {
                    selectables[i].onFocus = true;
                    selectables[i].cursor.SetActive(true);
                    selectables[i].infoText.UpdateContent(selectables[i].info);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Set menu section no navegable.
    /// </summary>
    public void DisableNavegable()
    {
        for (int i = 0; i < selectables.Length; i++){
            selectables[i].onFocus = false;
            selectables[i].cursor.SetActive(false);        
        }
    }
}
