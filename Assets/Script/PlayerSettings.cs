using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerSettings : MonoBehaviourPunCallbacks
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private Slider healthBar;
    private PhotonView _pv;
    
    private void Awake()
    {
        _pv = GetComponent<PhotonView>();
    }
    
    void Start()
    {
        health = maxHealth;
        healthBar.value = health;
    }

    public void TakeDamage(int damage)
    {
        _pv.RPC("UpdateHealth", RpcTarget.All, damage);
    }

    [PunRPC]
    public void UpdateHealth(int value)
    {
        health -= value;
        if (health <= 0)
        {
            health = maxHealth;
            transform.GetComponentInChildren<PlayerController>().Respawn();
        }
        healthBar.value = health;
    }
}
