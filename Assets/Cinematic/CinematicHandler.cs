using System.Collections;
using TMPro;
using UnityEngine;

public class CinematicHandler : MonoBehaviour
{
    public GameObject board1, board2, board3, menu;
    public TextAsset story1, story2, story3;
    public TextMeshProUGUI story1text, story2text, story3text;

    void Start()
    {
        story1text.text = "";
        story2text.text = "";
        story3text.text = "";
        menu.SetActive(false);
        StartCoroutine(Board1Coroutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
            menu.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StopAllCoroutines();
            if (board1.active)
            {
                board1.SetActive(false);
                StartCoroutine(Board2Coroutine());

            }
            else if (board2.active)
            {
                board2.SetActive(false);
                StartCoroutine(Board3Coroutine());
            }
            else if (board3.active)
            {
                StopCoroutine(Board3Coroutine());
                gameObject.SetActive(false);
                menu.SetActive(true);
            }
        }
    }

    IEnumerator Board1Coroutine()
    {
        board1.SetActive(true);
        foreach (char t in story1.text)
        {
            story1text.text += t;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2f);

        board1.SetActive(false);
        StartCoroutine(Board2Coroutine());
    }
    IEnumerator Board2Coroutine()
    {
        board2.SetActive(true);

        foreach (char t in story2.text)
        {
            story2text.text += t;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2f);

        board2.SetActive(false);
        StartCoroutine(Board3Coroutine());
    }

    IEnumerator Board3Coroutine()
    {
        board3.SetActive(true);

        foreach (char t in story3.text)
        {
            story3text.text += t;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
        menu.SetActive(true);
    }
}
