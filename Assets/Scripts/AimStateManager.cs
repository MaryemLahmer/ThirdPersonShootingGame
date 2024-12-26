using UnityEngine;
public class AimStateManager : MonoBehaviour
{
    [SerializeField] private float mouseSense = 1;
    float xAxis, yAxis;
    [SerializeField] Transform camFollowPos;
    
    void Start()
    {
        
    }

    void Update()
    {
        xAxis += Input.GetAxis("Mouse X") * mouseSense;
        yAxis += Input.GetAxis("Mouse Y") * mouseSense;
        yAxis = Mathf.Clamp(yAxis, -80, 80);
        
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }
}
