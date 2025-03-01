using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    private CharacterController controller;
    private Camera playerCamera;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float lookSpeed = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        CameraLook();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 moveVector = transform.right * moveX + transform.forward * moveY;
        controller.Move(moveVector.normalized * speed * Time.deltaTime);
    }

    void CameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(Vector3.up * mouseX);
        playerCamera.transform.Rotate(Vector3.left * mouseY);
    }
}
