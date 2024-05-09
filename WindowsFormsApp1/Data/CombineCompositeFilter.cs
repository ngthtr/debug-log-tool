using BrightIdeasSoftware;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp1.Data
{
    internal class CombineCompositeFilter : CompositeFilter
    {
        private CombineHandler combineHandler;
        private CombineOneHandler combineOneHandler = new CombineOneHandler();
        private CombineTwoHandler combineTwoHandler = new CombineTwoHandler();
        private CombineThreeHandler combineThreeHandler = new CombineThreeHandler();
        private CombineFourHandler combineFourHandler = new CombineFourHandler();
        private CombineFiveHandler combineFiveHandler = new CombineFiveHandler();
        private CombineSixHandler combineSixHandler = new CombineSixHandler();
        private CombineSevenHandler combineSevenHandler = new CombineSevenHandler();
        private CombineEightHandler combineEightHandler = new CombineEightHandler();

        //public List<bool> combines;
        public CombineCompositeFilter(List<IModelFilter> filters, List<bool> combines) : base(filters)
        {
            if (combines[0] == true && combines[1] == true && combines[2] == true)
                combineHandler = combineOneHandler;
            else if (combines[0] == false && combines[1] == true && combines[2] == true)
                combineHandler = combineTwoHandler;
            else if (combines[0] == true && combines[1] == false && combines[2] == true)
                combineHandler = combineThreeHandler;
            else if (combines[0] == true && combines[1] == true && combines[2] == false)
                combineHandler = combineFourHandler;
            else if (combines[0] == false && combines[1] == false && combines[2] == true)
                combineHandler = combineFiveHandler;
            else if (combines[0] == false && combines[1] == true && combines[2] == false)
                combineHandler = combineSixHandler;
            else if (combines[0] == true && combines[1] == false && combines[2] == false)
                combineHandler = combineSevenHandler;
            else if (combines[0] == false && combines[1] == false && combines[2] == false)
                combineHandler = combineEightHandler;

        }

        public override bool FilterObject(object modelObject)
        {
            bool isWord = Filters[0].Filter(modelObject);
            bool isTag = Filters[1].Filter(modelObject);
            bool isPid = Filters[2].Filter(modelObject);
            bool isTid = Filters[3].Filter(modelObject);
            bool isLevel = Filters[4].Filter(modelObject);

            return combineHandler.getCombine(isWord, isTag, isPid, isTid) && isLevel;
        }


        abstract class CombineHandler
        {
            public abstract bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid);
        }

        class CombineOneHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return isWord && isTag && isPid && isTid;
            }
        }

        class CombineTwoHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return isWord || isTag && isPid && isTid;
            }
        }

        class CombineThreeHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return isWord && isTag || isPid && isTid;
            }
        }

        class CombineFourHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return isWord && isTag && isPid || isTid;
            }
        }

        class CombineFiveHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return isWord || isTag || isPid && isTid;
            }
        }

        class CombineSixHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return isWord || isTag && isPid || isTid;
            }
        }

        class CombineSevenHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return isWord && isTag || isPid || isTid;
            }
        }

        class CombineEightHandler : CombineHandler
        {
            public override bool getCombine(bool isWord, bool isTag, bool isPid, bool isTid)
            {
                return isWord || isTag || isPid || isTid;
            }
        }
    }
}
