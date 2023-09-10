using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterInventory : MonoBehaviour
{
    private Dictionary<char, int> letters = new Dictionary<char, int>();

    public void AddLetter(char letter)
    {
        if (letters.ContainsKey(letter))
            letters[letter]++;
        else
            letters[letter] = 1;
    }

    public Dictionary<char, int> GetLetters()
    {
        return new Dictionary<char, int>(letters);
    }

    public bool CanFormWord(string word)
    {
        Dictionary<char, int> temp = new Dictionary<char, int>(letters);
        foreach (char c in word)
        {
            if (temp.ContainsKey(c) && temp[c] > 0)
                temp[c]--;
            else
                return false;
        }
        return true;
    }

    public void RemoveWord(string word)
    {
        if (CanFormWord(word))
        {
            foreach (char c in word)
            {
                letters[c]--;
                if (letters[c] == 0)
                    letters.Remove(c);
            }
        }
    }
}
