using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp1.Data
{
    internal class FilterHandler
    {
        private const string TAG = "FilterHandler";

        public static string or = "Or";
        public static string and = "And";

        public static string orSig = "|";
        public static string andSig = "&";

        public static int KEY_TAG_SHOW = 0;
        public static int KEY_TAG_HIDE = 1;
        public static int KEY_WORD_SHOW = 2;
        public static int KEY_WORD_HIDE = 3;
        public static int KEY_PID_SHOW = 4;
        public static int KEY_PID_HIDE = 5;
        public static int KEY_TID_SHOW = 6;
        public static int KEY_TID_HIDE = 7;

        public static int KEY_COMBINE_1 = 0;
        public static int KEY_COMBINE_2 = 1;
        public static int KEY_COMBINE_3 = 2;

        private List<string> levelRevert;
        public List<string> levels = new List<string>
        {
            Log.LevelV,
            Log.LevelD,
            Log.LevelI,
            Log.LevelW,
            Log.LevelE,
        };
        public Dictionary<int, bool> combines = new Dictionary<int, bool>
        {
            { KEY_COMBINE_1, true },
            { KEY_COMBINE_2, true },
            { KEY_COMBINE_3, true },
        };
        public Dictionary<int, List<string>> keys = new Dictionary<int, List<string>>
        {
            { KEY_TAG_SHOW, new List<string>()},
            { KEY_TAG_HIDE, new List<string>()},
            { KEY_WORD_SHOW, new List<string>()},
            { KEY_WORD_HIDE, new List<string>()},
            { KEY_PID_SHOW, new List<string>()},
            { KEY_PID_HIDE, new List<string>()},
            { KEY_TID_SHOW, new List<string>()},
            { KEY_TID_HIDE, new List<string>()},
        };
        public Dictionary<int, bool> keysCombineType = new Dictionary<int, bool>
        {
            { KEY_TAG_SHOW, false /* default false -> or*/ },
            { KEY_TAG_HIDE, false },
            { KEY_WORD_SHOW, false },
            { KEY_WORD_HIDE, false },
            { KEY_PID_SHOW, false },
            { KEY_PID_HIDE, false },
            { KEY_TID_SHOW, false },
            { KEY_TID_HIDE, false },
        };

        private CombineHandler combineHandler;
        private CombineOneHandler combineOneHandler = new CombineOneHandler();
        private CombineTwoHandler combineTwoHandler = new CombineTwoHandler();
        private CombineThreeHandler combineThreeHandler = new CombineThreeHandler();
        private CombineFourHandler combineFourHandler = new CombineFourHandler();
        private CombineFiveHandler combineFiveHandler = new CombineFiveHandler();
        private CombineSixHandler combineSixHandler = new CombineSixHandler();
        private CombineSevenHandler combineSevenHandler = new CombineSevenHandler();
        private CombineEightHandler combineEightHandler = new CombineEightHandler();

        public static string convertKeyCombineType(bool isCombineType)
        {
            return isCombineType ? andSig : orSig;
        }
        internal void update()
        {
            Logger.logD(TAG, "update");
            List<string> standarLevels = new List<string>()
            {
                Log.LevelV,
                Log.LevelD,
                Log.LevelI,
                Log.LevelW,
                Log.LevelE
            };
            if (levels.Contains(Log.LevelV))
            {
                standarLevels.Remove(Log.LevelV);
            }
            if (levels.Contains(Log.LevelD))
            {
                standarLevels.Remove(Log.LevelD);
            }
            if (levels.Contains(Log.LevelI))
            {
                standarLevels.Remove(Log.LevelI);
            }
            if (levels.Contains(Log.LevelW))
            {
                standarLevels.Remove(Log.LevelW);
            }
            if (levels.Contains(Log.LevelE))
            {
                standarLevels.Remove(Log.LevelE);
            }
            levelRevert = standarLevels;

            bool isTagEmpty = keys[KEY_TAG_SHOW].Count == 0 && keys[KEY_TAG_HIDE].Count == 0;
            bool isWordEmpty = keys[KEY_WORD_SHOW].Count == 0 && keys[KEY_WORD_HIDE].Count == 0;
            bool isPidEmpty = keys[KEY_PID_SHOW].Count == 0 && keys[KEY_PID_HIDE].Count == 0;
            bool isTidEmpty = keys[KEY_TID_SHOW].Count == 0 && keys[KEY_TID_HIDE].Count == 0;

            if (isTagEmpty || isWordEmpty)
            {
                combines[KEY_COMBINE_1] = true;
            }
            if (isWordEmpty || isPidEmpty)
            {
                combines[KEY_COMBINE_2] = true;
            }
            if (isPidEmpty || isTidEmpty)
            {
                combines[KEY_COMBINE_3] = true;
            }

            if (combines[KEY_COMBINE_1] == true && combines[KEY_COMBINE_2] == true && combines[KEY_COMBINE_3] == true)
                combineHandler = combineOneHandler;
            else if (combines[KEY_COMBINE_1] == false && combines[KEY_COMBINE_2] == true && combines[KEY_COMBINE_3] == true)
                combineHandler = combineTwoHandler;
            else if (combines[KEY_COMBINE_1] == true && combines[KEY_COMBINE_2] == false && combines[KEY_COMBINE_3] == true)
                combineHandler = combineThreeHandler;
            else if (combines[KEY_COMBINE_1] == true && combines[KEY_COMBINE_2] == true && combines[KEY_COMBINE_3] == false)
                combineHandler = combineFourHandler;
            else if (combines[KEY_COMBINE_1] == false && combines[KEY_COMBINE_2] == false && combines[KEY_COMBINE_3] == true)
                combineHandler = combineFiveHandler;
            else if (combines[KEY_COMBINE_1] == false && combines[KEY_COMBINE_2] == true && combines[KEY_COMBINE_3] == false)
                combineHandler = combineSixHandler;
            else if (combines[KEY_COMBINE_1] == true && combines[KEY_COMBINE_2] == false && combines[KEY_COMBINE_3] == false)
                combineHandler = combineSevenHandler;
            else if (combines[KEY_COMBINE_1] == false && combines[KEY_COMBINE_2] == false && combines[KEY_COMBINE_3] == false)
                combineHandler = combineEightHandler;
        }
        internal bool filter(object modelObject)
        {
            Log log = (Log)modelObject;

            bool isTag = isPresented(log.Tag.ToLower(), 
                                        keys[KEY_TAG_SHOW], 
                                        keysCombineType[KEY_TAG_SHOW], 
                                        keys[KEY_TAG_HIDE], 
                                        keysCombineType[KEY_TAG_HIDE]);

            bool isWord = isPresented(log.Message.ToLower(), 
                                        keys[KEY_WORD_SHOW], 
                                        keysCombineType[KEY_WORD_SHOW], 
                                        keys[KEY_WORD_HIDE], 
                                        keysCombineType[KEY_WORD_HIDE]);

            bool isPid = isPresented(log.Pid.ToLower(), 
                                        keys[KEY_PID_SHOW], 
                                        keysCombineType[KEY_PID_SHOW], 
                                        keys[KEY_PID_HIDE], 
                                        keysCombineType[KEY_PID_HIDE]);

            bool isTid = isPresented(log.Tid.ToLower(), 
                                        keys[KEY_TID_SHOW], 
                                        keysCombineType[KEY_TID_SHOW], 
                                        keys[KEY_TID_HIDE], 
                                        keysCombineType[KEY_TID_HIDE]);

            bool isLevel = isPresented(log.Level);

            return combineHandler.getCombine(isTag, isWord, isPid, isTid) && isLevel;
        }
        private bool isPresented(string content, List<string> shows, bool combineTypeShow, List<string> hides, bool combineTypeHide)
        {
            if (string.IsNullOrEmpty(content))
            {
                if (!(shows.Count == 0) || !(hides.Count == 0))
                {
                    return false;
                }
            }

            bool isShow;
            if (shows.Count == 0)
            {
                isShow = true;
            }
            else
            {
                isShow = combineTypeShow ? shows.All(content.Contains) : isShow = shows.Any(content.Contains);
            }

            bool isHide;
            if (hides.Count == 0)
            {
                isHide = false;
            }
            else
            {
                isHide = combineTypeHide ? hides.All(content.Contains) : hides.Any(content.Contains);
            }
            return isShow && !isHide;
        }
        private bool isPresented(string level)
        {
            return string.IsNullOrEmpty(level) ? true : !levelRevert.Contains(level);
        }

        abstract class CombineHandler
        {
            public abstract bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid);
        }

        class CombineOneHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return ((isTag && isWord) && isPid) && isTid;
            }
        }

        class CombineTwoHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return ((isTag || isWord) && isPid) && isTid;
            }
        }

        class CombineThreeHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return ((isTag && isWord) || isPid) && isTid;
            }
        }

        class CombineFourHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return ((isTag && isWord) && isPid) || isTid;
            }
        }

        class CombineFiveHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return ((isTag || isWord) || isPid) && isTid;
            }
        }

        class CombineSixHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return ((isTag || isWord) && isPid) || isTid;
            }
        }

        class CombineSevenHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return ((isTag && isWord) || isPid) || isTid;
            }
        }

        class CombineEightHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return ((isTag || isWord) || isPid) || isTid;
            }
        }
    }
}
