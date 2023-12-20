using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace App.Modules.UserAccess.Application.Authentication;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        byte[] salt;
        byte[] buffer2;

        using (var bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8, HashAlgorithmName.SHA256))
        {
            salt = bytes.Salt;
            buffer2 = bytes.GetBytes(0x20);
        }

        var dst = new byte[0x31];
        Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
        Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);

        return Convert.ToBase64String(dst);
    }

    public static bool IsPasswordValid(string hashedPassword, string providedPassword)
    {
        byte[] buffer4;
        var src = Convert.FromBase64String(hashedPassword);

        if ((src.Length != 0x31) || (src[0] != 0))
        {
            return false;
        }

        var dst = new byte[0x10];
        Buffer.BlockCopy(src, 1, dst, 0, 0x10);

        var buffer3 = new byte[0x20];
        Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);

        using (var bytes = new Rfc2898DeriveBytes(providedPassword, dst, 0x3e8, HashAlgorithmName.SHA256))
        {
            buffer4 = bytes.GetBytes(0x20);
        }

        return ByteArraysEqual(buffer3, buffer4);
    }

    [MethodImpl(MethodImplOptions.NoOptimization)]
    private static bool ByteArraysEqual(byte[] a, byte[] b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (a.Length != b.Length)
        {
            return false;
        }

        var areSame = true;
        for (var i = 0; i < a.Length; i++)
        {
            areSame &= a[i] == b[i];
        }

        return areSame;
    }
}
