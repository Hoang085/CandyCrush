using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GameDataPersistence
{
    public int DeathCount;
    
    //To save collection require object
    public SerializableDictionary<string,bool> CoinsCollected;
    
    public GameDataPersistence()
    {
        this.DeathCount = 0;
        this.CoinsCollected = new SerializableDictionary<string, bool>();
    }

}
