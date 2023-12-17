using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  
using System.IO;

public class InventoryManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI inventoryText;  // Use TextMeshProUGUI instead of Text
    [SerializeField] private TextMeshProUGUI pointsText;     // Use TextMeshProUGUI instead of Text
    [SerializeField] private TMP_InputField userInputField;  // Use TMP_InputField instead of InputField
    [SerializeField] private Button addLettersButton;
    [SerializeField] private Button formWordButton;

    [Header("Notification Prefab")]
    [SerializeField] private GameObject insufficientLettersPrefab;

    private Inventory inventory = new Inventory();
    private int points = 0;

    private void Start()
    {
        UpdateUI();

        addLettersButton.onClick.AddListener(AddLetters);
        formWordButton.onClick.AddListener(FormWord);
    }

    public void AddLetters()
    {
        string input = userInputField.text.ToLower();
        Dictionary<char, int> lettersToAdd = new Dictionary<char, int>();

        foreach (char c in input)
        {
            if (lettersToAdd.ContainsKey(c))
            {
                lettersToAdd[c]++;
            }
            else
            {
                lettersToAdd[c] = 1;
            }
        }

        inventory.AddLetters(lettersToAdd);
        UpdateUI();
    }

    public void FormWord()
    {
        string word = userInputField.text.ToLower();
        if (inventory.HasLettersForWord(word))
        {
            inventory.DeductLetters(word);
            points += WordPointCalculator.CalculatePoints(word);
            UpdateUI();
        }
        else
        {
            ShowInsufficientLettersNotification(word);  // Pass the word for a custom message
        }
    }

    private void ShowInsufficientLettersNotification(string attemptedWord = "")
    {
        if (insufficientLettersPrefab == null)
        {
            Debug.LogWarning("Insufficient letters prefab is not assigned in the InventoryManager!");
            return;
        }

        // Instantiate the prefab to show the notification
        var notification = Instantiate(insufficientLettersPrefab, transform);

        // Optionally, customize the message based on the attempted word
        TextMeshProUGUI notificationText = notification.GetComponentInChildren<TextMeshProUGUI>();
        if (notificationText != null && !string.IsNullOrEmpty(attemptedWord))
        {
            notificationText.text = $"You don't have enough letters to form '{attemptedWord}'.";
        }

        // Destroy the notification after a few seconds
        Destroy(notification, 3f);
    }




    public void UpdateUI()
    {
        inventoryText.text = inventory.ToString();
        pointsText.text = $"Points: {points}";
    }
}
