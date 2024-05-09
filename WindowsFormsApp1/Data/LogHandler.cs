using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApp1.Data
{
    internal class LogHandler
    {
        private const string TAG = "LogHandler";
        private int Line = 0;
        private string fileName = string.Empty;
        private string pathLog = string.Empty;
        private string pathPuss = string.Empty;
        private Process process;

        internal void startGetLogAdb()
        {
            if (process == null)
            {
                process = new Process();
                process.StartInfo = new ProcessStartInfo("cmd", "/c" + "adb logcat");
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
            }
            process.Start();
            readLogFromAdb();
        }
        internal void stopGetLogAdb()
        {
            process.Kill();
        }
        
        internal List<Log> readLogFromFile()
        {
            List<Log> logs = new List<Log>();
            pathLog = getPathFile();

            if (!string.IsNullOrEmpty(pathLog))
            {
                pathPuss = pathLog + ".puss";
                fileName = pathLog.Split('\\').Last();
                logs = readLogs(pathLog);
            }
            return logs;
        }
        internal List<Log> readBookMarkFromFile(List<Log> logs)
        {
            List<Log> bookmarks = new List<Log>();
            if (string.IsNullOrEmpty(pathPuss))
            {
                return bookmarks;
            }
            if (File.Exists(pathPuss))
            {
                Logger.logD(TAG, "Read bookmarks from file");
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(pathPuss);
                XmlNodeList nodeList = xmldoc.SelectNodes("/Puss/Bookmarks/Item");
                foreach (XmlNode node in nodeList)
                {
                    int line = int.Parse(node.Attributes["id"].Value.ToString());
                    bookmarks.Add(logs[line - 1]);
                }
            }
            return bookmarks;
        }

        internal string saveLogToFile(List<Log> logs, List<Log> bookmarks)
        {
            Logger.logD(TAG, "Save log to file");
            pathLog = createPathFile();
            pathPuss = pathLog + ".puss";
            if (pathLog != null)
            {
                saveLogs(pathLog, logs);
                saveBookMarksToFile(pathPuss, bookmarks);
                return pathLog.Split('\\').Last();
            }
            return null;
        }
        internal void saveBookMarksToFile(string pathFile, List<Log> bookmarks)
        {
            Logger.logD(TAG, "Save bookmarks from file");
            if (File.Exists(pathFile))
            {
                using (FileStream fileStream = File.Open(pathFile, FileMode.Open))
                {
                    fileStream.SetLength(0);
                    fileStream.Close();
                }
            }

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement pussElement = xmlDoc.CreateElement("Puss");
            xmlDoc.AppendChild(pussElement);

            XmlElement bookmarksElement = xmlDoc.CreateElement("Bookmarks");
            pussElement.AppendChild(bookmarksElement);

            foreach (Log bookmark in bookmarks)
            {
                XmlElement itemElement = xmlDoc.CreateElement("Item");
                itemElement.SetAttribute("id", bookmark.Line.ToString());
                bookmarksElement.AppendChild(itemElement);
            }
            xmlDoc.Save(pathFile);
        }
        
        private string getPathFile()
        {
            Logger.logD(TAG, "Open file event");
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Logger.logD(TAG, "pathLog = " + fileDialog.FileName);
                    return fileDialog.FileName;
                }
            }
            return string.Empty;
        }
        
        private void readLogFromAdb()
        {

        }
        private string createPathFile()
        {
            using (SaveFileDialog fileDialog = new SaveFileDialog())
            {
                fileDialog.AddExtension = true;
                fileDialog.DefaultExt = "log";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    return fileDialog.FileName;
                }
            }
            return null;
        }
        private List<Log> readLogs(string pathFile)
        {
            Logger.logD(TAG, "Read logs from file");
            List<Log> logs = new List<Log>();
            if (File.Exists(pathFile))
            {
                using (StreamReader reader = new StreamReader(pathFile))
                {
                    Line = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        logs.Add(convertLog(line));
                    }
                }
                return logs;

            }
            return null;
        }
        
        private void saveLogs(string pathFile, List<Log> logs)
        {
            if (File.Exists(pathFile))
            {
                using (FileStream fileStream = File.Open(pathFile, FileMode.Open))
                {
                    fileStream.SetLength(0);
                    fileStream.Close();
                }
            }
            using (StreamWriter sw = new StreamWriter(pathFile))
            {
                foreach (Log log in logs)
                {
                    sw.WriteLine(log.ToString());
                }
                sw.Close();
            }
        }
        
        private Log convertLog(string line)
        {
            Line += 1;
            string[] datas = new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
            string[] lineSplited = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < 7; i++)
            {
                if (i <= lineSplited.Length - 1)
                {
                    if (i == 6)
                    {
                        datas[6] = line.Substring(line.IndexOf(datas[5]) + datas[5].Length + 1);
                        break;
                    }
                    datas[i] = lineSplited[i];
                }
            }
            return new Log(Line, datas[0], datas[1], datas[2], datas[3], datas[4], datas[5], datas[6]);

        }
        internal string getFileName()
        {
            return fileName;
        }
        internal void resetLine()
        {
            Line = 0;
        }
    }
}
