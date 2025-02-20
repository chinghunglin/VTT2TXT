using System.Globalization;
using System.Text.RegularExpressions;

namespace VTT2TXT
{
    public partial class Form1 : Form
    {
        string[] extensions = { ".vtt", ".srt" };

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
            Btn_Filter_Click(Btn_Filter, EventArgs.Empty);
        }

        private void InitializeUI()
        {
            Lbl_Path.Text = AppContext.BaseDirectory;
        }

        private void UpdateExtensions()
        {
            string filterText = Tbx_Filter.Text;

            if (!string.IsNullOrWhiteSpace(filterText))
            {
                extensions = new string[] { };

                string[] filterParts = filterText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // extract extension names start with '*.' and update extensions array
                extensions = filterParts
                    .Where(f => f.StartsWith("*."))
                    .Select(f => f.Substring(1)) // strip '*', keep extension name only
                    .ToArray();
            }
        }

        /// <summary>
        /// 檢查是否已經以全形中文逗號結尾，若未以全形中文逗號結尾，則加上並回傳
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static private string EnsureEndsWithFullWidthComma(string input)
        {
            // 檢查是否已經以全形中文逗號結尾
            if (input.EndsWith("，"))
            {
                return input;
            }

            // 若未以全形中文逗號結尾，則加上並回傳
            return input + "，";
        }

        /// <summary>
        /// check if input line is empty line or time line or number line
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static private bool IsEmptyOrTimeOrNumberLine(String input)
        {
            // Regex to match lines to remove
            var timePattern = new Regex(@"^\d{2}:\d{2}:\d{2}[.,]\d{3} --> \d{2}:\d{2}:\d{2}[.,]\d{3}");
            var numberPattern = new Regex(@"^\d+$");
            
            if (string.IsNullOrWhiteSpace(input) || timePattern.IsMatch(input) || numberPattern.IsMatch(input))
            { 
                return true; 
            }

            return false;
        }

        static private string RemoveTagsAndTimestamps(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input; // 如果字串為空或 null，直接回傳
            }

            // 定義正規表示法，匹配 <c>、</c> 和 <時間戳> 的模式
            string pattern = @"<c>|</c>|<\d{2}:\d{2}:\d{2}[.,]\d{3}>";

            // 使用 Regex 替換匹配的內容為空字串
            return Regex.Replace(input, pattern, string.Empty);
        }

        static private bool FileIsValid(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                MessageBox.Show("File path is invalid.");
                return false;
            }

            if (!File.Exists(file))
            {
                MessageBox.Show("File does not exist.");
                return false;
            }

            // Check if the file is a VTT or SRT file
            string extension = Path.GetExtension(file).ToLower();
            if (extension != ".vtt" && extension != ".srt")
            {
                MessageBox.Show("File is not a VTT or SRT file.");
                return false;
            }

            return true;
        }

        public static void ProcessSubtitleFile(string filePath)
        {
            if (!FileIsValid(filePath))
            {
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                var outputLines = new List<string>();
                string lastLine = string.Empty;
                String thisLine = string.Empty;

                foreach (string line in lines)
                {
                    // Skip empty lines, time lines, and number lines
                    if (IsEmptyOrTimeOrNumberLine(line))
                    {
                        continue;
                    }

                    thisLine  = EnsureEndsWithFullWidthComma(RemoveTagsAndTimestamps(line));

                    if (thisLine == lastLine) {
                        continue;
                    }

                    outputLines.Add(thisLine);
                    lastLine = thisLine;
                }

                // Write the processed lines to a new .txt file
                string newFilePath = Path.ChangeExtension(filePath, ".txt");
                File.WriteAllLines(newFilePath, outputLines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void Btn_SetPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "選擇一個目錄"
            })
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    Lbl_Path.Text = folderBrowserDialog.SelectedPath;
                    FillListboxWithFiles(Lbl_Path.Text);
                }
            }
        }

        private void Btn_Filter_Click(object sender, EventArgs e)
        {
            UpdateExtensions();

            FillListboxWithFiles(Lbl_Path.Text);
        }

        private void FillListboxWithFiles(string tgt_path)
        {
            try
            {
                var files = Directory.GetFiles(tgt_path)
                    .Where(file => extensions.Contains(Path.GetExtension(file).ToLower()));

                if (orderByName.Checked)
                {
                    files = files.OrderByDescending(file => File.GetCreationTime(file));
                }

                Lbx_Files.Items.Clear();

                foreach (var file in files)
                {
                    Lbx_Files.Items.Add(Path.GetFileName(file));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"讀取檔案失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_ConvSelected_Click(object sender, EventArgs e)
        {
            if (Lbx_Files.SelectedItems.Count == 0)
            {
                MessageBox.Show("請先選擇至少一個檔案！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string directoryPath = Lbl_Path.Text.Trim();

            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
            {
                MessageBox.Show("目錄路徑無效或不存在！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var item in Lbx_Files.SelectedItems)
            {
                string fileName = item.ToString();
                string fullFilePath = Path.Combine(directoryPath, fileName);

                if (File.Exists(fullFilePath))
                {
                    ProcessSubtitleFile(fullFilePath);
                }
                else
                {
                    MessageBox.Show($"檔案不存在：{fullFilePath}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            MessageBox.Show("檔案轉換完成");
        }

        private void Btn_ConvAll_Click(object sender, EventArgs e)
        {
            if (Lbx_Files.Items.Count == 0)
            {
                MessageBox.Show("該目錄無符合過濾規則的檔案！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string directoryPath = Lbl_Path.Text.Trim();

            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
            {
                MessageBox.Show("目錄路徑無效或不存在！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var item in Lbx_Files.Items)
            {
                string fileName = item.ToString();
                string fullFilePath = Path.Combine(directoryPath, fileName);

                if (File.Exists(fullFilePath))
                {
                    ProcessSubtitleFile(fullFilePath);
                }
                else
                {
                    MessageBox.Show($"檔案不存在：{fullFilePath}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            MessageBox.Show("檔案轉換完成");
        }
    }
}
