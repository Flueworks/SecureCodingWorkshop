﻿/*
MIT License

Copyright (c) 2021

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace SecureCodingWorkshop.Hybrid_;

public class HybridEncryption
{
    public static EncryptedPacket EncryptData(byte[] original, RSAWithRSAParameterKey rsaParams)
    {
        // Generate our session key.
        var sessionKey = RandomNumberGenerator.GetBytes(32);

        // Create the encrypted packet and generate the IV.
        var encryptedPacket = new EncryptedPacket { Iv = RandomNumberGenerator.GetBytes(16) };

        // Encrypt our data with AES.
        encryptedPacket.EncryptedData = AesEncryption.Encrypt(original, sessionKey, encryptedPacket.Iv);

        // Encrypt the session key with RSA
        encryptedPacket.EncryptedSessionKey = rsaParams.EncryptData(sessionKey);

        return encryptedPacket;
    }

    public static byte[] DecryptData(EncryptedPacket encryptedPacket, RSAWithRSAParameterKey rsaParams)
    {
        // Decrypt AES Key with RSA.
        var decryptedSessionKey = rsaParams.DecryptData(encryptedPacket.EncryptedSessionKey);

        // Decrypt our data with  AES using the decrypted session key.
        var decryptedData = AesEncryption.Decrypt(encryptedPacket.EncryptedData,
            decryptedSessionKey, encryptedPacket.Iv);

        return decryptedData;
    }
}