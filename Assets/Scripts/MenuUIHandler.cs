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

    public void WelcomeToBase()
    {
        menuAnimation.Play("FadeOut");
        GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(WelcomeToBaseCoroutine());

        IEnumerator WelcomeToBaseCoroutine()
        {
            yield return new WaitForSeconds(1f);
            ChangeSection("Base");
            menuAnimation.Play("FadeIn");
            GetComponent<CanvasGroup>().interactable = true;
        }
    }

    public void BaseToSettings()
    {
        menuAnimation.Play("FadeOut");
        GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(BaseToSettingsCoroutine());

        IEnumerator BaseToSettingsCoroutine()
        {
            yield return new WaitForSeconds(1f);
            ChangeSection("Settings");
            menuAnimation.Play("FadeIn");
            GetComponent<CanvasGroup>().interactable = true;
        }
    }

    public void SettingsToBase()
    {
        menuAnimation.Play("FadeOut");
        GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(SettingsToBaseCoroutine());

        IEnumerator SettingsToBaseCoroutine()
        {
            yield return new WaitForSeconds(1f);
            ChangeSection("Base");
            menuAnimation.Play("FadeIn");
            GetComponent<CanvasGroup>().interactable = true;
        }
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

    public void BaseToLobby()
    {
        cameraAnimator.SetTrigger("BaseToLobby");
        menuAnimation.Play("FadeOut");
        GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(BaseToSelectionCoroutine());

        IEnumerator BaseToSelectionCoroutine()
        {
            yield return new WaitForSeconds(2f);
            ChangeSection("Lobby");
            menuAnimation.Play("FadeIn");
            GetComponent<CanvasGroup>().interactable = true;
        }
    }

    public void LobbyToBase()
    {
        cameraAnimator.SetTrigger("LobbyToBase");
        menuAnimation.Play("FadeOut");
        GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(BaseToSelectionCoroutine());

        IEnumerator BaseToSelectionCoroutine()
        {
            yield return new WaitForSeconds(2f);
            ChangeSection("Base");
            menuAnimation.Play("FadeIn");
            GetComponent<CanvasGroup>().interactable = true;
        }
    }

    public void LobbyToFight()
    {
        cameraAnimator.SetTrigger("LobbyToFight");
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

    public void FightToMiniBoss()
    {
        gameManager.FightEndedCleanup();
        gameManager.FighterSelected(gameManager.fighterIndex);

        cameraAnimator.SetTrigger("FightToMiniBoss");
        menuAnimation.Play("FadeOut");
        GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(FightToBossCoroutine());

        IEnumerator FightToBossCoroutine()
        {
            yield return new WaitForSeconds(2f);
            ChangeSection("");
            gameManager.StartFight();
        }
    }

    public void MiniBossToBase()
    {
        cameraAnimator.SetTrigger("MiniBossToBase");
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

    public void MiniBossToBoss()
    {
        gameManager.FightEndedCleanup();
        gameManager.FighterSelected(gameManager.fighterIndex);

        cameraAnimator.SetTrigger("MiniBossToBoss");
        menuAnimation.Play("FadeOut");
        GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(FightToBossCoroutine());

        IEnumerator FightToBossCoroutine()
        {
            yield return new WaitForSeconds(2f);
            ChangeSection("");
            gameManager.StartFight();
        }
    }

    public void BossToBase()
    {
        cameraAnimator.SetTrigger("BossToBase");
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

    public void ContinueToNextFight()
    {
        if (gameManager.totalWins == 1) FightToMiniBoss();
        else if (gameManager.totalWins == 2) MiniBossToBoss();
        else BossToBase();
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
