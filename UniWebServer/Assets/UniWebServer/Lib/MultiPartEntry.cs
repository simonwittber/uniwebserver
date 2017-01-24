using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UniWebServer
{
    public class MultiPartEntry
    {
        public readonly Headers headers;

        public string Value { get; private set; }

        public string Name { get; private set; }

        public string Filename { get; private set; }

        public MultiPartEntry ()
        {
            this.headers = new Headers ();
        }

        public static Dictionary<string, MultiPartEntry> Parse (Request request)
        {
            var mps = new Dictionary<string, MultiPartEntry> ();
            var contentType = request.headers.Get ("Content-Type");
            if (contentType.Contains ("multipart/form-data")) {
                
                var boundary = request.body.Substring(0, request.body.IndexOf("\r\n")) + "\r\n";
                var parts = request.body.Split (new string[] { boundary }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts) {
                    var sep = part.IndexOf ("\r\n\r\n");
                    if (sep == -1)
                        continue;
                    var headerText = part.Substring (0, sep);
                    var mp = new MultiPartEntry ();
                    mp.headers.Read (headerText);
                    mp.Value = part.Substring (sep);
                    if (mp.headers.Contains ("Content-Disposition")) {
                        var s = mp.headers.Get ("Content-Disposition");
                        var nm = new Regex (@"(?<=name\=\"")(.*?)(?=\"")").Match (s);
                        if (nm.Success)
                            mp.Name = nm.Value.Trim ();
                        var fm = new Regex (@"(?<=filename\=\"")(.*?)(?=\"")").Match (s);
                        if (fm.Success)
                            mp.Filename = fm.Value.Trim ();
                    }
                    if (mp.Name != null)
                        mps.Add (mp.Name, mp);
                }
                
            }
            return mps;
        }
    }
}