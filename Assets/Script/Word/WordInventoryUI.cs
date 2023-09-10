using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordInventoryUI : MonoBehaviour
{
    public LetterInventory letterInventory;
    public GameObject letterUIPrefab;
    public InputField wordInputField;

    private Dictionary<char, GameObject> letterUIMap = new Dictionary<char, GameObject>();

    public void UpdateUI()
    {
        // Clear previous UI
        foreach (var item in letterUIMap)
        {
            Destroy(item.Value);
        }
        letterUIMap.Clear();

        // Create new UI
        foreach (var letter in letterInventory.GetLetters())
        {
            GameObject go = Instantiate(letterUIPrefab, transform);
            go.GetComponentInChildren<Text>().text = letter.Key.ToString();
            go.transform.GetChild(1).GetComponent<Text>().text = letter.Value.ToString();
            letterUIMap[letter.Key] = go;
        }
    }

    public void OnSendButtonClick()
    {
        string word = wordInputField.text;
        if (letterInventory.CanFormWord(word))
        {
            letterInventory.RemoveWord(word);
            UpdateUI();
            wordInputField.text = "";
        }
        else
        {
            Debug.Log("Cannot form word from inventory.");
        }
    }
}
