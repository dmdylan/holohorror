using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty { Easy = 3, Normal, Hard }
[CreateAssetMenu(menuName = "Difficulty Object/New Difficulty Object")]
public class DifficultySO : ScriptableObject
{
    public Difficulty GameDifficulty { get; set; }
}
