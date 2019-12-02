using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIMenu : MonoBehaviour
{

    protected GameManager gameManager;
    protected Canvas canvas;

    public bool IsActive { get; protected set; }

    protected abstract void OnShow();

    public void Show()
    {
        StartCoroutine(ShowCoroutine());
    }

    protected abstract void OnHide();

    public void Hide()
    {
        StartCoroutine(HideCoroutine());
    }

    public IEnumerator ShowCoroutine()
    {
        IsActive = true;
        canvas.enabled = true;
        OnShow();
        yield return null;
    }

    public IEnumerator HideCoroutine()
    {
        IsActive = false;
        canvas.enabled = false;
        OnHide();
        yield return null;
    }

}
