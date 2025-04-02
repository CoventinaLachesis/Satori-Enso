using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum BulletPatternType
{
    OrbitingBulletRing,
    SpiralShot,
    RadialBurst,
    WaveAttack,
    SweepingArc,
    BulletRings
}
public enum ShootDirection
{
    Up,        // 90 degrees
    Down,      // 270 degrees
    Left,      // 180 degrees
    Right,     // 0 degrees
    UpLeft,    // 135 degrees
    UpRight,   // 45 degrees
    DownLeft,  // 225 degrees
    DownRight  // 315 degrees
}
public enum BulletMotionType
{
    Straight,
    SineWave,
    Zigzag,
    Custom
}


[Serializable]
public class BulletPatternConfig
{
    [Header("General Settings")]
    public BulletPatternType patternType;
    public float delayBeforeStart = 0f;
    public GameObject bulletPrefab; // assign per pattern

    [Header("Common Parameters")]

    public int bulletCount = 12;
    public float speed = 5f;

    [Header("Motion Settings")]
    public BulletMotionType motionType = BulletMotionType.Straight;
    [ConditionalField("motionType", BulletMotionType.Zigzag, BulletMotionType.SineWave)]
    public float waveFrequency = 5f;
    [ConditionalField("motionType", BulletMotionType.Zigzag, BulletMotionType.SineWave)]
    public float waveAmplitude = 0.5f;

    [Header("Pattern Specific Settings")]
    [ConditionalField("patternType", BulletPatternType.OrbitingBulletRing)]
    public float initialRadius = 2f;

    [ConditionalField("patternType", BulletPatternType.OrbitingBulletRing)]
    public float expansionRate = 0.5f;

    [ConditionalField("patternType", BulletPatternType.OrbitingBulletRing)]
    public float sizeIncreaseRate = 0.2f;

    [ConditionalField("patternType", BulletPatternType.SpiralShot)]
    public int spirals = 3;

    [ConditionalField("patternType", BulletPatternType.BulletRings)]
    public int rings = 3;

    [ConditionalField("patternType", BulletPatternType.WaveAttack,BulletPatternType.SweepingArc)]
    public ShootDirection shootDirection = ShootDirection.Right;


}

// ConditionalField Attribute
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class ConditionalFieldAttribute : PropertyAttribute
{
    public string ConditionFieldName { get; private set; }
    public object[] CompareValues { get; private set; }

    // Constructor for single value
    public ConditionalFieldAttribute(string conditionFieldName, object compareValue)
    {
        ConditionFieldName = conditionFieldName;
        CompareValues = new object[] { compareValue };
    }

    // Constructor for multiple values (OR logic)
    public ConditionalFieldAttribute(string conditionFieldName, params object[] compareValues)
    {
        ConditionFieldName = conditionFieldName;
        CompareValues = compareValues;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
public class ConditionalFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute conditionalField = attribute as ConditionalFieldAttribute;

        // Get the condition property
        string conditionPath = property.propertyPath.Replace(property.name, conditionalField.ConditionFieldName);
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditionPath);

        if (conditionProperty != null)
        {
            bool shouldDisplay = false;

            // Check if the condition property matches any of the compare values (OR logic)
            if (conditionalField.CompareValues != null)
            {
                foreach (object compareValue in conditionalField.CompareValues)
                {
                    if (conditionProperty.propertyType == SerializedPropertyType.Enum)
                    {
                        if (conditionProperty.enumValueIndex == (int)compareValue)
                        {
                            shouldDisplay = true;
                            break; // Exit loop if a match is found
                        }
                    }
                    else if (conditionProperty.propertyType == SerializedPropertyType.Boolean)
                    {
                        if (conditionProperty.boolValue == (bool)compareValue)
                        {
                            shouldDisplay = true;
                            break; // Exit loop if a match is found
                        }
                    }
                }
            }

            // Only draw the field if the condition is met
            if (shouldDisplay)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
        else
        {
            // If the condition property is not found, show a warning and draw the field
            Debug.LogWarning($"ConditionalField: Property '{conditionalField.ConditionFieldName}' not found.");
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute conditionalField = attribute as ConditionalFieldAttribute;
        string conditionPath = property.propertyPath.Replace(property.name, conditionalField.ConditionFieldName);
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditionPath);

        if (conditionProperty != null)
        {
            bool shouldDisplay = false;

            if (conditionalField.CompareValues != null)
            {
                foreach (object compareValue in conditionalField.CompareValues)
                {
                    if (conditionProperty.propertyType == SerializedPropertyType.Enum)
                    {
                        if (conditionProperty.enumValueIndex == (int)compareValue)
                        {
                            shouldDisplay = true;
                            break;
                        }
                    }
                    else if (conditionProperty.propertyType == SerializedPropertyType.Boolean)
                    {
                        if (conditionProperty.boolValue == (bool)compareValue)
                        {
                            shouldDisplay = true;
                            break;
                        }
                    }
                }
            }

            if (!shouldDisplay)
            {
                return -EditorGUIUtility.standardVerticalSpacing; // Hide the field
            }
        }

        return EditorGUI.GetPropertyHeight(property, label);
    }
}
#endif