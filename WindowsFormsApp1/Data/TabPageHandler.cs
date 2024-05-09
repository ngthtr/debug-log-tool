using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1.Data
{
    internal class TabPageHandler : TabPage
    {
        private const string TAG = "TabPageHandler";
        public const string LineCol = "Line";
        public const string DateCol = "Date";
        public const string TimeCol = "Time";
        public const string PidCol = "Pid";
        public const string TidCol = "Tid";
        public const string LevelCol = "Level";
        public const string TagCol = "Tag";
        public const string MsgCol = "Message";

        public const int IndexLineCol = 0;
        public const int IndexDateCol = 1;
        public const int IndexTimeCol = 2;
        public const int IndexPidCol = 3;
        public const int IndexTidCol = 4;
        public const int IndexLevelCol = 5;
        public const int IndexTagCol = 6;
        public const int IndexMsgCol = 7;

        private const int DeWidthLineCol = 50;
        private const int DeWidthDateCol = 50;
        private const int DeWidthTimeCol = 100;
        private const int DeWidthPidCol = 50;
        private const int DeWidthTidCol = 50;
        private const int DeWidthLevelCol = 30;
        private const int DeWidthTagCol = 200;
        private const int DeWidthMsgCol = 3000;

        public const int SHORTED_BY_LINE = 0;
        public const int SHORTED_BY_TIME = 1;

        public List<bool> columns = new List<bool>()
        {
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
        };
        public List<bool> controlButtons = new List<bool>()
        {
            true,
            false,
            false
        };
        public List<Log> logs = new List<Log>();
        public List<Log> bookmarks = new List<Log>();
        public List<Log> shortedLog = new List<Log>();
        public string textSubItem = string.Empty;
        public int from = 0;
        public int to = int.MaxValue;
        public int typeShorted;


        private FastObjectListView lvLog;
        private FastObjectListView lvBookMark;
        private FilterHandler filterHandler;
        private LogHandler logHandler;
        private Renderer renderer;
        private string nameTabPage;

        public TabPageHandler(string _nameTabPage)
        {
            lvBookMark = Form1.Instance.lvBookMark;
            nameTabPage = _nameTabPage;
        }

        public void init()
        {
            filterHandler = new FilterHandler();
            logHandler = new LogHandler();
            renderer = new Renderer(filterHandler);

            lvLog = createListView();
            setUpTabPage(lvLog);
            lvLog.Refresh();
            logs = createData();
            lvLog.SetObjects(logs);

        }

        // =============== UI ====================// 
        private FastObjectListView createListView()
        {
            Logger.logD(TAG, "Create list view");

            FastObjectListView listView = new FastObjectListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                BackColor = Settings.getCurrentTheme()[Settings.LIST_VIEW_COLOR]
            };

            listView.AllColumns.Add(new OLVColumn
            {
                Text = LineCol,
                AspectName = LineCol,
                Width = DeWidthLineCol,
                DisplayIndex = IndexLineCol
            });
            listView.AllColumns.Add(new OLVColumn
            {
                Text = DateCol,
                AspectName = DateCol,
                Width = DeWidthDateCol,
                DisplayIndex = IndexDateCol
            });
            listView.AllColumns.Add(new OLVColumn
            {
                Text = TimeCol,
                AspectName = TimeCol,
                Width = DeWidthTimeCol,
                DisplayIndex = IndexTimeCol
            });
            listView.AllColumns.Add(new OLVColumn
            {
                Text = PidCol,
                AspectName = PidCol,
                Width = DeWidthPidCol,
                DisplayIndex = IndexPidCol
            });
            listView.AllColumns.Add(new OLVColumn
            {
                Text = TidCol,
                AspectName = TidCol,
                Width = DeWidthTidCol,
                DisplayIndex = IndexTidCol
            });
            listView.AllColumns.Add(new OLVColumn
            {
                Text = LevelCol,
                AspectName = LevelCol,
                Width = DeWidthLevelCol,
                DisplayIndex = IndexLevelCol
            });
            listView.AllColumns.Add(new OLVColumn
            {
                Text = TagCol,
                AspectName = TagCol,
                Width = DeWidthTagCol,
                DisplayIndex = IndexTagCol
            });
            listView.AllColumns.Add(new OLVColumn
            {
                Text = MsgCol,
                AspectName = MsgCol,
                Width = DeWidthMsgCol,
                DisplayIndex = IndexMsgCol
            });

            listView.RebuildColumns();

            listView.Location = new Point(0, 0);
            listView.UseFiltering = true;
            listView.OwnerDraw = true;
            listView.GridLines = true;
            listView.HideSelection = false;
            listView.UseCellFormatEvents = true;
            listView.UnfocusedHighlightBackgroundColor = SystemColors.Highlight;
            listView.UnfocusedHighlightForegroundColor = Color.White;
            listView.Font = Settings.Font;
            listView.DefaultRenderer = renderer;
            listView.AllColumns[IndexLineCol].Sortable = false;
            listView.AllColumns[IndexDateCol].Sortable = false;
            listView.AllColumns[IndexTimeCol].Sortable = false;
            listView.AllColumns[IndexPidCol].Sortable = false;
            listView.AllColumns[IndexTidCol].Sortable = false;
            listView.AllColumns[IndexLevelCol].Sortable = false;
            listView.AllColumns[IndexTagCol].Sortable = false;
            listView.AllColumns[IndexMsgCol].Sortable = false;

            listView.DoubleClick += mark_log_event;
            listView.FormatRow += format_row_event;
            listView.Click += click_field_log_event;

            return listView;
        }
        private void setUpTabPage(FastObjectListView listView)
        {
            Logger.logD(TAG, "Setup tabpage");
            Text = nameTabPage;
            Controls.Add(listView);
            BorderStyle = BorderStyle.FixedSingle;
        }

        // ================ EVENT ================//

        private void click_field_log_event(object sender, EventArgs e)
        {
            Logger.logD(TAG, "Select field log event");

            if (lvLog.SelectedObject is Log)
            {
                Point mousePos = lvLog.PointToClient(Control.MousePosition);
                ListViewHitTestInfo hitTest = lvLog.HitTest(mousePos);
                int columnIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
                textSubItem = lvLog.SelectedItem.SubItems[columnIndex].Text;
                Form1.Instance.tbInfo.Text = textSubItem;
            }
        }
        private void mark_log_event(object sender, EventArgs e)
        {
            Logger.logD(TAG, "Mark log event");

            if (lvLog.SelectedObject is Log selectedLog)
            {
                if (selectedLog.Mark)
                {
                    unmarkLog(selectedLog);
                }
                else
                {
                    markLog(selectedLog);
                }
            }
        }
        private void format_row_event(object sender, FormatRowEventArgs e)
        {
            if (e.Model is Log log)
            {
                if (log.Mark == true && e.Item.BackColor == Color.LightGray)
                {
                    e.Item.BackColor = Color.DarkKhaki;
                }
                if (log.Mark == false && e.Item.BackColor == Color.DarkKhaki)
                {
                    e.Item.BackColor = Color.LightGray;
                }
                e.Item.ForeColor = log.Color;
            }
        }
        private void markLog(Log log)
        {
            log.Mark = true;
            lvBookMark.AddObject(log);
            bookmarks.Add(log);
        }
        private void unmarkLog(Log log)
        {
            log.Mark = false;
            lvBookMark.RemoveObject(log);
            bookmarks.Remove(log);
        }
        private Log findLog(int line, FastObjectListView listview)
        {
            Log log = null;
            int left = 0;
            int right = listview.GetItemCount() - 1;
            int mid = 0;
            while (left <= right)
            {
                mid = (left + right) / 2;
                log = (Log)listview.GetItem(mid).RowObject;
                if (log.Line < line)
                {
                    left = mid + 1;
                }
                else if (log.Line > line)
                {
                    right = mid - 1;
                }
                else
                {
                    return log;
                }
            }
            if (log.Line > line)
            {
                return log;
            }
            return (Log)listview.GetItem(mid + 1).RowObject;
        }
        private int findPosition(int line)
        {
            int mid = -1;
            int left = 0;
            int right = logs.Count - 1;
            while (left < right - 1)
            {
                mid = (left + right) / 2;
                if (logs[mid].Line < line)
                {
                    left = mid;
                }
                else if (logs[mid].Line > line)
                {
                    right = mid;
                }
                else
                {
                    return mid;
                }
            }
            return mid;
        }

        // ================== LOGIC ===============//
        internal void clearLog()
        {
            if (lvLog != null && lvBookMark != null)
            {
                logs.Clear();
                lvLog.SetObjects(logs);
                lvLog.Refresh();

                bookmarks.Clear();
                lvBookMark.SetObjects(bookmarks);
                lvBookMark.Refresh();

                logHandler.resetLine();
            }
        }
        internal void clearShorted()
        {
            lvLog.SetObjects(logs);
        }
        internal void clearBookMark()
        {
            foreach (Log bookmark in bookmarks)
            {
                bookmark.Mark = false;
            }
            bookmarks.Clear();
            lvBookMark.SetObjects(bookmarks);
            lvBookMark.Refresh();
        }
        internal void startLogAdb()
        {
            logs.Clear();
            lvLog.Items.Clear();
            logHandler.startGetLogAdb();
            controlButtons[0] = false;
            controlButtons[1] = true;
            controlButtons[2] = true;
        }
        internal void stopLogAdb()
        {
            logHandler.stopGetLogAdb();
            controlButtons[0] = true;
            controlButtons[1] = false;
            controlButtons[2] = false;
        }
        internal void reload()
        {
            Logger.logD(TAG, "Reload");

            reloadFilter();

            reloadHighLights();

            reloadShortedLog();

            reloadColumns();
        }
        internal void reloadFont()
        {
            lvLog.Font = Settings.Font;
        }
        internal void reloadTheme()
        {
            lvLog.BackColor = Settings.getCurrentTheme()[Settings.LIST_VIEW_COLOR];
        }
        internal void reloadFilter()
        {
            Logger.logD(TAG, "Reload filter");

            filterHandler.update();
            lvLog.ModelFilter = new ModelFilter(filterHandler.filter);
        }
        internal void reloadHighLights()
        {
            Logger.logD(TAG, "Reload highlight");

            lvLog.Refresh();
        }
        internal void reloadColumns()
        {
            Logger.logD(TAG, "Reload column");
            for (int i = 0; i < 5; i++)
            {
                lvLog.AllColumns[i].IsVisible = columns[i];
            }
            lvLog.RebuildColumns();
        }
        internal void reloadShortedLog()
        {
            Logger.logD(TAG, "Reload shorted log");

            shortedLog.Clear();

            int fromPosition = 0;
            int toPosition = logs.Count - 1;

            if (typeShorted == SHORTED_BY_LINE)
            {
                if (from > logs[0].Line)
                {
                    fromPosition = findPosition(from);
                }
                if (to < logs[logs.Count - 1].Line)
                {
                    toPosition = findPosition(to);
                }
            }

            if (fromPosition <= toPosition)
            {
                shortedLog = logs.GetRange(fromPosition, toPosition - fromPosition);
                lvLog.SetObjects(shortedLog);
            }
        }
        internal void setCombines(Dictionary<int, bool> combines)
        {
            if (filterHandler != null)
            {
                filterHandler.combines = combines;
            }
        }
        internal void setKeys(Dictionary<int, List<string>> keys)
        {
            if (filterHandler != null)
            {
                filterHandler.keys = keys;
            }
        }
        internal void setLevels(List<string> levels)
        {
            if (filterHandler != null)
            {
                filterHandler.levels = levels;
            }
        }
        internal void setKeysCombineType(Dictionary<int, bool> keysCombineType)
        {
            if (filterHandler != null)
            {
                filterHandler.keysCombineType = keysCombineType;
            }
        }
        internal void setHighLights(Dictionary<int, string> highlights)
        {
            if (renderer != null)
            {
                renderer.highlights = highlights;
            }
        }
        internal Dictionary<int, List<string>> getKeys()
        {
            if (filterHandler != null)
            {
                return filterHandler.keys;
            }
            return null;
        }
        internal Dictionary<int, bool> getKeysCombineType()
        {
            if (filterHandler != null)
            {
                return filterHandler.keysCombineType;
            }
            return null;
        }
        internal Dictionary<int, string> getHighLights()
        {
            if (renderer != null)
            {
                return renderer.highlights;
            }
            return null;
        }
        internal void focusLog(Log log)
        {
            Log logShow;
            Log firstLog = (Log)lvLog.GetItem(0).RowObject;
            Log lastLog = (Log)lvLog.GetItem(lvLog.GetItemCount() - 1).RowObject;

            if (log.Line < firstLog.Line) logShow = firstLog;
            else if (log.Line > lastLog.Line) logShow = lastLog;
            else
            {
                logShow = findLog(log.Line, lvLog);
            }

            lvLog.SelectObject(logShow);
            lvLog.EnsureVisible(lvLog.IndexOf(logShow));

        }

        // ================ Emulator ==============//

        private List<Log> createData()
        {
            List<Log> logs = new List<Log>();
            string[] levels = { Log.LevelV, Log.LevelD, Log.LevelI, Log.LevelW, Log.LevelE };
            Random random = new Random();
            for (int i = 1; i < 50; i++)
            {
                Log log = new Log(i, i.ToString(), "11:11:11.123", random.Next(50000).ToString(), random.Next(50000).ToString(), levels[random.Next(5)], "tag" + random.Next(10000).ToString(), "message " + random.Next(10000).ToString() + "message" + random.Next(10000).ToString() + " message message message homquaemdichuahuonghoanotua");
                logs.Add(log);
            }
            return logs;
        }


    }
}
