using Pathoschild.Http.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage call logs.
	/// </summary>
	/// <seealso cref="ZoomNet.Resources.ICallLogs" />
	/// <remarks>
	/// See <a href="https://marketplace.zoom.us/docs/api-reference/phone/methods/#tag/Call-Logs">Zoom documentation</a> for more information.
	/// </remarks>
	public class CallLogs : ICallLogs
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="CallLogs" /> class.
		/// </summary>
		/// <param name="client">The HTTP client.</param>
		internal CallLogs(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Get call logs for specified user.
		/// </summary>
		/// <param name="userId">The user Id or email address.</param>
		/// <param name="from">The start date.</param>
		/// <param name="to">The end date.</param>
		/// <param name="type">Type of call log.</param>
		/// <param name="phoneNumber">Phone number for filtering call log.</param>
		/// <param name="recordsPerPage">The number of records to return.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="UserCallLog" />.
		/// </returns>
		public Task<PaginatedResponseWithToken<UserCallLog>> GetAsync(string userId, DateTime? from = null, DateTime? to = null, CallLogType type = CallLogType.All, string phoneNumber = null, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
		{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"phone/users/{userId}/call_logs")
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithArgument("type", type.ToEnumString())
				.WithArgument("phone_number", phoneNumber)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithToken<UserCallLog>("call_logs");
		}

		/// <summary>
		/// Get call logs for an entire account.
		/// </summary>
		/// <param name="from">The start date.</param>
		/// <param name="to">The end date.</param>
		/// <param name="type">Type of call log.</param>
		/// <param name="pathType">Filter the API response by path of the call.</param>
		/// <param name="timeType">Enables you to search call logs by start or end time.</param>
		/// <param name="siteId">Unique identifier of the site. Use this query parameter if you have enabled multiple sites and would like to filter the response of this API call by call logs of a specific phone site.</param>
		/// <param name="chargedCallLogs">Whether to filter API responses to include call logs that only have a non-zero charge.</param>
		/// <param name="recordsPerPage">The number of records to return.</param>
		/// <param name="pagingToken">The paging token.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		/// An array of <see cref="AccountCallLog" />.
		/// </returns>
		public Task<PaginatedResponseWithTokenAndDateRange<AccountCallLog>> GetAsync(DateTime? from = null, DateTime? to = null, CallLogType type = CallLogType.All, CallLogPathType? pathType = null, CallLogTimeType? timeType = CallLogTimeType.StartTime, string siteId = null, bool chargedCallLogs = false, int recordsPerPage = 30, string pagingToken = null, CancellationToken cancellationToken = default)
			{
			if (recordsPerPage < 1 || recordsPerPage > 300)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), "Records per page must be between 1 and 300");
			}

			return _client
				.GetAsync($"phone/call_logs")
				.WithArgument("from", from.ToZoomFormat(dateOnly: true))
				.WithArgument("to", to.ToZoomFormat(dateOnly: true))
				.WithArgument("type", type.ToEnumString())
				.WithArgument("path", pathType?.ToEnumString())
				.WithArgument("timeType", timeType?.ToEnumString())
				.WithArgument("site_id", siteId)
				.WithArgument("charged_call_logs", chargedCallLogs)
				.WithArgument("page_size", recordsPerPage)
				.WithArgument("next_page_token", pagingToken)
				.WithCancellationToken(cancellationToken)
				.AsPaginatedResponseWithTokenAndDateRange<AccountCallLog>("call_logs");
		}
	}
}
