using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [SerializeField] Transform m_Target;
    [SerializeField] Vector3 m_CameraRig = new Vector3(0, 0, -10);
    [SerializeField, Range(0.01f, 1.0f)] float m_Speed = 0.5f;
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, m_Target.position + m_CameraRig, m_Speed);
    }
}
