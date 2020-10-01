using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankFiringSystem : MonoBehaviour
{

    public float _cooldown = 0.75f;

    public enum State
    {
        ReadyToFire = 0,
        OnCooldown
    }

    protected State mState = State.ReadyToFire;

    protected float mCooldownCounter;

    // Start is called before the first frame update
    void Start()
    {
        mCooldownCounter = _cooldown;
    }

    // Update is called once per frame
    void Update()
    {

        switch(state)
        {
            case State.ReadyToFire:
                break;

            case State.OnCooldown:
                mCooldownCounter -= Time.deltaTime;
            if (mCooldownCounter <= 0)
                state = State.ReadyToFire;
            break;

            default:
                break;
        }

    }

    public bool Fire()
    {
        if (state == State.ReadyToFire)
        {
            //change state
            state = State.OnCooldown;
            return true;
        }

        return false;
    }

    public float GetCurrCooldown()
    {
        return mCooldownCounter;
    }


    public State state
    {
        get { return mState; }
        set
        {
            if (mState != value)
            {
                switch (mState)
                {
                    case State.ReadyToFire:
                        break;

                    case State.OnCooldown:
                        mCooldownCounter = _cooldown;
                        break;

                    default:
                        break;
                }

                mState = value;
            }
        }
    }
}
