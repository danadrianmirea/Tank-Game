using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friendly : MonoBehaviour
{
    public float _moveSpeed = 2f;
    protected Rigidbody mRigidBody;
    private Vector3 lookPos;

    public Turret target;

    public float _maxHealth;
    protected float mHealth;

    public enum State
    {
        MOVING=0,
        TAKING_DAMAGE,
        DEATH,
        INACTIVE
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
            case State.INACTIVE:
                break;
            default:
                break;
        }


    }

    public void TakeDamage(float damage)
    {
        if (mState != State.INACTIVE || mState != State.DEATH)
        {
            mHealth -= damage;
            if (mHealth > 0)
                state = State.TAKING_DAMAGE;
            else
                state = State.DEATH;
        }
    }


    //protected void Death()
    //{
    //    //PlaySFX(_clipTankExplode);
    //    StartCoroutine(ChangeState(State.INACTIVE, 1f));
    //}

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

        if (Vector3.Distance(transform.position, target.transform.position) < 1f)
        {
            Object.Destroy(this.gameObject);
            Debug.Log("reached target");
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
                switch (mState)
                {
                    case State.MOVING:
                        break;
                    case State.TAKING_DAMAGE:
                        break;
                    case State.DEATH:
                        //Death();
                        break;
                    case State.INACTIVE:
                        break;
                    default:
                        break;


                }
                mState = value;

            }

        }

    }
}
