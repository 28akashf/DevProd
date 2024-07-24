using Accord.Statistics;
using Azure;
using DevProdWebApp.Models;
using DevProdWebApp.Repository;
using DevProdWebApp.Utilities;
using DevProdWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Activity = System.Diagnostics.Activity;


namespace DevProdWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProjectRepo _projectRepo;
        private readonly IMetricRepo _metricRepo;
        private readonly IDeveloperRepo _developerRepo;
        private readonly ISettingsRepo _settingsRepo;
        private readonly IToolMetricRepo _toolMetricRepo;
        private readonly IToolMetricValueRepo _toolMetricValueRepo;

        public HomeController(ILogger<HomeController> logger, IProjectRepo projectRepo, IMetricRepo metricRepo, IDeveloperRepo developerRepo, ISettingsRepo settingsRepo, IToolMetricRepo toolMetricRepo, IToolMetricValueRepo toolMetricValueRepo)
        {
            _logger = logger;
            _projectRepo = projectRepo;
            _metricRepo = metricRepo;
            _developerRepo = developerRepo;
            _settingsRepo = settingsRepo;
            _toolMetricRepo = toolMetricRepo;
            _toolMetricValueRepo = toolMetricValueRepo;
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
            var list = await _metricRepo.GetAllMetrics();
            ViewData["Projects"] = await _projectRepo.GetAllProjects();
            return View(list);
        }

        public async Task<bool> SaveSettings(string methodology, string preprocessing, string param)
        {
            string lambda = string.Empty;
            if(!string.IsNullOrEmpty(param))
            {
                var wts = JsonConvert.DeserializeObject<JObject>(param)["weights"];      
                foreach(var item in wts)
                {
                   var metric = await _toolMetricRepo.GetToolMetricByName(item["metric"].ToString().Remove(0,2));
                    metric.Weight = (double)item["weight"];
                    _toolMetricRepo.UpdateToolMetric(metric);
                }
                try
                {
                     lambda = JsonConvert.DeserializeObject<JObject>(param)["lam"].ToString();

                }
                catch (Exception)
                {


                }
            }
            Setting defaultSetting =  await _settingsRepo.GetSettingsById(1);
            if (defaultSetting == null)
            {
                await _settingsRepo.AddSettings(new Setting() { Methodolgy = methodology, Preprocessing = preprocessing, Parameters=lambda });
            }
            else
            {
                 defaultSetting.Methodolgy = methodology;
                 defaultSetting.Preprocessing = preprocessing;
                 defaultSetting.Parameters = lambda;
                _settingsRepo.UpdateSettings(defaultSetting);
            }
          
            return true;
        }

        public async Task<bool> SaveScale(string scaleObject, string table)
        {

              var metric = await _toolMetricRepo.GetToolMetricByName(table);
              metric.Scale = scaleObject;
             _toolMetricRepo.UpdateToolMetric(metric);
                 return true;
        }
        public async Task<IActionResult> Projects()
        {
            var list = await _projectRepo.GetAllProjects();
            return View(list);
        }

      

        public async Task<IActionResult> Developers()
        {
            var list = await _developerRepo.GetAllDevelopers();
            ViewData["Projects"] = await _projectRepo.GetAllProjects(); 
            return View(list);
        }

        public Dictionary<string,Dictionary<string,double>> GetMaxMinMeanStdDev(JArray arr)
        {
            Dictionary<string, List<double>> rawList = new Dictionary<string, List<double>>();
            rawList["LOC"] = new List<double>();
            rawList["Commits"] = new List<double>();
            rawList["Tasks_Completed"] = new List<double>();
            rawList["No_of_emails"] = new List<double>();
            rawList["Keystrokes_Mouseclicks"] = new List<double>();
            foreach (var item in arr)
            {
                foreach (var task in item["Data"])
                {
                    rawList["LOC"].Add((double)task["TaskData"]["LOC"]);
                    rawList["Commits"].Add((double)task["TaskData"]["Commits"]);
                    rawList["Tasks_Completed"].Add((double)task["TaskData"]["Tasks_Completed"]);
                    rawList["No_of_emails"].Add((double)task["TaskData"]["No_of_emails"]);
                    rawList["Keystrokes_Mouseclicks"].Add((double)task["TaskData"]["Keystrokes_Mouseclicks"]);
                }
            }
            Dictionary<string, Dictionary<string, double>> metrics = new Dictionary<string, Dictionary<string, double>>();
            foreach (var item in rawList.Keys)
            {
                metrics[item] = new Dictionary<string, double>();
                metrics[item].Add("Min", rawList[item].Min());
                metrics[item].Add("Max", rawList[item].Max());
                metrics[item].Add("Mean", rawList[item].Average());
                metrics[item].Add("Stddev", Math.Sqrt(rawList[item].Average(v => Math.Pow(v - rawList[item].Average(), 2))));
            }
  
                return metrics;
        }

        public JArray MinMaxNormalizeRawData (JArray arr)
        {
             var dict = GetMaxMinMeanStdDev(arr);
            foreach (var item in arr)
            {
                foreach (var task in item["Data"])
                {
                    task["TaskData"]["LOC"] = MinMaxNormalize((double)task["TaskData"]["LOC"], dict["LOC"]["Min"], dict["LOC"]["Max"]);
                    task["TaskData"]["Commits"] = MinMaxNormalize((double)task["TaskData"]["Commits"], dict["Commits"]["Min"], dict["Commits"]["Max"]);
                    task["TaskData"]["Tasks_Completed"] = MinMaxNormalize((double)task["TaskData"]["Tasks_Completed"], dict["Tasks_Completed"]["Min"], dict["Tasks_Completed"]["Max"]);
                    task["TaskData"]["No_of_emails"] = MinMaxNormalize((double)task["TaskData"]["No_of_emails"], dict["No_of_emails"]["Min"], dict["No_of_emails"]["Max"]);
                    task["TaskData"]["Keystrokes_Mouseclicks"] = MinMaxNormalize((double)task["TaskData"]["Keystrokes_Mouseclicks"], dict["Keystrokes_Mouseclicks"]["Min"], dict["Keystrokes_Mouseclicks"]["Max"]);
                }

            }
            return arr;
        }

        public JArray ZScoreNormalizeRawData(JArray arr)
        {
            var dict = GetMaxMinMeanStdDev(arr);
            foreach (var item in arr)
            {
                foreach (var task in item["Data"])
                {
                    task["TaskData"]["LOC"] = ZScoreNormalize((double)task["TaskData"]["LOC"], dict["LOC"]["Mean"], dict["LOC"]["Stddev"]);
                    task["TaskData"]["Commits"] = ZScoreNormalize((double)task["TaskData"]["Commits"], dict["Commits"]["Mean"], dict["Commits"]["Stddev"]);
                    task["TaskData"]["Tasks_Completed"] = ZScoreNormalize((double)task["TaskData"]["Tasks_Completed"], dict["Tasks_Completed"]["Mean"], dict["Tasks_Completed"]["Stddev"]);
                    task["TaskData"]["No_of_emails"] = ZScoreNormalize((double)task["TaskData"]["No_of_emails"], dict["No_of_emails"]["Mean"], dict["No_of_emails"]["Stddev"]);
                    task["TaskData"]["Keystrokes_Mouseclicks"] = ZScoreNormalize((double)task["TaskData"]["Keystrokes_Mouseclicks"], dict["Keystrokes_Mouseclicks"]["Mean"], dict["Keystrokes_Mouseclicks"]["Stddev"]);
                }

            }
            return arr;
        }

        public Dictionary<string, List<ToolMetricValue>> MinMaxNormalizeRawData(Dictionary<string,List<ToolMetricValue>> dictionaryRaw)
        {
            Dictionary<string, List<ToolMetricValue>> dictionaryProcessed = new Dictionary<string, List<ToolMetricValue>>();
           
            foreach (var item in dictionaryRaw.Keys)
            {
               var arrCast = dictionaryRaw[item].ToList();
                
                var min = arrCast.Select(x=>Double.Parse(x.Value)).Min();
                var max = arrCast.Select(x => Double.Parse(x.Value)).Max();
                List<ToolMetricValue> arrCastCopy = new List<ToolMetricValue>();
                for (int i = 0; i < arrCast.Count; i++)
                {
                    arrCastCopy.Add(new ToolMetricValue() { Id = arrCast[i].Id, ToolMetric = arrCast[i].ToolMetric, ToolMetricId = arrCast[i].ToolMetricId, Value = arrCast[i].Value });
                    arrCastCopy[i].Value =  MinMaxNormalize(Double.Parse(arrCast[i].Value), min, max).ToString();
                }
                dictionaryProcessed.Add(item, arrCastCopy);
            }
            return dictionaryProcessed;
        }

        public Dictionary<string, List<ToolMetricValue>> ZScoreNormalizeRawData(Dictionary<string, List<ToolMetricValue>> dictionaryRaw)
        {
            Dictionary<string, List<ToolMetricValue>> dictionaryProcessed = new Dictionary<string, List<ToolMetricValue>>();
            foreach (var item in dictionaryRaw.Keys)
            {
                var arrCast = dictionaryRaw[item].ToList();
                var mean = Measures.Mean(arrCast.Select(x => Double.Parse(x.Value)).ToArray());
                var stdDev = Measures.StandardDeviation(arrCast.Select(x => Double.Parse(x.Value)).ToArray());
                List<ToolMetricValue> arrCastCopy = new List<ToolMetricValue>();
                for (int i = 0; i < arrCast.Count; i++)
                {
                    arrCastCopy.Add(new ToolMetricValue() { Id = arrCast[i].Id, ToolMetric = arrCast[i].ToolMetric, ToolMetricId = arrCast[i].ToolMetricId, Value = arrCast[i].Value });
                    arrCastCopy[i].Value = ZScoreNormalize(Double.Parse(arrCastCopy[i].Value), mean, stdDev).ToString();
                }
                dictionaryProcessed.Add(item, arrCastCopy);
            }
            return dictionaryProcessed; 
            
        }
        public JArray LoadJson(string preprocs)
        {
                      
               using (StreamReader r = new StreamReader("Data/Source.json"))
               {
                string json = r.ReadToEnd();
                JArray array = JsonConvert.DeserializeObject<JArray>(json);
                if (!string.IsNullOrEmpty(preprocs))
                {
                    switch(preprocs)
                    {
                        case "minmax":
                            MinMaxNormalizeRawData(array);
                            break;
                        case "zscore":
                            ZScoreNormalizeRawData(array);
                            break;
                        default:
                            break;

                    }
                }
                    return array;
                }
            
        }

        public static double ZScoreNormalize(double value, double mean, double stdDev)
        {
            double normalizedValue = (value - mean) / stdDev;
            return normalizedValue;
        }
        public static double MinMaxNormalize(double value, double min, double max )
        {           
            double normalizedValue = (value - min) / (max - min);
            return normalizedValue;
        }

        public async Task<IActionResult> ToolDashboard()
        {

            //var binaryList = new List<int>(Initializer.Binary);
            //var continuousList = new List<double>(Initializer.Continuous);
            //var discreteList = new List<int>(Initializer.Discrete);
            //ToolMetric tm1 = await _toolMetricRepo.AddToolMetric(new ToolMetric() { Name = "m1", Weight = 0.3, SettingId = 1 });
            //ToolMetric tm2 = await _toolMetricRepo.AddToolMetric(new ToolMetric() { Name = "m2", Weight = 0.4, SettingId = 1 });
            //ToolMetric tm3 = await _toolMetricRepo.AddToolMetric(new ToolMetric() { Name = "m3", Weight = 0.3, SettingId = 1 });
            //foreach (var item in binaryList)
            //{
            //    await _toolMetricValueRepo.AddToolMetricValue(new ToolMetricValue() { ToolMetricId = 2, Value = item.ToString() });
            //}
            //foreach (var item in continuousList)
            //{
            //    await _toolMetricValueRepo.AddToolMetricValue(new ToolMetricValue() { ToolMetricId = 3, Value = item.ToString() });
            //}
            //foreach (var item in discreteList)
            //{
            //    await _toolMetricValueRepo.AddToolMetricValue(new ToolMetricValue() { ToolMetricId = 4, Value = item.ToString() });
            //}
            var allMetrics = await _toolMetricRepo.GetAllToolMetrics();
            Dictionary<string, List<ToolMetricValue>> metricDictionary = new Dictionary<string, List<ToolMetricValue>>();
    
            List<int> listCount = new List<int>();
            foreach(var metric in allMetrics)
            {
                var metricList = await _toolMetricValueRepo.GetToolMetricValuesByMetricId(metric.Id);
                listCount.Add(metricList.Count);   
                metricDictionary.Add(metric.Name,metricList);               
            }
            MList list = new MList();
            list.metricDictionary = metricDictionary;
            list.maxCount = listCount.Max();

            //list.maxCount = Math.Max(list.m1List.Count, Math.Max(list.m2List.Count, list.m3List.Count));
            var settings =  await _settingsRepo.GetSettingsById(1);
            //PREPROCESSING
            switch(settings.Preprocessing)
            {
                case "minmax":
                 list.metricProcDictionary =  MinMaxNormalizeRawData(metricDictionary);
                    break;
                case "zscore":
                    list.metricProcDictionary = ZScoreNormalizeRawData(metricDictionary);
                    break;
                default:
                    break;
            }
            //SCALING

            //METHODOLOGY

             //STEP 1:


            //STEP 2:
            double result = 0;
            //switch (settings.Methodolgy)
            //{               
            //    case "sum":
            //        result = (list.metricProcDictionary.Values.Select(x=>x.Select(z=>z.Value).Average()).Average() + list.m2ListProc.Average() + list.m3ListProc.Average())/3;
            //        break;
            //    case "wtsum":
            //        var weights = JsonConvert.DeserializeObject<JObject>(settings.Parameters);
            //        result = list.m1ListProc.Average() * (double)weights["wtm1"] + list.m2ListProc.Average() * (double)weights["wtm2"] + list.m3ListProc.Average() * (double)weights["wtm3"];
            //        break;
            //    case "wtprod":
            //        weights = JsonConvert.DeserializeObject<JObject>(settings.Parameters);
            //        result = Math.Pow(list.m1ListProc.Average(),(double)weights["wtm1"]) * Math.Pow(list.m2ListProc.Average(), (double)weights["wtm2"]) * Math.Pow(list.m3ListProc.Average(), (double)weights["wtm3"]);
            //        break;
            //    case "wtwaspas":
            //        weights = JsonConvert.DeserializeObject<JObject>(settings.Parameters);
            //        var wtsum = list.m1ListProc.Average() * (double)weights["wtm1"] + list.m2ListProc.Average() * (double)weights["wtm2"] + list.m3ListProc.Average() * (double)weights["wtm3"];
            //        var wtprod = Math.Pow(list.m1ListProc.Average(), (double)weights["wtm1"]) * Math.Pow(list.m2ListProc.Average(), (double)weights["wtm2"]) * Math.Pow(list.m3ListProc.Average(), (double)weights["wtm3"]);
            //        result = ((double)weights["lam"] * wtsum) + ((1 - (double)weights["lam"]) * wtprod);
            //        break;
            //    case "gmean":
            //        double prod = (list.m1ListProc.Average() * list.m2ListProc.Average() * list.m3ListProc.Average());                
            //        result = Math.Pow(prod, (1.0 / 3));                    
            //        break;
            //    default:
            //        break;
            //}
            list.score = Math.Round(result,2);

            return View(list);
        }

        public async Task<IActionResult> ToolSettings()
        {
            var settings = await _settingsRepo.GetSettingsWithMetricsById(1);
            SettingsViewModel vm = new SettingsViewModel();
            vm.Methodolgy = settings.Methodolgy;
            vm.Preprocessing = settings.Preprocessing;
            try
            {
            vm.Parameters = settings.Parameters;
            vm.ToolMetricList = settings.ToolMetricList;
            vm.ToolMetricScaleList = new List<MetricScale>();
            foreach(var metric in settings.ToolMetricList)
            {
                    List<ScaleObject> scaleObjList = new List<ScaleObject>();
                    try
                    {
                        scaleObjList = JsonConvert.DeserializeObject<List<ScaleObject>>(metric.Scale);
                    }
                    catch (Exception)
                    {

                       
                    }
                vm.ToolMetricScaleList.Add(new MetricScale() { MetricName=metric.Name,ScaleObjects= scaleObjList });
            }
           
               // vm.Parameters = JsonConvert.DeserializeObject<JObject>(settings.Parameters);
            }
            catch (Exception)
            {

            }
            try
            {
               // vm.ScaleM1 = JsonConvert.DeserializeObject<JArray>(JsonConvert.DeserializeObject<Dictionary<string, string>>(settings.Scale)["tblm1"]);
            }
            catch (Exception)
            {
            }
            try
            {
              //  vm.ScaleM2 = JsonConvert.DeserializeObject<JArray>(JsonConvert.DeserializeObject<Dictionary<string, string>>(settings.Scale)["tblm2"]);
            }
            catch (Exception)
            {

               
            }
            try
            {
                //vm.ScaleM3 = JsonConvert.DeserializeObject<JArray>(JsonConvert.DeserializeObject<Dictionary<string, string>>(settings.Scale)["tblm3"]);
            }
            catch (Exception)
            {

            }
            return View(vm);
        }
            public IActionResult Dashboard()
        {   
            JArray array =  LoadJson(string.Empty);
            List<string> developers = new List<string>();
            foreach (var item in array)
            {
                developers.Add(item["Developer"].ToString());
            }
            ViewData["Developers"] = developers;
            return View();
        }

        public IActionResult CalculateProductivity(string developer, string method, string preproc, string weights)
        {
            double? result = 0;
            JArray array = LoadJson(preproc);
            int taskCount = 0;
            foreach (var item in array)
            {
                if(item["Developer"].ToString() == developer)
                {
                    taskCount = item["Data"].Count();
                    foreach (var task in item["Data"])
                    {
                        var metricDictionary = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(task["TaskData"].ToString());
                        double? n = metricDictionary?.Values.Count;
                        switch (method)
                        {
                            case "sum":
                                var sum = metricDictionary?.Values.Sum(x => Convert.ToDouble(x));
                                result += sum;
                                break;
                            case "wtsum":
                                var Jweight = JsonConvert.DeserializeObject<JObject>(weights);
                                double wtsum = 0.0;
                              foreach(var key in metricDictionary?.Keys)
                                {
                                    wtsum += ((double)metricDictionary[key] * (double)Jweight[key]);
                                }
                                result += wtsum;
                                break;
                            case "wtamean":
                                 Jweight = JsonConvert.DeserializeObject<JObject>(weights);
                                double wtamean = 0.0;
                                foreach (var key in metricDictionary?.Keys)
                                {
                                    wtamean += ((double)metricDictionary[key] * (double)Jweight[key]);
                                }
                               double sumOfWeights =  Jweight.ToObject<Dictionary<string, double>>().Values.Sum();
                                result += (wtamean / sumOfWeights);
                                break;
                            case "wtprodmodel":
                                Jweight = JsonConvert.DeserializeObject<JObject>(weights);
                                double wtprodmodel = 1.0;
                                result = 1.0;
                                foreach (var key in metricDictionary?.Keys)
                                {
                                    wtprodmodel *= Math.Pow((double)metricDictionary[key],(double)Jweight[key]);
                                }
                               
                                result *= wtprodmodel;
                                break;
                            case "gmean":
                                double prod = 1;

                             foreach(var x in metricDictionary?.Values.ToList())
                                {
                                    prod *= Convert.ToDouble(x);
                                }
                                var gmean = Math.Pow(prod,(1.0/n.Value));
;                                result += gmean;
                                break;
                            case "hmean":
                                double reciprocalsum = 0;
                                foreach (var x in metricDictionary?.Values.ToList())
                                {
                                    reciprocalsum += (1/Convert.ToDouble(x));
                                }
                                var hmean = (n.Value / reciprocalsum);
                                result += hmean;
                                break;
                            case "amean":
                                var amean = metricDictionary?.Values.Sum(x => Convert.ToDouble(x));
                                result += (amean/n);
                                break;
                            case "median":
                                var list = metricDictionary?.Values.ToList();
                                list = list.OrderBy(x=>(int)x).ToList();
                                var median =  (n%2==0)? ((int)list.ElementAt((int)n/2) + (int)list.ElementAt(((int)n/2)+1))/2 : (int)list.ElementAt(((int)n/2));
                                result += median;
                                break;
                            case "mode":
                                list = metricDictionary?.Values.ToList();
                                Dictionary<string, int> occurrence = new Dictionary<string, int>();
                                foreach(var i in list)
                                {
                                    if(occurrence.ContainsKey(i.ToString()))
                                    {
                                        occurrence[i.ToString()] = occurrence[i.ToString()] + 1;
                                    }
                                    else
                                    {
                                        occurrence[i.ToString()] = 1;
                                    }
                                }
                                result = occurrence.Values.Max(x=>x);
                                break;
                            default: 
                                break;
                        }
                      
                     
                    }
                }
                
            }
            Productivity z = new Productivity();
            z.Score = result.Value/ taskCount;
           z.Method = method;
            z.Developer = developer;
            return PartialView("PartialViewDashboard",z);   
        }
        #region crud
        public async Task<bool> AddProject(string name, string description)
        {
           await _projectRepo.AddProject(new Models.Project() { Name=name,Description=description});
            //   var list = await _projectRepo.GetAllProjects();
            //   return View("./Projects",list);
            return true;
        }

        public async Task<bool> AddDeveloper(string fname, string lname, string uname, int projectId)
        {
            
           var dev = await _developerRepo.AddDeveloper(new Models.Developer() { FirstName=fname,LastName=lname,Username=uname });
            Models.Project? project = await _projectRepo.GetProjectById(projectId);
            if (project?.Developers == null)
            {
                project.Developers = new List<Developer>();
            }
                project.Developers.Add(dev);
            
            _projectRepo.UpdateProject(project);

            return true;
        }

        public async Task<bool> AddMetrics(string metric, string weight, int projectId)
        {
            var newMetric = await _metricRepo.AddMetric(new Models.Metric() { Name = metric, Weight = Double.Parse(weight) });
            Models.Project? project = await _projectRepo.GetProjectById(projectId);
            if (project?.ProjectMetrics == null)
            {
                project.ProjectMetrics = new List<Models.Metric>();
            }
            project.ProjectMetrics.Add(newMetric);
            _projectRepo.UpdateProject(project);
            newMetric.ProjectName = project.Name;
            _metricRepo.UpdateMetric(newMetric);
            return true;
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region PerfromanceMetrics
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
            a += ($"Total lines added: {totalLinesAdded}"+"\n");
            a += ($"Total lines deleted: {totalLinesDeleted}" + "\n");
            a += ($"Net lines added: {totalLinesAdded - totalLinesDeleted}" + "\n");
            Result result = new Result() { Data = a };
            return View("./Index", result);
        }




        //GET ALL TASKS FROM THE TRELLO BOARD
        public async Task<IActionResult> GetTaskList(string username, string project)
        {
            string a = string.Empty;
            var client = new TrelloDotNet.TrelloClient("a9fd64c9429e1577e6e7b973d2df303f", "ATTA912c6c3cc630adc321a7f668f8883cef1ea49f3df685aa97ff957be4633e7fac670CD8C5");
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
               a+= new string($"- [{project}] #{issue.Number}: {issue.Title}" + "\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching issues: {ex.Message}");
        }
            Result result = new Result() { Data= a};
            return View("./Index",result);
        }

        #endregion
    }
}
