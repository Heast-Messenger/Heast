namespace Core.Network;

// @formatter:off

[Flags]
public enum ErrorCodes
{
    None                    = 0,
    InvalidKey              = 1 << 0,
    
    EmailEmpty              = 1 << 1,
    InvalidEmail            = 1 << 2,
    
    PasswordEmpty           = 1 << 3,
    PasswordTooShort        = 1 << 4,
    PasswordTooLong         = 1 << 5,
    InvalidPassword         = 1 << 6,
    
    UsernameEmpty           = 1 << 7,
    UsernameTooLong         = 1 << 8,
    UsernameTooShort        = 1 << 9,
    InvalidUsername         = 1 << 10,
    
    UsernameExists          = 1 << 11,
    EmailExists             = 1 << 12,
    
    SignupError             = 1 << 13,
    LoginError              = 1 << 14
}

// @formatter:on