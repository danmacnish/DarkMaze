﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Pix
{
    public class PlayerTouchController : MonoBehaviour
    {
        public bool debug = true;
        public int health = 3;
        public Light m_light;
        public float deathfade = 1.5f;

        public float moveDistance = 1.0f;

        public PlayableDirector camTimeline;
        public float m_Speed = 12f;                 // How fast the tank moves forward and back.
        public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
        public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.

        private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
        private string m_TurnAxisName;              // The name of the input axis for turning.
        private Rigidbody m_Rigidbody;              // Reference used to move the tank.
        private float m_MovementInputValue;         // The current value of the movement input.
        private float m_TurnInputValue;             // The current value of the turn input.
        private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
        private float time = 0.0f;
        private float lightstartingintensity;
        private ParticleSystem[] m_particleSystems; // References to all the particles systems used by the Tanks

        private Camera cam;

        private Vector3 targetPosition;


        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_light = GetComponentInChildren<Light>();
            lightstartingintensity = m_light.intensity;
            cam = Camera.main;
        }


        private void OnEnable()
        {
            // When the tank is turned on, make sure it's not kinematic.
            m_Rigidbody.isKinematic = false;

            // Also reset the input values.
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;

            // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
            // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
            // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Play();
            }
        }


        private void OnDisable()
        {
            // When the tank is turned off, set it to kinematic so it stops moving.
            m_Rigidbody.isKinematic = true;

            // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Stop();
            }
        }


        private void Start()
        {
            // The axes names are based on player number.
            m_MovementAxisName = "Vertical";
            m_TurnAxisName = "Horizontal";
            targetPosition = transform.position;
        }


        private void Update()
        {
            if (health <= 0 && camTimeline.state != PlayState.Playing)
            {
                m_light.intensity = Mathf.Lerp(lightstartingintensity, 0, time / deathfade);
                time += Time.deltaTime;
            }
        }



        private void FixedUpdate()
        {
            // Adjust the rigidbodies position and orientation in FixedUpdate.
            GetTargetPosition();
            Move();
        }

        private void Move()
        {
            if (Vector3.Distance(m_Rigidbody.position, targetPosition) > 0.2)
            {
                Vector3 direction = targetPosition - transform.position;
                direction.Normalize();
                m_Rigidbody.MovePosition(transform.position + direction * m_Speed * Time.fixedDeltaTime);
            }
        }
        private void GetTargetPosition()
        {
            // get input
            if (Input.touchCount > 0)
            {
                if (debug) Debug.Log("touch detected");
                //get first touch
                Touch t = Input.GetTouch(0);
                // if (t.phase == TouchPhase.Began)
                // {
                    // raycast to level
                    RaycastHit hit;
                    Ray ray = cam.ScreenPointToRay(t.position);
                    if (Physics.Raycast(ray, out hit))
                    {
                        targetPosition = hit.point;

                        // calculate dot product with forward vector
                        // float dot = Vector3.Dot(direction, transform.forward);
                        // if (dot > 0.5f)
                        // {
                        //     //move forward
                        //     if (debug) Debug.Log("move forward");
                        //     //m_Rigidbody.MovePosition(m_Rigidbody.position + transform.forward * Time.fixedDeltaTime);
                        //     m_Rigidbody.AddForce(transform.forward * m_Speed);

                        // }
                        // else if (dot >= -0.5f && dot <= 0.5f)
                        // {
                        //     if (debug) Debug.Log("move left or right");
                        //     //turn left or right

                        // }
                        // else
                        // {
                        //     //go backwards
                        //     if (debug) Debug.Log("move backwards");

                        // }
                        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
                    }
                //}
            }
        }

        public void Hit()
        {
            health -= 1;
        }
    }
}

