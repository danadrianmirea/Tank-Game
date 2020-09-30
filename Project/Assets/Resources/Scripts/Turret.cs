using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour

{

    protected Rigidbody mRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
    }

    //public override void OnEpisodeBegin()
    //{
    //    // if it fell
    //    if (this.transform.localPosition.y < 0)
    //    {
    //        this.mRigidbody.angularVelocity = Vector3.zero;
    //        this.mRigidbody.velocity = Vector3.zero;
    //        this.transform.localPosition = _startingPosition;
    //    }

    //    // Change th eposition of the target at every new epochw
    //    // 4 to the left and 4 to the right
    //    _target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    //}
}
