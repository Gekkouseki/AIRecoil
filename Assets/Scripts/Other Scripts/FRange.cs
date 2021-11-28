using UnityEngine;

[System.Serializable]
public class FRange
{
    [SerializeField]
    private float m_value;
    public float Value { get => m_value; 
        set
        {
        m_value = value;
        Clamp();
        } 
    }

    [SerializeField]
    private float minValue;
    public float MinValue { get => minValue; }

    [SerializeField]
    private float maxValue;
    public float MaxValue { get => maxValue; }

    public void Clamp()
    {
        m_value = Mathf.Clamp(m_value, minValue, maxValue);
    }

    public float DiffValue(float value)
    {
        m_value = value;
        if (value < minValue)
            return value - minValue;
        else if (value > maxValue)
            return value - maxValue;
        else
            return 0.0f;
    }

    public float ClampValue(float value)
    {
        m_value = value;
        Clamp();
        return m_value;
    }

    public float GetAddValue(float angle,float add)
    {
        if (angle + add < minValue)
            return minValue - angle;
        else if (angle + add > maxValue)
            return maxValue + angle;
        else
            return add;
    }

    public void OnValidate()
    {
        if (minValue > maxValue)
            minValue = maxValue;
        if (maxValue < minValue)
            maxValue = minValue;
        Clamp();
    }
}
