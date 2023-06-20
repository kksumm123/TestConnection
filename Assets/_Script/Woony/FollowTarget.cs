using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform m_target;
    public Transform Target
    {
        get => m_target;
        set
        {
            Debug.Log($"카메라 타겟 변경 {m_target} -> {value}", gameObject);
            m_target = value;
        }
    }

    Transform originTarget;

    Vector3 originPos;
    [SerializeField] Vector3 offset = new Vector3(0, 17.85f, -19.38f);
    [SerializeField] Vector3 tortureOffset = new Vector3(0, 17.85f, -19.38f);
    [SerializeField] Vector3 rotation = new Vector3(35, 0, 0);

    [Header("Follow 좌표축"), SerializeField] bool followX;
    [SerializeField] bool followY;
    [SerializeField] bool followZ;
    [SerializeField] float followLerpValue = 0.05f;

    // todo: 임시 카메라 변수
    public float zoomInFieldOfView = 38f;
    public float zoomOutFieldOfView = 60f;

    void Start()
    {
        Debug.Assert(Target != null, "타겟 설정해주세요", gameObject);
        originPos = transform.position;
        originTarget = Target;
    }
    Vector3 nextPosition;


    void FixedUpdate()
    {
        if (Target == originTarget)
            nextPosition = SetNextPosition(offset);
        else
            nextPosition = SetNextPosition(tortureOffset);

        transform.position = Vector3.Lerp(transform.position, nextPosition, followLerpValue);
        transform.rotation = Quaternion.Euler(rotation);
    }

    Vector3 SetNextPosition(Vector3 inputOffset)
    {
        return new Vector3(followX ? Target.position.x + inputOffset.x : originPos.x,
                                            followY ? Target.position.y + inputOffset.y : originPos.y,
                                            followZ ? Target.position.z + inputOffset.z : originPos.z);
    }

    [ContextMenu("타겟에 대한 오프셋 설정")]
    public void SetOffset()
    {
        if (Target != null)
        {
            offset = transform.position - Target.position;
#if UNITY_EDITOR
            // 에디터에서의 수정사항을 반영하기위한 호출
            // 호출되지 않으면, 스크립트로 인스펙터 노출된 필드 변경시 수정사항이 반영되지 않아
            // 값이 이전으로 돌아감
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
#endif
        }
        else
            print("타겟이 비어있습니다");
    }
}
