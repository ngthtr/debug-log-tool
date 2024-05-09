using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp1.Data
{
    internal class TabControlHandler
    {
        private const string TAG = "TabControlHandler";

        public TabPageHandler currentTabPageHandler;
        private LogHandler logHandler;
        private List<TabPageHandler> tabPageHandlers = new List<TabPageHandler>();
        private List<Log> logs = new List<Log>();
        private List<Log> bookmarks = new List<Log>();
        private TabControl tabControl;

        public TabControlHandler()
        {
            tabControl = Form1.Instance.tabControl; ;
            logHandler = new LogHandler();
        }

        internal void changeTabPage(int index)
        {
            Logger.logD(TAG, "Change tabpage");

            if (index < tabPageHandlers.Count)
            {
                currentTabPageHandler = tabPageHandlers[index];
            }
        }
        internal void createTabPage(string fileNameTabPage)
        {
            Logger.logD(TAG, "Create tabpage");

            if (fileNameTabPage == null)
            {
                fileNameTabPage = "new page";
            }
            TabPageHandler tabPageHandler = new TabPageHandler(fileNameTabPage);
            tabPageHandler.logs = new List<Log>(logs);
            tabPageHandler.bookmarks = new List<Log>(bookmarks);
            tabPageHandler.init();
            tabPageHandlers.Add(tabPageHandler);
            tabControl.TabPages.Insert(tabControl.TabCount, tabPageHandler);
            tabControl.SelectedIndex = tabPageHandlers.Count;
            logs.Clear();
            bookmarks.Clear();

        }
        internal void reload()
        {
            if (currentTabPageHandler != null)
            {
                currentTabPageHandler.reload();
            }
        }
        internal void reloadFilter()
        {
            if (currentTabPageHandler != null)
            {
                currentTabPageHandler.reloadFilter();
            }
        }
        internal void reloadColumn()
        {
            if (currentTabPageHandler != null)
            {
                currentTabPageHandler.reloadColumns();
            }
        }
        internal void reloadHighLight()
        {
            if (currentTabPageHandler != null)
            {
                currentTabPageHandler.reloadHighLights();
            }
        }
        internal void setHightLight(Dictionary<int, string> hightlights)
        {
            if (currentTabPageHandler != null)
            {
                currentTabPageHandler.setHighLights(hightlights);
            }
        }
        internal void setLevels(List<string> levels)
        {
            if (currentTabPageHandler != null)
            {
                currentTabPageHandler.setLevels(levels);
            }
        }
        internal void setKeys(Dictionary<int, List<string>> keys)
        {
            if (currentTabPageHandler != null)
            {
                currentTabPageHandler.setKeys(keys);
            }
        }
        internal void setKeysCombineType(Dictionary<int, bool> keysCombineType)
        {
            if (currentTabPageHandler != null)
            {
                currentTabPageHandler.setKeysCombineType(keysCombineType);
            }
        }
        internal void setCombines(Dictionary<int, bool> combines)
        {
            if (currentTabPageHandler != null)
            {
                currentTabPageHandler.setCombines(combines);
            }
        }
        internal void setColumns(List<bool> columns)
        {
            if (currentTabPageHandler != null)
            {
                Logger.logD(TAG, "Set column");
                currentTabPageHandler.columns = columns;
            }
        }
        internal void setShorted(int from, int to, int typeShorted)
        {
            if (currentTabPageHandler != null)
            {
                currentTabPageHandler.from = from;
                currentTabPageHandler.to = to;
                currentTabPageHandler.typeShorted = typeShorted;
                currentTabPageHandler.reloadShortedLog();
            }
        }
        internal void startLogAdb()
        {
            if (currentTabPageHandler != null)
            {
                Logger.logD(TAG, "Start read log adb");
                currentTabPageHandler.startLogAdb();
            }
        }
        internal void stopLogAdb()
        {
            if (currentTabPageHandler != null)
            {
                Logger.logD(TAG, "Stop read log adb");
                currentTabPageHandler.stopLogAdb();
            }
        }
        internal void saveLog()
        {
            if (currentTabPageHandler != null)
            {
                Logger.logD(TAG, "Save log");
                string fileName = logHandler.saveLogToFile(currentTabPageHandler.logs, currentTabPageHandler.bookmarks);
                if (fileName != null)
                {
                    currentTabPageHandler.Text = fileName;
                }
            }
        }
        internal void openLog()
        {
            Logger.logD(TAG, "Open log");

            logs = logHandler.readLogFromFile();
            if (logs.Count > 0)
            {
                bookmarks = logHandler.readBookMarkFromFile(logs);
                createTabPage(logHandler.getFileName());
            }
        }
        internal void focusLog(Log log)
        {
            if (currentTabPageHandler != null)
            {
                Logger.logD(TAG, "Focus log with line = " + log.Line.ToString());
                currentTabPageHandler.focusLog(log);
            }
        }
        internal void clearLogs()
        {
            if (currentTabPageHandler != null)
            {
                currentTabPageHandler.clearLog();
            }
        }
        internal void clearBookMark()
        {
            if (currentTabPageHandler != null)
            {
                currentTabPageHandler.clearBookMark();
            }
        }
        internal void reloadSettings()
        {
            foreach (TabPageHandler tabpage in tabPageHandlers)
            {
                tabpage.reloadFont();
                tabpage.reloadTheme();
            }
        }
        internal void removeShorted()
        {
            if (currentTabPageHandler != null)
            {
                currentTabPageHandler.clearShorted();
            }
        }
        internal void removeTabPage()
        {
            if (currentTabPageHandler != null)
            {
                tabPageHandlers.Remove(currentTabPageHandler);
                tabControl.TabPages.Remove(currentTabPageHandler);
                tabControl.SelectedIndex = tabControl.TabPages.Count - 1;
            }
        }

    }
}
