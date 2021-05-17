using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSections : MonoBehaviour
{
    public bool displayed;
    public GameObject[] sections;

    /// <summary>
    /// Display sections.
    /// </summary>
    public void DisplaySections()
    {
        for (int i = 0; i < sections.Length; i++) {
            sections[i].SetActive(true);
        }
        displayed = true;
    }

    /// <summary>
    /// Hide sections.
    /// </summary>
    public void HideSections()
    {
        for (int i = 0; i < sections.Length; i++) {
            sections[i].SetActive(false);
        }
        displayed = false;
    }

}
