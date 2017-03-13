using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BuyalotV1._0.Gateway
{
    public class NotificationConsistentKey
    {
        public static string ReturnURLConsistent(string STATUS, string MERCHANT, string CURRENCY, string COUNTRY, string REFERENCE, string AMOUNT, string BANK, string DATE, string RECEIPT, string TNXID, string CUSTOM1, string CUSTOM2, string CUSTOM3, string CUSTOM4, string CUSTOM5, string PRIVATE_KEY)
        {
            SHA512 SHA512HashCreator = SHA512.Create();

            #region Custom Field Null Checking

            if (CUSTOM1 == null)
                CUSTOM1 = string.Empty;
            if (CUSTOM2 == null)
                CUSTOM2 = string.Empty;
            if (CUSTOM3 == null)
                CUSTOM3 = string.Empty;
            if (CUSTOM4 == null)
                CUSTOM4 = string.Empty;
            if (CUSTOM5 == null)
                CUSTOM5 = string.Empty;

            #endregion

            StringBuilder concatenatedString = new StringBuilder();

            concatenatedString.Append(STATUS);
            concatenatedString.Append(MERCHANT);
            concatenatedString.Append(COUNTRY);
            concatenatedString.Append(CURRENCY);
            concatenatedString.Append(REFERENCE);
            concatenatedString.Append(AMOUNT);
            concatenatedString.Append(BANK);
            concatenatedString.Append(DATE);
            concatenatedString.Append(RECEIPT);
            concatenatedString.Append(TNXID);
            concatenatedString.Append(CUSTOM1);
            concatenatedString.Append(CUSTOM2);
            concatenatedString.Append(CUSTOM3);
            concatenatedString.Append(CUSTOM4);
            concatenatedString.Append(CUSTOM5);
            concatenatedString.Append(PRIVATE_KEY);

            byte[] EncryptedData = SHA512HashCreator.ComputeHash(Encoding.UTF8.GetBytes(concatenatedString.ToString()));

            StringBuilder CONSISTENT_KEY = new StringBuilder();

            for (int i = 0; i < EncryptedData.Length; i++)
            {
                CONSISTENT_KEY.Append(EncryptedData[i].ToString("X2"));
            }

            return CONSISTENT_KEY.ToString().ToUpper();
        }
    }
}