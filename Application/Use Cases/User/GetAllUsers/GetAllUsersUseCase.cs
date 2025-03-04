using AutoMapper;
using Domain.Interfaces;

namespace Application.Use_Cases.User.GetAllUsers
{
    public class GetAllUsersUseCase : IGetAllUsersUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> ExecuteAsync(CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.UserRepository.GetAllWithRolesAsync(cancellationToken);
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }
    }
}
