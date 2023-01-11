using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    //Reference from Unity IDE
    public GameObject chesspiece;

    //Matrices needed, positions of each of the GameObjects
    //Also separate arrays for the players in order to easily keep track of them all
    //Keep in mind that the same objects are going to be in "positions" and "playerBlack"/"playerWhite"
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    //current turn
    private string currentPlayer = "white";

    //Game Ending
    private bool gameOver = false;

    //Unity calls this right when the game starts, there are a few built in functions
    //that Unity can call for you

    public void StartOGGame()
    {
        playerWhite = new GameObject[] { Create("white_rook", 0, 0), Create("white_knight", 1, 0),
            Create("white_bishop", 2, 0), Create("white_queen", 3, 0), Create("white_king", 4, 0),
            Create("white_bishop", 5, 0), Create("white_knight", 6, 0), Create("white_rook", 7, 0),
            Create("white_pawn", 0, 1), Create("white_pawn", 1, 1), Create("white_pawn", 2, 1),
            Create("white_pawn", 3, 1), Create("white_pawn", 4, 1), Create("white_pawn", 5, 1),
            Create("white_pawn", 6, 1), Create("white_pawn", 7, 1) };
        playerBlack = new GameObject[] { Create("black_rook", 0, 7), Create("black_knight",1,7),
            Create("black_bishop",2,7), Create("black_queen",3,7), Create("black_king",4,7),
            Create("black_bishop",5,7), Create("black_knight",6,7), Create("black_rook",7,7),
            Create("black_pawn", 0, 6), Create("black_pawn", 1, 6), Create("black_pawn", 2, 6),
            Create("black_pawn", 3, 6), Create("black_pawn", 4, 6), Create("black_pawn", 5, 6),
            Create("black_pawn", 6, 6), Create("black_pawn", 7, 6) };

        //Set all piece positions on the positions board
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

    public void StartChess960()
    {
        List<GameObject> whitePieces = new();
        List<GameObject> blackPieces = new();

        // Create arrays of strings that represent pieces
        string[] whiteStrs = SetUpWhite960();
        string[] blackStrs = SetUpBlack960(whiteStrs);

        for (int i = 0; i < whiteStrs.Length; i++)
        {
            whitePieces.Add(Create(whiteStrs[i], i, 0));
            whitePieces.Add(Create("white_pawn", i, 1));
            blackPieces.Add(Create("black_pawn", i, 6));
            blackPieces.Add(Create(blackStrs[i], i, 7));
        }

        playerWhite = whitePieces.ToArray();
        playerBlack = blackPieces.ToArray();

        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>(); //We have access to the GameObject, we need the script
        cm.name = name; //This is a built in variable that Unity has, so we did not have to declare it before
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate(); //It has everything set up so it can now Activate()
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        //Overwrites either empty space or whatever was there
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }
    }

    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            //Using UnityEngine.SceneManagement is needed here
            SceneManager.LoadScene("Game"); //Restarts the game by loading the scene over again
        }
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;

        //Using UnityEngine.UI is needed here
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }

    public void ResetBoard()
    {
        for (int i = 0; i < positions.GetLength(0); i++) 
        {
            for (int j = 0; j < positions.GetLength(1); j++)
            {
                positions[i,j] = null;
            }
        }
    }

    public string[] SetUpWhite960()
    {
        string[] whitePieces = new string[8];

        int r;
        int n = Random.Range(0, 959);

        // First iteration
        r = n % 4; // Divide by 4, take the remainder
        SetWBishop(whitePieces, r, true); // Place a bishop on a light space based on the remainder
        n = (n - r) / 4; // Get the quotient

        // Second iteration
        r = n % 4; // Divide by 4, take the remainder
        SetWBishop(whitePieces, r, false); // Place a bishop on a dark space based on the remainder
        n = (n - r) / 4; // Get the quotient

        // Third iteration
        r = n % 6; // Divide by 6, take the remainder
        SetWQueen(whitePieces, r); // Place the queen on the rth spot
        n = (n - r) / 6; // Get the quotient

        // Fourth iteration
        SetWKnights(whitePieces, n); // Set the knights up based on the quotient

        // Fifth iteration
        SetWRooksKing(whitePieces); // Place the rest of the pieces where the king's between rooks

        return whitePieces;
    }

    private void SetWBishop(string[] arr, int remainder, bool lightPieces)
    {
        switch (remainder) // There's probably a better way to do this
        {
            case 0:
                if (lightPieces)
                    arr[1] = "white_bishop";
                else
                    arr[0] = "white_bishop";
                break;
            case 1:
                if (lightPieces)
                    arr[3] = "white_bishop";
                else
                    arr[2] = "white_bishop";
                break;
            case 2:
                if (lightPieces)
                    arr[5] = "white_bishop";
                else
                    arr[4] = "white_bishop";
                break;
            case 3:
                if (lightPieces)
                    arr[7] = "white_bishop";
                else
                    arr[6] = "white_bishop";
                break;
        }
    }

    private static void SetWQueen(string[] arr, int remainder)
    {
        int position = 0;

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != null) 
                continue;

            if (position >= remainder)
            {
                arr[i] = "white_queen";
                break;
            }

            position++;
        }
    }

    private void SetWKnights(string[] arr, int remainder)
    {
        int[,] knPos = {
            { 0, 0 }, { 0, 1 },
            { 0, 2 }, { 0, 3 },
            { 1, 0 }, { 1, 1 },
            { 1, 2 }, { 2, 0 },
            { 2, 1 }, { 3, 0 }
        };

        int n1Pos = 0;
        int n2Pos = 0;

        int endPos = 0;

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != null)
                continue;

            if (n1Pos >= knPos[remainder, 0])
            {
                arr[i] = "white_knight";
                endPos = i;
                break;
            }

            n1Pos++;
        }

        for (int i = endPos; i < arr.Length; i++)
        {
            if (arr[i] != null)
                continue;

            if (n2Pos >= knPos[remainder, 1])
            {
                arr[i] = "white_knight";
                break;
            }

            n2Pos++;
        }
    }

    private static void SetWRooksKing(string[] arr)
    {
        string pieceStr = "white_rook";

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != null)
                continue;

            arr[i] = pieceStr;

            if (pieceStr == "white_rook")
                pieceStr = "white_king";
            else
                pieceStr = "white_rook";
        }
    }

    public string[] SetUpBlack960(string[] whitePieces)
    { 
        List<string> blackPieces = new();

        foreach (string w in whitePieces)
        {
            switch (w)
            {
                case "white_rook":
                    blackPieces.Add("black_rook");
                    break;
                case "white_knight":
                    blackPieces.Add("black_knight");
                    break;
                case "white_bishop":
                    blackPieces.Add("black_bishop");
                    break;
                case "white_queen":
                    blackPieces.Add("black_queen");
                    break;
                case "white_king":
                    blackPieces.Add("black_king");
                    break;
            }
        }

        return blackPieces.ToArray();
    }

    public GameObject[] GetPlayerWhite() => playerWhite;
    public GameObject[] GetPlayerBlack() => playerBlack;
}
