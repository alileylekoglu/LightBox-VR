using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public void LoadScene0()
    {
        SceneManager.LoadScene(0);
    }
    
    public void LoadScene1()
    {
        SceneManager.LoadScene(1);
    }
    
   
}
