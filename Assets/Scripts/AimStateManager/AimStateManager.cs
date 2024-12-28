using System;
using UnityEngine;
using Cinemachine;

public class AimStateManager : MonoBehaviour
{
    private AimBaseState currentState;
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

    [SerializeField] private Transform aimPos;
    [SerializeField] private float aimSmoothSpeed = 20f;
    [SerializeField] LayerMask aimMask;
    [SerializeField] Camera mainCamera;

    void Start()
    {
        hipFov = vcam.m_Lens.FieldOfView;
        animator = GetComponent<Animator>();
        SwitchState(Hip);
        isAiming = false;
        animator.SetLayerWeight(1, 0);
    }

    void Update()
    {
        xAxis += Input.GetAxis("Mouse X") * mouseSense/2;
        yAxis += Input.GetAxis("Mouse Y") * mouseSense*2;
        yAxis = Mathf.Clamp(yAxis, -80, 80);
        currentState.UpdateState(this);
        vcam.m_Lens.FieldOfView = Mathf.Lerp(vcam.m_Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);
        currentFov = isAiming ? adsFov : hipFov;
        
        Vector2 screenCenter  = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmoothSpeed * Time.deltaTime);


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

    private void Shoot()
    {
        // Add your shooting logic here
    }
}