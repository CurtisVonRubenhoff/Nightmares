using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {


  [SerializeField]
  Image Veil;
  [SerializeField]
  Text Message;
  private bool fadeIn = false;
  private float counter = 0f;

	public void StartGame() {
    StartCoroutine(FadeAndStart());
		 
	}

  private IEnumerator FadeAndStart() {
    fadeIn = true;
    Debug.Log("waiting");
    yield return new WaitForSeconds(1);
    Debug.Log("Done waiting");
    SceneManager.LoadSceneAsync(1);
  }

  public void Update() {
    if (fadeIn){
      Debug.Log("doing something");
      counter += Time.deltaTime;
      var curTex = Message.color;
      var curVeil = Veil.color;

      Message.color = Color.Lerp(curTex, new Color(curTex.r, curTex.g, curTex.b, 1f), counter);
      Veil.color = Color.Lerp(curVeil, new Color(curVeil.r, curVeil.g, curVeil.b, 1f), counter);

    }
  }
}
