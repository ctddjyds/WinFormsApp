using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduNetworkSearch
{
    public class SearchResult
    {
        public BDWPResource[] resources { get; set; }
    }

    public class BDWPResource
    {
        public string title { get; set; }
        public string content { get; set; }
        public string unescapedUrl { get; set; }
    }

}

