using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShipControl : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Vector3 centerPosition;
    private float shipHeight;

    private bool sailsUp;

    public Animator sailsAnimator;

    public SpriteRenderer shipSpriteRenderer;
    public CameraScript cameraScript;
    public GameObject frontWaveParent;
    public GameObject gameOverPanel;
    public Text instructionText;

    public AudioClip screamingSound;
    public AudioClip crashSound;
    public AudioClip splash1Sound, splash2Sound;
    public AudioClip sailsSound;
    private AudioSource source;

    public float forceVerticalScale = 1f;
    public float forceHorizontalScale = 1f;
    public float forceJumpScale = 10f;
    public float floatingLimit = 2f;

    public float crashVolume = 0.6f;
    public float splash1Volume = 1f;
    public float splash2Volume = 1f;
    public float screamVolume = 1f;
    public float sailsVolume = 0.7f;

    private bool forceUp = false;
    private bool flipped = false;
    private bool gameOver = false;
    private bool jumped = false;
    private bool crashed = false;

	// Use this for initialization
	void Start ()
    {
        gameOverPanel.SetActive(false);

        rb2D = gameObject.GetComponent<Rigidbody2D>();
        centerPosition = rb2D.transform.position;
        shipHeight = shipSpriteRenderer.sprite.bounds.size.y * shipSpriteRenderer.gameObject.transform.localScale.y;
        sailsUp = false;

        source = GetComponent<AudioSource>();

        // initial floating position
        rb2D.transform.SetPositionAndRotation(centerPosition + (Vector3.up * shipHeight / floatingLimit), Quaternion.identity);

        StartCoroutine(ShowInstructions());
    }
	
	// Update is called once per frame
	void Update ()
    {
        Floating();

        if (gameOver == false)
        {
            Sailing();
            Flip();
            Jump();
        }

        if (gameOver == true)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision happened!");

        if (crashed == false)
        {
            source.PlayOneShot(crashSound, crashVolume);
            gameOver = true;
            gameOverPanel.SetActive(true);
            crashed = true;
        }
    }

    private void Floating()
    {
        Vector3 forceToApply;
        if (forceUp == false)
        {
            float forceScale = -1f * forceVerticalScale * Mathf.Abs((rb2D.transform.position.y - (centerPosition.y - shipHeight / floatingLimit)));
            forceToApply = Vector3.up * forceScale;
            //Debug.Log("Force up: " + forceUp);
            //Debug.Log(forceScale);
        }
        else
        {
            float forceScale = forceVerticalScale * Mathf.Abs((rb2D.transform.position.y - (centerPosition.y + shipHeight / floatingLimit)));
            forceToApply = Vector3.up * forceScale;
            //Debug.Log("Force up: " + forceUp);
            //Debug.Log(forceScale);
        }

        if (forceToApply.y < 0 && rb2D.transform.position.y < centerPosition.y - shipHeight / floatingLimit
            || forceToApply.y > 0 && rb2D.transform.position.y > centerPosition.y + shipHeight / floatingLimit)
        {
            forceUp = !forceUp;
        }

        rb2D.AddForce(forceToApply);
    }

    private void Sailing()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            sailsUp = !sailsUp;
            source.PlayOneShot(sailsSound, sailsVolume);
        }

        if (sailsUp)
        {
            rb2D.AddForce(forceHorizontalScale * Vector2.left);
        }
        sailsAnimator.SetBool("sailing", sailsUp);
    }

    void Flip()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Vector3 newScale = rb2D.transform.localScale;
            newScale.y = -newScale.y;
            rb2D.transform.localScale = newScale;

            Vector3 verticalShift;
            Color frontWaveNewColor;
            SpriteRenderer[] frontWaveChildrenSpriteRenderers = frontWaveParent.GetComponentsInChildren<SpriteRenderer>();

            if (flipped)
            {
                verticalShift = (Vector3.up * shipHeight);
                frontWaveNewColor = frontWaveChildrenSpriteRenderers[0].color;
                frontWaveNewColor.a = 1f;
            
            }
            else
            {
                verticalShift = (Vector3.down * shipHeight);
                frontWaveNewColor = frontWaveChildrenSpriteRenderers[0].color;
                frontWaveNewColor.a = 0.3f;
            }

            rb2D.transform.position += verticalShift;
            centerPosition += verticalShift;

            foreach (var childSpriteRenderer in frontWaveChildrenSpriteRenderers)
            {
                childSpriteRenderer.color = frontWaveNewColor;
            }

            cameraScript.VerticalShift(verticalShift);

            flipped = !flipped;

            source.PlayOneShot(screamingSound, screamVolume);

            if (jumped == true)
            {
                jumped = false;
                source.PlayOneShot(splash2Sound, splash2Volume);
            }
        }
    }

    void LateUpdate()
    {
        //Debug.Log("1: " + jumped + (rb2D.transform.position.y < centerPosition.y + shipHeight / floatingLimit) + (rb2D.velocity.y < 0));
        //Debug.Log("2: " + jumped + (rb2D.transform.position.y > centerPosition.y - shipHeight / floatingLimit) + (rb2D.velocity.y > 0));

        if (!flipped && jumped
            && rb2D.transform.position.y < centerPosition.y + shipHeight / floatingLimit
            && rb2D.velocity.y < 0)
        {
            jumped = false;
            source.PlayOneShot(splash2Sound, splash2Volume);
        }
        
        if (flipped && jumped
            && rb2D.transform.position.y > centerPosition.y - shipHeight / floatingLimit
            && rb2D.velocity.y > 0)
        {
            jumped = false;
            source.PlayOneShot(splash2Sound, splash2Volume);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumped == false)
        {
            if (jumped == false)
            {
                jumped = true;
                source.PlayOneShot(splash1Sound, splash1Volume);
            }
            
            float factor = flipped ? -1f : 1f;
            rb2D.AddForce(Vector2.up * forceJumpScale * factor, ForceMode2D.Impulse);
        }
    }

    private IEnumerator ShowInstructions()
    {
        // Debug.Log("Entered coroutine!");
        yield return new WaitForSeconds(3f);
        // Debug.Log("About to hide the instructions!");
        instructionText.gameObject.SetActive(false); 
    }
}
