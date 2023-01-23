using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Stamina))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0)] private float _walkSpeed;
    [SerializeField, Min(0)] private float _runSpeed;
    [SerializeField, Min(0)] private float _jumpForce;
    [SerializeField, Min(0)] private float _gravityScale;

    [Header("View")]
    [SerializeField] private CameraShaker _shaker;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField, Min(0)] private float _mouseSensitivity;

    private Vector3 _movementDirection;
    private Vector3 _movementVelocity;
    private CharacterController _controller;
    private Stamina _stamina;
    private float _cameraPitch = 0;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _stamina = GetComponent<Stamina>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdateCharactedMovement();
        UpdateCameraRotaion();
    }

    private void UpdateCharactedMovement()
    {
        _movementDirection = new Vector3
        (
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        ).normalized;
        _movementDirection = transform
            .TransformDirection(_movementDirection);

        if (_controller.isGrounded)
        {
            _movementVelocity.y = -1;
            if (Input.GetKeyDown(KeyCode.Space))
                _movementVelocity.y = _jumpForce;
        }
        else
            _movementVelocity.y -= _gravityScale * Time.deltaTime;

        if (_movementDirection != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift) && _stamina.TryUseStamina())
            { 
                _movementDirection *= _runSpeed;
                _shaker.SetActiveShaker();
            }
            else
            {
                _movementDirection *= _walkSpeed;
                _shaker.SetActiveShaker(false);
            }
        }
        else
            _shaker.SetActiveShaker(false);


        _controller.Move((_movementDirection
                + _movementVelocity) * Time.deltaTime);
    }

    private void UpdateCameraRotaion()
    {
        Vector2 mousePosition = new Vector2
        (
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );
        
        _cameraPitch -= mousePosition.y * _mouseSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90, 90);
        _cameraTransform.localEulerAngles = Vector3.right * _cameraPitch;

        transform.Rotate(Vector3.up * mousePosition.x * _mouseSensitivity);
    }
}
