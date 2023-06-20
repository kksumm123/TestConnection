using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class PathDrawer : MonoBehaviour
{
    [TextArea(8, 8), SerializeField]
    const string DESCRIPTION = @"Made By Woony
사용법
1. 먼저 해당 스크립트가 붙은 오브젝트를 선택되어야 합니다
2. 바닥 오브젝트 레이어에 대한 Hit Layer를 지정해줍니다
3. Scene View에 포커스를 두고 우클릭 또는 우클릭 후 살짝 드래그 해줍니다
4. 지정된 DrawKey를 꾹 누른채로 마우스를 움직입니다
만약 경로를 지우고 싶다면, 지정된 eraseKey를 누르세요";
    // Made By Woony
    // How To Use
    // 1. must be Selected Object that contain this Script.
    // 2. setting hit object's LayerMask
    // 3. focus on scene view, and one click mouse right button.
    // 4. press SpaceBar key down, and move mouse pointer
    // if use remove path, press ctrl key

    [Header("Key"), SerializeField] KeyCode drawKey = KeyCode.Space;
    [SerializeField] KeyCode eraseKey = KeyCode.LeftControl;

    [Header("Color"), SerializeField] Color startColor = Color.red;
    [SerializeField] float colorStep = 3;
    [SerializeField] float dotLineSpace = 5f;

    [Header("Draw"), SerializeField] float minDistance = 0.5f;
    [SerializeField] float maxDistance = 3f;
    [SerializeField] LayerMask hitLayer;
    [SerializeField] List<Vector3> path = new List<Vector3>();

#if UNITY_EDITOR
    bool isInit = false;
    private void OnDestroy()
    {
        SceneView.duringSceneGui -= OnScene;
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnScene;
    }
    private void OnEnable()
    {
        Init();
        SceneView.duringSceneGui -= OnScene;
        SceneView.duringSceneGui += OnScene;
        isInit = true;
    }

    private void Init()
    {
        if (hitLayer == 0)
            hitLayer = 1 << LayerMask.NameToLayer("House");
    }

    Ray ray;
    Vector3 dir;
    void OnScene(SceneView sceneView)
    {
        if (Selection.Contains(gameObject) == false)
        {
            ray = new Ray();
            return;
        }

        if (Event.current.keyCode == eraseKey)
            path.Clear();

        if (Event.current.keyCode != drawKey)
            return;

        // Path 내역을 취소스택에 저장하기 위해 호출
        // 버그인지 각 path.add마다 적용되지 않고 drawkey가 눌리고 뗌에 따라 저장
        // 추천사항 : 여러번 덧칠하듯 그리기
        Undo.RecordObject(this, "Undo PathDraw");

        var mousePos = Event.current.mousePosition;
        float ppp = EditorGUIUtility.pixelsPerPoint;
        mousePos.y = sceneView.camera.pixelHeight - mousePos.y * ppp;
        mousePos.x *= ppp;

        ray = sceneView.camera.ScreenPointToRay(mousePos);
        dir = sceneView.camera.transform.forward;
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 100f, hitLayer))
        {

            if ((path.Count == 1) && (Vector3.Distance(path[0], raycastHit.point) > maxDistance))
            {
                path.Clear();
                return;

            }
            else if (path.Count > 0)
            {
                if (IsDrawable(path[path.Count - 1], raycastHit) == false)
                    return;
            }

            path.Add(raycastHit.point);

            // 에디터에서의 수정사항을 반영하기위한 호출
            // 호출되지 않으면, 스크립트로 인스펙터 노출된 필드 변경시 수정사항이 반영되지 않아
            // 값이 이전으로 돌아감
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
        }
    }
    bool IsDrawable(Vector3 startPos, RaycastHit raycastHit)
    {
        if ((Vector3.Distance(startPos, raycastHit.point) < minDistance)
            || (Vector3.Distance(startPos, raycastHit.point) > maxDistance))
            return false;

        return true;
    }

    private void OnDrawGizmos()
    {
        if (isInit == false)
            return;
        if (Selection.Contains(gameObject) == false)
        {
            ray = new Ray();
            return;
        }

        SceneView.RepaintAll();

        Gizmos.color = Color.white;
        Gizmos.DrawRay(ray.origin, dir * 100f);

        if (path.Count > 1)
        {
            Handles.color = startColor;
            Handles.DrawDottedLine(path[0], path[1], dotLineSpace);
            Handles.Label(path[0], "Start");

            for (int i = 1; i < path.Count - 1; i++)
            {
                Handles.color = IncreaseColorH(Handles.color);
                Handles.DrawDottedLine(path[i], path[i + 1], dotLineSpace);
            }
            Handles.Label(path[path.Count - 1], "End");
        }
    }

    float h, s, v;
    Color IncreaseColorH(Color color)
    {
        Color.RGBToHSV(color, out h, out s, out v);
        h += (1 + colorStep) / 255f;
        return Color.HSVToRGB(h, s, v);
    }
#endif
}

