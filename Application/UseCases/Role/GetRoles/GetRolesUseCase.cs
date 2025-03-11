using AutoMapper;
using Domain.Interfaces;

namespace Application.Use_Cases.Role.GetRoles
{
    public class GetRolesUseCase : IGetRolesUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRolesUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Domain.Entities.Role>> ExecuteAsync(CancellationToken cancellationToken)
        {
            var roles = await _unitOfWork.RoleRepository.GetAllAsync(cancellationToken);
            return roles;
        }
    }
}
