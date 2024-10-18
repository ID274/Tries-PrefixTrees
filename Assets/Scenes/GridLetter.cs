using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class GridLetter : MonoBehaviour
{
    public TextMeshProUGUI letterText;

    private void Start()
    {
        letterText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void OnLetterClick()
    {
        // Logic to handle selecting letters for forming a word
        // This could involve adding the letter to a string, and then checking if it forms a word
        string selectedLetter = letterText.text;
        Debug.Log("Letter clicked: " + selectedLetter);

        // You could build a word from selected letters here and pass it to CheckWord
        if (WordSearchManager.Instance.inputPrefabs.Count > WordSearchManager.Instance.currentInputCount)
        {
            WordSearchManager.Instance.inputPrefabs[WordSearchManager.Instance.currentInputCount].GetComponent<TextMeshProUGUI>().text = selectedLetter;
            WordSearchManager.Instance.AddNewLetter(selectedLetter);
            WordSearchManager.Instance.currentInputCount++;
        }

    }
}
