using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;


public class OneLevelController : MonoBehaviour
{
    [SerializeField] private Save Save;

    public int count, miss;
    public bool endGame = false;

    [SerializeField] private GameObject PanelGameEnd, PanelGamePause, PanelCountGet;
    [Header("GameEnd")]
    [SerializeField] private TextMeshProUGUI m_TextScoreEnd, m_TextMissEnd;
    [Header("GamePause")]
    [SerializeField] private TextMeshProUGUI m_TextScorePause;

    [Header("Game")]
    private GameObject BallInstantiate;
    private float x, y, wait;

    [SerializeField] private TextMeshProUGUI m_TextScore, m_TextMiss, m_TextCountDown;

    [SerializeField] private GameObject Ball, Parent;

    [SerializeField] private float minX, maxX, minY, maxY;

    public void CountDown()
    {
        m_TextCountDown.text = "3...";
        StartCoroutine(CountGet());
    }
    IEnumerator CountGet()
    {
        for (int i = 3; i > 0; i--)
        {
            m_TextCountDown.text = i.ToString() + "...";
            yield return new WaitForSeconds(1f);
        }
        PanelCountGet.SetActive(false);
        StartGame();
    }
    public void MissClick()
    {
        miss++;
        m_TextMiss.text = miss.ToString() + "/3";
        if (miss >= 3)
            GameEnd();
    }
    public void ScoreUpdate()
    {
        count++;
        m_TextScore.text = count.ToString();
    }
    
    public void StartGame()
    {
        Repeat();

        wait = 1.3f;
        StartCoroutine(InstantiateBall(0.1f));
    }
    
    public void GameEnd()
    {
        endGame = true;
        StopAllCoroutines();

        PanelGameEnd.SetActive(true);
        m_TextScoreEnd.text = count.ToString();
        m_TextMissEnd.text = miss.ToString();

        Save.LoadGame();
        if (Save.score_SpeedTap < count)
        {
            Save.score_SpeedTap = count;
            Save.SaveGame();
        }
    }
    public void GamePause()
    {
        Time.timeScale = 0f;
        PanelGamePause.SetActive(true);
        m_TextScorePause.text = count.ToString();
    }
    public void GameUnPause()
    {
        Time.timeScale = 1f;
        PanelGamePause.SetActive(false);
    }
    public void Repeat()
    {
        endGame = false;
        count = 0; miss = 0;
        m_TextScore.text = "0"; m_TextMiss.text = "0/3";
        StopAllCoroutines();


        Time.timeScale = 1f;
    }
    IEnumerator InstantiateBall(float _wait)
    {
        yield return new WaitForSeconds(_wait);
        x = Random.Range(minX, maxX); y = Random.Range(minY, maxY);
        BallInstantiate = Instantiate(Ball, Parent.transform);
        BallInstantiate.GetComponent<BallClick>().Controller = this;
        BallInstantiate.transform.Translate(new Vector2(x, y));

        if (wait > 0.42)
            wait -= 0.003f;
        StartCoroutine(InstantiateBall(wait));
    }
}
