namespace School.Core.Features.Users.Queries.Responses
{
    public class GetPaginatedUsersResponse
    {
        public GetPaginatedUsersResponse(string fullName, string email, string? address, string? country)
        {
            FullName = fullName;
            Email = email;
            Address = address;
            Country = country;
        }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }


    }
}
