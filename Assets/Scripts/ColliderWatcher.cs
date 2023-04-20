using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineCollider))]
public class ColliderWatcher : MonoBehaviour
{
    [field:SerializeField] private bool IsTargetObscured;
    [field:SerializeField] private bool CameraWasDisplaced;

    CinemachineCollider _cache;
    CinemachineVirtualCamera _vcam;

    void Awake()
    {
        _cache = GetComponent<CinemachineCollider>();
        _vcam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        IsTargetObscured = _cache.IsTargetObscured(_vcam);
        CameraWasDisplaced = _cache.CameraWasDisplaced(_vcam);
    }
}