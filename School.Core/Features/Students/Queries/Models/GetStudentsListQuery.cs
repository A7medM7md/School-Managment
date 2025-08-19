using MediatR;
using School.Core.Bases;
using School.Core.Features.Students.Queries.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Core.Features.Students.Queries.Models
{
    public class GetStudentsListQuery : IRequest<Response<List<GetStudentsListResponse>>>
    {

    }
}
