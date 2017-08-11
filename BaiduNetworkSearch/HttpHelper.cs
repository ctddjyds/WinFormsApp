using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BaiduNetworkSearch
{
class HttpHelper
{
    static readonly string urlTemplate = "http://pan1234.com/server3?jsoncallback=jQuery191042552483269501273_1478315152726&q={0}&start={1}";
    public static SearchResult Requset(string key, string start)
    {
        string url = string.Format(urlTemplate, key, start);
        HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
        httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
        httpRequest.Host = "pan1234.com";
        httpRequest.Referer = "http://pan.java1234.com/result.jsp?wp=0&op=0&ty=gn&q=" + Uri.EscapeUriString(key);
        try
        {
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            Stream s = httpResponse.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            string jsonString = sr.ReadToEnd();
            string jsonProcessed = null;
            if ((jsonProcessed = JsonPreProcessing(jsonString)) != null)
            {
                SearchResult searchResult = UtilityClass.GetObject<SearchResult>(jsonProcessed);
                return searchResult;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public static string JsonPreProcessing(string jsonString)
    {
        int startIndex = jsonString.IndexOf("(");
        if (startIndex > 0)
        {
            string json = jsonString.Substring(startIndex + 1);
            return "{\"resources\":" + json.Remove(json.Length - 3) + "}";
        }
        else
        {
            return null;
        }
    }
}
}
