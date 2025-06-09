using TaskManagement.DTO.Office.User;
using TaskManagement.MobileApp.Models.Collections;

namespace TaskManagement.MobileApp.Helpers.Builders;

public class UserItemBuilder
{
    private readonly UserResponse _userResponse;

    private UserItemBuilder(UserResponse userResponse)
    {
        _userResponse = userResponse;
    }

    public static UserItemBuilder From(UserResponse userResponse)
    {
        return new UserItemBuilder(userResponse);
    }

    public UserItem Build()
    {
        return new UserItem(
            _userResponse.Id,
            _userResponse.FirstName,
            _userResponse.LastName
        );
    }
}