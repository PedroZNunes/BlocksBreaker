using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Level
{
    public string levelText;
    public bool isUnlocked;
}


public class LevelSelectManager : MonoBehaviour
{

    private Color colorOn;
    private Color colorOff;

    public int worldNumber = 1;

    [SerializeField] private Sprite buttonSprite;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private List<Level> LevelList;

    private Transform parent;
    private bool isLoaded = false;

    private GridLayoutGroup gridLayout;
    private float spacingXTallScreen = 10f;

    private RectTransform gridTransform;


    void OnValidate()
    {
        //Fill Text Names
        for (int i = 0; i < LevelList.Count; i++)
        {
            string levelName = string.Format("{0:00}", (i + 1));
            LevelList[i].levelText = levelName;
        }
    }


    void Start()
    {
        SetGridSizeAndSpacing();

        parent = transform;
        TestUnlocked();
    }

    private void SetGridSizeAndSpacing()
    {
        float parentWidth = GetComponentInParent<HorizontalLayoutGroup>().GetComponent<RectTransform>().rect.width;

        this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentWidth);

        gridLayout = GetComponent<GridLayoutGroup>();

        float spacingX = ((parentWidth - 195 * 4 - (gridLayout.padding.left * 2)) / 3f);

        Debug.Log("spacing_size : " + spacingX);

        gridLayout.spacing = new Vector2(spacingX, gridLayout.spacing.y);
}

void TestUnlocked()
{
    print(LevelList.Count);
    for (int i = 0; i < LevelList.Count; i++)
    {
        string levelID = worldNumber.ToString() + LevelList[i].levelText;
        LevelList[i].isUnlocked = PlayerPrefsManager.IsLevelUnlocked(levelID);
    }
}

public void AssignColors(Color colorOn, Color colorOff)
{
    this.colorOn = colorOn;
    this.colorOff = colorOff;
}

public void CreateButtons()
{
    if (!isLoaded)
    {
        for (int i = 0; i < LevelList.Count; i++)
        {
            Level level = LevelList[i];
            GameObject newButton = Instantiate(levelButtonPrefab, parent) as GameObject;
            newButton.transform.localScale = Vector3.one;

            Button button = newButton.GetComponent<Button>();
            button.image.color = colorOn;


            button.transition = Selectable.Transition.ColorTint;
            ColorBlock colors = button.colors;
            colors.disabledColor = colorOff;
            button.colors = colors;


            LevelButtonTemplate buttonTemplate = newButton.GetComponent<LevelButtonTemplate>();
            buttonTemplate.LevelText.text = level.levelText;
            buttonTemplate.isUnlocked = level.isUnlocked;
            buttonTemplate.worldNumber = worldNumber;
        }
        isLoaded = true;
    }
}

}



