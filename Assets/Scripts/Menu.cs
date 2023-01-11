using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private Game game;

    public void StartChessGame()
    {
        game.ResetBoard();
        game.StartOGGame();
        gameObject.SetActive(false);
    }

    public void StartChess960()
    { 
        game.ResetBoard();
        game.StartChess960();
        gameObject.SetActive(false);
    }
}
