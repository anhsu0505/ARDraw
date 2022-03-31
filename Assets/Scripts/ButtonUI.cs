using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private string newGameLevel = "Main";

    public void NewGameButton()
    {
      
            
                SceneManager.LoadScene(newGameLevel);
            }
        
    
}
