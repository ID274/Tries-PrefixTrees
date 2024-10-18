using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public static GridGenerator Instance { get; private set; }

    public GameObject gridPrefab, gridLayoutGroup;

    [Header("Grid Settings")]
    public int width;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        for (int i = 0; i < width; i++)
        {
            GameObject prefabInstance = Instantiate(gridPrefab, gridLayoutGroup.transform, false);
            WordSearchManager.Instance.inputPrefabs.Add(prefabInstance);
        }
    }

}
