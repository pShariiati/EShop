using EShop.Entities.Identity;

namespace EShop.Entities;

public class UserInformation : BaseEntity
{
    #region Fields

    public string FullName { get; set; }
    public DateTime BirthDate { get; set; }
    public string NationalCode { get; set; }
    public EyeColor EyeColor { get; set; }
    public int UserId { get; set; }

    #endregion

    #region Relations

    public User User { get; set; }

    #endregion
}

public enum EyeColor : byte
{
    Green,
    Blue,
    Black,
    Brown
}
