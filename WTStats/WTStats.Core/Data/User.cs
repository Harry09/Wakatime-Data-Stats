using System;

using Newtonsoft.Json;

namespace WTStats.Data
{
    public class User
    {
		[JsonProperty("created_at")]
		public DateTimeOffset CreatedAt { get; set; }

		[JsonProperty("display_name")]
		public string DisplayName { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("email_public")]
		public bool EmailPublic { get; set; }

		[JsonProperty("full_name")]
		public string FullName { get; set; }

		[JsonProperty("has_premium_features")]
		public bool HasPremiumFeatures { get; set; }

		[JsonProperty("human_readable_website")]
		public string HumanReadableWebsite { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("is_email_confirmed")]
		public bool IsEmailConfirmed { get; set; }

		[JsonProperty("is_hireable")]
		public bool IsHireable { get; set; }

		[JsonProperty("languages_used_public")]
		public bool LanguagesUsedPublic { get; set; }

		[JsonProperty("last_heartbeat")]
		public DateTimeOffset LastHeartbeat { get; set; }

		[JsonProperty("last_plugin")]
		public string LastPlugin { get; set; }

		[JsonProperty("last_plugin_name")]
		public string LastPluginName { get; set; }

		[JsonProperty("last_project")]
		public string LastProject { get; set; }

		[JsonProperty("location")]
		public string Location { get; set; }

		[JsonProperty("logged_time_public")]
		public bool LoggedTimePublic { get; set; }

		[JsonProperty("modified_at")]
		public object ModifiedAt { get; set; }

		[JsonProperty("photo")]
		public string Photo { get; set; }

		[JsonProperty("photo_public")]
		public bool PhotoPublic { get; set; }

		[JsonProperty("plan")]
		public string Plan { get; set; }

		[JsonProperty("timeout")]
		public long Timeout { get; set; }

		[JsonProperty("timezone")]
		public string Timezone { get; set; }

		[JsonProperty("username")]
		public string Username { get; set; }

		[JsonProperty("website")]
		public string Website { get; set; }

		[JsonProperty("writes_only")]
		public bool WritesOnly { get; set; }
	}
}
