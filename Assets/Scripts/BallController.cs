using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class BallController : MonoBehaviour
{
    public TMP_Text _ballCountText = null;
    public List<GameObject> _balls = new List<GameObject>();
    public float horizontalSpeed;
    public float moveSpeed;
    public float horizontalLimit;
    public float horizontalLimity;
    private float _horizontal;
    public int targetCount = 0;
    public int ballCount = 0;
    public int gateNumber = 0;
    public GameObject ballPref;
    public GameObject panel;
    public GameObject finishPanel;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        if (!PlayerPrefs.HasKey("nextLvl"))
        {
            Debug.Log("Yoktu getirdik");
            PlayerPrefs.SetInt("nextLvl", 0);
        }
        else
        {
            if(PlayerPrefs.GetInt("nextLvl") != SceneManager.GetActiveScene().buildIndex)
            {
                Debug.Log(PlayerPrefs.GetInt("nextLvl").ToString());
                SceneManager.LoadScene(PlayerPrefs.GetInt("nextLvl"));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        HorizontalBallMove();
        ForwardBallMove();
        UpdateText();
        ballCount = _balls.Count - 1;
        if(_balls.Count <= 0)
        {
            GameOver();
        }
    }

    private void HorizontalBallMove()
    {

        float newX;
        if (Input.GetMouseButton(0))
        {
            _horizontal = Input.GetAxisRaw("Mouse X");
        }
        else
        {
            _horizontal = 0;
        }
        newX = transform.position.x + _horizontal * horizontalSpeed * Time.deltaTime;
        newX = Mathf.Clamp(newX, horizontalLimity, horizontalLimit);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    private void ForwardBallMove()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BallStack"))
        {
            other.gameObject.transform.SetParent(transform);
            other.gameObject.GetComponent<SphereCollider>().enabled = false;
            other.gameObject.transform.localPosition = new Vector3(0f, 0f, _balls[_balls.Count - 1].transform.localPosition.z - 1f);
            _balls.Add(other.gameObject);
        }

        if (other.gameObject.CompareTag("Gate"))
        {
          
            gateNumber = other.gameObject.GetComponent<GateController>().GetGateNumber();
            targetCount = ballCount + gateNumber;
            if (_balls.Count > 0)
            {
                if (gateNumber > 0)
                {
                    Create();
                }
                else if (gateNumber < 0)
                {
                    Delete();
                }
            }
           
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            finishPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Create()
    {
        for (int i = 0; i < gateNumber; i++)
        {
            GameObject newBall = Instantiate(ballPref);
            newBall.transform.SetParent(transform);
            newBall.GetComponent<SphereCollider>().enabled = false;
            newBall.transform.localPosition = new Vector3(0f, 0f, _balls[_balls.Count - 1].transform.localPosition.z - 1f);
            _balls.Add(newBall);
        }
    }
    public void Delete()
    {
        
        for (int i = ballCount; i >= targetCount+1; i--)
        {
            if (i<0)
            {
                break;
            }
            _balls[i].SetActive(false);
            _balls.RemoveAt(i);
            
        }
    }

    public void UpdateText()
    {
        _ballCountText.text = _balls.Count.ToString();
    }
    public void Restart()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        panel.SetActive(true);
    }
    public void Finish()
    {
        if(PlayerPrefs.GetInt("nextLvl") == 4)
        {
            PlayerPrefs.SetInt("nextLvl", 0);
        }
        else
        {
            PlayerPrefs.SetInt("nextLvl", PlayerPrefs.GetInt("nextLvl") + 1);
        }
        SceneManager.LoadScene(PlayerPrefs.GetInt("nextLvl"));
    }

}
