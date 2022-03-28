using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

/// <summary>
/// Requires the following packages:
///   Newtonsoft.Json
///   System.ValueTuple
/// Also requires HttpQueryStringBuilder class.
/// Example use:
///   HttpHelper.Get("https://api.example.com/status", new { userId = "1234" });
///     will result to https://api.example.com/status?userId=1234
/// </summary>

public static class HttpHelper
{
    private static readonly string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.51 Safari/537.36 Edg/99.0.1150.39";

    /// <exception cref="WebException" />
    public static string Get(string URI, dynamic Query = null, IEnumerable<(string header, string value)> Headers = null)
    {
        if (Query != null) URI += "?" + HttpQueryStringBuilder.BuildQueryString(Query);

        var request = (HttpWebRequest)WebRequest.Create(URI);
        if (Headers != null) foreach (var header in Headers) request.Headers.Add(header.header, header.value);

        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        request.UserAgent = UserAgent;

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }

    /// <exception cref="WebException" />
    public static string Post(string URI, dynamic Params, IEnumerable<(string header, string value)> Headers = null, string Method = "POST")
    {
        var parameters = HttpQueryStringBuilder.BuildQueryString(Params);
        var bytes = Encoding.UTF8.GetBytes(parameters);

        return PostAnBytesArray(URI, bytes, Headers: Headers, Method: Method);
    }

    /// <exception cref="WebException" />
    public static string PostAsJson(string URI, object Params, IEnumerable<(string header, string value)> Headers = null, string Method = "POST")
    {
        var parameters = Newtonsoft.Json.JsonConvert.SerializeObject(Params);
        var bytes = Encoding.UTF8.GetBytes(parameters);

        return PostAnBytesArray(URI, bytes, ContentType: "application/json", Headers, Method);
    }


    /// <exception cref="WebException" />
    public static string PostAnBytesArray(string URI, byte[] Bytes, string ContentType = "application/x-www-form-urlencoded", IEnumerable<(string header, string value)> Headers = null, string Method = "POST")
    {
        var request = (HttpWebRequest)WebRequest.Create(URI);
        if (Headers != null) foreach (var header in Headers) request.Headers.Add(header.header, header.value);

        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        request.ContentLength = Bytes.Length;
        request.ContentType = ContentType;
        request.Method = Method;
        request.UserAgent = UserAgent;

        using (Stream requestBody = request.GetRequestStream())
        {
            requestBody.Write(Bytes, 0, Bytes.Length);
        }

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
}