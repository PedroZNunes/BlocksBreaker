using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LevelButtonTemplate : MonoBehaviour
{

    //public Text LevelText;
    public TextMeshProUGUI LevelText;
    public bool isUnlocked;
    public int worldNumber;

    private readonly LevelManager levelManager;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.interactable = (isUnlocked);
        button.onClick.AddListener(() => ButtonClicked());
    }

    void ButtonClicked()
    {
        LevelManager.LoadLevel("02 Level_" + worldNumber + LevelText.text);
    }
}
