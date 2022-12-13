using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    private Transform objectSeg;
    [SerializeField] float playerHigth=1f;
    [SerializeField] float wallDistance=0.1f;
    [SerializeField] float maxDistance = 14f;
    [SerializeField] float minDistance = 1.8f;

    [SerializeField] float xFast=200f;
    [SerializeField] float yFast = 200f;
    [SerializeField] int   yLimitMin=-80;
    [SerializeField] int   yLimitMax=80;

    [SerializeField] int   zoomFast=40;
    [SerializeField] float zoomDamping=3f; 
    [SerializeField] float RotationDamping=5f;


    [SerializeField] LayerMask collisionLayer=-1;
   

    [SerializeField] float xDeg=0f;
    [SerializeField] float yDeg=0f;

    [SerializeField] float currentDistance;
    [SerializeField] float baseDistance;
    [SerializeField] float desiredDistance;
    [SerializeField] float correctDistance;

    // Start is called before the first frame update
    void Start()
    {
        GameObject pc = GameObject.Find("Player");
        if (pc !=null) {

            objectSeg = pc.transform;
        }

        Vector3 angles = transform.eulerAngles;
        xDeg = angles.x;
        yDeg = angles.y;

        currentDistance = baseDistance;
        desiredDistance = baseDistance;
        correctDistance = baseDistance;

        if (GetComponent<Rigidbody>()) {

            GetComponent<Rigidbody>().freezeRotation=true;
        
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GameObject pc = GameObject.Find("Player");

        if (pc == null)
        {

            return;

        }
        else {

            objectSeg = pc.transform;
        }

        Vector3 vTargetOffSet;
        if (!objectSeg) { return; }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {

            xDeg += Input.GetAxis("Mouse X") * xFast * 0.02f;
            yDeg += Input.GetAxis("Mouse Y") * yFast * 0.02f;

        }

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) {

            float angleRotationObject = objectSeg.eulerAngles.y;
            float angleRotationCamera = objectSeg.eulerAngles.y;

            xDeg = Mathf.LerpAngle(angleRotationCamera, angleRotationObject, RotationDamping * Time.deltaTime);

        }

        yDeg = CorrectAngle(yDeg, yLimitMin, yLimitMax);


        Quaternion rotationEndCamera = Quaternion.Euler(yDeg,xDeg,0);

        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomFast * Mathf.Abs(desiredDistance);
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        correctDistance = desiredDistance;


        vTargetOffSet = new Vector3(0, playerHigth, 0);
        Vector3 positionEndCamera = objectSeg.position - (rotationEndCamera.y * Vector3.forward * desiredDistance + vTargetOffSet);

        RaycastHit collisionHit;
        Vector3 positiRoyalonObject = new Vector3(objectSeg.position.x,objectSeg.position.y + playerHigth, objectSeg.position.z);


        bool isCorrect = false;

        if (Physics.Linecast(positiRoyalonObject,positionEndCamera,out collisionHit,collisionLayer.value)) {

            correctDistance = Vector3.Distance(positiRoyalonObject,collisionHit.point)-wallDistance;
            isCorrect = true;
        }

        currentDistance = !isCorrect || correctDistance < currentDistance ? Mathf.Lerp(currentDistance, correctDistance, Time.deltaTime
            * zoomDamping) : correctDistance;


        currentDistance = Mathf.Clamp(currentDistance,minDistance,maxDistance);

        positionEndCamera = objectSeg.position - (rotationEndCamera*Vector3.forward*currentDistance+vTargetOffSet);
        transform.rotation = rotationEndCamera;
        transform.position = positionEndCamera;

    }



    private static float CorrectAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle,min,max);

    }
}
