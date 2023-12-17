using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<char, int> letters = new Dictionary<char, int>();

    public Inventory()
    {
        for (char c = 'a'; c <= 'z'; c++)
        {
            letters[c] = 5;  // Initialize each letter with a count of 10
        }
    }

    public void AddLetters(Dictionary<char, int> letterDict)
    {
        foreach (KeyValuePair<char, int> entry in letterDict)
        {
            if (letters.ContainsKey(entry.Key))
            {
                letters[entry.Key] += entry.Value;
            }
        }
    }

    public bool HasLettersForWord(string word)
    {
        Dictionary<char, int> tempInventory = new Dictionary<char, int>(letters);
        foreach (char c in word)
        {
            if (tempInventory.ContainsKey(c) && tempInventory[c] > 0)
            {
                tempInventory[c]--;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public void DeductLetters(string word)
    {
        foreach (char c in word)
        {
            if (letters.ContainsKey(c) && letters[c] > 0)
            {
                letters[c]--;
            }
        }
    }

    public override string ToString()
    {
        string result = "";
        foreach (KeyValuePair<char, int> entry in letters)
        {
            result += $"{entry.Key}: {entry.Value}, ";
        }
        return result;
    }
}

