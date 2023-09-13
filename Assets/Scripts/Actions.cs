using System.Collections;
using System.Collections.Generic;
using System;

namespace MyGame
{
    public static class Actions
    {
        public static Action<Enemy> OnEnemyKilled;
        public static Action OnLevelFailed;
        public static Action OnLevelWon;
    }
}
