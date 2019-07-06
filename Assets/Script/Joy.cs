using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joy : MonoBehaviour
{

    public Vector3 Dir { get; private set; }
    public bool canjoy = true;
    public Transform line;
    private RectTransform dirpic, cl;
    private RectTransform tr;

    private Vector2 old;
    private float maxdis, dirpicdis;
    private Graphic dirpicgp;
    private CanvasGroup cg;
    // Use this for initialization
    void Start()
    {
        tr = GetComponent<RectTransform>();
        cl = tr.GetChild(0).GetComponent<RectTransform>();
        dirpic = tr.GetChild(1).GetComponent<RectTransform>(); ;
        dirpicgp = dirpic.GetComponent<Graphic>();
        cg = tr.GetComponent<CanvasGroup>();
        maxdis = (tr.sizeDelta.x - cl.sizeDelta.x) * 0.5f;
        dirpicdis = dirpic.anchoredPosition.x;
        StartCoroutine(showjoy(false));
    }

    int finger = -1;
    void Update()
    {
        if (!canjoy)
        {
            return;
        }
#if UNITY_EDITOR
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (Camera.main.ScreenToViewportPoint(Input.mousePosition).x > 0.3f)
        //    {
        //        return;
        //    }
        //    tr.position = old = Input.mousePosition;
        //    StartCoroutine(showjoy(true));
        //}
        //else if (Input.GetMouseButton(0))
        //{
        //    Vector2 newpos = Input.mousePosition;
        //    var dir = newpos - old;
        //    dir = Vector3.ClampMagnitude(dir, maxdis);
        //    if (dir.sqrMagnitude > 0.2)
        //    {
        //        cl.anchoredPosition = dir;
        //        dirpic.anchoredPosition = Vector3.Normalize(dir) * dirpicdis;
        //        dirpic.right = dir;
        //        Dir = new Vector3(dir.x, 0, dir.y);
        //        line.right = Dir;
        //        if (dirpicgp.canvasRenderer.GetColor().a < 0.001f)
        //        {
        //            dirpicgp.CrossFadeAlpha(1, 0.2f, true);
        //        }
        //    }
        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    cl.anchoredPosition = Vector3.zero;
        //    dirpicgp.CrossFadeAlpha(0, 0.35f, true);
        //    StopAllCoroutines();
        //    StartCoroutine(showjoy(false));
        //}
#endif

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    {
                        if (Camera.main.ScreenToViewportPoint(Input.GetTouch(i).position).x < 0.3f)
                        {
                            //控制move
                            finger = Input.GetTouch(i).fingerId;
                            tr.position = old = Input.GetTouch(i).position;
                            StartCoroutine(showjoy(true));
                        }
                    }
                }
                else if (Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    if (Input.GetTouch(i).fingerId == finger)
                    {
                        var dir = Input.GetTouch(i).position - old;
                        dir = Vector3.ClampMagnitude(dir, maxdis);
                        if (dir.sqrMagnitude > 0.2)
                        {
                            cl.anchoredPosition = dir;
                            dirpic.anchoredPosition = Vector3.Normalize(dir) * dirpicdis;
                            dirpic.right = dir;
                            Dir = new Vector3(dir.x, 0, dir.y);
                            line.right = Dir;
                            if (dirpicgp.canvasRenderer.GetColor().a < 0.001f)
                            {
                                dirpicgp.CrossFadeAlpha(1, 0.2f, true);
                            }
                        }
                    }
                }
                else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    if (Input.GetTouch(i).fingerId == finger)
                    {
                        cl.anchoredPosition = Vector3.zero;
                        dirpicgp.CrossFadeAlpha(0, 0.35f, true);
                        StopAllCoroutines();
                        StartCoroutine(showjoy(false));
                        finger = -1;
                    }
                }
            }
        }
    }

    IEnumerator showjoy(bool show)
    {
        if (show)
        {
            while (cg.alpha < 1)
            {
                cg.alpha += Time.deltaTime * 2;
                yield return null;
            }
        }
        else
        {
            while (cg.alpha > 0.3f)
            {
                cg.alpha -= Time.deltaTime;
                yield return null;
            }
        }
    }
}
