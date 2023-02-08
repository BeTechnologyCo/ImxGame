using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameContext
{
    private static readonly GameContext instance = new GameContext();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static GameContext()
    {
    }


    public static GameContext Instance { get { return instance; } }


    public string Token { get; set; }
    public string Account { get; set; }

    private GameContext()
    {

    }
}
