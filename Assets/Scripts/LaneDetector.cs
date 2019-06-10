using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneDetector : MonoBehaviour {

  private GameManager GM;
  public int myLane;

  void Start()
  {
    GM = GameManager.instance;
  }

	// Use this for initialization
	void OnTriggerEnter(Collider other)
  {
    if(other.tag == "Player") {
      GM.PadFound(myLane);
    }
  }
}
