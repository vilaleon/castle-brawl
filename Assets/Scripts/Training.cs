using UnityEngine;

public class Training : MonoBehaviour
{
    [SerializeField]
    private GameObject[] sections;

    private MenuUIHandler menuHandler;
    private int index = 0;

    private void Awake()
    {
        menuHandler = GetComponentInParent<MenuUIHandler>();
    }

    public void Next()
    {
        if ( index == sections.Length - 1)
        {
            menuHandler.TrainingToBase();
        }
        else
        {
            sections[index++].SetActive(false);
            sections[index].SetActive(true);
        }
    }

    public void Back()
    {
        if (index == 0)
        {
            index = 0;
            menuHandler.TrainingToBase();
        }
        else
        {
            sections[index--].SetActive(false);
            sections[index].SetActive(true);
        }
    }
}
