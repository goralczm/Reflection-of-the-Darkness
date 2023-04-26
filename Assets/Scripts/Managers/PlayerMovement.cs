using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 12f;
    [SerializeField] private float _runSpeed = 15f;

    private CharacterController _controller;
    private Vector3 _velocity;

    private const float GRAVITY = -9.81f;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        float outputSpeed = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _speed;

        // Movement
        Vector3 move = transform.right * horizontalAxis + transform.forward * verticalAxis;
        _controller.Move(move * outputSpeed * Time.deltaTime);

        // Gravity
        _velocity.y += GRAVITY * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
