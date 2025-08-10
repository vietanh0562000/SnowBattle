using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCurveDumper : MonoBehaviour {

      public AnimationCurve bounceOutCurve = new AnimationCurve(new Keyframe[]
       {
            new Keyframe (0,0,0.07789838f,0.07789838f,0,0.5f),
            new Keyframe (0.5f,1,3.917495f,-3.185179f,0.1066752f,0.5112603f),
            new Keyframe (0.65f,0.75f,0,0,0,0),
            new Keyframe (0.8f,1,3.463799f,-1.871237f,0.5f,0.5f),
            new Keyframe (0.9f,0.9f,0,0,0.15f,0.15f),
            new Keyframe (1.0f,1,2.012381f,0,0.4891294f,0),
       });


    public AnimationCurve b1 = new AnimationCurve(new Keyframe[]
    {
        new Keyframe (0.000000f,0.000000f,10.710800f,10.710800f,0.000000f,0.206804f),
new Keyframe (0.625000f,1.000000f,-4.990141f,3.466648f,0.320586f,0.428737f),
new Keyframe (0.760677f,1.250000f,0.002363f,0.002363f,0.326222f,0.371945f),
new Keyframe (0.900000f,1.000000f,-3.057821f,3.515377f,0.482728f,0.362515f),
new Keyframe (0.950000f,1.101887f,0.000000f,0.000000f,0.603669f,0.241969f),
new Keyframe (1.000000f,1.000000f,-5.013115f,0.000000f,0.352130f,0.000000f),


    });



	// Use this for initialization
	void Start () {
        string s = "";

        for (int i = 0; i < bounceOutCurve.keys.Length; i++)
        {
            Keyframe k = bounceOutCurve.keys[i];
            s += string.Format("new Keyframe ({0:0.000000f},{1:0.000000f},{2:0.000000f},{3:0.000000f},{4:0.000000f},{5:0.000000f}),\n",
                               k.time, k.value,  k.inTangent, k.outTangent, k.inWeight, k.outWeight);
        }

        Debug.Log(s);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
