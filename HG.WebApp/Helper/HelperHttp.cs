using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace HG.WebApp.Helper
{
    public class HelperHttp
    {
        private static string language = "vi-VN";
        /// <summary>
        /// Call GET API async method from client
        /// </summary>
        /// <typeparam name="T">Data type return</typeparam>
        /// <param name="url">API url </param>
        /// <param name="token">token authen</param>
        /// <returns>Data type return</returns>
        public static async Task<T> GetAPIAsync<T>(string url, string token = null) where T : new()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.BaseAddress = new Uri(url);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Add("Accept-Language", language);
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsAsync<T>();
                    }
                    else
                    {
                        HttpContext.Current.Response.StatusCode = (int)response.StatusCode;
                        return new T();
                    }
                }
            }
            catch (Exception ex)
            {
                return new T();
            }
        }

        /// <summary>
        /// Call GET API sync method from client
        /// </summary>
        /// <typeparam name="T">Data type return</typeparam>
        /// <param name="url">API url </param>
        /// <param name="token">token authen</param>
        /// <returns>Data type return</returns>
        public static T GetAPISync<T>(string url, string token = null) where T : new()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.BaseAddress = new Uri(url);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //httpClient.DefaultRequestHeaders.Add("Accept-Language", language);
                        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    }
                    HttpResponseMessage response = httpClient.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<T>().Result;
                    }
                    else
                    {
                        HttpContext.Current.Response.StatusCode = (int)response.StatusCode;
                        return new T();
                    }
                }
            }
            catch (Exception ex)
            {
                return new T();
            }
        }

        /// <summary>
        /// GetAPISync Không cần token
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static T GetAPISync<T>(string url) where T : new()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //httpClient.DefaultRequestHeaders.Add("Accept-Language", language);
                    HttpResponseMessage response = httpClient.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<T>().Result;
                    }
                    else
                    {
                        HttpContext.Current.Response.StatusCode = (int)response.StatusCode;
                        return new T();
                    }
                }
            }
            catch (Exception ex)
            {
                return new T();
            }
        }

        /// <summary>
        /// Call POST API method from client
        /// </summary>
        /// <typeparam name="T">Data type return</typeparam>
        /// <param name="url">API url </param>
        /// <param name="data">Post data</param>
        /// <param name="token">token authen</param>
        /// <returns>Data type return</returns>
        public static async Task<T> PostAPIAsync<T>(string url, string data = null, string token = null) where T : new()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Add("Accept-Language", language);

                    // Gui kem token xac thuc voi cac api yeu cau login
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    // Gui body cua post la trong
                    if (string.IsNullOrEmpty(data))
                    {
                        data = string.Empty;
                    }
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsAsync<T>();
                    }
                    else
                    {
                        if (HttpContext.Current.Response != null)
                            HttpContext.Current.Response.StatusCode = (int)response.StatusCode;
                        return new T();
                    }
                }
            }
            catch (Exception ex)
            {
                return new T();
            }
        }

        /// <summary>
        /// Call POST API method from client
        /// </summary>
        /// <typeparam name="T">Data type return</typeparam>
        /// <param name="url">API url </param>
        /// <param name="data">Post data</param>
        /// <param name="token">token authen</param>
        /// <returns>Data type return</returns>
        public static T PostAPISync<T>(string url, string data = null, string token = null) where T : new()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Add("Accept-Language", language);
                    // Gui kem token xac thuc voi cac api yeu cau login
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    // Gui body cua post la trong
                    if (string.IsNullOrEmpty(data))
                    {
                        data = string.Empty;
                    }

                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsAsync<T>().Result;
                    }
                    else
                    {
                        HttpContext.Current.Response.StatusCode = (int)response.StatusCode;

                        return new T();
                    }
                }
            }
            catch (Exception ex)
            {
                return new T();
            }
        }

    
        /// <summary>
        /// Get url api
        /// </summary>
        /// <param name="baseUrl">Base url api</param>
        /// <param name="actionApi">Action of api</param>
        /// <returns>Full url api</returns>
        public static string UrlAPI(string baseUrl, string actionApi)
        {
            StringBuilder sb = new StringBuilder(baseUrl);
            sb.Append(actionApi);
            return sb.ToString();
        }

        /// <summary>
        /// Get html template
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string DownloadHtmlTemplate(string url)
        {
            using (var w = new WebClient())
            {
                w.Encoding = Encoding.UTF8;
                var jsonData = string.Empty;
                try
                {
                    jsonData = w.DownloadString(url);
                }
                catch (Exception ex)
                {
                }
                return jsonData;
            }
        }

        /// <summary>
        /// Get file from outsite
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static byte[] DownloadFile(string url)
        {
            using (var w = new WebClient())
            {
                try
                {
                    return w.DownloadData(url);
                }
                catch (Exception ex)
                {
                  
                }
                return null;
            }
        }



        public static int HttpPost(string linkRequest, string dataPost, out string resultData)
        {
            var result = 0;
            resultData = "";
            var encoding = new UTF8Encoding();
            var byteData = encoding.GetBytes(dataPost);
            var httpRequest = (HttpWebRequest)WebRequest.Create(linkRequest);
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            httpRequest.Method = "POST";
            httpRequest.ContentLength = byteData.Length;
            httpRequest.Referer = "";
            try
            {
                var dataStream = httpRequest.GetRequestStream();
                dataStream.Write(byteData, 0, byteData.Length);
                dataStream.Close();
                var httpResponse = httpRequest.GetResponse();
                dataStream = httpResponse.GetResponseStream();
                if (dataStream != null)
                {
                    var reader = new StreamReader(dataStream);
                    resultData = reader.ReadToEnd();
                    result = 1;
                    reader.Close();
                    dataStream.Close();
                }
                httpResponse.Close();
            }
            catch (Exception ex)
            {

                resultData = ex.Message;
            }
            return result;
        }

        public static async Task<string> HttpPostAsync(string linkRequest, string dataPost)
        {
            var resultData = "000";
            var encoding = new UTF8Encoding();
            var byteData = encoding.GetBytes(dataPost);
            var httpRequest = (HttpWebRequest)WebRequest.Create(linkRequest);
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            httpRequest.Method = "POST";
            httpRequest.ContentLength = byteData.Length;
            httpRequest.Referer = "";
            try
            {
                var dataStream = httpRequest.GetRequestStream();
                dataStream.Write(byteData, 0, byteData.Length);
                dataStream.Close();
                var httpResponse = await httpRequest.GetResponseAsync();//.GetResponse();
                dataStream = httpResponse.GetResponseStream();
                if (dataStream != null)
                {
                    var reader = new StreamReader(dataStream);
                    resultData = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                }
                httpResponse.Close();
            }
            catch (Exception ex)
            {

            }
            return resultData;
        }
    }
    public static class HttpContext
        {
            private static Microsoft.AspNetCore.Http.IHttpContextAccessor m_httpContextAccessor;


    public static void Configure(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            m_httpContextAccessor = httpContextAccessor;
        }


        public static Microsoft.AspNetCore.Http.HttpContext Current
        {
            get
            {
                return m_httpContextAccessor.HttpContext;
            }
        }

    }
}
