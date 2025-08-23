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
            public const string List = Prefix;                    // GET: api/v1/students
            public const string GetById = Prefix + ByIdRoute;     // GET: api/v1/students/{id}
            public const string Create = Prefix;                  // POST: api/v1/students
            public const string Update = Prefix + ByIdRoute;      // PUT: api/v1/students/{id}
            public const string Delete = Prefix + ByIdRoute;      // DELETE: api/v1/students/{id}
        }




    }
}
