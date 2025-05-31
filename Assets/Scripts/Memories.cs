using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "Memories", menuName = "Scriptable Objects/Memories")]
public class Memories : ScriptableObject
{
    public Sprite memory_first;
    public Sprite memory_second;
    public bool isAnswerFirst;
}
