using System;
using System.Net.Http;
using System.Text;

namespace RailRoadCounter
{

	public class HttpConnector
	{

		private static HttpClient _client;

		public static HttpClient Client
		{
			get
			{
				if (_client == null)
				{
					_client = new HttpClient();
				}
				return _client;
			}

			set
			{
				_client = value;
			}
		}

		public HttpConnector()
		{
		}

		public static HttpRequestMessage CreateGetConnection(Uri uri)
		{
			var request = new HttpRequestMessage()
			{
				RequestUri = uri,
				Method = HttpMethod.Get
			};
			request.Headers.TryAddWithoutValidation("Content-Type", "application/xml; charset=windows-1251");
			//request.Headers.Add(AppResources.RequestHeader, AppResources.ReqeustHeaderValue);
			return request;
		}

		public static HttpRequestMessage CreatePostConnection(Uri uri, string jsonRequest)
		{
			var request = new HttpRequestMessage()
			{
				RequestUri = uri,
				Method = HttpMethod.Post,
				Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json")
			};

			//request.Headers.Add(AppResources.RequestHeader, AppResources.ReqeustHeaderValue);
			return request;
		}
	}
}
