using UnityEngine;
using UnityEngine.Video;

public class FogueiraTrigger : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlay;
    [SerializeField] private GameManager gameManager;

    private CircleCollider2D circleCollider;

    private void Start()
    {
        videoPlay = GetComponent<VideoPlayer>();
        gameManager = GetComponent<GameManager>();
        circleCollider  = GetComponent<CircleCollider2D>();

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (Input.GetKeyDown("E"))
        {


        }
    }

}
