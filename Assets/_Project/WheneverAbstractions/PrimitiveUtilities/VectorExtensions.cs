﻿using System.Collections.Generic;
using UnityEngine;

namespace WheneverAbstractions._Project.WheneverAbstractions.PrimitiveUtilities
{
    public class VectorExtensions
    {
        public static IEnumerable<Vector2> GetAdjacentTiles(Vector2 position)
        {
            yield return position + Vector2.up;
            yield return position + Vector2.down;
            yield return position + Vector2.left;
            yield return position + Vector2.right;
        }
    }
}