using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{
    [SerializeField] private string destinationScene;
    [SerializeField] private Vector3 destinationPosition;

    private IEnumerator Teleport(Transform player)
    {
        yield return SceneManager.LoadSceneAsync(
            destinationScene,
            LoadSceneMode.Additive
        );

        player.position = destinationPosition;

        yield return SceneManager.UnloadSceneAsync(gameObject.scene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Teleport(collision.transform));
        }
    }
}