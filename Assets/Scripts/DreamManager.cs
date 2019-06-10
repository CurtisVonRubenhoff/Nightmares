using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DreamManager : MonoBehaviour {

    public float wakeFulness = 0;
    private float stingAmount = 1;
    public float difficulty = 0;

    [Header("Game Management")]
    [SerializeField]
    private float wakeRate;
    [SerializeField]
    private float stingStep;
    [SerializeField]
    private float stingDecayRate;

    [Header("Things affected by how close you are to waking up.")]

    [SerializeField]
    private Transform camHolder;
    [SerializeField]
    private Vector3 cameraStartPos, cameraEndPos;
    [SerializeField]
    private Vector3 cameraStartRot, cameraEndRot;
    [SerializeField]
    private Vector2 cameraFOVStart_End;
    [SerializeField]
    private MeshRenderer Cylendar_inside, Cylendar_Outside;
    [SerializeField]
    private Color Cylendar_inside_Color_Start, ylendar_inside_Color_End, Cylendar_outside_Color_Start, Cylendar_outside_Color_End;

    [SerializeField]
    private Gradient Inside_Cylinder_Gradient, Outside_Cylinder_Gradient;


    [Header("Things affected by  the penalty after you hit an object.")]
    [SerializeField]
    private AudioSource amix;

    [SerializeField]
    private MeshRenderer background_inner, background_outer;
    [SerializeField]
    private Color background_inner_Color_Start, backgroung_inner_Color_End, background_outer_Color_Start, backgroung_outer_Color_End;

    public static DreamManager instance;



  void Start()
    {
        if (DreamManager.instance == null) DreamManager.instance = this;
    }

	
	// Update is called once per frame
	void Update ()
    {
        wakeFulness += wakeRate * Time.deltaTime;
        Mathf.Clamp(wakeFulness, 0, 1);
        difficulty += Time.deltaTime;


        if (stingAmount > 0) stingAmount -= stingDecayRate * Time.deltaTime;
        else stingAmount = 0;

        //difficulty
        GameManager.instance.SetDifficulty(difficulty);

        //things affected by wakefulness
        //camera pos/rot
        camHolder.localPosition = Vector3.Lerp(cameraStartPos, cameraEndPos, wakeFulness);
        camHolder.localRotation = Quaternion.Slerp(Quaternion.Euler(cameraStartRot), Quaternion.Euler(cameraEndRot), wakeFulness);
        //camera fov
        Camera.main.fieldOfView = Mathf.Lerp(cameraFOVStart_End.x, cameraFOVStart_End.y, wakeFulness);
        //cylendar colors
        Color _newcolor_outside = Outside_Cylinder_Gradient.Evaluate(wakeFulness);
        Cylendar_Outside.material.SetColor("_EmissionColor", _newcolor_outside);
        Color _newcolor_inside = Inside_Cylinder_Gradient.Evaluate(1 - wakeFulness);
        Cylendar_inside.material.SetColor("_EmissionColor", _newcolor_inside);
        //waking world fade-in



        //things affected by sting Amound
        //audio distortion

        //background colors
        Color _newcolor_background_outside = Color.Lerp(background_outer_Color_Start, backgroung_outer_Color_End, stingAmount);
        background_outer.material.SetColor("_EmissionColor", _newcolor_background_outside);
        Color _newcolor_background_inside = Color.Lerp(background_inner_Color_Start, backgroung_inner_Color_End, stingAmount);
        background_inner.material.SetColor("_EmissionColor", _newcolor_background_inside);

    }

    public void HitAThing()
    {
        wakeFulness = 0;
        stingAmount = Mathf.Clamp(stingAmount + stingStep, 0, 1);
    }

    
}
