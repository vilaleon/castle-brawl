using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIHandler : MonoBehaviour
{
    [Serializable]
    private class Section
    {
        public string name;
        public GameObject sectionObject;
    }

    [SerializeField]
    private Section[] sections;

    public void ChangeSection(string name)
    {
        foreach (var section in sections)
        {
            section.sectionObject.SetActive(section.name == name);
        }
    }


    private GameManager gameManager;

    public GameObject baseSection;
    public GameObject trainingSection;


    private Animator cameraAnimator;
    private Animation menuAnimation;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        cameraAnimator = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        menuAnimation = GetComponent<Animation>();
    }

    public void BaseToTraining()
    {
        cameraAnimator.SetTrigger("BaseToTraining");
        menuAnimation.Play("FadeOut");
        GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(BaseToTrainingCoroutine());

        IEnumerator BaseToTrainingCoroutine()
        {
            yield return new WaitForSeconds(2f);
            ChangeSection("Training");
            menuAnimation.Play("FadeIn");
            GetComponent<CanvasGroup>().interactable = true;
        }
    }

    public void TrainingToBase()
    {
        cameraAnimator.SetTrigger("TrainingToBase");
        menuAnimation.Play("FadeOut");
        GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(TrainingToBaseCoroutine());

        IEnumerator TrainingToBaseCoroutine()
        {
            yield return new WaitForSeconds(2f);
            ChangeSection("Base");
            menuAnimation.Play("FadeIn");
            GetComponent<CanvasGroup>().interactable = true;
        }
    }

    public void BaseToSelection()
    {
        cameraAnimator.SetTrigger("BaseToSelection");
        menuAnimation.Play("FadeOut");
        GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(BaseToSelectionCoroutine());

        IEnumerator BaseToSelectionCoroutine()
        {
            yield return new WaitForSeconds(2f);
            ChangeSection("Selection");
            menuAnimation.Play("FadeIn");
            GetComponent<CanvasGroup>().interactable = true;
        }
    }

    public void SelectionToBase()
    {
        cameraAnimator.SetTrigger("SelectionToBase");
        menuAnimation.Play("FadeOut");
        GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(SelectionToBaseCoroutine());

        IEnumerator SelectionToBaseCoroutine()
        {
            yield return new WaitForSeconds(2f);
            ChangeSection("Base");
            menuAnimation.Play("FadeIn");
            GetComponent<CanvasGroup>().interactable = true;
        }
    }

    public void SelectionToFight()
    {
        cameraAnimator.SetTrigger("SelectionToFight");
        menuAnimation.Play("FadeOut");
        GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(SelectionToFightCoroutine());

        IEnumerator SelectionToFightCoroutine()
        {
            yield return new WaitForSeconds(2f);
            ChangeSection("");
            gameManager.StartFight();
        }
    }

    public void FightWin()
    {
        ChangeSection("Win");
        menuAnimation.Play("FadeIn");
        GetComponent<CanvasGroup>().interactable = true;
    }

    public void FightLost()
    {
        ChangeSection("Lost");
        menuAnimation.Play("FadeIn");
        GetComponent<CanvasGroup>().interactable = true;
    }

    public void FightToBase()
    {
        cameraAnimator.SetTrigger("FightToBase");
        menuAnimation.Play("FadeOut");
        GetComponent<CanvasGroup>().interactable = false;
        gameManager.FightEndedCleanup();
        StartCoroutine(FightToBaseCoroutine());

        IEnumerator FightToBaseCoroutine()
        {
            yield return new WaitForSeconds(2f);
            ChangeSection("Base");
            menuAnimation.Play("FadeIn");
            GetComponent<CanvasGroup>().interactable = true;
        }
    }

    public void ExitGame()
    {
        GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(ExitGameCoroutine());

        IEnumerator ExitGameCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
        }
    }
}
