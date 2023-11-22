using filemanagementapi.Domain.UserModel;

namespace filemanagementapi.Interfaces;
public interface IAuth
{
    bool Register(UserModel userModel);
    Tuple<int,string> GetToken(UserModel userModel);
}
