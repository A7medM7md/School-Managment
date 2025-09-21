namespace School.Data.Helpers.Email
{
    public class EmailContent
    {
        public string Subject { get; set; } = "Notification from School System";

        public string RecipientName { get; set; } = "User";

        public string LeadText { get; set; } = "You have a new message:";

        public string BodyText { get; set; } = "Please check your account for details.";

        public string ActionLink { get; set; } = "#";

        public string ActionText { get; set; } = "View Details";
    }
}
