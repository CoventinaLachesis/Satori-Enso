using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum BulletType
{
    Typing,
    Math
}

public class BossBullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;
    public BulletType bulletType = BulletType.Typing;

    private Vector2 direction;

    [Header("Typing Mode")]
    public string word = "Hello"; // For Typing Mode

    [Header("Math Mode")]
    public string equation = "3+2"; // Displayed equation
    private int correctAnswer = 5;  // Correct answer for Math Mode

    private Text bulletText;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Start()
    {
        if (gameObject.scene.rootCount != 0)
        {
            Destroy(gameObject, lifetime);
        }

        bulletText = GetComponentInChildren<Text>();

        if (bulletText != null)
        {
            bulletText.text = bulletType == BulletType.Typing ? word : equation;
        }
    }

    private void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    public bool CheckAnswer(string input)
    {
        if (bulletType == BulletType.Typing)
        {
            return input.Equals(word, System.StringComparison.OrdinalIgnoreCase);
        }
        else if (bulletType == BulletType.Math)
        {
            if (int.TryParse(input, out int result))
            {
                return result == correctAnswer;
            }
        }
        return false;
    }
}
