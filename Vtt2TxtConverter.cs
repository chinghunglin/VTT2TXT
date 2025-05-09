using System.Globalization;
using System.Text.RegularExpressions;

namespace VTT2TXT
{
    public class Vtt2TxtConverter
    {
        public string FilePath { get; set; }

        public static void ProcessSubtitleFile(string filePath, int maxSentencesOfABlock = 15)
        {
            if (!FileIsValid(filePath))
            {
                return;
            }

            try
            {
                var lines = File.ReadAllLines(filePath).ToList();
                var outputLines = new List<string>();
                string lastLine = string.Empty;
                String thisLine = string.Empty;

                var tempLines = new List<string>();
                DateTime? lastTimestamp = null;
                int count = 0;

                // 移除 VTT 檔案前 3 行標頭（如果存在）
                ProcessFirst3HeaderLines(lines);

                foreach (var line in lines)
                {
                    if (IsTimestamp(line, out DateTime currentTimestamp))
                    {
                        if (isTimeDiffOverLimit(currentTimestamp, lastTimestamp) || (count >= maxSentencesOfABlock))
                        {
                            outputLines.Add(string.Join("", tempLines));
                            outputLines.Add(" ");
                            tempLines.Clear();
                            count = 0;
                        }

                        lastTimestamp = currentTimestamp;
                    }
                    else if (IsNumberLine(line))
                    {
                        continue;
                    }
                    else if (!string.IsNullOrWhiteSpace(line))
                    {
                        // tempLines.Add(line);
                        thisLine = EnsureEndsWithFullWidthComma(RemoveTagsAndTimestamps(line));

                        if (thisLine == lastLine)
                        {
                            continue;
                        }

                        tempLines.Add(thisLine);
                        lastLine = thisLine;
                        count++;
                    }
                }

                if (tempLines.Count > 0)
                {
                    outputLines.Add(string.Join("", tempLines));
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

        private static bool FileIsValid(string file)
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

        private static void ProcessFirst3HeaderLines(List<string> lines)
        {
            if (lines.Count >= 3 && lines[0] == "WEBVTT" && lines[1].StartsWith("Kind:") && lines[2].StartsWith("Language:"))
            {
                lines.RemoveRange(0, 3);
            }
        }

        private static bool IsTimestamp(string line, out DateTime timestamp)
        {
            timestamp = DateTime.MinValue;
            var match = Regex.Match(line, @"(\d{2}:\d{2}:\d{2},\d{3})|(\d{2}:\d{2}:\d{2}\.\d{3})");
            if (match.Success)
            {
                return DateTime.TryParseExact(match.Value, new[] { "HH:mm:ss,fff", "HH:mm:ss.fff" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out timestamp);
            }
            return false;
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

        static private bool IsNumberLine(String input)
        {
            // Regex to match lines to remove
            var numberPattern = new Regex(@"^\d+$");

            if (numberPattern.IsMatch(input))
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

        // currentTimestamp - lastTimestamp.Value).TotalSeconds > secToNewBlock
        private static bool isTimeDiffOverLimit(DateTime currentTimestamp, DateTime? lastTimestamp, double secToNewBlock = 3.5)
        {
            return lastTimestamp.HasValue && (currentTimestamp - lastTimestamp.Value).TotalSeconds > secToNewBlock;
        }
    }
}
