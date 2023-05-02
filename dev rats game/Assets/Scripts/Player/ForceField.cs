using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ForceField : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    [SerializeField] private int steps;
    [SerializeField] public float radius;
    [Range(0f, 360f)]
    [SerializeField] public float angle;
    [SerializeField] private float health;
    [SerializeField] private float regenAmount;
    [SerializeField] private float regenTime;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] public TextMeshProUGUI lengthText;

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

    private void Start()
    {
        _health = health;
        _steps = steps;

        vertexPos = new List<Vector2>();
        closedPoints = new List<int>();
        curAngle = angle;
        DrawForceField();
        maxHealth = _health;
        multiplier = _steps / _health;
        curRegenTime = regenTime;
        canRegen = false;
    }

    private void Update()
    {
        health = _health/2;
        steps = _steps/2;

        healthText.text = new string("Health : " + health);
        lengthText.text = new string("Shield Length : " + steps);
        if (curRegenTime <= 0f && canRegen)
        {
            curRegenTime = regenTime;
            RegenerateShield(regenAmount);
        }
        else if(curRegenTime >= 0f)
            curRegenTime -= Time.deltaTime;

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
        if (_health - damage <= 0f)
            return;
        _health -= damage;
        _steps = Mathf.RoundToInt(multiplier * _health);
        ShrinkField(line.positionCount - _steps);
        canRegen = false;
        Invoke("StartRegen", 2f);
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
    }

    public void RegenerateShield(float regenAmount)
    {
        if (_health + regenAmount > maxHealth)
            return;

        _health += regenAmount;
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
    }

    private void StartRegen()
    {
        canRegen = true;
    }
}