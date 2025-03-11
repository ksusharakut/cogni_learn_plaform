using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Use_Cases.User.DeleteUser
{
    public interface IDeleteUserUseCase
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
        Task ExecuteAsync(int userId, CancellationToken cancellationToken);
    }
}
