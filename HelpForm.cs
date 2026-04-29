namespace OnceHumanFontManager;

public partial class HelpForm : Form
{
    public HelpForm()
    {
        InitializeComponent();
        BuildHelpContent();
    }

    private void BuildHelpContent()
    {
        rtbHelp.Clear();

        AppendCentered("ONCE HUMAN 폰트매니저 v0.1", 14F, FontStyle.Bold,
            Color.FromArgb(40, 70, 130));
        AppendCentered("─────────────────────────────────────────",
            9F, FontStyle.Regular, Color.Gray);
        AppendBlankLine();

        AppendHeader("프로그램 소개", Color.FromArgb(70, 130, 180));
        AppendBody("ONCE HUMAN 게임의 한국어 폰트를 원하는 폰트로 손쉽게 교체할 수 있는 도구입니다.");
        AppendBody("[적용] 버튼 한 번으로 폰트가 적용되고, [복원] 버튼으로 원본 폰트로 즉시 되돌릴 수 있습니다.");
        AppendBlankLine();

        AppendHeader("기본 사용법", Color.FromArgb(70, 130, 180));
        AppendBody("1. font 폴더안에 6개의 파일을 넣어주세요.");
        AppendBody("    · 적용 3개  :  h73_brand_font_new ,  h73bold ,  h73light");
        AppendBody("    · 원본 3개 :  h73_brand_font_new_ori ,  h73bold_ori ,  h73light_ori");
        AppendBody("    (font 폴더는 프로그램 시작 시 자동으로 생성됩니다)");
        AppendBlankLine();
        AppendBody("2. [자동 찾기] 또는 [경로 찾기] 버튼으로 ONCE HUMAN 게임 경로를 지정합니다.");
        AppendBody("    · 자동 찾기는 LoadingBay → Steam → 일반 경로 → 드라이브 루트 순으로 탐색합니다.");
        AppendBody("    · 한 번 찾은 경로는 자동 저장되어 다음 실행 시 자동으로 불러옵니다.");
        AppendBlankLine();
        AppendBody("3. [적용] 버튼  →  게임에 폰트가 적용됩니다.");
        AppendBody("4. [복원] 버튼  →  원본 폰트로 되돌립니다.");
        AppendBlankLine();

        AppendHeader("다른 폰트로 교체하는 방법 (간편 — [변환] 버튼 사용)",
            Color.FromArgb(148, 102, 178));
        AppendBody("1. 사용하고 싶은 폰트(.ttf)를 다운로드 받습니다.");
        AppendBlankLine();
        AppendBody("2. exe 옆에 'Change' 폴더에 다운로드한 TTF 파일을 넣어주세요.");
        AppendBody("    (Change 폴더는 프로그램 시작 시 자동으로 생성됩니다)");
        AppendBlankLine();
        AppendBody("3. [변환] 버튼을 클릭합니다.");
        AppendBody("    → Change 폴더의 TTF 파일이 자동으로 3개로 복사되어");
        AppendBody("       font 폴더에 h73_brand_font_new, h73bold, h73light 이름으로 생성됩니다.");
        AppendBlankLine();
        AppendBody("4. [적용] 버튼을 누르면 새로운 폰트가 게임에 적용됩니다!");
        AppendBlankLine();

        AppendHeader("다른 폰트로 교체하는 방법 (수동)",
            Color.FromArgb(180, 100, 70));
        AppendBody("1. 사용하고 싶은 폰트(.ttf)를 다운로드 받습니다.");
        AppendBody("2. 다운로드 받은 폰트 파일을 3개로 복사합니다.");
        AppendBody("3. 복사한 3개 파일의 이름을 각각 아래와 같이 변경합니다.");
        AppendBody("    · h73light");
        AppendBody("    · h73bold");
        AppendBody("    · h73_brand_font_new");
        AppendBody("4. font 폴더에 덮어쓰기로 넣어주세요.");
        AppendBody("5. [적용] 버튼을 누르면 새로운 폰트가 게임에 적용됩니다!");
        AppendBlankLine();

        AppendHeader("주의사항", Color.DarkRed);
        AppendBody("· 게임 실행 중에는 파일 잠금으로 적용/복원이 실패할 수 있습니다. 게임을 종료한 후 사용하세요.");
        AppendBody("· _ori 파일은 원본 폰트로 복원 시 사용됩니다. 절대 삭제하거나 변경하지 마세요.");
        AppendBody("· 처음 사용 전 게임 폴더의 폰트 파일을 별도 백업해두는 것을 권장합니다.");

        rtbHelp.SelectionStart = 0;
        rtbHelp.ScrollToCaret();
    }

    private void AppendCentered(string text, float size, FontStyle style, Color color)
    {
        rtbHelp.SelectionAlignment = HorizontalAlignment.Center;
        rtbHelp.SelectionFont = new Font("맑은 고딕", size, style);
        rtbHelp.SelectionColor = color;
        rtbHelp.AppendText(text + "\n");
    }

    private void AppendHeader(string text, Color color)
    {
        rtbHelp.SelectionAlignment = HorizontalAlignment.Left;
        rtbHelp.SelectionFont = new Font("맑은 고딕", 11F, FontStyle.Bold);
        rtbHelp.SelectionColor = color;
        rtbHelp.AppendText("▣ " + text + "\n");
    }

    private void AppendBody(string text)
    {
        rtbHelp.SelectionAlignment = HorizontalAlignment.Left;
        rtbHelp.SelectionFont = new Font("맑은 고딕", 9.5F, FontStyle.Regular);
        rtbHelp.SelectionColor = Color.FromArgb(40, 40, 40);
        rtbHelp.AppendText(text + "\n");
    }

    private void AppendBlankLine()
    {
        rtbHelp.AppendText("\n");
    }
}
