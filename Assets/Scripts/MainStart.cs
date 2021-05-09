using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainStart : MonoBehaviour
{
    public Image black; //페이드 아웃
    public Image black2; //페이드 인

    float time = 0f;    //페이드인아웃
    float F_time = 1f;  //페이드인아웃

    float delay = 0.0f;     //타이머
    float currentTime;     //타이머

    int s;      //버튼 구분

    //게임시작
    public void Start()
    {
        black.gameObject.SetActive(false);
        black2.gameObject.SetActive(true);
        FadeEnd();  //게임 메인화면으로 페이드 인
    }

    //업데이트
    public void Update()
    {
        delay += Time.deltaTime;
        if (s == 1 && currentTime + 1 < delay) { SceneManager.LoadScene("Start"); }
        if (s == 2 && currentTime + 1 < delay) { SceneManager.LoadScene("Help"); }
        if (s == 3 && currentTime + 1 < delay) { Application.Quit(); }
        if (s == 4 && currentTime + 1 < delay) { SceneManager.LoadScene("Main"); }
    }
    //게임 시작 버튼
    public void StartGame(GameObject button)
    {
        FadeStart(); currentTime = delay; s = 1;
    }
    //게임 도움 버튼
    public void HelpGame(GameObject button)
    {
        FadeStart(); currentTime = delay; s = 2;
    }
    //게임 종료 버튼
    public void QuitGame(GameObject button)
    {
        FadeStart(); currentTime = delay; s = 3;
    }
    //게임 메인으로 돌아가기 버튼
    public void ReturnMainGame(GameObject button)
    {
        FadeStart(); currentTime = delay; s = 4;
    }
    //페이드 아웃
    public void FadeStart()
    {
        time = 0;
        black.gameObject.SetActive(true);
        StartCoroutine(FadeFlow());
    }
    IEnumerator FadeFlow()
    {
        Color alpha = black.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            black.color = alpha;
            yield return null;
        }
        yield return null;
    }
    //페이드 인
    public void FadeEnd()
    {
        time = -1;
        StartCoroutine(FadeFlow2());
    }
    IEnumerator FadeFlow2()
    {
        Color alpha = black2.color;
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            black2.color = alpha;
            yield return null;
        }
        black2.gameObject.SetActive(false);
        yield return null;
    }
}
