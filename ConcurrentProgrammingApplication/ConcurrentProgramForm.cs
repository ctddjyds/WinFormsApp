using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConcurrentProgrammingApplication
{
    public partial class ConcurrentProgramForm : Form
    {
        public ConcurrentProgramForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// simple Demo Method use Delay
        /// Task.Delay 适合用于对异步代码进行单元测试或者实现重试逻辑。
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        static async Task<string> DownloadStringWithRetries(string uri)
        {
            using (var client = new HttpClient())
            {
                // 第1次重试前等1秒，第2次等2秒，第3次等4秒。  
                var nextDelay = TimeSpan.FromSeconds(1);
                for (int i = 0; i != 3; ++i)
                {
                    try
                    {
                        return await client.GetStringAsync(uri);
                    }
                    catch
                    {
                    }
                    await Task.Delay(nextDelay);
                    nextDelay = nextDelay + nextDelay;
                }
                // 最后重试一次，以便让调用者知道出错信息。  
                return await client.GetStringAsync(uri);
            }
        }
        static async Task<string> DownloadStringWithTimeout(string uri)
        {
            using (var client = new HttpClient())
            {
                var downloadTask = client.GetStringAsync(uri);
                var timeoutTask = Task.Delay(3000);

                var completedTask = await Task.WhenAny(downloadTask, timeoutTask);
                if (completedTask == timeoutTask)
                    return null;
                return await downloadTask;
            }
        }
        /// <summary>
        /// 进度报告
        /// 最好把T定义为一个不可变类型，或者至少是值类型。如果T是一个可变的引用类型，就必须在每次调用IProgress<T>.Report时，创建一个单独的副本。
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        async Task MyMethodAsync(IProgress<double> progress = null)
        {
            double percentComplete = 0;
            for(int i=1;i<=100;i++)
            {
                await Task.Delay(1000);
                percentComplete = i;
                if (progress != null)
                        progress.Report(percentComplete);
            }
        }
        //Progress<T> 会在创建时捕获当前上下文，并且在这个上下文中调用回调函数。这意味着，
        //如果在 UI 线程中创建了 Progress<T>，就能在 Progress<T> 的回调函数中更新 UI，即使异
        //步方法是在后台线程中调用 Report 的。
        public async Task CallMyMethodAsync()
        {
            var progress = new Progress<double>();
            progress.ProgressChanged += (sender, args) =>
            {
                progressBar.Value = (int)args;
            };
            await MyMethodAsync(progress);
        }

        private async void btnReport_Click(object sender, EventArgs e)
        {
            this.btnReport.Enabled = false;
            await CallMyMethodAsync();
            this.btnReport.Enabled = true;
        }
        /// <summary>
        /// Task.WhenAll方法等待任务全部完成，
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        static async Task<string> DownloadAllAsync(IEnumerable<string> urls)
        {
            var httpClient = new HttpClient();
            // 定义每一个 url 的使用方法。  
            var downloads = urls.Select(url => httpClient.GetStringAsync(url));
            // 注意，到这里，序列还没有求值，所以所有任务都还没真正启动。  
            // 下面，所有的 URL 下载同步开始。  
            Task<string>[] downloadTasks = downloads.ToArray();
            // 到这里，所有的任务已经开始执行了。  
            // 用异步方式等待所有下载完成。  
            string[] htmlPages = await Task.WhenAll(downloadTasks);
            return string.Concat(htmlPages);
        }
    }
}
