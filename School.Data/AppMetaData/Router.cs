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

        // ================= Department Routes =================
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

        // ================= User Routes =================
        public static class UserRouting
        {
            public const string Prefix = Base + "/users";
            public const string Create = Prefix;                  // POST: api/v1/users
            public const string PaginatedList = Prefix + "/paginated";      // GET: api/v1/users/paginated?
            public const string GetById = Prefix + ByIdRoute;     // GET: api/v1/users/{id}
            public const string Update = Prefix + ByIdRoute;      // PUT: api/v1/users/{id}
            public const string Delete = Prefix + ByIdRoute;      // DELETE: api/v1/users/{id}
            public const string ChangePassword = Prefix + "/changePassword" + ByIdRoute;      // PUT: api/v1/users/changePassword/{id}

        }

        // ================= AuthN Routes =================
        public static class AuthenticationRouting
        {
            public const string Prefix = Base + "/authentication";
            public const string SignIn = Prefix + "/signin";
            public const string RefreshToken = Prefix + "/refresh";
            public const string ValidateToken = Prefix + "/validate";
        }

        // ================= AuthZ Routes =================
        public static class AuthorizationRouting
        {
            public const string RolePrefix = Base + "/roles";
            public const string ClaimPrefix = Base + "/claims";
            public const string UserPrefix = Base + "/users";

            // Roles
            public const string GetRoles = RolePrefix; // GET: api/v1/roles
            public const string GetRoleById = RolePrefix + ByIdRoute; // GET: api/v1/roles/{id}
            public const string AddRole = RolePrefix; // POST: api/v1/roles
            public const string EditRole = RolePrefix + ByIdRoute; // PUT: api/v1/roles/{id}
            public const string DeleteRole = RolePrefix + ByIdRoute; // DELETE: api/v1/roles/{id}

            // User Roles
            public const string GetUserRoles = UserPrefix + ByIdRoute + "/roles"; // GET: api/v1/users/{id}/roles
            public const string AssignRole = UserPrefix + ByIdRoute + "/roles"; // POST: api/v1/users/{id}/roles
            public const string UpdateUserRoles = UserPrefix + ByIdRoute + "/roles"; // PUT: api/v1/users/{id}/roles
            public const string RevokeUserRole = UserPrefix + ByIdRoute + "/roles" + "/{roleId}"; // DELETE: api/v1/users/{id}/roles/{roleId}

            // User Claims
            public const string GetUserClaims = UserPrefix + ByIdRoute + "/claims"; // GET: api/v1/users/{id}/claims
            public const string UpdateUserClaims = UserPrefix + ByIdRoute + "/claims"; // PUT: api/v1/users/{id}/claims


        }


    }
}
