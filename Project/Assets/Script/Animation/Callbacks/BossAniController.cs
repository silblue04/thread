using System.Collections;
using System;
using UnityEngine;

public class BossAniController : MonoBehaviour
{
    public Action OnHide = null;

    public void HideEvent()
    {
        OnHide?.Invoke();
        OnHide = null;

        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
