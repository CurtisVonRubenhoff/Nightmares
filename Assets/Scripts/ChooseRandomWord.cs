using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChooseRandomWord : MonoBehaviour {
  GameManager GM;

	// Use this for initialization
	void Start () {
		var childComponent = transform.GetChild(0);
    var textPro = childComponent.GetComponent<TextMeshPro>();
    GM = GameManager.instance;

    chooseWordAndBeDoneWithIt(ref textPro);
	}
	
	// Update is called once per frame
	void chooseWordAndBeDoneWithIt(ref TextMeshPro comp) {
    var thisChoice = Random.Range(0, wordlist.ListOfWords.Count);
    comp.text = wordlist.ListOfWords[thisChoice];
  }

  void OnTriggerEnter(Collider player)
  {
    if (player.tag == "Player") {
      GM.StartCoroutine(GM.onPlayerHit());
    }
  }
}
