using Application.Use_Cases.Role.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Use_Cases.Role.CreateRole
{
    public interface ICreateRoleUseCase
    {
        Task ExecuteAsync(RoleDTO roleDTO, CancellationToken cancellationToken);
    }
}
