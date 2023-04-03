using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class BezierCurve : MonoBehaviour
{
    public Transform[] controlPoints;
    public LineRenderer lineRenderer;
    public bool deleteCurve;
    public bool isTentacleGround; //TODO: isTentacleGround değişkeni kol yere değdiğinde true olacak , kol geri çekileceği zaman false
    private int curveCount = 0;    
    private int layerOrder = 0;
    private int SEGMENT_COUNT = 14;
    private bool isDrawDone = false;

    public float speed = 1f;
    public UnityEvent deleteCurveEvent;
    public UnityEvent addCurveEvent;
    private MimicStates mimicStates;
    void Start()
    {
        if (!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.sortingLayerID = layerOrder;
        curveCount = (int)controlPoints.Length / 3;
        deleteCurveEvent.AddListener(DeleteCurveEvent);
        addCurveEvent.AddListener(ADDcurveEvent);
        mimicStates = MimicStates.walking;
        //StartCoroutine(DrawCurve()); 

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AddTentacle();
        }

        if (Input.GetMouseButtonUp(0))
        {
            RemoveTentacle();
        }
        switch (mimicStates)
        {
            case MimicStates.standing:
                deleteCurveEvent.Invoke();
                mimicStates = MimicStates.tentacleOff;
                break;
            case MimicStates.walking:
                addCurveEvent.Invoke();
                mimicStates = MimicStates.tentacleOn;
                break;
            case MimicStates.tentacleOn:
                if (isTentacleGround)   //TODO: isTentacleGround değişkeni kol yere değdiğinde true olacak , kol geri çekileceği zaman false 
                {
                    DrawCurves();
                }
                if (deleteCurve)
                {
                    mimicStates = MimicStates.standing;
                }
                break;
            case MimicStates.tentacleOff:
                if (!deleteCurve)
                {
                    mimicStates = MimicStates.walking;
                }
                break;
            case MimicStates.death:
                
                break;
        }
        Debug.Log(mimicStates + " " + deleteCurve);
    }

    public void RemoveTentacle()
    {
        deleteCurve = true;
    }

    public void AddTentacle()
    {
        deleteCurve = false;
    }
     void DrawCurves()
    {
        for (int j = 0; j <curveCount; j++)
        {
            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                float t = i / (float)SEGMENT_COUNT;
                int nodeIndex = j * 3;
                Vector3 pixel = CalculateCubicBezierPoint(t, controlPoints [nodeIndex].localPosition, controlPoints [nodeIndex + 1].localPosition, controlPoints [nodeIndex + 2].localPosition, controlPoints [nodeIndex + 3].localPosition);
                lineRenderer.SetVertexCount(((j * SEGMENT_COUNT) + i));
                lineRenderer.SetPosition((j * SEGMENT_COUNT) + (i - 1), pixel);
            }
            
        }
    }
    IEnumerator DrawCurve()
    {
        if (!isDrawDone)
        {
            for (int j = 0; j < curveCount; j++)
            {
                for (int i = 1; i <= SEGMENT_COUNT; i++)
                {
                    float t = i / (float) SEGMENT_COUNT;
                    int nodeIndex = j * 3;
                    Vector3 pixel = CalculateCubicBezierPoint(t, controlPoints[nodeIndex].localPosition,
                        controlPoints[nodeIndex + 1].localPosition, controlPoints[nodeIndex + 2].localPosition,
                        controlPoints[nodeIndex + 3].localPosition);
                    lineRenderer.SetVertexCount(((j * SEGMENT_COUNT) + i));
                    lineRenderer.SetPosition((j * SEGMENT_COUNT) + (i - 1), pixel);
                    var delay = new WaitForSeconds(0.001f / speed * Time.deltaTime);
                    yield return delay;
                }

            }

            isTentacleGround = true;
            isDrawDone = true;

        }
    }

    public void DeleteCurveEvent()
    {
        StartCoroutine(DeleteCurve());
    }

    public void ADDcurveEvent()
    {
        StartCoroutine(DrawCurve());
    }
    
    IEnumerator DeleteCurve()
    {
        if (isDrawDone)
        {
            isTentacleGround = false;
            for (int i = lineRenderer.positionCount; i > 0; i--)
            {
                lineRenderer.SetPosition(i - 1, new Vector3(0, 0, 0));
                lineRenderer.SetVertexCount(i - 1);
                var delay = new WaitForSeconds(0.001f / speed * Time.deltaTime);
                yield return delay;
            }
            isDrawDone = false;
        }
    }
    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        
        Vector3 p = uuu * p0; 
        p += 3 * uu * t * p1; 
        p += 3 * u * tt * p2; 
        p += ttt * p3; 
        
        return p;
    }

    enum MimicStates
    {
        walking,
        standing,
        tentacleOn,
        tentacleOff,
        death
    }
}