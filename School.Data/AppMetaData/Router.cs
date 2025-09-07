namespace School.Data.AppMetaData
{
    public static class Router
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public const string ByIdRoute = "/{id}"; // Due To We Use It In More Than One Controller

        // ================= Student Routes =================
        public static class StudentRouting
        {
            public const string Prefix = Base + "/students";
            public const string List = Prefix + "/all";                    // GET: api/v1/students/all
            public const string GetById = Prefix + ByIdRoute;     // GET: api/v1/students/{id}
            public const string Create = Prefix;                  // POST: api/v1/students
            public const string Update = Prefix + ByIdRoute;      // PUT: api/v1/students/{id}
            public const string Delete = Prefix + ByIdRoute;      // DELETE: api/v1/students/{id}

            public const string PaginatedList = Prefix + "/paginated";      // GET: api/v1/students/paginated?
        }

        public static class DepartmentRouting
        {
            public const string Prefix = Base + "/departments";
            public const string List = Prefix + "/all";                    // GET: api/v1/departments/all
            public const string GetById = Prefix + ByIdRoute;     // GET: api/v1/departments/{id}
            public const string Create = Prefix;                  // POST: api/v1/departments
            public const string Update = Prefix + ByIdRoute;      // PUT: api/v1/departments/{id}
            public const string Delete = Prefix + ByIdRoute;      // DELETE: api/v1/departments/{id}

            public const string PaginatedList = Prefix + "/paginated";      // GET: api/v1/departments/paginated?
        }

        public static class UserRouting
        {
            public const string Prefix = Base + "/users";
            public const string Create = Prefix;                  // POST: api/v1/users
            public const string PaginatedList = Prefix + "/paginated";      // GET: api/v1/users/paginated?
            public const string GetById = Prefix + ByIdRoute;     // GET: api/v1/users/{id}
            public const string Update = Prefix + ByIdRoute;      // PUT: api/v1/users/{id}
            public const string Delete = Prefix + ByIdRoute;      // DELETE: api/v1/users/{id}

        }


    }
}
