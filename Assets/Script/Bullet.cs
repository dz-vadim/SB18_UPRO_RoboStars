using UnityEngine;
using Photon.Pun;
public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletInfo _info;
    private Rigidbody _rb;
    private PhotonView _pv;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _pv = GetComponent<PhotonView>();
        _info.render = gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!_pv.IsMine) return;
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerSettings>().TakeDamage(_info.damage);
        }
        PhotonNetwork.Destroy(gameObject);
    }
    public void StartMove(Vector3 direction)
    {
        _rb.velocity = direction  * _info.speed;
    }
}
