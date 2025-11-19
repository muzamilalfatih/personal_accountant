namespace personal_accountant.DTOs.Mapper
{
    public static class UserDTOMapper
    {
        public static UserPublicDTO ToUserPublicDTO(this UserDTO userDTO)
        {
            return new UserPublicDTO(userDTO.Id, userDTO.FirstName, userDTO.LastName, userDTO.Email, userDTO.Role);
        }
    }
}
