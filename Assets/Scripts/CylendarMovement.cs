using UnityEngine;
using System.Collections;

public class CylendarMovement : MonoBehaviour
{
    public float cylinderVSpeedCalibration;
    
    private Vector2 materialOffset = Vector2.zero;
    private Material material;
    [SerializeField]
    private float vSpeed;

    void Start()
    {
        this.material = GetComponent<Renderer>().material;
        materialOffset = material.GetTextureOffset("_MainTex");
    }

    void Update()
    {
        materialOffset.y += cylinderVSpeedCalibration * vSpeed * Time.deltaTime;
        material.SetTextureOffset("_MainTex", materialOffset);
    }
}
