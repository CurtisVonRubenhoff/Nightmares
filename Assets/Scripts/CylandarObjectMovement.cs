using UnityEngine;
using System.Collections;

public class CylandarObjectMovement : MonoBehaviour
{

    public GameObject oneUp;
    public float minScale;
    public float maxScale;
    public float badGuyFadeInDistance;
    public float badGuyScaleInDistance;
    public float badGuyAlpha;
    public float badGuyFadeInRate;
    public float badGuyFadeOutRate;
    public float badGuyVSpeedCalibration;
    public float badGuyDestroyLocation;
    [SerializeField]
    private float vSpeed;
    
    //triggered by collision with explosion collider
    public bool die;
    //triggered on gamemanager by player colliding with snitch
    public bool dissappear;

    private float myScale;
    public float fadeandScaleInStart;
    public float fadeInEnd;
    public float fadePercent;
    public float scaleInEnd;
    public float scalePercent;
    public float myposition;
    public bool scaleAndFade = true;
    
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        //spriteRenderer = transform.FindChild("BadGuy").GetComponent<SpriteRenderer>();
        spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        //give a random scale at awake
        myScale = Random.Range(minScale, maxScale);

        if (scaleAndFade)
        {
            //set initial scale to zero
            this.transform.localScale = new Vector3(0, 0, 1);
            //set alpha to zero
            this.spriteRenderer.material.color = new Color(0, 0, 0, 0);
            //set Z axis start and end point
            fadeandScaleInStart = this.transform.position.z;
            fadeInEnd = fadeandScaleInStart - badGuyFadeInDistance;
            //give a random position around cylendar at awake
            this.transform.RotateAround(Vector3.zero, Vector3.forward, Random.Range(0, 360));
        }
        else
        {
            this.transform.localScale = new Vector3(myScale, myScale, 1);
            this.spriteRenderer.material.color = new Color(0, 0, 0, badGuyAlpha);
            //give a random position around cylendar at awake
            this.transform.RotateAround(Vector3.zero, Vector3.forward, Random.Range(0, 360));
        }
        
    }

    void Update()
    {
        if (scaleAndFade)
        {
            if (this.transform.position.z > fadeInEnd)
            {
                fadePercent = (fadeandScaleInStart - this.transform.position.z) / badGuyFadeInDistance;
                this.spriteRenderer.material.color = new Color(0, 0, 0, badGuyAlpha * fadePercent);
            }

            if (this.transform.position.z > scaleInEnd)
            {
                scalePercent = (fadeandScaleInStart - this.transform.position.z) / badGuyScaleInDistance;
                this.transform.localScale = new Vector3(myScale * scalePercent, myScale * scalePercent, 1);
            }
        }
            
        //move according to game manager
        this.transform.position += new Vector3(0, 0, badGuyVSpeedCalibration * vSpeed * Time.deltaTime);
        
        //destroy when it gets past
        if (this.transform.position.z < badGuyDestroyLocation) Destroy(this.gameObject);

        //triggered when colliding with explosion collider
        if (die) Die();

        //triggered when snitch is hit
    }

    void Die()
    {
        if (spriteRenderer.material.color.a > 0)
        {
            this.spriteRenderer.material.color -= new Color(0, 0, 0,badGuyFadeOutRate * Time.deltaTime);
        }
        else
        {
            Instantiate(oneUp, new Vector3(0,0,this.transform.position.z), this.transform.rotation);
            Destroy(this.gameObject);
        }
    }

    void Dissappear()
    {
        Destroy(this.gameObject);
    }
}
