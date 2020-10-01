using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _moveSpeed = 2f;
    protected Rigidbody mRigidBody;
    private Vector3 lookPos;

    public Turret target;
    public TankManager tankManager;

    public float _maxHealth = 1f;
    protected float mHealth;

    public enum State
    {
        MOVING=0,
        TAKING_DAMAGE,
        DEATH
    }
    protected State mState;





    void Awake()
    {
        // Get turret from heirachy
        GameObject turretObj = GameObject.Find("Turret");
        if (turretObj != null)
        {
            target = turretObj.GetComponent<Turret>();
        }

        // Get tank manager
        GameObject tankManagerObj = GameObject.Find("TankManager");
        if (tankManagerObj != null)
        {
            tankManager = tankManagerObj.GetComponent<TankManager>();
        }

        tankManager.AddEnemyToList(this);


        mRigidBody = GetComponent<Rigidbody>();
        //Set rotation
        lookPos = target.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(lookPos);
        // Set health
        mHealth = _maxHealth;


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

        // use state cos u allow outside code to access mState
        switch (state)
        {
            // Can only move when Idle or moving    
            case State.MOVING:
            case State.TAKING_DAMAGE:
                break;
            case State.DEATH:
                break;
            default:
                break;
        }


    }

    public void TakeDamage(float damage)
    {
        Debug.Log("enemy taking damage");
        if ( mState != State.DEATH)
        {
            mHealth -= damage;
            if (mHealth <= 0)
            {
                state = State.DEATH;
            }
            
        }
    }


    protected void Death()
    {
        tankManager.EnemyKilled(this);
        Object.Destroy(this.gameObject);
        Debug.Log("enemy shot");
    }

    private IEnumerator ChangeState(State state, float delay)
    {
        // Delay
        yield return new WaitForSeconds(delay);

        // Change state
        this.state = state;
    }

    protected void Move()
    {
        //Vector3 moveVect = transform.forward * _moveSpeed * Time.deltaTime ; //mVertical is +1 or -1
        //mRigidBody.MovePosition(mRigidBody.position + moveVect);

        float step = _moveSpeed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

        if (Vector3.Distance(transform.position, target.transform.position) < 2f)
        {
            tankManager.EnemyReachedTurret(this);
            Object.Destroy(this.gameObject);
            Debug.Log("enemy reached target");
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    public State state
    {
        get => mState;
        set
        {
            if (mState != value)
            {
                switch (value)
                {
                    case State.MOVING:
                        break;
                    case State.TAKING_DAMAGE:
                        break;
                    case State.DEATH:
                        Death();
                        break;
                    default:
                        break;


                }
                mState = value;

            }

        }

    }
}
