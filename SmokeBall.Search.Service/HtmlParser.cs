﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using SmokeBall.Search.Models;
using SmokeBall.Search.Models.Interfaces;
using SmokeBall.Search.Service.Interfaces;

namespace SmokeBall.Search.Service
{
    public class HtmlParser : IHtmlParser
    {
        public IList<ISearchResult> GetGoogleSearchResults(string inputHtml)
        {
            const string findUrlPattern =
                "(<div\\sstyle=\\\"display:inline-block\\\"\\sclass=\\\"TbwUpd\\\"><cite\\sclass=\\\"iUh30.*?\\\">)(?<url>.*?)(<\\/cite><\\/div>)";
            const string findUrlAltPattern = "(<div\\sclass=\\\"hJND5c\\\"\\sstyle=\\\"margin-bottom:2px\\\"><cite>)(?<url>.*?)(<\\/cite>)";

            //  <div class="hJND5c" style="margin-bottom:2px"><cite>
            var pattern = findUrlPattern;
            var matchingItem = Regex.Match(inputHtml, pattern, RegexOptions.IgnoreCase);
            if (!matchingItem.Success)
                pattern = findUrlAltPattern;

            matchingItem = Regex.Match(inputHtml, pattern, RegexOptions.IgnoreCase);

            IList<ISearchResult> results = new List<ISearchResult>();
            var position = 0;
            while (matchingItem.Success)
            {
                ISearchResult search = new SearchResult
                {
                    Position = ++position,
                    Url = matchingItem.Groups["url"].Value.Replace("<b>","").Replace("</b>","").Trim()
                };
                results.Add(search);
                matchingItem = matchingItem.NextMatch();
            }

            return results;
        }
    }
}