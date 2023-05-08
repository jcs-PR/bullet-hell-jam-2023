using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ForceField : MonoBehaviour
{
    [SerializeField] private  LineRenderer line;
    [SerializeField] private Material color;
    [SerializeField] private  Material transparent;
    [SerializeField] private float steps;
    [SerializeField] public float radius;
    [Range(0f, 360f)]
    [SerializeField] public float angle;
    [SerializeField] private float health;
    [SerializeField] private float regenAmount;
    [SerializeField] private float regenTime;

    List<Vector2> vertexPos;
    List<int> closedPoints;
    float _health;
    int _steps;
    float curAngle;
    float multiplier;
    int maxClosedPoints;
    int minClosedPoints;
    float maxHealth;
    float curRegenTime;
    bool canRegen;
    MeshCollider meshCollider;
    LineRenderer _line;
    int prevSteps;
    float maxSteps;

    private void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        _health = health;
        _steps = (int) steps;
        vertexPos = new List<Vector2>();
        closedPoints = new List<int>();
        curAngle = angle;
        maxHealth = _health;
        multiplier = _steps / _health;
        curRegenTime = regenTime;
        prevSteps = _steps;
        maxSteps = _steps * multiplier;
        canRegen = false;
        //DrawForceField();
        //GenerateCollider();
    }

    private void Update()
    {
        health = _health * multiplier * multiplier;
        steps = _steps * multiplier;

        if (curRegenTime <= 0f && canRegen)
        {
            curRegenTime = regenTime;
            RegenerateShield(regenAmount);
        }
        else if(curRegenTime >= 0f)
            curRegenTime -= Time.deltaTime;

        if (Input.GetKey(KeyCode.E))
        {
            DoDamage(10f);
        }
        if (steps == 0f)
            health = 0f;
    }

    public Vector3 DirFromAngle(float angle)
    {
        angle -= transform.eulerAngles.z;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0f);
    }

    private void DrawForceField()
    {
        line.positionCount = _steps + 1;

        for (int curStep = 0; curStep <= _steps; curStep++)
        {
            float progress = (float) curStep / _steps;
            float curRadian = progress * curAngle;
            //Vector3 position = transform.position + (DirFromAngle(curAngle / 2) * radius);
            Vector3 position = DirFromAngle((curRadian)) * radius;
            vertexPos.Add(position);
        }
        foreach (var point in vertexPos)
        {
            line.SetPosition(vertexPos.IndexOf(point), point);
        }
        vertexPos.Clear();
        transform.rotation = Quaternion.EulerAngles(0f, 0f, angle/2 * Mathf.Deg2Rad);

        for (int i = 0; i < line.positionCount; i++)
        {
            vertexPos.Add(line.GetPosition(i));
        }
    }

    public void DoDamage(float damage)
    {
        if (steps >= maxSteps/2)
        {
            _health -= damage;
            _steps = Mathf.RoundToInt(multiplier * _health);
            ShrinkField(line.positionCount - _steps);
            canRegen = false;
            CancelInvoke("StartRegen");
            Invoke("StartRegen", 2f);
        }
    }

    public void ShrinkField(int pointsToClose)
    {
        int count = closedPoints.Count;
        if(pointsToClose != 0)
        {
            for (int i = count; i < pointsToClose; i++)
            {
                closedPoints.Add(i);
            }
            minClosedPoints = closedPoints.Count;
            maxClosedPoints = line.positionCount - closedPoints.Count;

            foreach (var point in closedPoints)
            {
                line.SetPosition(point, line.GetPosition(minClosedPoints));
                line.SetPosition(line.positionCount - point-1, line.GetPosition(maxClosedPoints));
            }
        }

        GenerateCollider();
    }

    public void RegenerateShield(float regenAmount)
    {
        if (_health >= maxHealth)
            return;
        _health = Mathf.Clamp(_health + regenAmount, 0f, maxHealth);
        _steps = Mathf.RoundToInt(multiplier * _health);
        int closedCount = closedPoints.Count;
        Vector2 startPos = Vector2.zero;
        Vector2 endPos = Vector2.zero;

        for (int i = line.positionCount - _steps-1; i < closedCount; i++)
        {
            int fromLast = line.positionCount - i-1;
            line.SetPosition(i, vertexPos[i]);
            line.SetPosition(fromLast, vertexPos[fromLast]);
            closedPoints.Remove(i);
            closedPoints.Remove(fromLast);
            if (startPos == Vector2.zero)
            {
                startPos = line.GetPosition(i);
                endPos = line.GetPosition(fromLast);
            }
        }

        foreach (var point in closedPoints)
        {
            line.SetPosition(point, startPos);
            line.SetPosition(line.positionCount - point - 1, endPos);
        }

        GenerateCollider();
    }

    private void StartRegen() => canRegen = true;

    public void MakeAvailable()
    {
        //gameObject.SetActive(true);
        line.material = color;
        meshCollider.enabled = true;
    }

    public void MakeUnavailable()
    {
        //gameObject.SetActive(false);
        line.material = transparent;
        meshCollider.enabled = false;
    }

    private void GenerateCollider()
    {
        if(prevSteps != steps)
        {
            _line = line;
            Mesh mesh = new Mesh();
            //line.Simplify(0.1f);
            line.BakeMesh(mesh);
            meshCollider.sharedMesh = mesh;
            prevSteps = _steps;
        }
    }
}