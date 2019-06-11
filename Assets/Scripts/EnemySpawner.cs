using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	private GameManager GM;
	private List<GameObject> EnemyObjectPool = new List<GameObject>();
	[SerializeField]
  private GameObject EnemyObject;
  [SerializeField]
	private int maxEnemies;
  private Transform myTransform;

	// Use this for initialization
	void Start () {
		GM = GameManager.instance;
    myTransform = this.transform;
    // subscribe to GM fire event
    GM.EnemySpawners += CreateEnemy;
	}

  public void ClearEnemies() {
    foreach(var enemy in EnemyObjectPool) {
      enemy.SetActive(false);
    }
  }

  public void CreateEnemy() {
    // asks the game manager if it should shoot
    if (GM.DoIShoot(gameObject)) {
      GameObject thisEnemy;

      //see if the pool hit its limit
      if (EnemyObjectPool.Count < maxEnemies) {
        // make new object if not
        thisEnemy = SpawnNewEnemy();
      } else {
        // otherwise don't
        thisEnemy = GrabFromPool();
        thisEnemy.transform.position = myTransform.position;
        thisEnemy.SetActive(true);
      }
    }
  }

  private GameObject SpawnNewEnemy() {
    GameObject newEnemy = GameObject.Instantiate(EnemyObject, myTransform) as GameObject;

    EnemyObjectPool.Add(newEnemy);
    return newEnemy;
  }

  private GameObject GrabFromPool() {
    return EnemyObjectPool.Find(enemy => enemy.activeSelf == false);
  }
}
