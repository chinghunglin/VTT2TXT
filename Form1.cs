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
        /// �ˬd�O�_�w�g�H���Τ���r�������A�Y���H���Τ���r�������A�h�[�W�æ^��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static private string EnsureEndsWithFullWidthComma(string input)
        {
            // �ˬd�O�_�w�g�H���Τ���r������
            if (input.EndsWith("�A"))
            {
                return input;
            }

            // �Y���H���Τ���r�������A�h�[�W�æ^��
            return input + "�A";
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
                return input; // �p�G�r�ꬰ�ũ� null�A�����^��
            }

            // �w�q���W��ܪk�A�ǰt <c>�B</c> �M <�ɶ��W> ���Ҧ�
            string pattern = @"<c>|</c>|<\d{2}:\d{2}:\d{2}[.,]\d{3}>";

            // �ϥ� Regex �����ǰt�����e���Ŧr��
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
                Description = "��ܤ@�ӥؿ�"
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
                MessageBox.Show($"Ū���ɮץ��ѡG{ex.Message}", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_ConvSelected_Click(object sender, EventArgs e)
        {
            if (Lbx_Files.SelectedItems.Count == 0)
            {
                MessageBox.Show("�Х���ܦܤ֤@���ɮסI", "����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string directoryPath = Lbl_Path.Text.Trim();

            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
            {
                MessageBox.Show("�ؿ����|�L�ĩΤ��s�b�I", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show($"�ɮפ��s�b�G{fullFilePath}", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            MessageBox.Show("�ɮ��ഫ����");
        }

        private void Btn_ConvAll_Click(object sender, EventArgs e)
        {
            if (Lbx_Files.Items.Count == 0)
            {
                MessageBox.Show("�ӥؿ��L�ŦX�L�o�W�h���ɮסI", "����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string directoryPath = Lbl_Path.Text.Trim();

            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
            {
                MessageBox.Show("�ؿ����|�L�ĩΤ��s�b�I", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show($"�ɮפ��s�b�G{fullFilePath}", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            MessageBox.Show("�ɮ��ഫ����");
        }
    }
}
