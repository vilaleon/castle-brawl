using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    private GameManager gameManager;
    private MenuUIHandler menuHandler;
    private GameObject currentFighter;

    public TextMeshProUGUI nameText;
    public Slider strengthSlider;
    public Slider agilitySlider;
    public Slider enduranceSlider;

    private int index = 0;
    private Vector3 position = new Vector3(9.5f, 1.2f, 8f);
    private Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);

    private void SetFighter()
    {
        Destroy(currentFighter);
        currentFighter = Instantiate(gameManager.fighters[index].obj, position, rotation);
        currentFighter.transform.SetParent(gameObject.transform);
        nameText.text = gameManager.fighters[index].name;
        strengthSlider.value = currentFighter.GetComponent<Fighter>().strength;
        agilitySlider.value = currentFighter.GetComponent<Fighter>().agility;
        enduranceSlider.value = currentFighter.GetComponent<Fighter>().endurance;
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        menuHandler = GetComponentInParent<MenuUIHandler>();
    }

    void OnEnable()
    {
        index = 0;
        SetFighter();
    }

    public void Previous()
    {
        if (--index == -1) index = gameManager.fighters.Length - 1;
        SetFighter();
    }

    public void Next()
    {
        if (++index == gameManager.fighters.Length) index = 0;
        SetFighter();
    }

    public void Back()
    {
        Destroy(currentFighter);
        menuHandler.SelectionToBase();
    }

    public void FighterSelected()
    {
        Destroy(currentFighter);
        gameManager.FighterSelected(index);
        menuHandler.SelectionToFight();
    }
}
