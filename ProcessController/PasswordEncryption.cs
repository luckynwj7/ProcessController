using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Windows;

namespace ProcessController
{
    public static class PasswordEncryption
    {
        // 출처 https://insurang.tistory.com/353
        public static string EncrypString(string inputText, string keyPass)
        {
            string encryptedData = "";
            try
            {
                // Rihndael class를 선언하고, 초기화
                RijndaelManaged rijndaelCipher = new RijndaelManaged();

                // 입력받은 문자열 바이트 배열로 변환
                byte[] plainText = System.Text.Encoding.Unicode.GetBytes(inputText);

                // 딕셔너리 공격을 대비해서 키를 더 풀기 어렵게 만들기 위해서
                // Salt를 사용한다.
                byte[] salt = Encoding.ASCII.GetBytes(keyPass.Length.ToString());

                //PasswordDeriveBytes 클래스를 사용해서 secretKey를 얻는다.
                PasswordDeriveBytes secretKey = new PasswordDeriveBytes(keyPass, salt);

                // Create a encrpytor from the existing secretKey bytes.
                // encryptor 객체를 secretKey로부터 만든다.
                // secretKey에는 32바이트
                // (Rijndael의 디폴트인 256bit가 바로 32바이트입니다)를 사용하고,
                // Initialization Vector로 16바이트
                // (역시 디폴트인 128비트가 바로 16바이트입니다)를 사용한다.
                ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));

                // 메모리 스트림 객체를 선언, 초기화
                MemoryStream memoryStream = new MemoryStream();

                // cryptoStream객체를 암호화된 데이터를 쓰기 위한 용도로 선언
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                // 암호화 프로세스가 진행된다
                cryptoStream.Write(plainText, 0, plainText.Length);

                // 암호화 종료
                cryptoStream.FlushFinalBlock();

                // 암호화된 데이터를 바이트 배열로 담는다.
                byte[] cipherBytes = memoryStream.ToArray();

                // 스트림 해제
                memoryStream.Close();
                cryptoStream.Close();

                // 암호화된 데이터를 Base64 인코딩된 문자열로 변환한다.
                encryptedData = Convert.ToBase64String(cipherBytes);
            }
            catch
            {
                MessageBox.Show("Fail Value");
            }
            return encryptedData;
        }

        public static string DecrypString(string inputText, string keyPass)
        {
            string decryptedData = "";
            try
            {
                RijndaelManaged rijndaelCipher = new RijndaelManaged();

                byte[] encryptedData = Convert.FromBase64String(inputText);
                byte[] salt = Encoding.ASCII.GetBytes(keyPass.Length.ToString());

                PasswordDeriveBytes secretKey = new PasswordDeriveBytes(keyPass, salt);

                // Decryptor 객체를 만든다.
                ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));

                MemoryStream memoryStream = new MemoryStream(encryptedData);

                // 데이터 읽기(복호화이므로) 용도로 cryptoStream객체를 선언, 초기화
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                // 복호화된 데이터를 담을 바이트 배열을 선언한다.
                // 길이는 알 수 없지만, 일단 복호화되기 전의 데이터의 길이보다는
                // 길지 않을 것이기 때문에 그 길이로 선언한다.
                byte[] plainText = new byte[encryptedData.Length];

                // 복호화 시작
                int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);

                memoryStream.Close();
                cryptoStream.Close();

                // 복호화된 데이터를 문자열로 바꾼다.
                decryptedData = Encoding.Unicode.GetString(plainText, 0, decryptedCount);
            }
            catch
            {
                MessageBox.Show("Failed Data");
            }
            // 최종결과 리턴
            return decryptedData;
        }
    }
}
