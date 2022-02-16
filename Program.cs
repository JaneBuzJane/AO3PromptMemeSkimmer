using System.Linq;

namespace Claim_Skimmer
{

    class Program
    {

        private class Prompt
        {

            public string Author { get; set; }
            public string[] Categories { get; set; }
            public string[] Characters { get; set; }
            public System.DateTime Date { get; set; }
            public string[] Fandoms { get; set; }
            public string[] FreeformTags { get; set; }
            public string PromptID { get; set; }
            public string[] Ratings { get; set; }
            public string[] Relationships { get; set; }
            public string[] Statuses { get; set; }
            public string Summary { get; set; }
            public string[] Tags { get; set; }
            public string Title { get; set; }
            public string[] Warnings { get; set; }
        }

        private static string Collection = "trans_positivity_fiction";
        private static int DelayMaximum = 6;
        private static int DelayMinimum = 5;
        private static string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:84.0) Gecko/20100101 Firefox/84.0";
        private static string UserName = "Flordibel";
        private static string UserPassword = "Neopia_21";

        static void Main(string[] args)
        {

            var cookies = new System.Net.CookieContainer();
            var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(@"https://archiveofourown.org/users/login");
            string responseBody;

            request.AutomaticDecompression = System.Net.DecompressionMethods.All;
            request.CookieContainer = cookies;
            request.Method = "GET";
            request.UserAgent = UserAgent;

            using (var response = request.GetResponse())
            using (var reader = new System.IO.StreamReader(response.GetResponseStream())) responseBody = reader.ReadToEnd();

            var authenticityTokenMatch = System.Text.RegularExpressions.Regex.Match
            (

                responseBody,
                @"<[^>]*id=['""]loginform(?:(?!<[^>]*name=['""]authenticity_token).)*<[^>]*value=['""]([^'""]+)",
                (

                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                    System.Text.RegularExpressions.RegexOptions.Multiline |
                    System.Text.RegularExpressions.RegexOptions.Singleline
                )
            );

            if (!authenticityTokenMatch.Success)
            {

                System.Console.WriteLine("Could not retrieve authenticity token.");

                System.Console.ReadLine();

                return;
            }

            request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(@"https://archiveofourown.org/users/login");
            request.AutomaticDecompression = System.Net.DecompressionMethods.All;
            request.CookieContainer = cookies;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.Referer = request.RequestUri.ToString();
            request.UserAgent = UserAgent;

            using (var writer = new System.IO.StreamWriter(request.GetRequestStream(), System.Text.Encoding.UTF8))
                writer.Write(string.Join("&", new System.Collections.Generic.Dictionary<string, string>
                {

                    { "utf8", "✓" },
                    { "authenticity_token", authenticityTokenMatch.Groups[1].Value },
                    { "user[login]", UserName },
                    { "user[password]", UserPassword },
                    { "user[remember_me]", "1" }

                }.Select(keyValue => string.Format("{0}={1}", System.Web.HttpUtility.UrlEncode(keyValue.Key), System.Web.HttpUtility.UrlEncode(keyValue.Value).Replace("%20", "+")))));

            using (var response = request.GetResponse())
            using (var reader = new System.IO.StreamReader(response.GetResponseStream())) responseBody = reader.ReadToEnd();

            var collection = cookies.GetCookies(request.RequestUri);

            if (collection == null || collection["user_credentials"] == null || collection["user_credentials"].Value != "1")
            {

                System.Console.WriteLine("Could not log in.");

                System.Console.ReadLine();

                return;
            }

            var prompts = new System.Collections.Generic.List<Prompt>();
            var resetEvent = new System.Threading.ManualResetEvent(false);

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {

                System.Text.RegularExpressions.MatchCollection matches;
                var random = new System.Random();

                for (int i = 1; !resetEvent.WaitOne(0); i++)
                {

                    request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(string.Format(@"https://archiveofourown.org/collections/{0}/requests?page={1}", System.Web.HttpUtility.UrlEncode(Collection), i));
                    request.AutomaticDecompression = System.Net.DecompressionMethods.All;
                    request.CookieContainer = cookies;
                    request.Method = "GET";
                    request.UserAgent = UserAgent;

                    using (var response = request.GetResponse())
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream())) responseBody = reader.ReadToEnd();

                    if ((matches = System.Text.RegularExpressions.Regex.Matches
                    (

                        responseBody,
                        @"
                        <[^>]*role=['""]article[^>]*>(?:(?!<[^>]*role=['""]article[^>]*>|<\!--\/content).)+
                        ",
                        (

                            System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                            System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace |
                            System.Text.RegularExpressions.RegexOptions.Multiline |
                            System.Text.RegularExpressions.RegexOptions.Singleline
                        )
                    )).Count == 0) break;

                    foreach (System.Text.RegularExpressions.Match articleMatch in matches)
                    {

                        var match = System.Text.RegularExpressions.Regex.Match
                        (

                            articleMatch.Value,
                            @"
                            <[^>]*action=['""][^'""]*prompt_id=(?<PromptID>[^\&'""]+)
                            ",
                            (

                                System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                                System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace
                            )
                        );

                        if (match.Groups["PromptID"].Success)
                        {

                            var prompt = new Prompt { PromptID = (match.Groups["PromptID"].Value ?? "").Trim() };

                            if ((match = System.Text.RegularExpressions.Regex.Match
                            (

                                articleMatch.Value,
                                @"<[^>]*class=['""]heading[^>]*>\s*(?<Title>(?:(?!\s+by\s+).)*)\s+by\s+(?<Author>(?:(?!<\/[^>]*>).)+)",
                                (

                                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                                    System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace |
                                    System.Text.RegularExpressions.RegexOptions.Multiline |
                                    System.Text.RegularExpressions.RegexOptions.Singleline
                                )
                            )).Success)
                            {

                                prompt.Author = System.Web.HttpUtility.HtmlDecode((match.Groups["Author"].Value ?? "")).Trim();
                                prompt.Title = System.Web.HttpUtility.HtmlDecode((match.Groups["Title"].Value ?? "")).Trim();
                            }

                            if ((match = System.Text.RegularExpressions.Regex.Match
                            (

                                articleMatch.Value,
                                @"
                                <[^>]*class=['""]required-tags[^>]*>\s*
                                <[^>]*>\s*<[^>]*class=['""][^'""]*symbol[^>]*>\s*<[^>]*>\s*<[^>]*>(?<Ratings>(?:(?!<\/[^>]*>).)+)(?:(?:(?!<\/[^>]*>).)*<\/[^>]*>\s*){3}\s*<\/[^>]*>\s*
                                <[^>]*>\s*<[^>]*class=['""][^'""]*symbol[^>]*>\s*<[^>]*>\s*<[^>]*>(?<Warnings>(?:(?!<\/[^>]*>).)+)(?:(?:(?!<\/[^>]*>).)*<\/[^>]*>\s*){3}\s*<\/[^>]*>\s*
                                <[^>]*>\s*<[^>]*class=['""][^'""]*symbol[^>]*>\s*<[^>]*>\s*<[^>]*>(?<Categories>(?:(?!<\/[^>]*>).)+)(?:(?:(?!<\/[^>]*>).)*<\/[^>]*>\s*){3}\s*<\/[^>]*>\s*
                                <[^>]*>\s*<[^>]*class=['""][^'""]*symbol[^>]*>\s*<[^>]*>\s*<[^>]*>(?<Statuses>(?:(?!<\/[^>]*>).)+)(?:(?:(?!<\/[^>]*>).)*<\/[^>]*>\s*){3}\s*<\/[^>]*>\s*
                                ",
                                (

                                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                                    System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace |
                                    System.Text.RegularExpressions.RegexOptions.Multiline |
                                    System.Text.RegularExpressions.RegexOptions.Singleline
                                )
                            )).Success)
                            {

                                prompt.Categories = GetFields("Categories", match);
                                prompt.Ratings = GetFields("Ratings", match);
                                prompt.Statuses = GetFields("Statuses", match);
                                prompt.Warnings = GetFields("Warnings", match);
                            }

                            if ((match = System.Text.RegularExpressions.Regex.Match
                            (

                                articleMatch.Value,
                                @"(?:<[^>]*class=['""]characters[^>]*>\s*<[^>]*class=['""]tag[^>]*>(?<Characters>(?:(?!<\/[^>]*>).)+)<\/[^>]*>\s*<\/[^>]*>\s*)+",
                                (

                                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                                    System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace |
                                    System.Text.RegularExpressions.RegexOptions.Multiline |
                                    System.Text.RegularExpressions.RegexOptions.Singleline
                                )
                            )).Success)
                            {

                                prompt.Characters = GetFields("Characters", match);
                            }

                            if ((match = System.Text.RegularExpressions.Regex.Match
                            (

                                articleMatch.Value,
                                @"<[^>]*class=['""]datetime[^>]*>(?<Date>(?:(?!<\/[^>]*>).)+)",
                                (

                                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                                    System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace |
                                    System.Text.RegularExpressions.RegexOptions.Multiline |
                                    System.Text.RegularExpressions.RegexOptions.Singleline
                                )
                            )).Success)
                            {

                                prompt.Date = System.DateTime.ParseExact((match.Groups["Date"].Value ?? ""), "dd MMM yyyy", System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
                            }


                            if ((match = System.Text.RegularExpressions.Regex.Match
                            (

                                articleMatch.Value,
                                @"(?:<[^>]*class=['""]tag[^>]*>(?<Fandoms>(?:(?!<\/[^>]*>).)+)<\/[^>]*>\s*)+",
                                (

                                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                                    System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace |
                                    System.Text.RegularExpressions.RegexOptions.Multiline |
                                    System.Text.RegularExpressions.RegexOptions.Singleline
                                )
                            )).Success)
                            {

                                prompt.Fandoms = GetFields("Fandoms", match);
                            }

                            if ((match = System.Text.RegularExpressions.Regex.Match
                            (

                                articleMatch.Value,
                                @"(?:<[^>]*class=['""]freeforms[^>]*>\s*<[^>]*class=['""]tag[^>]*>(?<FreeformTags>(?:(?!<\/[^>]*>).)+)<\/[^>]*>\s*<\/[^>]*>\s*)+",
                                (

                                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                                    System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace |
                                    System.Text.RegularExpressions.RegexOptions.Multiline |
                                    System.Text.RegularExpressions.RegexOptions.Singleline
                                )
                            )).Success)
                            {

                                prompt.FreeformTags = GetFields("FreeformTags", match);
                            }

                            if ((match = System.Text.RegularExpressions.Regex.Match
                            (

                                articleMatch.Value,
                                @"(?:<[^>]*class=['""]relationships[^>]*>\s*<[^>]*class=['""]tag[^>]*>(?<Relationships>(?:(?!<\/[^>]*>).)+)<\/[^>]*>\s*<\/[^>]*>\s*)+",
                                (

                                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                                    System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace |
                                    System.Text.RegularExpressions.RegexOptions.Multiline |
                                    System.Text.RegularExpressions.RegexOptions.Singleline
                                )
                            )).Success)
                            {

                                prompt.Relationships = GetFields("Relationships", match);
                            }

                            if ((match = System.Text.RegularExpressions.Regex.Match
                            (

                                articleMatch.Value,
                                @"<[^>]*class=['""][^'""]*summary[^>]*>\s*<[^>]*>\s*(?<Summary>(?:(?!<\/[^>]*>).)+)",
                                (

                                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                                    System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace |
                                    System.Text.RegularExpressions.RegexOptions.Multiline |
                                    System.Text.RegularExpressions.RegexOptions.Singleline
                                )
                            )).Success)
                            {

                                prompt.Summary = System.Web.HttpUtility.HtmlDecode((match.Groups["Summary"].Value ?? "")).Trim();
                            }

                            if ((match = System.Text.RegularExpressions.Regex.Match
                            (

                                articleMatch.Value,
                                @"<[^>]*class=['""]optional\x20+tags[^>]*>\s*(?:<[^>]*>\s*<[^>]*class=['""]tag[^>]*>(?<Tags>(?:(?!<\/[^>]*>).)+)<\/[^>]*>\s*<\/[^>]*>\s*)+",
                                (

                                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                                    System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace |
                                    System.Text.RegularExpressions.RegexOptions.Multiline |
                                    System.Text.RegularExpressions.RegexOptions.Singleline
                                )
                            )).Success)
                            {

                                prompt.Tags = GetFields("Tags", match);
                            }

                            prompts.Add(prompt);
                        }
                    }

                    resetEvent.WaitOne(random.Next(DelayMinimum, DelayMaximum) * 1000);
                }

                using (var writer = new System.IO.StreamWriter(string.Format("{0}.csv", Collection)))
                {

                    writer.WriteLine(string.Join(",", new[]
                    {

                        "Author",
                        "Categories",
                        "Characters",
                        "Date",
                        "Fandoms",
                        "Freeform Tags",
                        "Prompt ID",
                        "Ratings",
                        "Relationships",
                        "Statuses",
                        "Summary",
                        "Tags",
                        "Title",
                        "Warnings"
                    }));

                    foreach (var prompt in prompts)
                    {

                        writer.WriteLine(string.Join(",", new[]
                        {

                            prompt.Author,
                            ((prompt.Categories == null) ? "" : string.Join(" ", prompt.Categories)),
                            ((prompt.Characters == null) ? "" : string.Join(" ", prompt.Characters)),
                            prompt.Date.ToString("yyyy-MM-dd"),
                            ((prompt.Fandoms == null) ? "" : string.Join(" ", prompt.Fandoms)),
                            ((prompt.FreeformTags == null) ? "" : string.Join(" ", prompt.FreeformTags)),
                            string.Format("https://archiveofourown.org/collections/{0}/prompts/{1}", Collection, prompt.PromptID),
                            ((prompt.Ratings == null) ? "" : string.Join(" ", prompt.Ratings)),
                            ((prompt.Relationships == null) ? "" : string.Join(" ", prompt.Relationships)),
                            ((prompt.Statuses == null) ? "" : string.Join(" ", prompt.Statuses)),
                            (prompt.Summary ?? ""),
                            ((prompt.Tags == null) ? "" : string.Join(" ", prompt.Tags)),
                            (prompt.Title ?? ""),
                            ((prompt.Warnings == null) ? "" : string.Join(" ", prompt.Warnings))
                        }));
                    }
                }

                System.Console.WriteLine("DONE!");
            });

            System.Console.ReadLine();

            resetEvent.Set();
        }

        private static string[] GetFields(string groupName, System.Text.RegularExpressions.Match match)
        {

            var fields = new System.Collections.Generic.List<string>();

            for (int i = 0; i < match.Groups[groupName].Captures.Count; i++)
                foreach (string field in System.Text.RegularExpressions.Regex.Split((match.Groups[groupName].Captures[i].Value ?? ""), @",\x20*"))
                    if (!string.IsNullOrWhiteSpace(field)) fields.Add(field.Trim());

            return fields.ToArray();
        }
    }
}