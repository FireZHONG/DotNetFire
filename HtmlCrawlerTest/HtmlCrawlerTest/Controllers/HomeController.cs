using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HtmlCrawlerTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public string Login()
        {
            var login = "https://github.com/login";
            var loginContent = (HttpWebRequest)WebRequest.Create(login);
            loginContent.Method = "GET";
            loginContent.AllowAutoRedirect = false;//服务端重定向。一般设置false
            loginContent.ContentType = "text/html; charset=utf-8";
            var loginResponse = (HttpWebResponse)loginContent.GetResponse();
            var headers = loginResponse.Headers;
            var loginCookies = loginResponse.Headers.Get("Set-Cookie");
            var loginHtml = new StreamReader(loginResponse.GetResponseStream()).ReadToEnd();
            var loginDoc = new HtmlDocument();
            loginDoc.LoadHtml(loginHtml);
            var loginNode = loginDoc.DocumentNode.SelectSingleNode("//form[@action='/session']");
            var authenticity_token = loginNode.SelectSingleNode("//input[@name='authenticity_token']").Attributes["value"].Value;
            var timestamp = loginNode.SelectSingleNode("//input[@name='timestamp']").Attributes["value"].Value;
            var timestamp_secret = loginNode.SelectSingleNode("//input[@name='timestamp_secret']").Attributes["value"].Value;

            var url = "https://github.com/session";
            var postData = "{'login': '924881152@qq.com','password':'swanyang199219','authenticity_token':'" + authenticity_token + "','commit':'Sign in','utf8':'✓','ga_id':'59346229.1570425759','timestamp':'" + timestamp + "','timestamp_secret':'" + timestamp_secret + "','webauthn-support':'supported','webauthn-iuvpaa-support':'unsupported'}";
            // 1.获取登录Cookie
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST"; // POST OR GET， 如果是GET, 则没有第二步传参，直接第三步，获取服务端返回的数据
            req.AllowAutoRedirect = false; //服务端重定向。设置false
            req.ContentType = "application/x-www-form-urlencoded";
            req.Headers = headers;

            var postBytes = Encoding.UTF8.GetBytes(postData);
            req.ContentLength = postBytes.Length;
            var postDataStream = req.GetRequestStream();
            postDataStream.Write(postBytes, 0, postBytes.Length);
            postDataStream.Close();
            req.CookieContainer = new CookieContainer();
            req.CookieContainer.SetCookies(req.RequestUri, loginCookies);//将登录的cookie值赋予此次的请求。

            var res = (HttpWebResponse)req.GetResponse();
            var cookies = res.Headers.Get("Set-Cookie");

            // 2.进入页面
            var contentUrl = "https://github.com/";
            var reqContent = (HttpWebRequest)WebRequest.Create(contentUrl);
            reqContent.Method = "GET";
            reqContent.AllowAutoRedirect = false;//服务端重定向。一般设置false
            reqContent.ContentType = "text/html; charset=utf-8";//数据一般设置这个值，除非是文件上传

            reqContent.CookieContainer = new CookieContainer();
            reqContent.CookieContainer.SetCookies(reqContent.RequestUri, cookies);//将登录的cookie值赋予此次的请求。

            var respContent = (HttpWebResponse)reqContent.GetResponse();
            string html = new StreamReader(respContent.GetResponseStream()).ReadToEnd();

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var nodes = doc.DocumentNode.SelectNodes("//head/meta[@name='octolytics-actor-login']");
            var text = nodes.FirstOrDefault().Attributes["content"].Value;
            return text;

        }

        public string test()
        {
            var defaultPage = "https://jdmcenter.jp/top/Frame/Login01.aspx";
            var defaultContent = (HttpWebRequest)WebRequest.Create(defaultPage);
            defaultContent.Method = "GET";
            defaultContent.ContentType = "text/html; charset=shift_jis";
            var defaultResponse = (HttpWebResponse)defaultContent.GetResponse();
            var defaultCookies = defaultResponse.Headers.Get("Set-Cookie");

            var login = "https://jdmcenter.jp/top/Frame/Login01.aspx";
            var loginContent = (HttpWebRequest)WebRequest.Create(login);
            loginContent.Method = "GET";
            loginContent.ContentType = "text/html; charset=shift_jis";
            loginContent.CookieContainer = new CookieContainer();
            loginContent.CookieContainer.SetCookies(loginContent.RequestUri, defaultCookies);
            var loginResponse = (HttpWebResponse)loginContent.GetResponse();
            var loginHtml = new StreamReader(loginResponse.GetResponseStream()).ReadToEnd();
            var loginDoc = new HtmlDocument();
            loginDoc.LoadHtml(loginHtml);
            var loginNode = loginDoc.DocumentNode.SelectSingleNode("//form[@action='./Login01.aspx']");
            var __VIEWSTATEGENERATOR = loginNode.SelectSingleNode("//input[@name='__VIEWSTATEGENERATOR']").Attributes["value"].Value;
            var __EVENTVALIDATION = loginNode.SelectSingleNode("//input[@name='__EVENTVALIDATION']").Attributes["value"].Value;
            var __VIEWSTATE = loginNode.SelectSingleNode("//input[@name='__VIEWSTATE']").Attributes["value"].Value;
            var LOGIN_BTN = loginNode.SelectSingleNode("//input[@name='LOGIN_BTN']").Attributes["value"].Value;

            var url = "https://jdmcenter.jp/top/Frame/Login01.aspx";
            // 1.获取登录Cookie
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST"; // POST OR GET， 如果是GET, 则没有第二步传参，直接第三步，获取服务端返回的数据
            req.ContentType = "application/x-www-form-urlencoded";
            req.Headers.Add("Referer", "https://jdmcenter.jp/top/Frame/Login01.aspx");
            req.Headers.Add("Host", "jdmcenter.jp");
            req.Headers.Add("Cache-Control", "max-age=0");
            req.Headers.Add("Origin", "https://jdmcenter.jp");
            req.Headers.Add("Upgrade-Insecure-Requests", "1");
            req.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
            req.Headers.Add("Sec-Fetch-User", "?1");
            req.Headers.Add("Sec-Fetch-Site", "same-origin");
            req.Headers.Add("Sec-Fetch-Mode", "navigate");
            req.CookieContainer = new CookieContainer();
            req.CookieContainer.SetCookies(req.RequestUri, defaultCookies);//将登录的cookie值赋予此次的请求。


            string formatString = "USERID_TXT={0}&USERPASSWORD_TXT={1}&__VIEWSTATE={2}&__VIEWSTATEGENERATOR={3}&__EVENTVALIDATION={4}&LOGIN_BTN={5}";
            string postString = string.Format(formatString, "9480006", "Ogihara213#", HttpUtility.UrlEncode(__VIEWSTATE), HttpUtility.UrlEncode(__VIEWSTATEGENERATOR), HttpUtility.UrlEncode(__EVENTVALIDATION), LOGIN_BTN);

            var postBytes = Encoding.UTF8.GetBytes(postString);
            req.ContentLength = postBytes.Length;
            var postDataStream = req.GetRequestStream();
            postDataStream.Write(postBytes, 0, postBytes.Length);
            postDataStream.Close();

            var res = (HttpWebResponse)req.GetResponse();
            var cookies = res.Headers.Get("Set-Cookie");

            // 2.进入页面
            var contentUrl = "https://jdmcenter.jp/top/Frame/Top.aspx";
            var reqContent = (HttpWebRequest)WebRequest.Create(contentUrl);
            reqContent.Method = "GET";
            reqContent.ContentType = "text/html; charset=shift_jis";

            reqContent.CookieContainer = new CookieContainer();
            reqContent.CookieContainer.SetCookies(reqContent.RequestUri, cookies);//将登录的cookie值赋予此次的请求。

            var respContent = (HttpWebResponse)reqContent.GetResponse();
            var finalCookie = respContent.Headers.Get("Set-Cookie");

            // 3.进入表单页面
            var formUrl = "https://jdmcenter.jp/top/Frame/InputInquiryCorpo.aspx";
            var formContent = (HttpWebRequest)WebRequest.Create(formUrl);
            formContent.Method = "GET";
            formContent.ContentType = "text/html; charset=shift_jis";

            formContent.CookieContainer = new CookieContainer();
            formContent.CookieContainer.SetCookies(formContent.RequestUri, finalCookie);//将登录的cookie值赋予此次的请求。

            var formRespContent = (HttpWebResponse)formContent.GetResponse();


            string html = new StreamReader(formRespContent.GetResponseStream(), Encoding.UTF8).ReadToEnd();

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var nameNode = doc.DocumentNode.SelectSingleNode("//form[@action='./InputInquiryCorpo.aspx']");
            var text = nameNode.InnerHtml;
            return text;
        }

    }
}
