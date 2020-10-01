using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

//    public int friendlySuccessCount = 0;
//    public int enemySuccessCount = 0;

//    public TankManager tankManager;

//    public enum State
//    {

//        GameLoads = 0, // when u first enter the scene, load everything
//        GamePrep, // Spwan tanks, and for restart
//        GameLoop, // stay in this state until there is a winner
//        GameEnds
//    };

//    private State mState = State.GameLoads;

//    public State state
//    {
//        get { return mState; }
//        set
//        {
//            if (mState != value)
//            {
//                mState = value;

//                switch (value)
//                {
//                    case State.GamePrep:
//                        InitGamePrep();
//                        break;

//                    case State.GameLoop:
//                        break;

//                    case State.GameEnds:
//                        InitGameEnd();
//                        break;

//                    default:
//                        break;
//                }
//            }
//        }
//    }

//    private void InitGamePrep()
//    {
//        Debug.Log("game prep");
//        tankManager.continueSpawn = true;
//        // Change state to game loop
//        state = State.GameLoop;
//    }

//    private void InitGameEnd()
//    {
//        //yield return new WaitForSeconds(3f);

//        tankManager.continueSpawn = false;
//        tankManager.Restart();
//        Restart();

//        // Reinitialize tanks
//        state = State.GamePrep;

//        Debug.Log("gameEnd");
//    }

//    void Restart()
//    {
//        friendlySuccessCount = 0;
//        enemySuccessCount = 0;
//}


//    // Start is called before the first frame update
//    void Start()
//    {

//        // Get gameManager from heirachy
//        GameObject tankManagerObj = GameObject.Find("TankManager");
//        if (tankManagerObj != null)
//        {
//            tankManager = tankManagerObj.GetComponent<TankManager>();
//        }

//        state = State.GamePrep;
//    }

//    // Update is called once per frame
//    void Update()
//    {

//        if (enemySuccessCount > 0)

//        {
//            Debug.Log(enemySuccessCount);

//        }


//        if (state == State.GameLoop)
//        {

//            if (friendlySuccessCount >= 3 || enemySuccessCount > 0)
//            {
//                state = State.GameEnds;
//            }


//        }

//    }
}
