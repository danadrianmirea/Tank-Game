using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friendly : MonoBehaviour
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

        GameObject tankManagerObj = GameObject.Find("TankManager");
        if (tankManagerObj != null)
        {
            tankManager = tankManagerObj.GetComponent<TankManager>();
        }

        tankManager.AddFriendlyToList(this);


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


    }

    public void TakeDamage(float damage)
    {

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
        tankManager.FriendlyKilled(this);
        Object.Destroy(this.gameObject);
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
            tankManager.FriendlyReachedTurret(this);
            Object.Destroy(this.gameObject);
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        switch (mState)
        {
            case State.MOVING:
                Move();
                break;

            case State.DEATH:
                break;
            default:
                break;
        }
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
                        Debug.Log("property set to death");
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
