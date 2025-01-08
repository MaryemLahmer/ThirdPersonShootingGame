using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.Animations.Rigging;
public class AimStateManager : MonoBehaviour
{
    public AimBaseState currentState;
    public HipFireState Hip = new HipFireState();
    public AimState Aim = new AimState();
    [SerializeField] private float mouseSense = 1;
    float xAxis, yAxis;
    [SerializeField] Transform camFollowPos;
    [HideInInspector] public Animator animator;
    [SerializeField] public CinemachineVirtualCamera vcam;
    public float adsFov = 40f;
    [HideInInspector] public float hipFov;
    [HideInInspector] public float currentFov;
    public float fovSmoothSpeed = 10f;
    public bool isAiming;
    
    [HideInInspector] public Transform aimPos;
    [HideInInspector] public Vector3 actualAimPos;
    [SerializeField] private float aimSmoothSpeed = 20f;
    [SerializeField] LayerMask aimMask;

    private float xFollowPos;
    float yFollowPos, ogYPos;
    [SerializeField] private float crouchCamHeight = 0.6f;
    [SerializeField] private float shoulderSwapSpeed = 10f;
    private MovementStateManager moving;

    [SerializeField] Camera mainCamera;

    private WeaponClassManager weapons;

    void Start()
    {
        moving = GetComponent<MovementStateManager>();
        xFollowPos = camFollowPos.localPosition.x;
        ogYPos = camFollowPos.localPosition.y;
        yFollowPos = ogYPos;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        hipFov = vcam.m_Lens.FieldOfView;
        animator = GetComponent<Animator>();
        SwitchState(Hip);
        isAiming = false;
        animator.SetLayerWeight(1, 0);
        weapons = GetComponent<WeaponClassManager>();

    }

    void Update()
    {
        xAxis += Input.GetAxis("Mouse X") * mouseSense / 2;
        yAxis += Input.GetAxis("Mouse Y") * mouseSense * 2;
        yAxis = Mathf.Clamp(yAxis, -80, 80);
        currentState.UpdateState(this);
        vcam.m_Lens.FieldOfView = Mathf.Lerp(vcam.m_Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);
        currentFov = isAiming ? adsFov : hipFov;
        weapons.currentWeapon().gameObject.SetActive(isAiming);
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
        {
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);
            actualAimPos = hit.point;
        }
        
        MoveCamera();
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles =
            new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }

    public void SwitchState(AimBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void MoveCamera()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2)) xFollowPos = -xFollowPos;
        if (moving.currentState == moving.Crouch) yFollowPos = crouchCamHeight;
        else yFollowPos = ogYPos;
        Vector3 newFollowPosition = new Vector3(xFollowPos, yFollowPos, camFollowPos.localPosition.z);
        camFollowPos.localPosition = Vector3.Lerp(camFollowPos.localPosition, newFollowPosition,
            shoulderSwapSpeed * Time.deltaTime);
    }
}