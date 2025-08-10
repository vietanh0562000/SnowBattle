using System.Collections;
using System.Collections.Generic;
using Framework.Tweening;
using UnityEngine;

public class AnimTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            TweenUtils.TweenFloat(this, 1, 2, 1, 0, ETweenType.BounceOut, 
                                  (float value) => { transform.localScale = Vector3.one * value; Debug.Log(value); }, null);
        
	}
}
