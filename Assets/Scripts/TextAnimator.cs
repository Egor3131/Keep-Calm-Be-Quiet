using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextAnimator : MonoBehaviour
{
    TMP_Text taskText => GetComponent<TMP_Text>();
    [SerializeField] string text;

    private void Start()
    {
        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(taskText);
        taskText.DOText(text, 4);

    }
}