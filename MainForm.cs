using System.Globalization;
using System.Text.RegularExpressions;

namespace VTT2TXT
{
    public partial class MainForm : Form
    {
        string[] extensions = { ".vtt", ".srt" };

        public MainForm()
        {
            InitializeComponent();
            InitializeUI();
            Btn_Filter_Click(Btn_Filter, EventArgs.Empty);
        }

        private void InitializeUI()
        {
            Lbl_Path.Text = AppContext.BaseDirectory;
        }

        // 改為傳入傳入，跟GUI解耦合
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

                if (orderByCreateTm.Checked)
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
                    Vtt2TxtConverter.ProcessSubtitleFile(fullFilePath);
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
                    Vtt2TxtConverter.ProcessSubtitleFile(fullFilePath);
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
