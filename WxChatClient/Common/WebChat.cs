using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WxChatClient.Common
{
    public class WebChat
    {
        private string cookie;
        private string token;

        public WebChat()
        {
            this.cookie = Global.COOKIE;
            this.token = Global.TOKEN;
        }

        public void Login(string username, string password)
        {
            var request = CreateUri("http://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN");
            using (var post = request.GetRequestStream())
            {
                var content = string.Format(
                    "username={0}&pwd={1}&f=json",
                    username, password);
                var buffer = Encoding.UTF8.GetBytes(content);
                post.Write(buffer, 0, buffer.Length);
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var sr = new StreamReader(stream))
            {
                Global.COOKIE = GetCookie(response.Headers["Set-Cookie"]);
                Global.TOKEN = GetToken(sr.ReadToEnd());
            }
        }

        /// <summary>
        /// 109147935
        /// </summary>
        /// <param name="fakeid"></param>
        public string GetUserInfo(string fakeid)
        {
            string uri = "http://mp.weixin.qq.com/cgi-bin/getcontactinfo";

            string refer = "https://mp.weixin.qq.com/cgi-bin/message?" +
                "t=message/list&count=20&day=7&lang=zh_CN&token=" + token;

            var request = CreateUri(uri, refer);
            this.SetCookie(request);
            using (var stream = request.GetRequestStream())
            {
                var content = string.Format(
                    "token={0}&lang=zh_CN&random={1}&f=json&ajax=1&t=ajax-getcontactinfo&fakeid={2}",
                    token,
                    ("0." + DateTime.Now.Ticks.ToString()).Substring(0, 18),
                    fakeid);
                var buffer = Encoding.UTF8.GetBytes(content);
                stream.Write(buffer, 0, buffer.Length);
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        private static HttpWebRequest CreateUri(
            string uri,
            string referer = "https://mp.weixin.qq.com/",
            string method = "POST")
        {
            var request = WebRequest.CreateHttp(uri);
            request.Method = method;
            request.Accept = "application/json, text/javascript, */*; q=0.01";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = referer;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.76";
            request.KeepAlive = true;

            return request;
        }

        private void SetCookie(HttpWebRequest request)
        {
            request.Headers.Add("Cookie", cookie);
        }

        private string GetToken(string input)
        {
            Regex regx = new Regex("(?<=token=)[0-9]+");
            return regx.Match(input).Value;
        }

        private string GetCookie(string header)
        {
            Regex slave_user = new Regex(@"slave_user=\w+;");
            Regex slave_sid = new Regex(@"slave_sid=\w+=;");
            return slave_user.Match(header).Value + " " +
                   slave_sid.Match(header).Value +
                   " bizuin=3091265724"; // 必须， 
                                         // 登录https://mp.weixin.qq.com/cgi-bin/home?t=home/index&lang=zh_CN&token=TOKEN获得
        }
    }
}
