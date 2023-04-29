﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : PortalTraveller
{
    public bool lockYCamera;
    public bool canJump;
    public bool canWalk;
    public string _footstepGroup;

    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float smoothMoveTime = 0.1f;
    public float jumpForce = 8;
    public float gravity = 18;

    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public float rotationSmoothTime = 0.1f;

    CharacterController controller;
    Camera cam;
    public float yaw;
    public float pitch;
    float smoothYaw;
    float smoothPitch;

    float yawSmoothV;
    float pitchSmoothV;
    float verticalVelocity;
    Vector3 velocity;
    Vector3 smoothV;

    bool jumping;
    float lastGroundedTime;
    float footstepTimer;
    private AudioManager _audioManager;

    private float yawSpeed = 100f;

    void Start()
    {
        yawSpeed = 100f;
        _audioManager = AudioManager.instance;
        cam = Camera.main;
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        controller = GetComponent<CharacterController>();

        yaw = transform.eulerAngles.y;
        pitch = cam.transform.localEulerAngles.x;
        smoothYaw = yaw;
        smoothPitch = pitch;
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (!canWalk)
            input = Vector2.zero;

        Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;
        Vector3 worldInputDir = transform.TransformDirection(inputDir);

        float currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : walkSpeed;
        if (input != Vector2.zero && footstepTimer <= 0)
        {
            _audioManager.PlayRandomFootstep(_footstepGroup);
            footstepTimer = (float)(currentSpeed / 2) - (float)(currentSpeed - walkSpeed) / 1.25f;
        }
        footstepTimer -= Time.deltaTime;
        Vector3 targetVelocity = worldInputDir * currentSpeed;
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref smoothV, smoothMoveTime);

        verticalVelocity -= gravity * Time.deltaTime;
        velocity = new Vector3(velocity.x, verticalVelocity, velocity.z);

        var flags = controller.Move(velocity * Time.deltaTime);
        if (flags == CollisionFlags.Below)
        {
            jumping = false;
            lastGroundedTime = Time.time;
            verticalVelocity = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            float timeSinceLastTouchedGround = Time.time - lastGroundedTime;
            if (controller.isGrounded || (!jumping && timeSinceLastTouchedGround < 0.15f))
            {
                jumping = true;
                verticalVelocity = jumpForce;
            }
        }

        /*float mX = Input.GetAxisRaw("HorizontalArrow");

        float mY = Input.GetAxisRaw("Mouse Y");

        // Verrrrrry gross hack to stop camera swinging down at start
        float mMag = Mathf.Sqrt(mX * mX + mY * mY);
        if (mMag > 5)
        {
            mX = 0;
            mY = 0;
        }*/
        
        //if (canWalk)
            //yaw += mX * mouseSensitivity;
        //pitch -= mY * mouseSensitivity;

        //if (lockYCamera)
            //pitch = cam.transform.localEulerAngles.x;

        //pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        //smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchSmoothV, rotationSmoothTime);
        //smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yaw, rotationSmoothTime);

        //transform.eulerAngles = Vector3.up * smoothYaw;
        //cam.transform.localEulerAngles = Vector3.right * smoothPitch;

    }

    public override void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle(smoothYaw, eulerRot.y);
        yaw += delta;
        smoothYaw += delta;
        transform.eulerAngles = Vector3.up * smoothYaw;
        velocity = toPortal.TransformVector(fromPortal.InverseTransformVector(velocity));
        Physics.SyncTransforms();
    }

    public void SetupCharacter(Vector3 newPos, Vector3 newRot)
    {
        transform.position = newPos;
        transform.eulerAngles = newRot;
        enabled = true;
        GetComponent<CharacterController>().enabled = true;
    }
}