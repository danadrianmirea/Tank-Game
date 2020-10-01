using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Turret : Agent

{
    //temp
    protected string mHorizontalAxisInputName = "Horizontal";
    protected float mHorizontalInputValue = 0f;

    //vars for OnActionReceived
    private int oldFriendlySuccessCount = 0;
    private int oldEnemySuccessCount = 0;
    private int oldFriendlyKilled = 0;

    public bool _inputIsEnabled = true;
    public TankManager tankManager;
    

    private float rotationSpeed = 180f;
    private float turretDamage = 1f;

    protected Rigidbody mRigidbody;
    protected RayPerceptionSensorComponent3D rayPerception;
    protected LineRenderer mLineRenderer;
    protected TankFiringSystem mTankFiringSystem;

    protected string mFireInputName = "Fire1";



    public enum State
    {
        IDLE = 0,
        MOVING,
        INACTIVE
    }
    protected State mState;


    void Start()
    {
        
    }

    void Awake()
    {
        mRigidbody = GetComponent<Rigidbody>();
        //tankManager = GetComponent<TankManager>();
        rayPerception = GetComponent<RayPerceptionSensorComponent3D>();
        mTankFiringSystem = GetComponent<TankFiringSystem>();
        mLineRenderer = GetComponent<LineRenderer>();
        mLineRenderer.SetWidth(0.2f, 0.2f);
        mLineRenderer.enabled = false;

    }

    void Update()
    {
        //MovementInput();
        //FireInput();

    }

    void FixedUpdate()
    {
        //Rotate();
    }

    //public void Rotate()
    //{
    //    float rotationDegree = rotationSpeed * Time.deltaTime * mHorizontalInputValue;
    //    Quaternion rotQuat = Quaternion.Euler(0f, rotationDegree, 0f);
    //    mRigidbody.MoveRotation(mRigidbody.rotation * rotQuat);
    //}





    //Agent Stuff

    public override void OnEpisodeBegin()
    {
        oldFriendlySuccessCount = 0;
        oldEnemySuccessCount = 0;
        oldFriendlyKilled = 0;

        transform.localPosition = new Vector3(0f, 0f, 0f);
        mRigidbody.angularVelocity = Vector3.zero;
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, Vector3.forward);
        transform.localRotation = rot;
        tankManager.Restart();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(mTankFiringSystem.GetCurrCooldown()); //1

        sensor.AddObservation(transform.rotation.y); //1
        sensor.AddObservation(transform.position); //3
        sensor.AddObservation(mRigidbody.angularVelocity.magnitude); //1

        //sensor.AddObservation(tankManager.GetClosestEnemy()); //3
    }

    //protected void Rotate( float dir )
    //{
    //    Debug.Log("rotate");
    //    float rotationDegree = rotationSpeed * Time.deltaTime * dir;
    //    Quaternion rotQuat = Quaternion.Euler(0f, rotationDegree, 0f);
    //    mRigidbody.MoveRotation(mRigidbody.rotation * rotQuat);
    //}

    public void MoveAgent(ActionSegment<int> act)
    {
        var rotateDir = Vector3.zero;
       
        var rotateAxis = act[0];
        var fireInput = act[1];

        switch (fireInput)
        {
            case 1:
                FireInput();
                break;
            default:
                break;

        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * rotationSpeed);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //switch (vectorAction[0])
        //{
        //    case 1:
        //        Rotate(1);
        //        break;
        //    case 2:
        //        Rotate(-1);
        //        break;
        //    case 3:
        //        Rotate(0);
        //        break;
        //}

        //if (vectorAction[1] == 1)
        //{
        //    FireInput();
        //}

        //Set rewards
        if (tankManager.GetFriendlySuccessCount() > oldFriendlySuccessCount)
        {
            SetReward(0.3f);
            oldFriendlySuccessCount = tankManager.GetFriendlySuccessCount();
        }
        else if (tankManager.GetFriendlyKilledCount() >oldFriendlyKilled)
        {
            Debug.Log("killed f");
            SetReward(-0.5f);
            oldFriendlyKilled = tankManager.GetFriendlyKilledCount();
        }
        else if (tankManager.GetEnemySuccesscount() > oldEnemySuccessCount)
        {
            SetReward(-0.7f);
            oldEnemySuccessCount = tankManager.GetEnemySuccesscount();
        }


        if (tankManager.GetFriendlySuccessCount() >= tankManager.FriendlySaveTowin)
        {
            Debug.Log("Game Won");
            SetReward(1.0f);
            EndEpisode();
        }
        else if (tankManager.GetFriendlyKilledCount() >= tankManager.FriendlyKilledToLose)
        {
            Debug.Log("Game Lost");
            SetReward(-1.0f);
            EndEpisode();
        }
        else if (tankManager.GetEnemySuccesscount() >= tankManager.EnemyEnterToLose)
        {
            Debug.Log("Game lost");
            SetReward(-1.0f);
            EndEpisode();
        }


        MoveAgent(actionBuffers.DiscreteActions);


    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut.Clear();
        // rotate
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 2;
        }
        if (Input.GetKey(KeyCode.F))
        {
            discreteActionsOut[1] = 1;
        }

    }

    //public override void Heuristic(float[] actionsOut)
    //{
    //    Debug.Log("heuristic");
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        actionsOut[0] = 1;
    //    }
    //    else if (Input.GetKey(KeyCode.A))
    //    {
    //        actionsOut[0] = 2;
    //    }
    //    else if (Input.GetKey(KeyCode.Q))
    //    {
    //        actionsOut[0] = 3;
    //    }
    //    if (Input.GetKey(KeyCode.F))
    //    {
    //        actionsOut[1] = 1;
    //    }
    //    else
    //    {
    //        actionsOut[1] = 0;
    //    }
    //}



    protected void FireInput()
    {
        //fire shots
        //if (Input.GetButton(mFireInputName))
        
            if (mTankFiringSystem.Fire()) 
            {
                Fire();
            }
        
    }


    protected void MovementInput()
    {
        mHorizontalInputValue = Input.GetAxis(mHorizontalAxisInputName);

        // Check movement and change states according to it
        if ( Mathf.Abs(mHorizontalInputValue) > 0.1f)
            state = State.MOVING;
        else state = State.IDLE;
    }

    protected IEnumerator RayLine(Vector3 v1)
    {
        
        mLineRenderer.SetPosition(0, v1);
        mLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.1f);
        mLineRenderer.enabled = false;
    }

    protected void Fire()
    {
        Debug.Log("fire");
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 20f))
        {
            Vector3 v1 = transform.TransformDirection(Vector3.forward) * hit.distance;
            StartCoroutine(RayLine(v1));
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red, 1f);
            if (hit.transform.tag == "enemy")
            {
                Debug.Log("hit enemy");
                Enemy enemyTank = hit.transform.gameObject.GetComponent<Enemy>();
                enemyTank.TakeDamage(turretDamage);
            }

            else if (hit.transform.tag == "friendly")
            {
                Debug.Log("hit friendly");
                Friendly friendlyTank = hit.transform.gameObject.GetComponent<Friendly>();
                friendlyTank.TakeDamage(turretDamage);
            }
            else
            {
                Debug.Log("unidentified hit obj");
            }
                

            
        }
        else
        {
            Vector3 v1 = transform.TransformDirection(Vector3.forward) * 20f;
            StartCoroutine(RayLine(v1));
        }

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
                    case State.IDLE:
                        break;
                    case State.MOVING:
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
