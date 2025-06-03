using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Questions", menuName = "Scriptable Objects/Questions")]
public class Questions : ScriptableObject
{
    [TextArea]
    public string Question;
    public bool isSarah;
}
