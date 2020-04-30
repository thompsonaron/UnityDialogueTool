using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string title;
    public string mainText;
    public List<Options> options;
}
[System.Serializable]
public class Options
{
    public string text;
    public string dialogueToJumpTo;
    public Argument arg;
}

[System.Serializable]
public class Argument {
    public string print;
}

[System.Serializable]
public class Print : Argument
{
    
}