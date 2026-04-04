using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject mainCharacter;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float height;
    [SerializeField] private float rearDistance;
    private Vector3 _cameraOffset;
    private Vector3 _currentVector;

    private void Start()
    {
        transform.position = new Vector3(
            mainCharacter.transform.position.x, 
            mainCharacter.transform.position.y + height, 
            mainCharacter.transform.position.z - rearDistance);
        transform.rotation = Quaternion.LookRotation(
            mainCharacter.transform.position - transform.position);
    }
    private void Update()
    {
        CameraMove();
    }
    public void SetOffset(Vector3 offset)
    {
        if (offset.z < 0)
        {
            _cameraOffset = offset  * 10;
        }
        else if (offset.z > 0)
        {
            _cameraOffset = offset * 3;
        }
        else
        {
            _cameraOffset = offset * 8;
        }
    }
    private void CameraMove()
    {
        _currentVector = new Vector3(
            mainCharacter.transform.position.x +  _cameraOffset.x,
            mainCharacter.transform.position.y + height,
            (mainCharacter.transform.position.z - rearDistance) + _cameraOffset.z);
        
        transform.position = Vector3.Lerp(transform.position, _currentVector, returnSpeed * Time.deltaTime);
    }
}
