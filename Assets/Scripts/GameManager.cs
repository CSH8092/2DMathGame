using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioSource Audio_Back;  //배경음악 재생
    public AudioSource Audio_Character;  //캐릭터 대사 오디오 재생
    public AudioSource Audio_Check;  //정답 체크 오디오 재생
    public AudioSource Audio_Play;  //플레이 오디오 재생
    public AudioClip clip_Character;  //캐릭터 대사 소리
    public AudioClip clip_Check_X;    //정답 체크X 소리
    public AudioClip clip_Check_O;    //정답 체크O 소리
    public AudioClip chance_Delete;   //키보드 누르는 소리
    public AudioClip choose_Answer;   //포스트잇 설정 및 답변 제출 소리
    public AudioClip back_Playing;   //플레이 중
    public AudioClip back_Lose;   //게임에 짐
    public AudioClip back_Win;   //게임에 이김

    public Sprite[] spritePost; //스프라이트 배열
    public Image[] health;  //사용자 5번의 기회
    public Image black; //페이드 아웃
    public Image black2; //페이드 인

    float ttime = 0f;   //페이드인아웃
    float F_time = 1f;  //페이드인아웃

    int RorQ=0;   //메인으로 돌아가거나 게임종료 구분
    int IsTimeOver=0;    //시간 촉박 대사 오류(업데이트로 계속 출력) 막기 위한 변수

    bool IsGameStart = false;   //게임 진짜 시작
    bool IsGameOver = true;    //게임 끝났으면 스페이스바 막힘 (재시작 위함)

    private int Item = 0;   //보너스 퀴즈 정함
    private int quizNumber1 = 0; //퀴즈숫자1
    private int quizNumber2 = 0; //퀴즈숫자2
    private int quizCal = 0;    //퀴즈연산자
    private int quizAnswerColor = 0; //퀴즈색깔
    private int quizAnswerNumber = 0; //퀴즈 정답
    private int AnswerNumber = 0;   //사용자의숫자정답
    private int AnswerColor = 0;    //사용자의색깔정답
    private int chance = 0; //사용자의 5번 기회
    private int bosslive = 0;   //보스 체력
    private float time; //타이머 시간
    private const float timeMax = 60f;  //타이머 시간 (1분)
    private string Rtext;   //게임 결과 텍스트 1
    private string Rank;    //게임 결과 텍스트 2
    private bool dieSound=false;  //음소거 버튼

    float AllTime = 0f;  //전체 걸린 시간
    int CorrectQuiz = 0;    //맞춘 전체 퀴즈
    int WrongQuiz = 0;      //틀린 전체 퀴즈

    Image imgQuiz;      //퀴즈
    Image imgCheck;    //정답 체크
    Image chracter; //캐릭터
    Image imgWhitePostit;   //정답 체크 하얀 포스트잇
    InputField inputAnswer; //정답 입력 창
    Text textNumber1;   //첫번째 숫자
    Text textCal;   //연산기호
    Text textNumber2;   //두번째 숫자
    Text textE; //=기호
    Text textCharacter; //캐릭터 대사
    Text textTimer; //시간 체크 텍스트
    Text textResult;    //결과 텍스트
    Text textBoss;  //보스 체력 대사 (문제 맞출때마다 퍼센트 오름)
    Slider sliderTimer; //타이머
    Slider sliderBoss;  //보스 체력
    Button check;   //체크버튼
    Button sound;   //음소거버튼

    //게임 시작
    private void Start()
    {
        //오브젝트 연결
        imgQuiz = GameObject.Find("Image_Quiz").GetComponent<Image>();
        imgCheck = GameObject.Find("Image_Check").GetComponent<Image>();
        imgWhitePostit = GameObject.Find("ImageWhite").GetComponent<Image>();
        chracter = GameObject.Find("Character").GetComponent<Image>();
        inputAnswer = GameObject.Find("InputField_InputAnswer").GetComponent<InputField>();
        textNumber1 = GameObject.Find("Text_Number1").GetComponent<Text>();
        textCal = GameObject.Find("Text_Cal").GetComponent<Text>();
        textNumber2 = GameObject.Find("Text_Number2").GetComponent<Text>();
        textCharacter = GameObject.Find("Text_Character").GetComponent<Text>();
        textE = GameObject.Find("Text_E").GetComponent<Text>();
        textTimer = GameObject.Find("Text_Timer").GetComponent<Text>();
        textBoss = GameObject.Find("Text_BossLive").GetComponent<Text>();
        textResult = GameObject.Find("Text_GameResult").GetComponent<Text>();
        sliderTimer = GameObject.Find("Slider_Timer").GetComponent<Slider>();
        sliderBoss = GameObject.Find("Slider_BossLive").GetComponent<Slider>();
        check = GameObject.Find("Button_Input").GetComponent<Button>();
        sound = GameObject.Find("Button_Sound").GetComponent<Button>();
        textCharacter.text = "반가워, 지옥에 온것을 환영해. 집에 보내달라고? 스페이스바를 눌러봐."; //대사 설정
        CharacterTalk();
        Audio_Back.clip = back_Playing;
        Audio_Back.Play();
        textResult.gameObject.SetActive(false);
        InitGame(); //초기화

        //페이드인아웃
        black.gameObject.SetActive(false);
        black2.gameObject.SetActive(true);
        FadeEnd();  //게임 시작 (페이드 인)
    }
    void InitGame()
    {
        //초기화
        imgQuiz.sprite = spritePost[0];    //노랑으로 초기화
        inputAnswer.image.sprite = spritePost[0]; inputAnswer.text = "";    //노랑으로 설정 및  초기화
        check.interactable = false; //체크버튼 비활성화
        time = timeMax; //시간 최대시간으로 맞춤
        sliderTimer.value = time;
        bosslive = 0;
        sliderBoss.value = bosslive;
        textBoss.text = bosslive.ToString() + "%";
        //없앰
        textNumber1.text = "";
        textNumber2.text = "";
        textCal.text = "";
        textE.text = "";
        imgCheck.gameObject.SetActive(false);
        sliderTimer.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0 / 255f, 251 / 255f, 181 / 255f);
        for (int i = 0; i < 5; i++) { health[i].gameObject.SetActive(true); }
    }
    //게임 실행 도중 계속 업데이트
    private void Update()
    {
        textTimer.text = Mathf.RoundToInt(time).ToString() + "초";
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }   //ESC 누르면 게임 종료
        //스페이스 누르면 게임 시작
        if (Input.GetKeyDown(KeyCode.Space) && IsGameOver) {
            check.interactable = true;  //버튼 활성화
            Animation aniCharacter = chracter.GetComponent<Animation>();
            aniCharacter.Play();
            textCharacter.text = "미안하지만, 그냥은 못보내줘. 내가 내는 문제를 맞추면 집에 보내줄게."; //대사 설정
            CharacterTalk();
            IsGameStart = true; IsGameOver = false; SetQuiz();
        }
        //R누르면 재시작
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("Start");
        }
        //애니메이션 실행
        Animation quiz = imgQuiz.GetComponent<Animation>(); quiz.Play();
        Animation a1 = health[0].GetComponent<Animation>(); a1.Play();
        Animation a2 = health[1].GetComponent<Animation>(); a2.Play();
        Animation a3 = health[2].GetComponent<Animation>(); a3.Play();
        Animation a4 = health[3].GetComponent<Animation>(); a4.Play();
        Animation a5 = health[4].GetComponent<Animation>(); a5.Play();
        Animation white = imgWhitePostit.GetComponent<Animation>(); white.Play();
        //타이머 관련
        if (IsGameStart) {
            AllTime += Time.deltaTime;
            if (time <= 0) { time = 0; GameOver(); }    //0초가 되면 기회 삭제 (= 게임오버)
            else if (time > 0) {
                time -= Time.deltaTime;
                //시간이 촉박한 경우
                if (time < 15f)
                {
                    IsTimeOver++; if (IsTimeOver == 1) { SetCharacter(0); }
                    sliderTimer.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(255 / 255f, 199 / 255f, 0 / 255f);
                    if (time < 5f)
                    {
                        sliderTimer.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(255 / 255f, 162 / 255f, 181 / 255f);
                    }
                }
                else IsTimeOver = 0;
            }
            sliderTimer.value = time / timeMax;
        }
        }
    //문제 설정
    void SetQuiz() {
        quizNumber1 = Random.Range(1, 100);
        quizNumber2 = Random.Range(1, 100);
        quizCal = Random.Range(0, 3);   //0,1,2 (나눗셈 연산은 제외함)
        quizAnswerColor = Random.Range(0, 3); //0,1,2

        Item = Random.Range(0, 6); //0 ~ 5까지 5가지 난수 발생

        textE.text = "=";
        //난수발생을 토대로 문제 제출 & 정답 도출
        textNumber1.text = quizNumber1.ToString();
        textNumber2.text = quizNumber2.ToString();
        if (quizCal == 0) {
            textCal.text = "+";
            quizAnswerNumber = quizNumber1 + quizNumber2;
        }
        else if (quizCal == 1) {
            textCal.text = "-";
            quizAnswerNumber = quizNumber1 - quizNumber2;
        }
        else {
            textCal.text = "*";
            quizAnswerNumber = quizNumber1 * quizNumber2;
        }
        imgQuiz.sprite = spritePost[quizAnswerColor];
        //Item이 0이고, 플레이어가 1문제 이상을 틀렸을 경우 해당 퀴즈를 보너스 퀴즈로 설정
        if (Item == 0 && chance >= 1)
        {
            textNumber1.color = Color.red; textCal.color = Color.red;
            textNumber2.color = Color.red; textE.color = Color.red;
            SetCharacter(7);
        }
        else
        {
            textNumber1.color = Color.black; textCal.color = Color.black;
            textNumber2.color = Color.black; textE.color = Color.black;
        }
        ChooseSoundPlay(); inputAnswer.text = ""; //문제 제출 소리 및 입력칸 초기화
    }
    //정답 체크 버튼
    public void CheckResult(GameObject button)
    {
        imgCheck.gameObject.SetActive(true);    //정답 체크 활성화
        if (inputAnswer.text == "") {
            //공백을 입력받았을 경우
            NoAnswer();
            SetCharacter(1);
            cheat();   //답 확인 치트 (콘솔창)
        }
        else {
            AnswerNumber = int.Parse(inputAnswer.text); //int형으로 변환
            if (AnswerNumber == quizAnswerNumber)
            {
                if (AnswerColor == quizAnswerColor)
                {
                    //숫자답과 색깔답이 둘다 맞은 경우
                    YesAnswer();
                }
                else
                {
                    //숫자답은 맞았지만 색깔답이 틀린 경우
                    NoAnswer();
                    SetCharacter(2);
                }
            }
            else
            {
                //숫자 답을 틀린 경우
                NoAnswer();
                SetCharacter(3);
            }
        }
        ChooseSoundPlay();  //소리 재생
        Animation a = imgCheck.GetComponent<Animation>(); a.Play();
        Audio_Check.Play(); //애니메이션 재생
    }
    //정답이 맞은 경우
    void YesAnswer() {
        CorrectQuiz++;
        imgCheck.sprite = Resources.Load("O", typeof(Sprite)) as Sprite;    //이미지 설정
        Audio_Check.clip = clip_Check_O;    //소리 설정
        if (quizCal == 0 || quizCal == 1) { time += 5; }    //덧셈 뺄셈인 경우 +5의 시간
        else { time += 10; } //곱셈인 경우 +10의 시간
        bosslive += 5; //한 문제당 5점 (총 20문제를 맞춰야 함)
        textBoss.text = bosslive.ToString() + "%";
        sliderBoss.value = bosslive;
        //맞춘 퀴즈가 보너스 퀴즈이고, 기회가 5개 미만인 경우(본 코드는 chance를 0부터 시작) 기회를 1개 올려줌
        if (Item == 0 && chance >= 1)
        {
            chance--;
            health[chance].gameObject.SetActive(true);
        }
        if (bosslive >= 100) { GameVictory(); }   //만약 게이지100을 채우면 게임 승리
        else {
            SetQuiz();  // 정답이 맞은 경우만 다음 단계로 넘어감
            SetCharacter(4);
        }
    }
    //정답이 틀린 경우
    void NoAnswer() {
        imgCheck.sprite = Resources.Load("X", typeof(Sprite)) as Sprite;    //이미지 설정
        Audio_Check.clip = clip_Check_X;    //소리 설정
        //기회 이미지 한개를 줄임
        health[chance].gameObject.SetActive(false);
        Audio_Play.clip = chance_Delete; Audio_Play.Play(); //기회가 사라지는 소리
        chance++; WrongQuiz++;
        //만약 5번의 기회를 다쓰면 게임오버
        GameOver();
        if (Item == 0) {
            //만약 문제가 보너스 퀴즈인 경우 다음 단계로 바로 넘어감
            SetQuiz();
        }
    }
    void GameVictory() {
        if (bosslive == 100)
        {
            chracter.sprite = Resources.Load("8", typeof(Sprite)) as Sprite;    //캐릭터 이미지 변경
            FadeResult();   //화면 전환
            IsGameStart = false;
            SetCharacter(6);    //게임 승리
            InitGame();
            SetResult(0);
            Audio_Back.clip = back_Win;
            Audio_Back.Play();
        }
    }
    void GameOver() {
        if (chance == 5 || time == 0) {
            chracter.sprite = Resources.Load("7", typeof(Sprite)) as Sprite;
            FadeResult();   //화면 전환
            IsGameStart = false;
            SetCharacter(5);    //게임오버
            InitGame();
            SetResult(1);
            Audio_Back.clip = back_Lose;
            Audio_Back.Play();
        }
    }
    void SetResult(int i) {
        if (i == 0) {
            Rtext = "◈ VICTORY ◈\n";
            if (WrongQuiz == 0) { Rank = "랭크 : S"; }
            else if (WrongQuiz == 1) { Rank = "랭크 : A"; }
            else if (WrongQuiz == 2) { Rank = "랭크 : B"; }
            else if (WrongQuiz == 3) { Rank = "랭크 : C"; }
            else { Rank = "랭크 : D"; }
        }
        else {
            Rtext = "◈ LOSE ◈\n";
            if (chance == 5) { Rank = "게임 오버 : 기회를 모두 사용!"; }
            else { Rank = "게임 오버 : 시간 초과!"; }
        }
        //게임 결과 랭크창
        textResult.gameObject.SetActive(true);
        textResult.text =
            "<게임 결과>\n" +
            Rtext + "\n" +
            "맞춘 문제 : " + CorrectQuiz + "개\n" +
            "오답을 제출 : " + WrongQuiz + "번\n" +
            "걸린 시간 : " + Mathf.RoundToInt(AllTime).ToString() + "초\n" +
            Rank + "\n";
    }
    void SetCharacter(int i) {
        int a = Random.Range(0, 3); //난수에 따른 대사 설정
        if (i == 0) {
            //시간이 촉박한 경우
            //난수(quizCal)에 따른 대사 설정 (a를 사용하면 Update되면서 난수가 계속 설정됨)
            if (quizCal == 0) { textCharacter.text = "이봐, 시간이 얼마 안남았다고!"; }
            else if (quizCal == 1) { textCharacter.text = "시간이 얼마 남지 않았어. 빨리좀 해봐!"; }
            else { textCharacter.text = "시간이 점점 줄어들고 있는데?"; }
            chracter.sprite = Resources.Load("6", typeof(Sprite)) as Sprite;    //캐릭터 이미지 변경
        }  
        if (i == 1) {
            //공백을 입력받은 경우
            if (a == 0) { textCharacter.text = "아무것도 쓰지 않고 내다니, 일부러 그런거야?"; }
            else if (a == 1) { textCharacter.text = "나랑 장난해? 여기 아무것도 안써져있어."; }
            else { textCharacter.text = "뭐야? 빈종이를 내다니, 포기한거야?"; }
            chracter.sprite = Resources.Load("4", typeof(Sprite)) as Sprite;    //캐릭터 이미지 변경
            GameOver(); //5번의 기회를 다 써버리면 게임오버
        }
        if (i == 2) {
            //숫자답은 맞았지만 색깔답이 틀린 경우
            if (a == 0) { textCharacter.text = "답은 맞았는데, 포스트잇 색깔이 틀렸어."; }
            else if (a == 1) { textCharacter.text = "실수인거지? 색깔좀 제대로 봐."; }
            else { textCharacter.text = "너, 룰은 알고있는거야? 다시 가르쳐줄 여유따윈 없다고!"; }
            chracter.sprite = Resources.Load("3", typeof(Sprite)) as Sprite;    //캐릭터 이미지 변경
            GameOver(); //5번의 기회를 다 써버리면 게임오버
        }
        if (i == 3)
        {
            //숫자 답을 틀린 경우
            if (a == 0) { textCharacter.text = "틀렸어. 다시 계산해봐."; }
            else if (a == 1) { textCharacter.text = "틀렸다고 계산기 쓰는건 반칙이야!"; }
            else {
                if(quizCal==0) { textCharacter.text = "틀렸는걸.... 실망인데?"; }
                else if (quizCal == 1) { textCharacter.text = "틀렸어! 집중좀 잘해봐!"; }
                else { textCharacter.text = "그게 그렇게 어려운 문제인거야? 다시 잘 계산해봐."; }
            }
            chracter.sprite = Resources.Load("2", typeof(Sprite)) as Sprite;    //캐릭터 이미지 변경
            GameOver(); //5번의 기회를 다 써버리면 게임오버
        }
        if (i == 4) {
            //정답을 맞춘 경우
            if (a == 0) { textCharacter.text = "정답이야! 맞춘걸 축하해."; }
            else if (a == 1) { textCharacter.text = "딩동댕~ 정답이야."; }
            else
            {
                if (quizCal == 0) { textCharacter.text = "꽤 하는걸? 이번엔 덧셈이야. 순식간에 풀수 있지?"; }    //현재 문제가 덧셈
                else if (quizCal == 1) { textCharacter.text = "맞췄구나! 잘했어. 이번엔 뺄셈 문제야. 음수 계산 까먹지 말고!"; }    //현재 문제가 뺄셈
                else { textCharacter.text = "문제 맞춘거 축하해. 이번엔 곱셈 문제인데... 복잡할텐데 잘해봐."; }    //현재 문제가 곱셈
            }
            chracter.sprite = Resources.Load("5", typeof(Sprite)) as Sprite;    //캐릭터 이미지 변경
        }
        if (i == 5) {
            //게임오버 경우
            chracter.sprite = Resources.Load("7", typeof(Sprite)) as Sprite;    //캐릭터 이미지 변경
            textCharacter.text = "게임 끝이야! 아쉽게 되었네. R을 눌러 다시 도전하면 받아는 줄게!";
        }
        if (i == 6)
        {
            chracter.sprite = Resources.Load("8", typeof(Sprite)) as Sprite;    //캐릭터 이미지 변경
            if (a == 0) { textCharacter.text = "축하해. 네가 이겼어. 잘가라고. R을 눌러 다시 도전하면 받아는 줄게! 농담이야."; }
            else if (a == 1) { textCharacter.text = "와, 너 정말 잘하는구나! 네 승리야. 다시 놀고싶어? R을 누르던가!"; }
            else { textCharacter.text = "꽤 하는데? 오랜만에 재밌었어. 한판 더할래? 하고싶으면 R을 눌러. 싫으면 어쩔 수 없고!"; }
        }
        if (i == 7)
        {
            //보너스 퀴즈가 나온 경우
            chracter.sprite = Resources.Load("5", typeof(Sprite)) as Sprite;    //캐릭터 이미지 변경
            textCharacter.text = "이게 뭐냐고? 보너스 퀴즈야. 정답이 맞으면 기회를 한 개 더 추가해 줄게! 틀리면 바로 다음문제로 넘어갈거야.";
        }
        Animation aniCharacter = chracter.GetComponent<Animation>();
        aniCharacter.Play();
        CharacterTalk();
    }
    void CharacterTalk() {
        StartCoroutine(TypingEffect(textCharacter.text));
    }
    //타이핑 이펙트
    IEnumerator TypingEffect(string text) { 
        yield return null;
        for (int i = 0; i <= text.Length; i++)
        {
            textCharacter.text = text.Substring(0, i);
            yield return new WaitForSeconds(0.07f);
            Audio_Character.clip = clip_Character;
            Audio_Character.Play();
        }
    }
    //게임 종료 버튼
    public void QuitGame(GameObject button)
    {
        black.gameObject.SetActive(true);
        RorQ = 0; FadeStart();
    }
    //게임 메인으로 돌아가기 버튼
    public void ReturnMainGame(GameObject button)
    {
        black.gameObject.SetActive(true);
        RorQ = 1; FadeStart();
    }
    //음소거 버튼
    public void DieSound(GameObject button) {
        if (dieSound == false) { Audio_Back.mute = true; dieSound = true; sound.image.sprite= Resources.Load("SoundOff", typeof(Sprite)) as Sprite; }
        else { Audio_Back.mute = false; dieSound = false; sound.image.sprite = Resources.Load("SoundOn", typeof(Sprite)) as Sprite; }
    }
    //정답 입력 포스트잇 색 변경 버튼
    public void Setpostit1(GameObject button)
    {
        inputAnswer.image.sprite = spritePost[0];
        AnswerColor = 0; ChooseSoundPlay();
    }
    public void Setpostit2(GameObject button)
    {
        inputAnswer.image.sprite = spritePost[1];
        AnswerColor = 1; ChooseSoundPlay();
    }
    public void Setpostit3(GameObject button)
    {
        inputAnswer.image.sprite = spritePost[2];
        AnswerColor = 2; ChooseSoundPlay();
    }
    void ChooseSoundPlay()
    {
        Audio_Play.clip = choose_Answer;
        Audio_Play.Play();
    }
    //페이드 아웃
    public void FadeStart()
    {
        ttime = 0;
        StartCoroutine(FadeFlow());
    }
    IEnumerator FadeFlow()
    {
        Color alpha = black.color;
        while (alpha.a < 1f)
        {
            ttime += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, ttime);
            black.color = alpha;
            yield return null;
        }
        if (RorQ == 1) { SceneManager.LoadScene("Main"); }
        else Application.Quit();
        yield return null;
    }
    //페이드 인
    public void FadeEnd()
    {
        ttime = -1; //로딩 시간을 벌기 위해 조금 오래 설정함
        StartCoroutine(FadeFlow2());
    }
    IEnumerator FadeFlow2()
    {
        Color alpha = black2.color;
        while (alpha.a > 0f)
        {
            ttime += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, ttime);
            black2.color = alpha;
            yield return null;
        }
        black2.gameObject.SetActive(false);
        yield return null;
    }
    public void FadeResult() {
        ttime = 0.5f;
        StartCoroutine(FadeFlow3());
    }
    IEnumerator FadeFlow3()
    {
        black.gameObject.SetActive(true);
        Color alpha = black.color;
        ttime = 0;
        while (alpha.a < 1f)
        {
            ttime += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, ttime);
            black.color = alpha;
            yield return null;
        }
        ttime = 0f;
        yield return new WaitForSeconds(1f);
        while (alpha.a > 0f)
        {
            ttime += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, ttime);
            black.color = alpha;
            yield return null;
        }
        black.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
    }
    void cheat() {
        print("정답" + quizAnswerNumber);        //치트용
    }
}