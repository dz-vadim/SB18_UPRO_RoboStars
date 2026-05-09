using UnityEngine;
using UnityEngine.AI;

namespace Unity.AI.Navigation.Samples
{
    /// <summary>
    /// Use physics raycast hit from mouse click to set agent destination
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class ClickToMove : MonoBehaviour
    {
        NavMeshAgent m_Agent;
        RaycastHit m_HitInfo = new RaycastHit();
        GameObject target;

        void Start()
        {
            target = GameObject.FindObjectOfType<PlayerController>().gameObject;
            m_Agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (target)
            {
                m_Agent.destination = target.transform.position;
            }
        }
    }
}