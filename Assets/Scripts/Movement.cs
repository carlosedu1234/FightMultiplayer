using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Photon.MonoBehaviour
{

    [SerializeField] float gravity=20f;
    [SerializeField] float rotateSpeed = 150f;
    [SerializeField] float moveSpeed=6f;
    [SerializeField] float ForceJump = 8;
    private Vector3 direction= Vector3.zero;
    // Update is called once per frame

    private void Start()
    {
        gameObject.name = "Player";
    }
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (photonView.isMine) {

            GetComponent<MeshRenderer>().material.color = Color.blue;

            if (controller.isGrounded)
            {
                MovementDirection();
            }

            direction.y -= gravity * Time.deltaTime;
            controller.Move(direction * Time.deltaTime);
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0);
        }

        


    }


    private void MovementDirection() {


        direction = new Vector3(0,0, Input.GetAxis("Vertical"));

        direction = transform.TransformDirection(direction);

        direction *= moveSpeed;

        if (Input.GetButton("Jump")) {

            direction.y = ForceJump;
        
        }

        
        
    
    
    }


}
