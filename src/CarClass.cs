using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car {

    [RequireComponent(typeof(CarController))]
    public class CarClass : MonoBehaviour
    {
        public GameObject carObj;
        public Cars cars;
        public float SteeringAngle { get; set; }
        public float Velocity { get; set; }
        public bool Cruising { get; private set; } // cruise control
        public bool mouse_hold;
        public float totThrot = 0.0f;
        public float mouse_start;
        public bool hasTouched = false;

        // Raycasting:
        public GameObject RayBox;
        public RaycastHit forwardHit, behindHit, rightHit, leftHit, leftAngleHit, rightAngleHit;
        public float forwardDistance, behindDistance, rightDistance, leftDistance, leftAngleDistance, rightAngleDistance = 0f;
        public Dictionary<int, float> RayList = new Dictionary<int, float>();

        private float maxDistance = 100.0f;
        private float rayLength = 2f;
        private Vector3 LeftAngVec = new Vector3(-1, 0, 1);
        private Vector3 RightAngVec = new Vector3(1, 0, 1);
        private Vector3 addVector = new Vector3(0, 0, 5);

        // Constructor
        public CarClass(GameObject carObj_, float steerAng_, float acc_)
        {
            carObj = carObj_;
            SteeringAngle = steerAng_;
            Velocity = acc_;

            for (int i = 0; i < 6; i++)
            {
                RayList.Add(i, 10f);
            }
        }

        public void UpdateRays()
        {
            Vector3 forwardVector = carObj.transform.TransformDirection(Vector3.forward) * rayLength;
            Vector3 behindVector = carObj.transform.TransformDirection(Vector3.back) * rayLength;
            Vector3 rightVector = carObj.transform.TransformDirection(Vector3.right) * rayLength;
            Vector3 leftVector = carObj.transform.TransformDirection(Vector3.left) * rayLength;
            Vector3 leftAngleVector = carObj.transform.TransformDirection(LeftAngVec) * rayLength;
            Vector3 rightAngleVector = carObj.transform.TransformDirection(RightAngVec) * rayLength;

            Vector3 rayStart = carObj.transform.position + forwardVector / 3.4f;

            Debug.DrawRay(rayStart, forwardVector, Color.red);
            Debug.DrawRay(rayStart, behindVector, Color.red);
            Debug.DrawRay(rayStart, rightVector, Color.red);
            Debug.DrawRay(rayStart, leftVector, Color.red);
            Debug.DrawRay(rayStart, leftAngleVector, Color.red);
            Debug.DrawRay(rayStart, rightAngleVector, Color.red);

            forwardDistance = Physics.Raycast(rayStart, (forwardVector), out forwardHit, maxDistance) ? forwardHit.distance : maxDistance;
            RayList[0] = forwardDistance / maxDistance; // normalize by dividing by maxDistance

            behindDistance = Physics.Raycast(rayStart, (behindVector), out behindHit, maxDistance) ? behindHit.distance : maxDistance;
            RayList[1] = behindDistance / maxDistance;  

            rightDistance = Physics.Raycast(rayStart, (rightVector), out rightHit, maxDistance) ? rightHit.distance : maxDistance;
            RayList[2] = rightDistance / maxDistance;

            leftDistance = Physics.Raycast(rayStart, (leftVector), out leftHit, maxDistance) ? leftHit.distance : maxDistance;
            RayList[3] = leftDistance / maxDistance;

            rightAngleDistance = Physics.Raycast(rayStart, (rightAngleVector), out rightAngleHit, maxDistance) ? rightAngleHit.distance : maxDistance;
            RayList[4] = rightAngleDistance / maxDistance;

            leftAngleDistance = Physics.Raycast(rayStart, (leftAngleVector), out leftAngleHit, maxDistance) ? leftAngleHit.distance : maxDistance;
            RayList[5] = leftAngleDistance / maxDistance;

            if (forwardDistance < 1 && forwardHit.collider.gameObject.name == "GroundTrackVerges" ||
                rightDistance < 2 && rightHit.collider.gameObject.name == "GroundTrackVerges" ||
                behindDistance < 5 && behindHit.collider.gameObject.name == "GroundTrackVerges" ||
                leftDistance < 2 && leftHit.collider.gameObject.name == "GroundTrackVerges" ||
                rightAngleDistance < 2 && rightAngleHit.collider.gameObject.name == "GroundTrackVerges" ||
                leftAngleDistance < 2 && leftAngleHit.collider.gameObject.name == "GroundTrackVerges")
            {
                hasTouched = true;
            }              
        }
    }
}