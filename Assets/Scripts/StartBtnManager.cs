using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBtnManager : MonoBehaviour
{
    [SerializeField] private string newGameLevel = "Main";

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void NewGameButton()
    {
        SceneManager.LoadScene(newGameLevel);
    }
}
