using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace OnceHumanFontManager;

public partial class MainForm : Form
{
    private const string FontSubPath = @"\res\ui\font\kr";

    // 적용/복원 대상 폰트 베이스 이름 (확장자 제외).
    // font 폴더 구성: 적용용 3개 + `_ori` 접미 원본 3개 = 총 6개.
    private static readonly string[] TargetFontBaseNames =
    {
        "h73_brand_font_new",
        "h73bold",
        "h73light",
    };

    private const string OriSuffix = "_ori";
    private const string OnceHumanSteamAppId = "2139460";
    private const string IniFileName = "Once_Human_Font_Manger.ini";
    private const string TempVdfJsonName = "libraryfolders.json";
    private const string ChangeDirName = "Change";
    private const string FontDirName = "font";

    // 도움말은 별도 플로팅 폼(HelpForm)으로 띄우며, 동시에 한 개만 유지
    private HelpForm? _helpForm;

    public MainForm()
    {
        InitializeComponent();
        Load += MainForm_Load;
        FormClosing += MainForm_FormClosing;
    }

    // 입력 경로 + FontSubPath = 실제 대상 경로
    private string GetTargetPath() =>
        txtGamePath.Text.TrimEnd('\\') + FontSubPath;

    private void txtGamePath_TextChanged(object sender, EventArgs e)
    {
        lblTargetPath.Text = string.IsNullOrWhiteSpace(txtGamePath.Text)
            ? "대상 경로: (미설정)"
            : "대상 경로: " + GetTargetPath();
    }

    // ─────────────────────────────────────────────────────────
    // INI 캐시 로드/세이브
    // ─────────────────────────────────────────────────────────

    private static string IniFullPath =>
        Path.Combine(AppContext.BaseDirectory, IniFileName);

    private void MainForm_Load(object? sender, EventArgs e)
    {
        SetStatus("프로그램 시작");
        EnsureWorkingFolders();

        string? cached = LoadCachedGamePath();
        if (!string.IsNullOrWhiteSpace(cached) && Directory.Exists(cached))
        {
            txtGamePath.Text = cached;
            SetStatus("이전 게임 경로 자동 로드: " + cached);
            SetStatus($"폰트 적용 대상 경로 (자동 접합 '{FontSubPath}'): {GetTargetPath()}");
        }
        else
        {
            SetStatus("게임 경로를 [자동 찾기] 또는 [경로 찾기] 로 설정하세요.");
        }
    }

    // exe 옆에 작업용 폴더(font, Change)가 없으면 자동 생성하고 결과를 로그에 남긴다
    private void EnsureWorkingFolders()
    {
        string baseDir = AppContext.BaseDirectory;
        foreach (var name in new[] { FontDirName, ChangeDirName })
        {
            string p = Path.Combine(baseDir, name);
            if (Directory.Exists(p))
            {
                SetStatus($"'{name}' 폴더 확인됨");
                continue;
            }
            try
            {
                Directory.CreateDirectory(p);
                SetStatus($"'{name}' 폴더가 없어 자동 생성했습니다");
            }
            catch (Exception ex)
            {
                SetStatus($"'{name}' 폴더 생성 실패: {ex.Message}", error: true);
            }
        }
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtGamePath.Text)
            && Directory.Exists(txtGamePath.Text))
        {
            SaveCachedGamePath(txtGamePath.Text);
        }
    }

    private static string? LoadCachedGamePath()
    {
        try
        {
            if (!File.Exists(IniFullPath)) return null;
            foreach (var raw in File.ReadAllLines(IniFullPath))
            {
                var line = raw.Trim();
                if (line.StartsWith("GamePath=", StringComparison.OrdinalIgnoreCase))
                    return line["GamePath=".Length..].Trim();
            }
        }
        catch { }
        return null;
    }

    private static void SaveCachedGamePath(string path)
    {
        try
        {
            File.WriteAllLines(IniFullPath, new[]
            {
                "[Settings]",
                "GamePath=" + path,
            });
        }
        catch { }
    }

    // ─────────────────────────────────────────────────────────
    // 경로 입력 / 탐색
    // ─────────────────────────────────────────────────────────

    private void btnBrowse_Click(object sender, EventArgs e)
    {
        using var dlg = new FolderBrowserDialog
        {
            Description = "ONCE HUMAN 게임 설치 폴더를 선택하세요",
            UseDescriptionForTitle = true
        };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            txtGamePath.Text = dlg.SelectedPath;
            SetStatus("수동 경로 선택: " + dlg.SelectedPath);
            SetStatus($"폰트 적용 대상 경로 (자동 접합 '{FontSubPath}'): {GetTargetPath()}");
        }
    }

    private void btnAutoFind_Click(object sender, EventArgs e)
    {
        // 1순위: 로딩베이 → 발견되면 즉시 종료 (Steam 검색하지 않음)
        string? found = FindByLoadingBay();
        string source = "LoadingBay";

        if (found == null)
        {
            found = FindBySteam();
            source = "Steam";
        }
        if (found == null)
        {
            found = FindByCommonPaths();
            source = "일반 경로";
        }
        if (found == null)
        {
            found = FindByDriveRoots();
            source = "드라이브 루트";
        }

        if (found != null)
        {
            txtGamePath.Text = found;
            SetStatus($"자동 찾기 성공 ({source}): " + found);
            SetStatus($"폰트 적용 대상 경로 (자동 접합 '{FontSubPath}'): {GetTargetPath()}");
            SaveCachedGamePath(found);
        }
        else
        {
            SetStatus("게임 경로를 자동으로 찾지 못했습니다. 직접 입력하세요.", error: true);
        }
    }

    // 1순위: LoadingBay 레지스트리
    // \game\23 의 하위 키값에서 Once_Human 을 먼저 찾고 그 키의 LastInstallPath 사용
    // (구조 호환을 위해 \game\23 자체의 LastInstallPath 도 폴백으로 시도)
    private static string? FindByLoadingBay()
    {
        try
        {
            const string baseKey = @"Software\LoadingBay\LoadingBayInstaller\game\23";
            using var key = Registry.CurrentUser.OpenSubKey(baseKey);
            if (key == null) return null;

            // (a) \game\23\Once_Human\LastInstallPath 시도
            foreach (var subName in key.GetSubKeyNames())
            {
                if (!subName.Contains("Once_Human", StringComparison.OrdinalIgnoreCase))
                    continue;
                using var sub = key.OpenSubKey(subName);
                var subPath = sub?.GetValue("LastInstallPath") as string;
                if (!string.IsNullOrWhiteSpace(subPath) && Directory.Exists(subPath))
                    return subPath;
            }

            // (b) \game\23 자체의 LastInstallPath 폴백
            var path = key.GetValue("LastInstallPath") as string;
            if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
                return path;
        }
        catch { }
        return null;
    }

    // 2순위: Steam
    // (1) HKLM\Wow6432Node\Valve\Steam 또는 HKCU\Valve\Steam → InstallPath
    // (2) {InstallPath}\steamapps\libraryfolders.vdf 읽기
    // (3) exe 옆에 libraryfolders.json 으로 복사
    // (4) 앱 ID 2139460 검색 → 해당 라이브러리 path 추출
    // (5) path\steamapps\common\Once Human 결과
    // (6) 임시 .json 삭제
    private static string? FindBySteam()
    {
        string? tempCopy = null;
        try
        {
            string? steamInstall = GetSteamInstallPath();
            if (string.IsNullOrWhiteSpace(steamInstall)) return null;

            string vdfPath = Path.Combine(steamInstall, "steamapps", "libraryfolders.vdf");
            if (!File.Exists(vdfPath)) return null;

            // exe 위치에 json 형태로 복사 (사양 요구)
            tempCopy = Path.Combine(AppContext.BaseDirectory, TempVdfJsonName);
            File.Copy(vdfPath, tempCopy, overwrite: true);

            string content = File.ReadAllText(tempCopy);
            string? libraryPath = FindLibraryPathContainingApp(content, OnceHumanSteamAppId);
            if (string.IsNullOrWhiteSpace(libraryPath)) return null;

            string oncehumanPath = Path.Combine(libraryPath, "steamapps", "common", "Once Human");
            return Directory.Exists(oncehumanPath) ? oncehumanPath : null;
        }
        catch { }
        finally
        {
            if (tempCopy != null)
            {
                try { File.Delete(tempCopy); } catch { }
            }
        }
        return null;
    }

    // Steam 설치 경로 — 64비트/32비트 + HKCU 모두 시도
    private static string? GetSteamInstallPath()
    {
        string[] candidates =
        {
            @"SOFTWARE\WOW6432Node\Valve\Steam",  // HKLM 32-bit view
            @"SOFTWARE\Valve\Steam",               // HKLM 64-bit view
        };

        foreach (var rel in candidates)
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(rel);
                var p = key?.GetValue("InstallPath") as string;
                if (!string.IsNullOrWhiteSpace(p) && Directory.Exists(p))
                    return p;
            }
            catch { }
        }

        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam");
            var p = key?.GetValue("InstallPath") as string;
            if (!string.IsNullOrWhiteSpace(p) && Directory.Exists(p))
                return p;
        }
        catch { }

        return null;
    }

    // libraryfolders.vdf 내용에서 특정 appId를 포함하는 라이브러리의 "path" 값을 반환
    // VDF 의 각 라이브러리 블록은 다음과 같은 구조:
    //   "<index>"
    //   {
    //       "path"   "C:\\SteamLibrary"
    //       ...
    //       "apps"
    //       {
    //           "<appId>"   "<size>"
    //       }
    //   }
    private static string? FindLibraryPathContainingApp(string vdfText, string appId)
    {
        var libraryRegex = new Regex(
            @"""path""\s+""([^""]+)""[\s\S]*?""apps""\s*\{([^}]*)\}",
            RegexOptions.Multiline);

        foreach (Match m in libraryRegex.Matches(vdfText))
        {
            string path = m.Groups[1].Value.Replace(@"\\", @"\");
            string apps = m.Groups[2].Value;
            // appId 가 따옴표 안에 있는지 확인
            if (Regex.IsMatch(apps, $@"""\s*{Regex.Escape(appId)}\s*"""))
                return path;
        }
        return null;
    }

    // 3순위: 일반 경로 목록
    private static string? FindByCommonPaths()
    {
        string[] candidates =
        [
            @"C:\Program Files\Once Human",
            @"C:\Program Files (x86)\Once Human",
            @"C:\Games\Once Human",
            @"D:\Once Human",
            @"D:\Games\Once Human",
            @"E:\Once Human",
            @"E:\Games\Once Human",
        ];
        return candidates.FirstOrDefault(Directory.Exists);
    }

    // 4순위: 고정 드라이브 루트 얕은 탐색
    private static string? FindByDriveRoots()
    {
        try
        {
            var drives = DriveInfo.GetDrives()
                .Where(d => d.DriveType == DriveType.Fixed && d.IsReady);

            foreach (var drive in drives)
            {
                var dirs = Directory.GetDirectories(drive.RootDirectory.FullName, "*Once*Human*",
                               SearchOption.TopDirectoryOnly);
                var found = dirs.FirstOrDefault(d =>
                    d.Contains("Once", StringComparison.OrdinalIgnoreCase) &&
                    d.Contains("Human", StringComparison.OrdinalIgnoreCase));
                if (found != null) return found;
            }
        }
        catch { }
        return null;
    }

    // ─────────────────────────────────────────────────────────
    // 적용 / 복원
    // ─────────────────────────────────────────────────────────

    // 적용: font 폴더의 적용용 3개 파일을 대상에 덮어쓰기로 복사
    // (대상에 같은 이름이 있으면 삭제 후 복사)
    private void btnApply_Click(object sender, EventArgs e)
    {
        if (!ValidatePaths(out string fontDir, out string target)) return;

        int ok = 0, fail = 0;
        var failNames = new List<string>();

        foreach (var baseName in TargetFontBaseNames)
        {
            try
            {
                // font 폴더에서 베이스 이름과 정확히 일치하는 파일 탐색 (_ori 제외)
                string? src = FindByBaseName(fontDir, baseName);
                if (src == null)
                {
                    fail++;
                    failNames.Add(baseName);
                    continue;
                }

                string dest = Path.Combine(target, Path.GetFileName(src));
                if (File.Exists(dest)) File.Delete(dest);
                File.Copy(src, dest);
                ok++;
            }
            catch
            {
                fail++;
                failNames.Add(baseName);
            }
        }

        SetStatus(fail == 0
            ? $"적용 완료: {ok}개 파일 복사됨"
            : $"적용 완료: {ok}개 성공, {fail}개 실패 ({string.Join(", ", failNames)})",
            error: fail > 0);
    }

    // 복원: font 폴더의 *_ori 파일을 대상에 복사하면서 이름에서 _ori 제거
    private void btnRestore_Click(object sender, EventArgs e)
    {
        if (!ValidatePaths(out string fontDir, out string target)) return;

        int ok = 0, fail = 0;
        var failNames = new List<string>();

        foreach (var baseName in TargetFontBaseNames)
        {
            string oriBaseName = baseName + OriSuffix;
            try
            {
                string? src = FindByBaseName(fontDir, oriBaseName);
                if (src == null)
                {
                    fail++;
                    failNames.Add(oriBaseName);
                    continue;
                }

                // 확장자는 원본 그대로 유지하면서 파일명에서 _ori만 제거
                string ext = Path.GetExtension(src);
                string destFileName = baseName + ext;
                string dest = Path.Combine(target, destFileName);

                if (File.Exists(dest)) File.Delete(dest);
                File.Copy(src, dest);
                ok++;
            }
            catch
            {
                fail++;
                failNames.Add(oriBaseName);
            }
        }

        SetStatus(fail == 0
            ? $"복원 완료: {ok}개 파일 복원됨"
            : $"복원 완료: {ok}개 성공, {fail}개 실패 ({string.Join(", ", failNames)})",
            error: fail > 0);
    }

    // 변환: Change 폴더의 TTF 파일 1개를 3개로 복사 → font 폴더에 h73_brand_font_new/h73bold/h73light 로 생성
    // (덮어쓰기 우선, 실패 시 삭제 후 재복사)
    private void btnConvert_Click(object sender, EventArgs e)
    {
        string changeDir = Path.Combine(AppContext.BaseDirectory, ChangeDirName);
        if (!Directory.Exists(changeDir))
        {
            Directory.CreateDirectory(changeDir);
            SetStatus($"'{ChangeDirName}' 폴더를 생성했습니다. 변환할 TTF 파일을 넣고 다시 시도하세요.",
                error: true);
            return;
        }

        var ttfFiles = Directory.GetFiles(changeDir, "*.ttf", SearchOption.TopDirectoryOnly);
        if (ttfFiles.Length == 0)
        {
            SetStatus($"'{ChangeDirName}' 폴더에 TTF 파일이 없습니다. 폰트(.ttf)를 넣고 다시 시도하세요.",
                error: true);
            return;
        }

        string fontDir = Path.Combine(AppContext.BaseDirectory, FontDirName);
        if (!Directory.Exists(fontDir))
            Directory.CreateDirectory(fontDir);

        string srcTtf = ttfFiles[0]; // Change 폴더의 첫 TTF 사용
        string ext = Path.GetExtension(srcTtf); // ".ttf"

        int ok = 0, fail = 0;
        var failNames = new List<string>();

        foreach (var baseName in TargetFontBaseNames)
        {
            string destPath = Path.Combine(fontDir, baseName + ext);
            try
            {
                // (1) 덮어쓰기 시도
                File.Copy(srcTtf, destPath, overwrite: true);
                ok++;
            }
            catch
            {
                // (2) 덮어쓰기 실패 시: 지우고 복사
                try
                {
                    if (File.Exists(destPath)) File.Delete(destPath);
                    File.Copy(srcTtf, destPath);
                    ok++;
                }
                catch
                {
                    fail++;
                    failNames.Add(baseName);
                }
            }
        }

        SetStatus(fail == 0
            ? $"변환 완료: {ok}개 폰트 파일을 '{FontDirName}' 폴더에 생성했습니다. [적용] 버튼을 누르세요."
            : $"변환 완료: {ok}개 성공, {fail}개 실패 ({string.Join(", ", failNames)})",
            error: fail > 0);

        // 변환 후처리: 1개 이상 성공 시 Change 폴더의 TTF 파일을 모두 삭제
        // (전부 실패 시 재시도 가능하도록 원본 보존)
        if (ok > 0)
        {
            int deleted = 0;
            var deleteFails = new List<string>();
            foreach (var f in ttfFiles)
            {
                try
                {
                    File.Delete(f);
                    deleted++;
                }
                catch
                {
                    deleteFails.Add(Path.GetFileName(f));
                }
            }
            if (deleted > 0)
                SetStatus($"'{ChangeDirName}' 폴더의 폰트 파일 {deleted}개 정리 완료");
            if (deleteFails.Count > 0)
                SetStatus($"'{ChangeDirName}' 폴더 정리 실패: {string.Join(", ", deleteFails)}",
                    error: true);
        }
    }

    // 게임 경로 + font 폴더 + 대상 폴더 유효성 한 번에 검증
    private bool ValidatePaths(out string fontDir, out string target)
    {
        fontDir = string.Empty;
        target = string.Empty;
        if (!ValidateGamePath()) return false;

        fontDir = Path.Combine(AppContext.BaseDirectory, FontDirName);
        if (!Directory.Exists(fontDir))
        {
            SetStatus($"font 폴더가 없습니다: {fontDir}", error: true);
            return false;
        }

        target = GetTargetPath();
        if (!Directory.Exists(target))
        {
            SetStatus($"대상 경로가 없습니다: {target}", error: true);
            return false;
        }
        return true;
    }

    // baseName과 확장자를 제외한 파일명이 정확히 일치하는 파일을 반환
    // (확장자가 없는 파일/.ttf/.otf 등 어떤 확장자든 매칭)
    private static string? FindByBaseName(string dir, string baseName)
    {
        return Directory
            .EnumerateFiles(dir, baseName + "*", SearchOption.TopDirectoryOnly)
            .FirstOrDefault(f =>
                string.Equals(Path.GetFileNameWithoutExtension(f), baseName,
                    StringComparison.OrdinalIgnoreCase));
    }

    private bool ValidateGamePath()
    {
        if (string.IsNullOrWhiteSpace(txtGamePath.Text))
        {
            SetStatus("게임 경로를 입력하거나 자동 찾기를 사용하세요.", error: true);
            return false;
        }
        return true;
    }

    // 로그창에 타임스탬프 + 색상 구분된 한 줄을 추가하고 자동 스크롤
    private void SetStatus(string message, bool error = false)
    {
        if (rtbLog == null || rtbLog.IsDisposed) return;

        rtbLog.SelectionStart = rtbLog.TextLength;
        rtbLog.SelectionLength = 0;

        // [HH:mm:ss]
        rtbLog.SelectionFont = new Font("Consolas", 9F);
        rtbLog.SelectionColor = Color.Gray;
        rtbLog.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ");

        // [정보]/[오류] 태그
        rtbLog.SelectionFont = new Font("맑은 고딕", 9F, FontStyle.Bold);
        rtbLog.SelectionColor = error ? Color.Crimson : Color.SeaGreen;
        rtbLog.AppendText(error ? "[오류] " : "[정보] ");

        // 본문
        rtbLog.SelectionFont = new Font("맑은 고딕", 9F, FontStyle.Regular);
        rtbLog.SelectionColor = Color.FromArgb(40, 40, 40);
        rtbLog.AppendText(message + "\n");

        rtbLog.SelectionStart = rtbLog.TextLength;
        rtbLog.ScrollToCaret();
    }

    // ─────────────────────────────────────────────────────────
    // 도움말 (플로팅 폼)
    // ─────────────────────────────────────────────────────────

    // 도움말 폼은 비모달(modeless)로 띄우며, 동시에 한 개만 유지한다.
    private void btnHelp_Click(object sender, EventArgs e)
    {
        if (_helpForm == null || _helpForm.IsDisposed)
        {
            _helpForm = new HelpForm();
            _helpForm.FormClosed += (_, _) => _helpForm = null;
            _helpForm.Show(this);
        }
        else
        {
            _helpForm.BringToFront();
            _helpForm.Activate();
        }
    }
}
