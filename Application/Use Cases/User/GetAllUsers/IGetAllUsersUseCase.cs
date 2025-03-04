namespace Application.Use_Cases.User.GetAllUsers
{
    public interface IGetAllUsersUseCase
    {
        Task<IEnumerable<UserDTO>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
