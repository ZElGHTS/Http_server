using System;
using System.Collections.Generic;

namespace TP1.library
{
    public class Header
    {
        private readonly Dictionary<string, List<string>> _headers;
        private string _path;

        public string Path
        {
            get { return _path; }
        }

        public Header()
        {
            _headers = new Dictionary<string, List<string>>();
        }

        public void SplitHeaders(string requestHeaders)
        {
            if (string.IsNullOrWhiteSpace(requestHeaders)) return;
            
            var first = true;
            var tempStorage = requestHeaders.Split("\r\n");

            foreach (var elem in tempStorage)
            {
                if (first)
                {
                    var splitPath = elem.Split(" /");
                    _path = splitPath[1].Split(" ")[0];
                    first = false;
                }
                else
                {
                    if (elem != "")
                    {
                        SplitForDictionary(elem);
                    }
                }
            }
        }

        private void SplitForDictionary(string header)
        {
            List<string> list;
            var splittedHeader = header.Split(": ");
            
            if (!_headers.ContainsKey(splittedHeader[0]))
            {
                list = new List<string>();
                _headers.Add(splittedHeader[0], list);
            }
            else
            {
                list = _headers[splittedHeader[0]];
            }
            list.Add(splittedHeader[1]);
        }
    }
}