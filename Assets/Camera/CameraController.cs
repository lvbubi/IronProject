using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    private Quaternion currentrot;
    private Vector3 currentpos;
    private GameObject MainCam;
    private int a = 0; //Tesco value trigger
    // Use this for initialization
    void Start () {
        MainCam = GameObject.Find("Main Camera");
    }
	
	// Update is called once per frame
	void Update () {

    }


}
