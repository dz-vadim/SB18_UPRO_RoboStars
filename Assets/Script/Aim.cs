using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using System.IO;

public class Aim : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private List<GameObject> allTargets;
    [SerializeField] private GameObject targetCylinder;
    [SerializeField] private float range;
    private PlayerInput _inputs;
    private PhotonView _pv;
    private CharacterController _controller;
    private GameObject _targetObj;
    private bool _canSearch = true;
    private int _targetCount;
    
    private void Awake()
    {
        _inputs = new PlayerInput();
        _controller = GetComponent<CharacterController>();
        _pv = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        _inputs.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        _inputs.CharacterControls.ChangeTarget.started -= SelectNewTarget;
        _inputs.CharacterControls.Fire.started -= OnFire;
        _inputs.CharacterControls.Disable();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void SetTargetStatus(bool isTarget)
    {
        targetCylinder.SetActive(isTarget);
    }
    
    private void SelectTarget()
    {
        if (_controller.velocity == Vector3.zero)
        {
            if (_canSearch)
            {
                InvokeRepeating("Calculate", 0f, 0.5f);
            }
        }
        else
        {
            if (_targetObj != null)
            {
                _targetObj.GetComponent<Aim>().SetTargetStatus(false);
                _targetObj = null;
            }

            _canSearch = true;
            CancelInvoke();
        }
    }

    private void Calculate()
    {
        _canSearch = false;
        allTargets.Clear();

        Collider[] hits = Physics.OverlapSphere(transform.position, range);

        foreach (Collider hit in hits)
        {
            GameObject tempObj = hit.gameObject;
            PhotonView targetPv = tempObj.GetComponentInParent<PhotonView>();

            if (tempObj.GetComponent<CharacterController>() && targetPv != null && !targetPv.IsMine)
            {
                allTargets.Add(tempObj);
            }
        }

        SelectNewTarget();
    }

    private void SelectNewTarget()
    {
        foreach (GameObject obj in allTargets)
        {
            obj.GetComponent<Aim>().SetTargetStatus(false);
        }

        if (allTargets.Count == 0)
        {
            _targetObj = null;
            _targetCount = 0;
            return;
        }

        if (_targetCount >= allTargets.Count)
        {
            _targetCount = 0;
        }

        _targetObj = allTargets[_targetCount];
        _targetObj.GetComponent<Aim>().SetTargetStatus(true);
    }
    
    private void SelectNewTarget(InputAction.CallbackContext context)
    {
        if (allTargets.Count == 0)
        {
            _targetObj = null;
            _targetCount = 0;
            return;
        }

        _targetCount++;

        foreach (GameObject obj in allTargets)
        {
            obj.GetComponent<Aim>().SetTargetStatus(false);
        }

        if (_targetCount >= allTargets.Count)
        {
            _targetCount = 0;
        }

        _targetObj = allTargets[_targetCount];
        _targetObj.GetComponent<Aim>().SetTargetStatus(true);
    }

    private void Start()
    {
        if (!_pv.IsMine) return;
        targetCylinder.SetActive(false);
        _inputs.CharacterControls.ChangeTarget.started += SelectNewTarget;
        _inputs.CharacterControls.Fire.started += OnFire;
    }

    private void FixedUpdate()
    {
        if (!_pv.IsMine) return;
        SelectTarget();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if (_targetObj)
        {
            Vector3 direction = (_targetObj.transform.position - transform.position).normalized;
            GameObject temp = PhotonNetwork.Instantiate(Path.
                Combine("Fireball"), transform.position, Quaternion.identity);
            
            temp.GetComponent<Bullet>().StartMove(direction);
            Physics.IgnoreCollision(temp.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}
