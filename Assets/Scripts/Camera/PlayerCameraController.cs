using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private CinemachinePositionComposer positionComposer;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [SerializeField] private Vector2 returnDamping = new Vector3(1f, 2f);
    [SerializeField] private float returnCameraDuration = 0.5f;
    
    private Vector3 _normalDamping;

    private void Awake()
    {
        _normalDamping = positionComposer.Damping;
    }

    public void OnPlayerTeleported()
    {
        StartCoroutine(ReturnCamera());
    }

    private IEnumerator ReturnCamera()
    {
        positionComposer.Damping = returnDamping;

        yield return new WaitForSeconds(returnCameraDuration);

        positionComposer.Damping = _normalDamping;
    }

    public void CameraShake(float power)
    {
        impulseSource.GenerateImpulse(power);
    }
}