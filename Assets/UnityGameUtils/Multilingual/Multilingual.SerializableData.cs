using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTextAll
{
    public List<GameText> gameTexts;
}
[System.Serializable]
public class GameText
{
    public string langnuage;
    public List<TextData> textDatas;
}
[System.Serializable]
public class TextData
{
    public string key;
    public string value;
}