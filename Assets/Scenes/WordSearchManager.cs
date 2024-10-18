using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UIElements;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class WordSearchManager : MonoBehaviour
{
    public static WordSearchManager Instance { get; private set; }

    public Dictionary<string, bool> wordsToFind = new Dictionary<string, bool>();

    //Placeholder list
    private List<string> placeholderWords = new List<string> { "UNITY", "GAME", "TRIE", "CODE", "SEARCH" };

    private string[,] grid;
    private int gridSize = 10;
    private Trie wordTrie;
    public TextMeshProUGUI wordListText;
    private bool usingPlaceholder = false;

    public GameObject layoutGroupObject;
    public GameObject gridLetterPrefab;

    public List<GameObject> inputPrefabs = new List<GameObject>();
    public int currentInputCount = 1;

    public Canvas canvas;
    public GraphicRaycaster graphicRaycaster;
    public bool clicked = false;

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

        if (Application.dataPath.Contains("Words.txt"))
        {
            string[] lines = File.ReadAllLines(@"Words.txt");
            foreach(string line in lines)
            {
                wordsToFind.Add(line, true);
            }
            usingPlaceholder = false;
        }
        else
        {
            //Placeholder logic
            foreach (string word in placeholderWords)
            {
                wordsToFind.Add(word, true);
            }
            usingPlaceholder = true;
        }

        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        wordTrie = new Trie();
        
        switch (usingPlaceholder)
        {
            case false:
                foreach ((string word, bool key) in wordsToFind)
                {
                    wordTrie.Insert(word);
                }
                break;
            case true:
                foreach (string word in placeholderWords)
                {
                    wordTrie.Insert(word);
                }
                break;
        }

        DisplayWords();
        PopulateGrid();
    }

    void PopulateGrid()
    {
        grid = new string[gridSize, gridSize];

        // For simplicity, randomly place letters into the grid
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                grid[i, j] = ((char)Random.Range(65, 91)).ToString(); // Random letter A-Z
                CreateGridLetter(i, j, grid[i, j]);
            }
        }

        // You would also add logic here to place the actual words into the grid
        // For now, we are only randomly generating the grid
    }

    void CreateGridLetter(int row, int col, string letter)
    {
        GameObject gridLetter = Instantiate(gridLetterPrefab, layoutGroupObject.transform);
        gridLetter.GetComponent<TextMeshProUGUI>().text = letter;
    }

    void DisplayWords()
    {
        wordListText.text = "Find the following words:\n";
        if (!usingPlaceholder)
        {
            foreach ((string word, bool key) in wordsToFind) // need to do this differently
            {
                wordListText.text += word + "\n";
            }
        }
        else
        {
            foreach (string word in placeholderWords)
            {
                wordListText.text += word + "\n";
            }
        }
    }

    public void CheckWord(string selectedWord)
    {
        if (wordTrie.Search(selectedWord))
        {
            Debug.Log("Word Found: " + selectedWord);
            // Logic for marking the word as found and updating the UI
        }
        else
        {
            Debug.Log("Word Not Found: " + selectedWord);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckWord(SubmitWord());
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveLastLetter();
        }
        if (Input.GetMouseButtonDown(0) && !clicked)
        {
            ClickLetter();
        }
    }

    public void RemoveLastLetter()
    {
        if (inputPrefabs.Count != 0 && currentInputCount != 0)
        {
            Debug.Log($"{currentInputCount} counter, Letter: '{inputPrefabs[currentInputCount - 1].GetComponent<TextMeshProUGUI>().text}'");
            inputPrefabs[currentInputCount - 1].GetComponent<TextMeshProUGUI>().text = "";
            currentInputCount--;
        }
    }
    public void AddNewLetter(string letter)
    {
        inputPrefabs[currentInputCount].GetComponent<TextMeshProUGUI>().text = letter;
    }

    public string SubmitWord()
    {
        string currentWord = "";
        foreach (GameObject inputPrefab in inputPrefabs)
        {
            string letter = inputPrefab.GetComponent<TextMeshProUGUI>().text;
            if (letter != "")
            {
                currentWord += letter;
            }
        }
        return currentWord;
    }

    public void ClickLetter()
    {
        clicked = true;
        PointerEventData ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();

        graphicRaycaster.Raycast(ped, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<GridLetter>())
            {
                GridLetter resultScript = result.gameObject.GetComponent<GridLetter>();
                resultScript.OnLetterClick();
            }
            Debug.Log($"{results.Count}, {result}\n");
        }
        clicked = false;

    }
}
