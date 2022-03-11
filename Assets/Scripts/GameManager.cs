using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (_isGameOver)
        {
            SceneManager.LoadScene(1); // Game Scence
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
