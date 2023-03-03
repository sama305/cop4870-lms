using System;
namespace Library.LMS.Models
{
	public class Announcement
	{
		public string Header { get; }
		public string Content { get; }

		public Announcement(string header, string description)
		{
			Header = header;
			Content = description;
		}

        public override string ToString()
        {
            return $"{Header}: {Content}";
        }
    }
}

