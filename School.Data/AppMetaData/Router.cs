using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Data.AppMetaData
{
    public static class Router
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public const string ByIdRoute = "/{id}"; // Due To We Use It In More Than One Controller

        public static class StudentRouting
        {
            public const string Prefix = Base + "/students";
            public const string List = Prefix + "/list";
            public const string GetById = Prefix + ByIdRoute;
        }




    }
}
