using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class PopupExit : UIBase
{
    public override void OnOpenStart(Param param = null)
    {
        base.OnOpenStart(param);
    }

    public void OnClickYesButton()
    {
        Application.Quit();
    }

    public void OnClickNoButton()
    {
        Close();
    }

    // public void OnClickSurveyButton()
    // {
    //     Application.OpenURL("https://forms.gle/fs1rMq3BrdYEchs79");
    // }
}