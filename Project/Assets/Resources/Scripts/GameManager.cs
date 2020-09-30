using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //private int friendlyCount=0;
    //private int enemyCount=0;

    public FriendlyManager friendlyManager;

    public enum State
    {

        GameLoads = 0, // when u first enter the scene, load everything
        GamePrep, // Spwan tanks, and for restart
        GameLoop, // stay in this state until there is a winner
        GameEnds
    };

    //private State mState = State.GameLoads;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
