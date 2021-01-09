using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level {
    public Platform[] platforms;

    public Platform[] GetPlatforms() {
        return platforms;
    }
}
