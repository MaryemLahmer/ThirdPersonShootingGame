using UnityEngine;
public class AimStateManager : MonoBehaviour
{
    private AimBaseState currentState;
    public HipFireState Hip = new HipFireState();
    public AimState Aim = new AimState();
    [SerializeField] private float mouseSense = 1;
    float xAxis, yAxis;
    [SerializeField] Transform camFollowPos;
    [HideInInspector] public Animator animator;
    public bool isAiming;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        SwitchState(Hip);
        isAiming = false;
        animator.SetLayerWeight(1,0);
        
    }

    void Update()
    {
        xAxis += Input.GetAxis("Mouse X") * mouseSense;
        yAxis += Input.GetAxis("Mouse Y") * mouseSense;
        yAxis = Mathf.Clamp(yAxis, -80, 80);
        currentState.UpdateState(this);
        
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }

    public void SwitchState(AimBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
