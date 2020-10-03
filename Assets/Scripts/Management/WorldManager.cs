using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class World
{
    public int worldNumber;
    public string title;
    public Color colorOn;
    public Color colorOff;
}

public class WorldManager : MonoBehaviour
{

    [SerializeField] private List<World> worldList = new List<World>();
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image nextArrowImage, previousArrowImage;
    [SerializeField] private GameObject worldsGrid;

    private List<LevelSelectManager> gridList;
    private int currentWorldNumber;

    void OnValidate()
    {

        LevelSelectManager[] tempGrids = FindObjectsOfType<LevelSelectManager>();
        if (worldList.Count != tempGrids.Length)
        {
            World[] worldsBackup = worldList.ToArray();
            worldList.Clear();
            int index = 0;
            while (worldList.Count < tempGrids.Length)
            {
                worldList.Add(worldsBackup[index]);
                index++;
            }
        }
        for (int i = 0; i < worldList.Count; i++)
        {
            World world = worldList[i];
            world.worldNumber = i + 1;
        }
    }

    void Awake()
    {
        currentWorldNumber = 1;
        gridList = new List<LevelSelectManager>();
        SortList();
    }

    void Start()
    {
        LoadMainGrid();
        LoadNextGrid();
    }


    void SortList()
    {
        LevelSelectManager[] tempGrids = FindObjectsOfType<LevelSelectManager>();
        List<string> temp = new List<string>();
        for (int i = 0; i < tempGrids.Length; i++)
        {
            temp.Add(tempGrids[i].name.ToString());
        }
        temp.Sort();
        for (int tempIndex = 0; tempIndex < temp.Count; tempIndex++)
        {
            for (int i = 0; i < tempGrids.Length; i++)
            {
                if (temp[tempIndex] == tempGrids[i].name)
                {
                    gridList.Add(tempGrids[i]);
                    break;
                }
                else
                {
                    continue;
                }
            }
        }
    }


    public bool NextWorld()
    {
        if (currentWorldNumber + 1 > worldList.Count)
            return false;

        currentWorldNumber = Mathf.Clamp(currentWorldNumber + 1, 1, worldList.Count);
        Color();
        LoadMainGrid();
        LoadNextGrid();
        return true;
    }

    public bool PreviousWorld()
    {
        if (currentWorldNumber - 1 < 1)
            return false;
        currentWorldNumber = Mathf.Clamp(currentWorldNumber - 1, 1, worldList.Count);
        Color();
        LoadMainGrid();
        return true;
    }



    void LoadGrid(int worldNumber)
    {
        for (int i = 0; i < gridList.Count; i++)
        {
            if (gridList[i].worldNumber == worldNumber)
            {
                gridList[i].AssignColors(worldList[i].colorOn, worldList[i].colorOff);
                gridList[i].CreateButtons();
            }
        }
    }

    void LoadMainGrid()
    {
        LoadGrid(currentWorldNumber);
        title.text = worldList[currentWorldNumber - 1].title;
    }

    void LoadNextGrid()
    {
        int nextWorldnumber = currentWorldNumber + 1;
        LoadGrid(nextWorldnumber);
    }


    void Color()
    {
        Color color = worldList[currentWorldNumber - 1].colorOn;
        previousArrowImage.color = color;
        nextArrowImage.color = color;
        title.color = color;
    }

}
