using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineCollider))]
public class ColliderWatcher : MonoBehaviour
{
    public bool IsTargetObscured;
    public bool CameraWasDisplaced;
    public CinemachineCollider _cache;
    public CinemachineVirtualCameraBase _middleRig;

    void Start()
    {
        _cache = this.GetComponent<CinemachineCollider>();
        _middleRig = _cache.VirtualCamera;
    }
    
    void Update()
    {
        IsTargetObscured = _cache.IsTargetObscured(_middleRig);
        CameraWasDisplaced = _cache.CameraWasDisplaced(_middleRig);
    }
}