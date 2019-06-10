using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPulsation : MonoBehaviour {

    [SerializeField]
    private Vector2 spread_Min_Max;
    [SerializeField]
    private float pulseRate;

    private Outline[] outlines;
    private float pingPong;

	// Use this for initialization
	void Start () {
        outlines = this.GetComponents<Outline>();
        pingPong = 60/pulseRate;
	}

    // Update is called once per frame
    void Update()
    {
        float _beat = Mathf.PingPong(Time.time, pingPong);
        float _spread = spread_Min_Max.x + _beat * (spread_Min_Max.y - spread_Min_Max.x);
        Vector2 _ed = new Vector2(_spread, _spread);

        foreach (Outline _out in outlines) _out.effectDistance = _ed;
    }
}
