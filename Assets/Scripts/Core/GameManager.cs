using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;
        
        public bool playerTurn = true;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if(Instance != this)
                Destroy(gameObject);
            
            DontDestroyOnLoad(gameObject);
        }
    }
}
