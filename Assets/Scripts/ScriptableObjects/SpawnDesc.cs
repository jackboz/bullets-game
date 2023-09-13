using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnDescription", order = 1)]
public class SpawnDesc : ScriptableObject
{
        public Vector2Int[] SquadSize;
        public Vector2Int DefaultSize = Vector2Int.one;
}
}