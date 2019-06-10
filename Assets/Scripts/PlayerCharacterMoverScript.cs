using UnityEngine;
using System.Collections;

public class PlayerCharacterMoverScript : MonoBehaviour {

    public float maxZRotation;
    public float maxYRotation;
    public float maxShift;
    //public float HitBackLocation;
    //public float HitForwardLocation;
    //public float hitRecoverySpeed;

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private AudioSource gameSound;
    [SerializeField]
    private AudioSource wooshSound;

    private Vector3 defaultLocation;
    private Transform cheracterGobalTransform;

    private bool canMove = true;

    void Start()
    {
        defaultLocation = this.transform.localPosition;
        cheracterGobalTransform = this.transform.parent;
    }

    void Update () 
    {
        //tilt left/right based on input
        float inputH = Input.GetAxis("Horizontal");

        if (Input.touchCount > 0) {
          inputH = ((Input.GetTouch(0).position.x/Screen.width) * 2) -1;
        }

        float input = (canMove) ? inputH : (inputH >= 0) ? 1f : -1f;
        gameSound.panStereo = inputH;

        this.transform.localEulerAngles = new Vector3(0, (input * maxYRotation), (input * maxZRotation));
        cheracterGobalTransform.Rotate(Vector3.forward, rotateSpeed * input * Time.deltaTime);
        //grab the z position
        float tempZLocation = this.transform.localPosition.z;
        //shift left/right while maintaining z position.
        transform.localPosition = new Vector3(defaultLocation.x + maxShift * input, defaultLocation.y, tempZLocation);

        /*if (this.transform.localPosition.z > defaultLocation.z)
        {
            if (this.transform.localPosition.z - hitRecoverySpeed * Time.deltaTime > defaultLocation.z)
            {
                this.transform.localPosition -= new Vector3(0, 0, hitRecoverySpeed * Time.deltaTime);
            }
            else
            {
                Vector3 tempLocationA1 = this.transform.localPosition;
                this.transform.localPosition = new Vector3(tempLocationA1.x, tempLocationA1.y, defaultLocation.z);
            }
        }
        else if (this.transform.localPosition.z < defaultLocation.z)
        {
            if (this.transform.localPosition.z + hitRecoverySpeed * Time.deltaTime < defaultLocation.z)
            {
                this.transform.localPosition += new Vector3(0, 0, hitRecoverySpeed * Time.deltaTime);
            }
            else
            {
                Vector3 tempLocationA2 = this.transform.localPosition;
                this.transform.localPosition = new Vector3(tempLocationA2.x, tempLocationA2.y, defaultLocation.z);
            }
        }   */ 
	}


    public IEnumerator BadGuyHit()
    {
        canMove = false;
        yield return new WaitForSeconds(.45f);
        canMove = true;
    }

    public void BloodFountainHit()
    {
        //Vector3 tempLocationC = this.transform.localPosition;
        //this.transform.localPosition = new Vector3(tempLocationC.x, tempLocationC.y, HitForwardLocation);
    }
}
