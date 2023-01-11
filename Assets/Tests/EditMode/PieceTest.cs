using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PieceTest
{
    private readonly Game game = Object.FindObjectOfType<Game>();

    [Test]
    public void WhiteStrCreationTest()
    {
        Assert.AreEqual(true, ContainsWhiteStr(game.SetUpWhite960()));
    }

    [Test]
    public void BlackStrCreationTest()
    {
        var bPieces = game.SetUpBlack960(game.SetUpWhite960());
        Assert.AreEqual(true, ContainsBlackStr(bPieces));
    }

    [Test]
    public void WhiteKingInPlace()
    {
        Assert.AreEqual(true, KingBetweenRooks(game.SetUpWhite960(), "white_rook", "white_king"));
    }

    [Test]
    public void BlackKingInPlace()
    {
        var bPieces = game.SetUpBlack960(game.SetUpWhite960());
        Assert.AreEqual(true, KingBetweenRooks(bPieces, "black_rook", "black_king"));
    }

    private bool ContainsWhiteStr(string[] pieceArr) 
    {
        string[] pieces =
        {
            "white_king", "white_queen", "white_bishop", "white_bishop",
            "white_knight", "white_knight", "white_rook", "white_rook"
        };

        pieceArr = pieceArr.OrderBy(p => p).ToArray();
        pieces = pieces.OrderBy(p => p).ToArray();

        return pieces.SequenceEqual(pieceArr);
    }

    private bool ContainsBlackStr(string[] pieceArr)
    {
        string[] pieces =
        {
            "black_king", "black_queen", "black_bishop", "black_bishop",
            "black_knight", "black_knight", "black_rook", "black_rook"
        };

        pieceArr = pieceArr.OrderBy(p => p).ToArray();
        pieces = pieces.OrderBy(p => p).ToArray();

        return pieces.SequenceEqual(pieceArr);
    }

    private bool KingBetweenRooks(string[] pieceArr, string rook, string king)
    {
        int r1 = -1, k = -1, r2 = -1;

        for (int i = 0; i < pieceArr.Length; i++)
        {
            if (pieceArr[i] == rook && r1 == -1)
            {
                r1 = i;
                continue;
            }

            if (pieceArr[i] == king)
            {
                k = i;
                continue;
            }

            if (pieceArr[i] == rook && r1 >= 0)
            { 
                r2 = i;
                break;
            }
        }

        return (r1 < k) && (r2 > k);
    }
}
