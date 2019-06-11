using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WAVE_PATTERN {
  LEFT_LINE,
  FLYING_V,
  RIGHT_LINE,
}

public class GameManager : MonoBehaviour {

	public static GameManager instance;
  [SerializeField]
  private PlayerCharacterMoverScript PC;
	public List<GameObject> spawnerList= new List<GameObject>();
  [SerializeField]
  private GameObject PressurePad;
	[SerializeField]
	private GameObject spawnerBase;
	[SerializeField]
	private int spawnerDistance = 25;
	[SerializeField]
	private int numberOfSpawners = 6;
  [SerializeField]
  private float waveWaitTime = 0;
  private bool isWaitingForWave = false;
  [HideInInspector]
  public bool gameIsRunning = false;
  private float currentWaitTime = 0f;
  private delegate Pattern patternMethod(int pl, int max, bool fill);
  public delegate void MakeEnemy();
  public MakeEnemy EnemySpawners;
	private List<int> nextSpawners = new List<int>();
  private bool fillToggle = false;
  public int playerLocation = 0;
  private float currentBestTime = 0f;
  private float currentTime = 0f;
  [SerializeField]
  private DreamManager DM;
  private int wavesCleared = 0;

  [SerializeField]
  private Kino.Datamosh datamoshEffect;
  [SerializeField]
  private Image backgroundImage;
  [SerializeField]
  private Text TimeText, BestTimeText;
  public bool playerTouchState = false;

  [SerializeField]
  private List<AudioSource> wooshSounds;

  [SerializeField]
  private Vector2 spawnerDistanceRange_Min_Step;
  [SerializeField]
  private Vector2 spawnerNumberRange_Min_Step;
  [SerializeField]
  private Image UI_Veil;
  public bool UI_fadingIn = false;
  private bool UI_fadingOut = false;
 [SerializeField]
  private Text UI_Text;
  private float counter = 0f;

	// Use this for initialization
	void Start () {
		if(GameManager.instance == null) {GameManager.instance = this;}
      
    currentBestTime = PlayerPrefs.GetFloat("high score");
    BestTimeText.text = string.Format("Best: {0}", currentBestTime.ToString("00.00"));
    StartCoroutine(SetupGame());
  }

    // Update is called once per frame
    void Update () {
      if(gameIsRunning) {
        currentWaitTime += Time.deltaTime;
        currentTime += Time.deltaTime;
        TimeText.text = Mathf.Round(currentTime).ToString("00");
        CheckWaveStatus();
        backgroundImage.color = new Color(1, 1, 1,1 - DreamManager.instance.wakeFulness);
      }

      

      if (UI_fadingIn) {
          counter += Time.deltaTime;
          var curTex = UI_Text.color;
          var curVeil = UI_Veil.color;

          UI_Text.color = Color.Lerp(curTex, new Color(curTex.r, curTex.g, curTex.b, 1f), counter);
          UI_Veil.color = Color.Lerp(curVeil, new Color(curVeil.r, curVeil.g, curVeil.b, 1f), counter);
          
        }
        if (UI_fadingOut)
        {
          counter += Time.deltaTime;
          var curTex = UI_Text.color;
          var curVeil = UI_Veil.color;

          UI_Text.color = Color.Lerp(curTex, new Color(curTex.r, curTex.g, curTex.b, 0f), counter);
          UI_Veil.color = Color.Lerp(curVeil, new Color(curVeil.r, curVeil.g, curVeil.b, 0f), counter);
        }
	}

	#region GameControl Methods
  IEnumerator SetupGame() {
    setUpSpawners();
    StartCoroutine(FadeUIIn(1f));
    yield return new WaitForSeconds(3f);
    gameIsRunning = true;
    StartCoroutine(FadeUIOut(1f));
  }

  IEnumerator FadeUIIn(float time) {
    counter = 0f;      
    UI_fadingIn = true;
    yield return new WaitForSeconds(time);
    UI_fadingIn = false;
  }

  IEnumerator FadeUIOut(float time) {
    counter = 0f;
    UI_fadingOut = true;
    yield return new WaitForSeconds(time);
    UI_fadingOut = false;
  }

  void setUpSpawners(){
    //This will set up the spawners and distribute them evenly around the cylinder
    var degreeOffset = 360f/numberOfSpawners;

    for (var i = 0; i < numberOfSpawners; i++) {
      // figure out corner angle of current iteration
      var degrees = i * degreeOffset;
      var rotation = Quaternion.identity;
      var radians = degreeToRadian(degrees);
      // use corner angles to find points on circumfrence of circle
      var xcoor = Mathf.Cos(radians);
      var ycoor = Mathf.Sin(radians);
      var newLocation = new Vector3(xcoor, ycoor, spawnerDistance);
      var padLocation = new Vector3(xcoor, ycoor, 0);

      //spawn at pointer location
      rotation.eulerAngles = new Vector3(0,0,degrees);
      GameObject thisSpawner = GameObject.Instantiate(spawnerBase, newLocation, rotation) as GameObject;
      GameObject pad = GameObject.Instantiate(PressurePad, padLocation, rotation) as GameObject;
      pad.GetComponent<LaneDetector>().myLane = i;
      spawnerList.Add(thisSpawner);
    }
  }

  void CheckWaveStatus() {
    if (currentWaitTime >= waveWaitTime) {
      currentWaitTime = 0;
      SetupPattern(Random.Range(0, 3));
      waveWaitTime = Random.Range(1,7);
    }
  }


  public void PadFound(int padNumber) {
    playerLocation = padNumber;
  }
	#endregion

	#region PatternControl Methods
  void SetupPattern(int index) {
    patternMethod thisPattern =  PatternGenerator.LeftLine;

    switch(index) {
      case 0:
        thisPattern = PatternGenerator.FlyingV;
        break;
      case 1:
        thisPattern = PatternGenerator.RightLine;
        break;
      case 2:
        break;
    }

    StartCoroutine(routine: StartPattern(thisPattern: thisPattern(playerLocation, numberOfSpawners, fillToggle)));
  }

  IEnumerator StartPattern(Pattern thisPattern) {
    // use for loop to ensure lines are processed in order
    for (var lineIndex = 0; lineIndex< thisPattern.Lines.Count; lineIndex++) {
      Line line = thisPattern.Lines[lineIndex];
      List<int> spawningPoints = new List<int>();

      for (var spotIndex=0; spotIndex< line.Spots.Count; spotIndex++) {
        bool spot = line.Spots[spotIndex];

        if (spot) {
          // we use this list of indecies so the spawners can check if they should fire
          spawningPoints.Add(spotIndex);
        }
      }

      nextSpawners = spawningPoints;
      // call event
      EnemySpawners();
      yield return new WaitForSeconds(line.WaitTime);
      spawningPoints.Clear();
      continue;
    }
  }

	#endregion

	private float degreeToRadian(float degrees)
	{
		return degrees * (Mathf.PI/180);
	}

  public void PlayerTouchInput() {
    playerTouchState = !playerTouchState;
  }

  public IEnumerator onPlayerHit() {
        string _bestText = "Best: ";
        if (currentTime > currentBestTime)
        {
            currentBestTime = currentTime;
            PlayerPrefs.SetFloat("high score", currentBestTime);
            _bestText = "NEW Best: ";
        }
    
    for(var i = 0; i < wooshSounds.Count; i++) {
      var current = wooshSounds[i];

      if (!current.isPlaying) {
        current.time = 0.9f;
        current.Play();

        i = wooshSounds.Count;
      }
    }
    
    PC.StartCoroutine(PC.BadGuyHit());
    currentTime = 0f;
    BestTimeText.text = string.Format(_bestText + currentBestTime.ToString("00.00"));
    DM.HitAThing();
    datamoshEffect.Glitch();
    ClearPillars();
    yield return new WaitForSeconds(1);
    datamoshEffect.Reset();
  }

  public void ClearPillars() {
    foreach(var spawner in spawnerList) {
      spawner.GetComponent<EnemySpawner>().ClearEnemies();
    }
  }

  public void QuitGame() {
    Debug.Log("quit");
  }

	public bool DoIShoot(GameObject thisSpawner) {
		return nextSpawners.Contains(spawnerList.IndexOf(thisSpawner));
	}

    public void SetDifficulty(float difficulty)
    {
        spawnerDistance = (int)(spawnerDistanceRange_Min_Step.x + (spawnerDistanceRange_Min_Step.y * difficulty));
        numberOfSpawners = (int)(spawnerNumberRange_Min_Step.x + (spawnerNumberRange_Min_Step.y * difficulty));
    }
}
