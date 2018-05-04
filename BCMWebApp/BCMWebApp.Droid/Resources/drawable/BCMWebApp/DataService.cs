using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BCMWebApp
{
    public class DataService
    {
        public static async Task<bool> getToken(string userName, string password)
        {
            string url = string.Format("{0}/token", GlobalVars._serverUrl);

            using (HttpClient httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var param = new Dictionary<string, string>();
                param.Add("grant_type", "password");
                param.Add("username", userName);
                param.Add("password", password);

                var request = new HttpRequestMessage(new HttpMethod("POST"), url)
                {
                    Content = new FormUrlEncodedContent(param)
                };

                try
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    var _cancelTokenSource = new CancellationTokenSource();
                    var _cancelToken = _cancelTokenSource.Token;
                    var result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(false);
                    if (result.IsSuccessStatusCode)
                    {
                        string resultContent = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                        GlobalVars.Usertoken = JObject.Parse(resultContent)["access_token"].ToString();
                        GlobalVars.UserName = string.Empty;
                        GlobalVars.UserId = 0;

                        return true;
                    }
                }
                catch (TaskCanceledException tex)
                {
                    throw tex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return false;
        }
        public static async Task<bool> getdataUsuario(string queryString)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVars.Usertoken);
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    var _cancelTokenSource = new CancellationTokenSource();
                    var _cancelToken = _cancelTokenSource.Token;
                    var request = new HttpRequestMessage(new HttpMethod("GET"), queryString);

                    var result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(false);

                    if (result.IsSuccessStatusCode)
                    {
                        string json = result.Content.ReadAsStringAsync().Result;
                        GlobalVars.UserId = long.Parse(JObject.Parse(json)["Id"].ToString());
                        GlobalVars.UserName = JObject.Parse(json)["Nombre"].ToString();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                return false;
            }

            return false;

        }
        public static async Task<bool> getEmpresasUsuario(string queryString)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVars.Usertoken);
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    var _cancelTokenSource = new CancellationTokenSource();
                    var _cancelToken = _cancelTokenSource.Token;
                    var request = new HttpRequestMessage(new HttpMethod("GET"), queryString);

                    var result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(false);

                    if (result.IsSuccessStatusCode)
                    {
                        string json = result.Content.ReadAsStringAsync().Result;
                        List<Empresa> dataEmpresa = new List<Empresa>();

                        JArray resArray = JArray.Parse(json);
                        try
                        {
                            for (int ndx = 0; ndx < resArray.Count; ndx++)
                            {
                                dataEmpresa.Add(new Empresa
                                {
                                    Id = long.Parse(((JObject)resArray[ndx])["Id"].ToString()),
                                    IdNivelUsuario = long.Parse(((JObject)resArray[ndx])["IdNivelUsuario"].ToString()),
                                    ImagenUrl = ((JObject)resArray[ndx])["ImageUrl"].ToString(),
                                    Nombre = ((JObject)resArray[ndx])["Nombre"].ToString(),
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            string Message = ex.Message;
                            throw ex;
                        }
                        GlobalVars.BotonEmpresa = dataEmpresa;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                return false;
            }

            return false;
        }
    }
}
