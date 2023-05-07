using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;



public class MenuTextAnimator : MonoBehaviour
{

    private enum DialogType
    {
        StandartDialogue,
        Monologue
    }


    [SerializeField] float delayBetweenLines = 3;
    [SerializeField] TMP_Text skipTextField;
    [SerializeField] TextSO monologueText;
    [SerializeField] float waitTime = 1.5f;
    [SerializeField] int nextSceneIndex;
    [SerializeField] bool loadNextScene;
    [SerializeField] bool playSounds;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] DialogType dialogueType;
    [SerializeField] TextSO dialogueOne;
    [SerializeField] TextSO dialogueTwo;
    [SerializeField] TMP_Text dialogueTextFieldOne;
    [SerializeField] TMP_Text dialogueTextFieldTwo;

    TMP_Text textField => GetComponent<TMP_Text>();
    private float speed = 1f;
    private bool skip = false;
    private Tween typeTween;
    private Tween dialogueTweenOne;
    private Tween dialogueTweenTwo;

    private string skipText = "Press Space to Skip";



    private void Start()
    {
        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(textField);

        switch (dialogueType)
        {
            case DialogType.Monologue:
                StartTyping();
                StartTypingSkipText();
                break;


            case DialogType.StandartDialogue:
                StartDialogue();
                break;
        }
    }


    private void Update()
    {
        SkipCutscene();
    }


    private void StartDialogue()
    {
        StartCoroutine(Dialogue());
    }


    IEnumerator Dialogue()
    {
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < dialogueOne.linesList.Count; i++)
        {
            dialogueTweenOne = dialogueTextFieldOne.DOText(dialogueOne.linesList[i], 20, true).SetSpeedBased().SetEase(Ease.Linear);

            while (dialogueTweenOne.IsPlaying() && !skip)
            {
                SoundManager.instance.PlayTypingSound();
                yield return new WaitForSeconds(0.16f);
            }

            yield return dialogueTweenOne.WaitForCompletion();

            dialogueTweenTwo = dialogueTextFieldTwo.DOText(dialogueTwo.linesList[i], 20, true).SetSpeedBased().SetEase(Ease.Linear);

            while (dialogueTweenTwo.IsPlaying() && !skip)
            {
                SoundManager.instance.PlayTypingSound();
                yield return new WaitForSeconds(0.16f);
            }

            yield return dialogueTweenTwo.WaitForCompletion();
            yield return new WaitForSeconds(delayBetweenLines);

            Tween eraseTween = dialogueTextFieldOne.DOText("", 5).SetSpeedBased().SetEase(Ease.Linear);
            Tween eraseTweenTwo = dialogueTextFieldTwo.DOText("", 5).SetSpeedBased().SetEase(Ease.Linear);
        }
        SceneLoader.instance.StartLoadinScene(nextSceneIndex);
    }




    private void SkipCutscene()
    {
        if (Input.GetKeyDown(KeyCode.Space) && typeTween.IsPlaying())
        {
            skip = true;
        }
    }

    private void StartTyping()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < monologueText.linesList.Count; i++)
        {
            var line = monologueText.linesList[i];
            typeTween = textField.DOText(line, 20, true).SetSpeedBased().SetEase(Ease.Linear);
            while (typeTween.IsPlaying() && !skip)
            {
                SoundManager.instance.PlayTypingSound();
                yield return new WaitForSeconds(0.16f);
            }

            if (playSounds && i == 2)
            {
                SoundManager.instance.PlayScreamSchoolSceneSound();
            }

            if (playSounds && i == 3)
            {
                SoundManager.instance.PlayLaughSchoolSceneSound();
            }

            if (playSounds && i == 6)
            {
                SoundManager.instance.PlayBellSchoolSceneSound();
            }

            if (skip)
            {
                textField.text = line;
                typeTween.Complete();
                skip = false;
                yield return new WaitForSeconds(delayBetweenLines / 2);
            }
            else
            {
                yield return typeTween.WaitForCompletion();
                yield return new WaitForSeconds(delayBetweenLines);
            }
            Tween eraseTween = textField.DOText("", 5).SetSpeedBased().SetEase(Ease.Linear);
            yield return eraseTween.WaitForCompletion();
        }

        if (loadNextScene)
        {
            SceneLoader.instance.StartLoadinScene(nextSceneIndex);
        }
        else
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime * speed;
                yield return null;
            }
        }
    }


    private void StartTypingSkipText()
    {
        StartCoroutine(TypeSkipText());
    }


    IEnumerator TypeSkipText()
    {
        yield return new WaitForSeconds(2f);
        Tween skipTween = skipTextField.DOText(skipText, 10).SetSpeedBased().SetEase(Ease.Linear);
        yield return skipTween.WaitForCompletion();
        yield return new WaitForSeconds(2f);
        skipTextField.DOText("", 10).SetSpeedBased();
    }


}
