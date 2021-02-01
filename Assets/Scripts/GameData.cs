using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData {
    public int level;
    public int snapshot;

    public GameData(int level, int snapshot) {
        this.level = level;
        this.snapshot = snapshot;
    }
}
