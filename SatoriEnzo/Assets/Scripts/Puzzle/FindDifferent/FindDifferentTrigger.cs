using UnityEngine;

public class FindDifferentTrigger : MonoBehaviour
{
    public FindDifferentPuzzle manager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Vector3 hitPos = transform.position;

        if (CompareTag("Correct"))
            manager.OnCorrectTouched(hitPos);
        else if (CompareTag("Wrong"))
            manager.OnWrongTouched(hitPos);
    }
}
