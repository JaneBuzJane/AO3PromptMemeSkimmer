using System.Linq;

namespace APMS
{
    public partial class FrmAPMS : System.Windows.Forms.Form
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

        private static int DelayMaximum = 6;
        private static int DelayMinimum = 5;
        private static string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:84.0) Gecko/20100101 Firefox/84.0";
        private static System.Text.RegularExpressions.Regex WhiteSpaceLeadingTrailingReplaceRegex = new(@"\A[\s\x00]+|[\s\x00]+\z");
        private static System.Text.RegularExpressions.Regex WhiteSpacePaddingReplaceRegex = new(@"[\x00\s]{2,}");
        private static System.Text.RegularExpressions.Regex WhiteSpacePatternReplaceRegex = new(@"(?<_>[\<\(\{])\s+|\s+(?<_>[\>\)\}])|(?<_>\x20)\s+");

        private System.Threading.CancellationTokenSource cancellation;
        private string configFilePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.ini", System.IO.Path.GetFileNameWithoutExtension(System.AppDomain.CurrentDomain.FriendlyName)));
        private string saveToFilePath;

        public FrmAPMS()
        {

            InitializeComponent();

            try
            {

                var configFile = new System.IO.FileInfo(configFilePath);

                if (configFile.Exists)
                {

                    System.Text.RegularExpressions.MatchCollection matches;

                    using (var reader = configFile.OpenText())
                        matches = System.Text.RegularExpressions.Regex.Matches
                        (

                            reader.ReadToEnd(),
                            @"(?<Name>[^\n\r=]+)=(?<Value>[^\r]+)",
                            (

                                System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                                System.Text.RegularExpressions.RegexOptions.Multiline |
                                System.Text.RegularExpressions.RegexOptions.Singleline
                            )
                        );

                    if (matches.Count != 0)
                    {

                        foreach (System.Text.RegularExpressions.Match match in matches)
                        {

                            switch ((match.Groups["Name"].Value ?? "").ToLower())
                            {

                                case "collectionid": txtConfigCollectionID.Text = match.Groups["Value"].Value; break;
                                case "password": txtConfigPassword.Text = match.Groups["Value"].Value; break;
                                case "username": txtConfigUserName.Text = match.Groups["Value"].Value; break;
                            }
                        }
                    }
                }
            }
            catch { }

            btnStartCancel.Text = "Start";

            tlpProgress.Visible = false;
        }

        private void btnConfigSaveTo_Click(object sender, System.EventArgs e)
        {

            if (sfdSaveAs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                saveToFilePath = sfdSaveAs.FileName;
        }

        private void btnStartStop_Click(object sender, System.EventArgs e)
        {

            if (cancellation == null)
            {

                if (string.IsNullOrWhiteSpace(txtConfigCollectionID.Text))
                {

                    ActiveControl = txtConfigCollectionID;

                    System.Windows.Forms.MessageBox.Show("A valid collection ID is required to continue.", Text, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    return;
                }

                if (string.IsNullOrWhiteSpace(txtConfigUserName.Text))
                {

                    ActiveControl = txtConfigUserName;

                    System.Windows.Forms.MessageBox.Show("A valid username is required to continue.", Text, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    return;
                }

                if (string.IsNullOrWhiteSpace(txtConfigPassword.Text))
                {

                    ActiveControl = txtConfigPassword;

                    System.Windows.Forms.MessageBox.Show("A valid password is required to continue.", Text, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    return;
                }

                if (string.IsNullOrWhiteSpace(saveToFilePath))
                {

                    ActiveControl = btnConfigSaveTo;

                    System.Windows.Forms.MessageBox.Show("Please select a save location for our output before continuing.", Text, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    return;
                }

                try
                {

                    using (var writer = new System.IO.StreamWriter(configFilePath, false))
                    {

                        writer.WriteLine("CollectionID={0}", txtConfigCollectionID.Text);

                        writer.WriteLine("Password={0}", txtConfigPassword.Text);

                        writer.WriteLine("Username={0}", txtConfigUserName.Text);
                    }
                }
                catch (System.Exception ex)
                {

                    System.Windows.Forms.MessageBox.Show(string.Format("An error occurred while saving your settings: {0}", ex.Message), Text, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                    return;
                }

                var cancellation =
                this.cancellation = new System.Threading.CancellationTokenSource();

                string collectionID = txtConfigCollectionID.Text;
                string password = txtConfigPassword.Text;
                string userName = txtConfigUserName.Text;

                btnStartCancel.Text = "Cancel";
                lblProgress.Visible = false;
                lblStatus.Text = "Logging in to AO3...";
                proProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
                proProgress.Visible = true;
                tlpConfig.Visible = false;
                tlpProgress.Visible = true;

                tlpProgress.SetColumnSpan(proProgress, 2);

                System.Threading.Tasks.Task.Factory.StartNew
                (
                    
                    () =>
                    {

                        try
                        {

                            try
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

                                    Invoke(new System.Windows.Forms.MethodInvoker(() =>
                                    {

                                        lblStatus.Text = "Could not retrieve authentication token.";
                                        proProgress.Visible = false;
                                    }));

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
                                        { "user[login]", userName },
                                        { "user[password]", password },
                                        { "user[remember_me]", "1" }

                                    }.Select(keyValue => string.Format("{0}={1}", System.Web.HttpUtility.UrlEncode(keyValue.Key), System.Web.HttpUtility.UrlEncode(keyValue.Value).Replace("%20", "+")))));

                                using (var response = request.GetResponse())
                                using (var reader = new System.IO.StreamReader(response.GetResponseStream())) responseBody = reader.ReadToEnd();

                                var collection = cookies.GetCookies(request.RequestUri);

                                if (collection == null || collection["user_credentials"] == null || collection["user_credentials"].Value != "1")
                                {

                                    Invoke(new System.Windows.Forms.MethodInvoker(() =>
                                    {

                                        lblStatus.Text = "The username or password provided were not valid.";
                                        proProgress.Visible = false;
                                    }));

                                    return;
                                }

                                System.Text.RegularExpressions.MatchCollection matches;
                                int pagesTotal = 0;
                                var prompts = new System.Collections.Generic.List<Prompt>();
                                var random = new System.Random();

                                for (int i = 1; !cancellation.IsCancellationRequested; i++)
                                {

                                    if (pagesTotal != 0)
                                    {

                                        Invoke(new System.Windows.Forms.MethodInvoker(() =>
                                        {

                                            lblProgress.Text = string.Format("{0}%", System.Math.Round(((float)(i - 1) / pagesTotal) * 100));
                                            proProgress.Value = System.Math.Min((i - 1), proProgress.Maximum);
                                        }));
                                    }

                                    request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(string.Format(@"https://archiveofourown.org/collections/{0}/requests?page={1}", System.Web.HttpUtility.UrlEncode(collectionID), i));
                                    request.AutomaticDecompression = System.Net.DecompressionMethods.All;
                                    request.CookieContainer = cookies;
                                    request.Method = "GET";
                                    request.UserAgent = UserAgent;

                                    using (var response = request.GetResponse())
                                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8)) responseBody = reader.ReadToEnd();

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

                                    if (pagesTotal == 0)
                                    {

                                        var match = System.Text.RegularExpressions.Regex.Match
                                        (

                                            responseBody,
                                            @"<li[^>]*>\s*<a[^>]*>\s*(\d+)\s*<\/a>\s*<\/li>\s*<li[^>]*class=['""]next",
                                            (

                                                System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                                                System.Text.RegularExpressions.RegexOptions.Multiline |
                                                System.Text.RegularExpressions.RegexOptions.Singleline
                                            )
                                        );

                                        if (match.Success && int.TryParse(match.Groups[1].Value, out pagesTotal))
                                        {

                                            Invoke(new System.Windows.Forms.MethodInvoker(() =>
                                            {

                                                lblProgress.Text = string.Format("{0}%", System.Math.Round(((float)(i - 1) / pagesTotal) * 100));
                                                lblProgress.Visible = true;
                                                lblStatus.Text = "Skimming...";
                                                proProgress.Maximum = pagesTotal;
                                                proProgress.Style = System.Windows.Forms.ProgressBarStyle.Blocks;
                                                proProgress.Value = (i - 1);
                                                proProgress.Visible = true;

                                                tlpProgress.SetColumnSpan(proProgress, 1);
                                            }));
                                        }
                                    }

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
                                                @"<[^>]*class=['""]heading[^>]*>\s*(?<Title>(?:(?![\n\r]).)*)\s+by\s+(?<Author>(?:(?!<\/[^>]*>).)+)",
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
                                                @"(?:<[^>]*class=['""]tag[\x20'""][^>]*>(?<Fandoms>(?:(?!<\/[^>]*>).)+)<\/[^>]*>\s*)+",
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
                                                @"<(?<Element>[^\x20>]+)[^>]*class=['""][^'""]*summary[^>]*>(?<Summary>(?:(?!<\/\k<Element>>).)+)",
                                                (

                                                    System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                                                    System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace |
                                                    System.Text.RegularExpressions.RegexOptions.Multiline |
                                                    System.Text.RegularExpressions.RegexOptions.Singleline
                                                )
                                            )).Success)
                                            {

                                                prompt.Summary = System.Web.HttpUtility.HtmlDecode((match.Groups["Summary"].Value ?? "")).Trim();
                                                prompt.Summary = System.Text.RegularExpressions.Regex.Replace(prompt.Summary, @"<[^>]*>", " ");
                                                prompt.Summary = CleanWhiteSpace(prompt.Summary);
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

                                    cancellation.Token.WaitHandle.WaitOne(random.Next(DelayMinimum, DelayMaximum) * 1000);
                                }

                                Invoke(new System.Windows.Forms.MethodInvoker(() =>
                                {

                                    lblProgress.Visible = false;
                                    lblStatus.Text = "Saving...";
                                    proProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
                                    proProgress.Visible = true;

                                    tlpProgress.SetColumnSpan(proProgress, 2);
                                }));

                                var workbook = new ExcelLibrary.SpreadSheet.Workbook();
                                var worksheet = new ExcelLibrary.SpreadSheet.Worksheet(string.Format("{0} ({1:n0} Prompt{2})", collectionID, prompts.Count, ((prompts.Count == 1) ? "" : "s")));

                                int cell = 0;
                                var charCounts = new System.Collections.Generic.Dictionary<string, int>(System.StringComparer.InvariantCultureIgnoreCase)
                                {

                                    { "Author", 0 },
                                    { "Categories", 0 },
                                    { "Characters", 0 },
                                    { "Date", 0 },
                                    { "Fandoms", 0 },
                                    { "Freeform Tags", 0 },
                                    { "Prompt ID", 0 },
                                    { "Ratings", 0 },
                                    { "Relationships", 0 },
                                    { "Statuses", 0 },
                                    { "Summary", 0 },
                                    { "Tags", 0 },
                                    { "Title", 0 },
                                    { "Warnings", 0 }
                                };
                                int row = 0;

                                foreach (string name in charCounts.Keys) WriteAndMeasureCell(ref cell, name, charCounts, row, worksheet, name);

                                foreach (var prompt in prompts)
                                {

                                    cell = 0;
                                    row++;

                                    WriteAndMeasureCell(ref cell, "author", charCounts, row, worksheet, prompt.Author);

                                    WriteAndMeasureCell(ref cell, "categories", charCounts, row, worksheet, ((prompt.Categories == null) ? "" : string.Join(", ", prompt.Categories)));

                                    WriteAndMeasureCell(ref cell, "characters", charCounts, row, worksheet, ((prompt.Characters == null) ? "" : string.Join(", ", prompt.Characters)));

                                    WriteAndMeasureCell(ref cell, "date", charCounts, row, worksheet, prompt.Date.ToString("yyyy-MM-dd"));

                                    WriteAndMeasureCell(ref cell, "fandoms", charCounts, row, worksheet, ((prompt.Fandoms == null) ? "" : string.Join(", ", prompt.Fandoms)));

                                    WriteAndMeasureCell(ref cell, "freeform tags", charCounts, row, worksheet, ((prompt.FreeformTags == null) ? "" : string.Join(", ", prompt.FreeformTags)));

                                    WriteAndMeasureCell(ref cell, "prompt id", charCounts, row, worksheet, string.Format("https://archiveofourown.org/collections/{0}/prompts/{1}", collectionID, prompt.PromptID));

                                    WriteAndMeasureCell(ref cell, "ratings", charCounts, row, worksheet, ((prompt.Ratings == null) ? "" : string.Join(", ", prompt.Ratings)));

                                    WriteAndMeasureCell(ref cell, "relationships", charCounts, row, worksheet, ((prompt.Relationships == null) ? "" : string.Join(", ", prompt.Relationships)));

                                    WriteAndMeasureCell(ref cell, "statuses", charCounts, row, worksheet, ((prompt.Statuses == null) ? "" : string.Join(", ", prompt.Statuses)));

                                    WriteAndMeasureCell(ref cell, "summary", charCounts, row, worksheet, (prompt.Summary ?? ""));

                                    WriteAndMeasureCell(ref cell, "tags", charCounts, row, worksheet, ((prompt.Tags == null) ? "" : string.Join(", ", prompt.Tags)));

                                    WriteAndMeasureCell(ref cell, "title", charCounts, row, worksheet, (prompt.Title ?? ""));

                                    WriteAndMeasureCell(ref cell, "warnings", charCounts, row, worksheet, ((prompt.Warnings == null) ? "" : string.Join(", ", prompt.Warnings)));
                                }

                                cell = 0;

                                foreach (var keyValue in charCounts)
                                {

                                    worksheet.Cells.ColumnWidth[(ushort)cell] = (ushort)(keyValue.Value * 300);

                                    cell++;
                                }

                                workbook.Worksheets.Add(worksheet);

                                workbook.Save(saveToFilePath);
                            }
                            finally
                            {

                                Invoke(new System.Windows.Forms.MethodInvoker(() =>
                                {

                                    btnStartCancel.Text = "Start";

                                    tlpConfig.Visible = true;
                                }));
                            }

                            Invoke(new System.Windows.Forms.MethodInvoker(() =>
                            {

                                tlpProgress.Visible = false;

                                System.Windows.Forms.MessageBox.Show("Done!", Text, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                            }));
                        }
                        catch (System.Exception ex)
                        {

                            Invoke(new System.Windows.Forms.MethodInvoker(() =>
                            {

                                tlpProgress.Visible = false;
                            }));
                        }

                        if (cancellation == this.cancellation) this.cancellation = null;

                        cancellation.Dispose();
                    },
                    cancellation.Token
                );
            }
            else
            {

                btnStartCancel.Enabled = false;

                cancellation.Cancel();

                cancellation = null;
            }
        }

        private static string CleanWhiteSpace(string value)
        {

            value = WhiteSpaceLeadingTrailingReplaceRegex.Replace(value, "");
            value = WhiteSpacePaddingReplaceRegex.Replace(value, " ");
            value = WhiteSpacePatternReplaceRegex.Replace(value, "${_}");

            return value;
        }

        private static string[] GetFields(string groupName, System.Text.RegularExpressions.Match match)
        {

            var fields = new System.Collections.Generic.List<string>();

            for (int i = 0; i < match.Groups[groupName].Captures.Count; i++)
                foreach (string field in System.Text.RegularExpressions.Regex.Split(System.Web.HttpUtility.HtmlDecode(match.Groups[groupName].Captures[i].Value ?? ""), @",\x20*"))
                    if (!string.IsNullOrWhiteSpace(field)) fields.Add(CleanWhiteSpace(field));

            return fields.ToArray();
        }

        private static void WriteAndMeasureCell(ref int cell, string charCountName, System.Collections.Generic.Dictionary<string, int> charCounts, int row, ExcelLibrary.SpreadSheet.Worksheet worksheet, string value)
        {

            value = System.Text.RegularExpressions.Regex.Replace
            (

                (value ?? ""),
                @"\s*<br\s*\/?>\s*",
                "\r\n"
            );
            worksheet.Cells[row, cell] = new(CleanWhiteSpace(value ?? ""));

            charCounts[charCountName] = System.Math.Max(value.Length, charCounts[charCountName]);
            cell++;
        }
    }
}
