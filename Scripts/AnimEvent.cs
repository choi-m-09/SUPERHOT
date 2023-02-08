using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimEvent : MonoBehaviour
{
    public TimeSystem Time;
    public TMP_Text Text;

    bool Loop = false;
    public void StartAnim()
    {
        this.gameObject.SetActive(false);
        Time.action = false;
    }
    public void EndAnim()
    {
        if(Time.action != true) Time.action = true;
        if (Loop)
        {
            Text.text = "SUPER";
            Loop = !Loop;
        }
        else
        {
            Text.text = "HOT";
            Loop = !Loop;
        }
    }
}
