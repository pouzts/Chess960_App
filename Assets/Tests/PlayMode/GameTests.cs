using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameTests
{
    [UnityTest]
    public IEnumerator RegularGameWhiteTest()
    {
        var go = new GameObject();
        var game = go.AddComponent<Game>();

        var cm_go = new GameObject();
        var cm = cm_go.AddComponent<Chessman>();
        game.chesspiece = cm.gameObject;

        game.StartOGGame();

        yield return null;

        string[] expected = { "white_rook", "white_knight", "white_bishop", "white_queen",
            "white_king", "white_bishop", "white_knight", "white_rook" };

        GameObject[] actual = game.GetPlayerWhite();

        Assert.AreEqual(true, PiecesInOGPos(expected, actual));
    }

    [UnityTest]
    public IEnumerator RegularGameBlackTest()
    {
        var go = new GameObject();
        var game = go.AddComponent<Game>();

        var cm_go = new GameObject();
        var cm = cm_go.AddComponent<Chessman>();
        game.chesspiece = cm.gameObject;

        game.StartOGGame();
        
        yield return null;

        string[] expected = { "black_rook", "black_knight", "black_bishop", "black_queen", 
            "black_king", "black_bishop", "black_knight", "black_rook" };

        GameObject[] actual = game.GetPlayerBlack();

        Assert.AreEqual(true, PiecesInOGPos(expected, actual));
    }

    private bool PiecesInOGPos(string[] expected, GameObject[] actual)
    {
        for (int i = 0; i < expected.Length; i++)
        { 
            if (expected[i] != actual[i].name)
                return false;
        }

        return true;
    }
}
