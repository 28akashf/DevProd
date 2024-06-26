using DevProdWebApp.Models;
using DevProdWebApp.Repository;
using DevProdWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using System.Diagnostics;
using System.Net;
using Activity = System.Diagnostics.Activity;

namespace DevProdWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProjectRepo _projectRepo;
        private readonly IMetricRepo _metricRepo;
        private readonly IDeveloperRepo _developerRepo;

        public HomeController(ILogger<HomeController> logger, IProjectRepo projectRepo, IMetricRepo metricRepo, IDeveloperRepo developerRepo)
        {
            _logger = logger;
            _projectRepo = projectRepo;
            _metricRepo = metricRepo;
            _developerRepo = developerRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public string Test(string param)
        {
            return "Testing....!"+param;
        }

        public async Task<IActionResult> Settings()
        {
            //var list = new MetricList();
            //list.Id = 1;
            //list.Project = "DevProd";
            //list.MetricSet = new List<ViewModels.Metric>() { new ViewModels.Metric() {Id=156,Name="Commits",Weight=0.2 }, new ViewModels.Metric() { Id = 213, Name = "LOC", Weight = 0.3 } };
            var list = await _metricRepo.GetAllMetrics();
            return View(list);
        }

        public async Task<IActionResult> Projects()
        {
            var list = await _projectRepo.GetAllProjects();
            return View(list);
        }

        public async Task<IActionResult> Developers()
        {
            var list = await _developerRepo.GetAllDevelopers();
            return View(list);
        }
        public async Task<bool> AddProject(string name, string description)
        {
           await _projectRepo.AddProject(new Models.Project() { Name=name,Description=description});
            //   var list = await _projectRepo.GetAllProjects();
            //   return View("./Projects",list);
            return true;
        }

        public async Task<bool> AddDeveloper(string fname, string lname, string uname)
        {
            await _developerRepo.AddDeveloper(new Models.Developer() { FirstName=fname,LastName=lname,Username=uname });          
            return true;
        }

        public async Task<bool> AddMetrics(string metric, string weight)
        {
            await _metricRepo.AddMetric(new Models.Metric() { Name = metric, Weight = Double.Parse(weight) });
            return true;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //GET COMMITS
        public IActionResult GetCommits(string username, string project)
        {
            username = "28akashf";
            project = "Meet";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var github = new GitHubClient(new Octokit.ProductHeaderValue(project));

            // You can also use credentials
            //var github = new GitHubClient(new Octokit.ProductHeaderValue("Meet"));
            // var tokenAuth = new Credentials("your-github-token");
            // github.Credentials = tokenAuth;

            var commits = github.Repository.Commit.GetAll(username, project,
                 new CommitRequest
                 {
                     Sha = "main", // Retrieve commits from a specific branch
                     Since = DateTime.Now.AddDays(-777) // Retrieve commits from the last 7 days                                        
                 },
                 new ApiOptions { PageSize = 100, PageCount = 1 }
         
                ).Result;
            string a = "";
            foreach (var commit in commits)
            {
               a += commit.Commit.Message + "\n";
            }
            Result result = new Result() { Data = a };
            return View("./Index", result);
        }





        //GET LINES OF CODE COMMITTED
        public IActionResult GetLinesOfCodeFromCommits(string username, string project)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var github = new GitHubClient(new Octokit.ProductHeaderValue("Meet"));
            //  var tokenAuth = new Credentials("your-github-token");
            // github.Credentials = tokenAuth;

            var owner = "28akashf";
            var repoName = "Meet";
            var startDate = new DateTime(2022, 1, 1);
            var endDate = new DateTime(2023, 12, 31);

            var request = new CommitRequest
            {
                Since = startDate,
                Until = endDate
            };

            var commits = github.Repository.Commit.GetAll(owner, repoName, request).Result;

            int totalLinesAdded = 0;
            int totalLinesDeleted = 0;

            foreach (var commit in commits)
            {
                //var zz = github.Repository.Commit.Get(owner, repoName, commit.Sha).Result;
                var commitStats = github.Repository.Commit.Get(owner, repoName, commit.Sha).Result.Stats;
                if (commitStats != null)
                {
                    totalLinesAdded += commitStats.Additions;
                    totalLinesDeleted += commitStats.Deletions;
                }
            }
            string a = string.Empty;
            a += ($"Total lines added: {totalLinesAdded}");
            a += ($"Total lines deleted: {totalLinesDeleted}");
            a += ($"Net lines added: {totalLinesAdded - totalLinesDeleted}");
            Result result = new Result() { Data = a };
            return View("./Index", result);
        }




        //GET ALL TASKS FROM THE TRELLO BOARD
        public async Task<IActionResult> GetTaskList(string username, string project)
        {
            string a = string.Empty;
            var client = new TrelloDotNet.TrelloClient("", "");
            var lists =  await client.GetListsOnBoardAsync("6dzGyuJX");
            lists = lists.Where(x=>x.Name.ToLower() == "completed").ToList();
            foreach (var list in lists)
            {
               var card =  await client.GetCardsInListAsync(list.Id);
                card.ForEach(x => a += x.Name+"\n");
            }           
            Result result = new Result() { Data = a };
            return View("./Index", result);
        }




        //GET TOTAL LINES OF CODE
        // var token = "your-github-token";
        //var owner = "28akashf";
        //var repoName = "Meet";
        //var startDate = new DateTime(2022, 1, 1);
        //var endDate = new DateTime(2023, 12, 31);

        //var graphQLHttpClient = new GraphQLHttpClient("https://api.github.com/graphql", new NewtonsoftJsonSerializer());
        //// graphQLHttpClient.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //var request = new GraphQLHttpRequest
        //{
        //    Query = @"
        //                query($owner: String!, $name: String!, $since: GitTimestamp!, $until: GitTimestamp!) {
        //                    repository(owner: $owner, name: $name) {
        //                        object(expression: ""main"") {
        //                            ... on Commit {
        //                                history(since: $since, until: $until) {
        //                                    nodes {
        //                                        additions
        //                                        deletions
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }",
        //    Variables = new
        //    {
        //        owner,
        //        name = repoName,
        //        since = startDate.ToString("o"),
        //        until = endDate.ToString("o")
        //    }
        //};

        //var response = Task.Run(() => graphQLHttpClient.SendQueryAsync<object>(request));
        //int totalLinesAdded = 0;
        //int totalLinesDeleted = 0;

        //dynamic data = response.Result.Data;
        //foreach (var node in data.Repository.Object.History.Nodes)
        //{
        //    totalLinesAdded += node.additions;
        //    totalLinesDeleted += node.deletions;
        //}

        //Console.WriteLine($"Total lines added: {totalLinesAdded}");
        //Console.WriteLine($"Total lines deleted: {totalLinesDeleted}");
        //Console.WriteLine($"Net lines added: {totalLinesAdded - totalLinesDeleted}");


        //OUTLOOK SENT MAIL
        //public void GetMail() { 
        //Microsoft.Office.Interop.Outlook.Application outlookApplication = null;
        //NameSpace outlookNamespace = null;
        //MAPIFolder sentMailFolder = null;

        //try
        //{
        //    outlookApplication = new Microsoft.Office.Interop.Outlook.Application();
        //    outlookNamespace = outlookApplication.GetNamespace("MAPI");
        //    sentMailFolder = outlookNamespace.GetDefaultFolder(OlDefaultFolders.olFolderSentMail);

        //    Console.WriteLine("Number of emails sent: " + sentMailFolder.Items.Count);
        //    foreach (object item in sentMailFolder.Items)
        //    {
        //        MailItem mailItem = item as MailItem;

        //        if (mailItem != null)
        //        {
        //            string body = mailItem.Body;
        //            string[] words = body.Split(' ');
        //            int wordCount = words.Length;

        //            Console.WriteLine("Email Subject: " + mailItem.Subject);
        //            Console.WriteLine("Number of words: " + wordCount);
        //            Console.WriteLine();
        //        }
        //    }
        //}
        //catch (System.Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}
        //finally
        //{
        //        //Dispose
        //    if (sentMailFolder != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(sentMailFolder);
        //    if (outlookNamespace != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(outlookNamespace);
        //    if (outlookApplication != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(outlookApplication);
        //}
        //}
        ////OUTLOOK SEND MAIL
        //public void GetFilteredMail()
        //{ 
        //Microsoft.Office.Interop.Outlook.Application outlookApplication = null;
        //NameSpace outlookNamespace = null;
        //MAPIFolder inboxFolder = null;

        //try
        //{
        //    outlookApplication = new Microsoft.Office.Interop.Outlook.Application();
        //    outlookNamespace = outlookApplication.GetNamespace("MAPI");
        //    inboxFolder = outlookNamespace.GetDefaultFolder(OlDefaultFolders.olFolderInbox);

        //    DateTime startDate = new DateTime(2022, 1, 1); // Start of the duration
        //    DateTime endDate = new DateTime(2022, 1, 31); // End of the duration

        //    Items restrictedItems = inboxFolder.Items.Restrict("[ReceivedTime] >= '" + startDate.ToString("g") + "' AND [ReceivedTime] <= '" + endDate.ToString("g") + "'");
        //    int emailCount = restrictedItems.Count;

        //    Console.WriteLine("Number of emails received between " + startDate.ToString("d") + " and " + endDate.ToString("d") + ": " + emailCount);
        //}
        //catch (System.Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}
        //finally
        //{
        //    //Dispose
        //    if (inboxFolder != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(inboxFolder);
        //    if (outlookNamespace != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(outlookNamespace);
        //    if (outlookApplication != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(outlookApplication);
        //}
        //}


        //MS TEAMS CHAT
        //static async Task GetChatMessages()
        //{
        //    string clientId = "your_client_id";
        //    string clientSecret = "your_client_secret";
        //    string tenantId = "your_tenant_id";
        //    string chatId = "your_chat_id";
        //    string startDate = "2022-01-01T00:00:00.000Z"; // Start of the duration
        //    string endDate = "2022-01-31T23:59:59.999Z"; // End of the duration

        //    string accessToken = await GetAccessToken(clientId, clientSecret, tenantId);

        //    HttpClient httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //    string requestUrl = $"https://graph.microsoft.com/v1.0/chats/{chatId}/messages?$filter=createdDateTime ge {startDate} and createdDateTime le {endDate}";

        //    HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string responseBody = await response.Content.ReadAsStringAsync();
        //        dynamic jsonData = JsonConvert.DeserializeObject(responseBody);

        //        int messageCount = jsonData.value.Count;

        //        Console.WriteLine("Number of chat messages between " + startDate + " and " + endDate + ": " + messageCount);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Failed to get chat messages");
        //    }
        //}

        //static async Task<string> GetAccessToken(string clientId, string clientSecret, string tenantId)
        //{
        //    string requestUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

        //    HttpClient httpClient = new HttpClient();

        //    var requestData = new[]
        //    {
        //            new KeyValuePair<string, string>("grant_type", "client_credentials"),
        //            new KeyValuePair<string, string>("client_id", clientId),
        //            new KeyValuePair<string, string>("client_secret", clientSecret),
        //            new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default")
        //        };

        //    HttpResponseMessage response = await httpClient.PostAsync(requestUrl, new FormUrlEncodedContent(requestData));

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string responseBody = await response.Content.ReadAsStringAsync();
        //        dynamic jsonData = JsonConvert.DeserializeObject(responseBody);

        //        return jsonData.access_token;
        //    }
        //    else
        //    {
        //        throw new System.Exception("Failed to get access token");
        //    }
        //}


        public async Task<IActionResult> GetBugs()
        {
            string a = string.Empty;
            // Replace with your GitHub personal access token
            //      var token = "YOUR_PERSONAL_ACCESS_TOKEN";

            // GitHub username
            //  var username = "TARGET_USERNAME";

            // Initialize GitHub client with authentication
            //var client = new GitHubClient(new ProductHeaderValue("GitHubIssueFetcher"))
            //{
            //    Credentials = new Credentials(token)
            //};
         var username = "28akashf";
         var project = "DevProd";
         ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var github = new GitHubClient(new Octokit.ProductHeaderValue(project));
            try
        {
                // Fetch issues assigned to the specified user
                //var issues = await github.Issue.GetAllForCurrent(new RepositoryIssueRequest
                //{
                //    Assignee = username,
                //    Filter = IssueFilter.Assigned,
                //    State = ItemStateFilter.All
                //});

                var issues = await github.Issue.GetAllForRepository(username,project);

                Console.WriteLine($"Issues assigned to {username}:");
                
            foreach (var issue in issues)
            {
               a+= new string($"- [{project}] #{issue.Number}: {issue.Title}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching issues: {ex.Message}");
        }
            Result result = new Result() { Data= a};
            return View("./Index",result);
        }
    }
}
