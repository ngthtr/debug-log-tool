using BrightIdeasSoftware;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1.Data
{
    internal class Renderer : BaseRenderer
    {
        private const string TAG = "Renderer";
        public static int KEY_HIGHLIGHT_1 = 0;
        public static int KEY_HIGHLIGHT_2 = 1;
        public static int KEY_HIGHLIGHT_3 = 2;
        public static int KEY_HIGHLIGHT_4 = 3;
        public static int KEY_HIGHLIGHT_5 = 4;
        public static int KEY_HIGHLIGHT_6 = 5;

        public Dictionary<int, string> highlights = new Dictionary<int, string>
        {
           { KEY_HIGHLIGHT_1, string.Empty },
           { KEY_HIGHLIGHT_2, string.Empty },
           { KEY_HIGHLIGHT_3, string.Empty },
           { KEY_HIGHLIGHT_4, string.Empty },
           { KEY_HIGHLIGHT_5, string.Empty },
           { KEY_HIGHLIGHT_6, string.Empty }
        };
        public Dictionary<int, SolidBrush> backBrushes = new Dictionary<int, SolidBrush>()
        {
            { KEY_HIGHLIGHT_1, new SolidBrush(Color.Cyan) },
            { KEY_HIGHLIGHT_2, new SolidBrush(Color.Yellow) },
            { KEY_HIGHLIGHT_3, new SolidBrush(Color.Lime) },
            { KEY_HIGHLIGHT_4, new SolidBrush(Color.Blue) },
            { KEY_HIGHLIGHT_5, new SolidBrush(Color.Magenta) },
            { KEY_HIGHLIGHT_6, new SolidBrush(Color.Red) }
        };
        public Dictionary<int, SolidBrush> foreBrushes = new Dictionary<int, SolidBrush>()
        {
            { KEY_HIGHLIGHT_1, new SolidBrush(Color.DarkBlue)},
            { KEY_HIGHLIGHT_2, new SolidBrush(Color.Red)},
            { KEY_HIGHLIGHT_3, new SolidBrush(Color.DarkRed)},
            { KEY_HIGHLIGHT_4, new SolidBrush(Color.White)},
            { KEY_HIGHLIGHT_5, new SolidBrush(Color.DarkBlue)},
            { KEY_HIGHLIGHT_6, new SolidBrush(Color.White)}
        };
        private SolidBrush redBoldBrush = new SolidBrush(Color.Red);
        private FilterHandler filterHandler;
        private StringFormat stringFormat;

        public Renderer(FilterHandler _filterHandler) : base()
        {
            filterHandler = _filterHandler;
            stringFormat = new StringFormat();
            stringFormat.LineAlignment = EffectiveCellVerticalAlignment;
            stringFormat.Trimming = StringTrimming.EllipsisCharacter;
            stringFormat.Alignment = ((Column != null) ? Column.TextStringAlign : StringAlignment.Near);

        }
        public override bool RenderSubItem(DrawListViewSubItemEventArgs e, Graphics g, Rectangle r, object model)
        {
            /* Default render */
            base.RenderSubItem(e, g, r, model);
            if (e.ColumnIndex == TabPageHandler.IndexMsgCol
                || e.ColumnIndex == TabPageHandler.IndexTagCol)
            {

                SolidBrush foreBrush = ((SolidBrush)TextBrush);
                SolidBrush backBrush = new SolidBrush(GetBackgroundColor());

                g.FillRectangle(backBrush, r);

                List<SubText> listSubTextBySearched = getAllSubTextBySearchedOrdered(e.ColumnIndex, redBoldBrush, backBrush);
                List<SubText> listSubTextByAdditional = getSubTextsByAdditional(listSubTextBySearched, foreBrush, backBrush, GetText());

                List<SubText> allSubTexts = new List<SubText>();
                allSubTexts.AddRange(listSubTextBySearched);
                allSubTexts.AddRange(listSubTextByAdditional);
                allSubTexts = allSubTexts.OrderBy(obj => obj.start).ToList();

                paint(allSubTexts, e.ColumnIndex, g, r);
            }

            return true;
        }
        private void paint(List<SubText> allSubTexts, int indexCol, Graphics g, Rectangle r)
        {
            float x = r.X;
            float y = r.Y;

            if (indexCol == TabPageHandler.IndexMsgCol)
            {
                foreach (SubText subText in allSubTexts)
                {
                    string text = GetText().Substring(subText.start, subText.end - subText.start + 1);
                    SizeF sizef = g.MeasureString(text, subText.font);
                    if (subText.backBrush.Color != GetBackgroundColor())
                    {
                        //g.FillRectangle(subText.backBrush, new RectangleF(x, y, sizef.Width, r.Height));
                    }
                    g.DrawString(text, subText.font, subText.foreBrush, x, y);
                    Logger.logD(TAG, "text = " + text + ", font = " + subText.font + ", x = " + x);
                    x = x + sizef.Width;
                }
                return;
            }

            if (indexCol == TabPageHandler.IndexTagCol)
            {
                foreach (SubText subText in allSubTexts)
                {
                    string text = GetText().Substring(subText.start, subText.end - subText.start + 1);
                    SizeF sizef = g.MeasureString(text, Font);
                    g.DrawString(text, subText.font, subText.foreBrush, x, y);
                    x = x + sizef.Width - 2;
                }
                return;
            }
        }
        private List<SubText> getAllSubTextBySearchedOrdered(int indexCol, SolidBrush foreBrush, SolidBrush backBrush)
        {
            if (indexCol == TabPageHandler.IndexMsgCol)
            {
                List<SubText> subTextList = new List<SubText>();

                List<string> keys = filterHandler.keys[FilterHandler.KEY_WORD_SHOW];
                if (keys.Count > 0)
                {
                    foreach (string key in keys)
                    {
                        List<SubText> subTexts = getSubTextBySearched(key, foreBrush, backBrush);
                        subTextList.AddRange(subTexts);
                    }
                }

                if (highlights.Count > 0)
                {
                    foreach (int key in highlights.Keys)
                    {
                        if (!string.IsNullOrEmpty(highlights[key]))
                        {
                            List<SubText> subTexts = getSubTextBySearched(highlights[key], foreBrushes[key], backBrushes[key]);
                            subTextList.AddRange(subTexts);
                        }
                    }

                }
                return subTextList.OrderBy(obj => obj.start).ToList();
            }

            if (indexCol == TabPageHandler.IndexTagCol)
            {
                List<string> keys = filterHandler.keys[FilterHandler.KEY_TAG_SHOW];
                if (keys.Count > 0)
                {
                    List<SubText> subTextList = new List<SubText>();
                    foreach (string key in keys)
                    {
                        List<SubText> subTexts = getSubTextBySearched(key, foreBrush, backBrush);
                        subTextList.AddRange(subTexts);
                    }
                    return subTextList.OrderBy(obj => obj.start).ToList();
                }
            }
            return new List<SubText>();
        }
        private List<SubText> getSubTextBySearched(string searched, SolidBrush foreBrush, SolidBrush backBrush)
        {
            List<SubText> subTexts = new List<SubText>();

            string subItemText = GetText().ToLower();
            int index = subItemText.IndexOf(searched, 0);
            while (index != -1)
            {
                subTexts.Add(new SubText(index, index + searched.Length - 1, foreBrush, backBrush, new Font(Font.FontFamily, Font.Size - 0.5f, FontStyle.Bold)));
                index = subItemText.IndexOf(searched, index + searched.Length);
            }

            return subTexts;
        }
        private List<SubText> getSubTextsByAdditional(List<SubText> listSubTexts, SolidBrush foreBrush, SolidBrush backBrush, string text)
        {
            int start = 0;
            List<SubText> listAdditionalSubTexts = new List<SubText>();
            foreach (SubText subText in listSubTexts)
            {
                if (subText.start > start)
                {
                    listAdditionalSubTexts.Add(new SubText(start, subText.start - 1, foreBrush, backBrush, Font));
                }
                start = subText.end + 1;
            }
            if (start < text.Length - 1)
            {
                listAdditionalSubTexts.Add(new SubText(start, text.Length - 1, foreBrush, backBrush, Font));
            }
            return listAdditionalSubTexts;
        }
        class SubText
        {
            public int start = 0;
            public int end = 0;
            public SolidBrush foreBrush;
            public SolidBrush backBrush;
            public Font font;

            public SubText(int _start, int _end, SolidBrush _foreBrush, SolidBrush _backBrush, Font _font)
            {
                start = _start;
                end = _end;
                foreBrush = _foreBrush;
                backBrush = _backBrush;
                font = _font;
            }
        }
    }
}
