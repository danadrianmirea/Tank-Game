using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using UnityEngine;

public class TankManager : MonoBehaviour
{

    public float FriendlySpawnTimer = 2.5f;
    public float EnemySpawnTimer = 2.5f;
    public float SpawnDistance = 20;
    public int FriendlySaveTowin = 10;
    public int FriendlyKilledToLose = 2;
    public int EnemyEnterToLose = 1;

    private int friendlySuccessCount = 0;
    private int enemySuccessCount = 0;
    private int friendlyKilled = 0;
    private float friendlyTimer;
    private float enemyTimer;
    private Vector3 distPos;
    private Vector3 dirPos;

    public GameObject _friendlyPrefab;
    public GameObject _enemyPrefab;
    public Turret target;
    public bool continueSpawn = true;
    public GameManager gameManager;

    protected List<Friendly> mFriendly = new List<Friendly>();
    protected List<Enemy> mEnemy = new List<Enemy>();



    public enum State
    {
        GameLoads =0,
        GamePrep,
        GameLoop ,
        GameEnds
    };

    private State mState = State.GameLoads;

    public State state
    {
        get { return mState; }
        set
        {
            if (mState != value)
            {
                mState = value;

                switch (value)
                {
                    case State.GamePrep:
                        //InitGamePrep();
                        break;

                    case State.GameLoop:
                        break;

                    case State.GameEnds:
                        //InitGameEnd();
                        break;

                    default:
                        break;
                }
            }
        }
    }

    //private void InitGamePrep()
    //{
    //    Debug.Log("game prep");
    //    continueSpawn = true;
    //    // Change state to game loop
    //    state = State.GameLoop;
    //}

    //private void InitGameEnd()
    //{
    //    //yield return new WaitForSeconds(3f);

    //    continueSpawn = false;
    //    Restart();

    //    // Reinitialize tanks
    //    state = State.GamePrep;

    //    Debug.Log("gameEnd");
    //}


    // Start is called before the first frame update
    void Start()
    {
        friendlyTimer = FriendlySpawnTimer;
        enemyTimer = EnemySpawnTimer;

        state = State.GamePrep;
    }

    void Awake()
    {
        // Get gameManager from heirachy
        GameObject gameManagerObj = GameObject.Find("GameManager");
        if (gameManagerObj != null)
        {
            gameManager = gameManagerObj.GetComponent<GameManager>();
        }
    }

    //Friendly should add and remove themselves from list
    public void AddFriendlyToList(Friendly tank) {

        mFriendly.Add(tank);
    }

    public void FriendlyReachedTurret(Friendly tank)
    {
        mFriendly.Remove(tank);
        friendlySuccessCount += 1;
    }
    public void FriendlyKilled(Friendly tank)
    {
        friendlyKilled += 1;
        mFriendly.Remove(tank);
    }

    //Enemy-related functions for Enemy
    public void AddEnemyToList(Enemy tank)
    {
        mEnemy.Add(tank);
    }
    public void EnemyReachedTurret(Enemy tank)
    {
        mEnemy.Remove(tank);
        enemySuccessCount += 1;
    }
    public void EnemyKilled(Enemy tank)
    {
        mEnemy.Remove(tank);
    }
    //public Vector3 GetClosestFriendly()
    //{
    //    Friendly closest = mFriendly[0];
    //    if (closest)
    //        return closest.transform.position;
    //    else
    //        return Vector3.positiveInfinity;
    //}

    public Vector3 GetClosestEnemy()
    {
        Enemy closest = mEnemy[0];
        if (closest)
            return closest.transform.position;
        else
            return Vector3.positiveInfinity;
    }

    public int GetFriendlySuccessCount()
    {
        return friendlySuccessCount;
    }
    public int GetFriendlyKilledCount()
    {
        return friendlyKilled;
    }
    public int GetEnemySuccesscount()
    {
        return enemySuccessCount;
    }





    void SpawnFriendly()
    {
        friendlyTimer -= Time.deltaTime;

        if (friendlyTimer <= 0)
        {
            // Set position
            distPos = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            // Set rotation
            dirPos = distPos - target.transform.position;
            dirPos.Normalize();
            dirPos *= SpawnDistance;


            Instantiate(_friendlyPrefab, dirPos, Quaternion.LookRotation(dirPos));

            friendlyTimer = FriendlySpawnTimer;
        }

    }


    void SpawnEnemy()
    {
        enemyTimer -= Time.deltaTime;

        if (enemyTimer <= 0)
        {
            // Set position
            distPos = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            // Set rotation
            dirPos = distPos - target.transform.position;
            dirPos.Normalize();
            dirPos *= SpawnDistance;


            Instantiate(_enemyPrefab, dirPos, Quaternion.LookRotation(dirPos));

            enemyTimer = EnemySpawnTimer;
        }


    }

    public void Restart() {

        
        foreach (Friendly friendly in mFriendly)
        {
            Object.Destroy(friendly.gameObject);
        }

        foreach (Enemy enemy in mEnemy)
        {
            Object.Destroy(enemy.gameObject);
        }


        mEnemy.Clear();
        mFriendly.Clear();

        friendlySuccessCount = 0;
        enemySuccessCount = 0;
        friendlyKilled = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (continueSpawn)
        {
            SpawnFriendly();
            SpawnEnemy();

        }

        if (state == State.GameLoop)
        {

            if (friendlyKilled >= FriendlyKilledToLose || enemySuccessCount > EnemyEnterToLose  )
            {
                Debug.Log("game lost");
                state = State.GameEnds;
            }
            else if (friendlySuccessCount >= FriendlySaveTowin)
            {
                Debug.Log("game won");
                state = State.GameEnds;
                 }

        }

    }
}
