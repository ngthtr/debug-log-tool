using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Data
{
    internal class CompositeSubFilter : CompositeFilter
    {
        private List<TextMatchFilter> mFilters;
        public CompositeSubFilter(List<TextMatchFilter> filters) : base(filters) 
        { 
            mFilters = filters;
        }
        public override bool FilterObject(object modelObject)
        {
            bool isShow = Filters[0].Filter(modelObject);
            bool isHide = mFilters[1] == null ? true : !Filters[1].Filter(modelObject);
            return isShow && isHide;
        }
    }
}
