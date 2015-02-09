namespace iFaith
{
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.Windows.Forms;

    [StandardModule]
    internal sealed class Definitions
    {
        public static string CurrentAddr = "0x0";
        public static bool DumpMode = false;
        public static int i = 0;
        public static string iBECIV = "";
        public static string iBECKey = "";
        public static string iBSSIV = "";
        public static string iBSSKey = "";
        public static bool iREB_mode = false;
        public static bool iRecoveryConnected = false;
        public static string RamdiskIV = "";
        public static string RamdiskKey = "";
        public static string rootfs_key = "";
        public static string rootfs_size = "xxx";

        public static void DetectiOSfromRawSHSH(string shshfile)
        {
            RichTextBox box = new RichTextBox();
            box.LoadFile(shshfile, RichTextBoxStreamType.PlainText);
            box.Text = box.Text.Replace(" ", "").Replace("\t", "");
            if (box.Text.Contains("QAAAAPjJPgDTHIHCjf4lMTc/V6wdwawMGXB2Ww=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "3.1 (7C144)";
                iFaith.xml_ipsw_md5 = "38638d6056b53f2d87a0f5fcb5584cdd";
            }
            else if (box.Text.Contains("QAAAAPhBPABPAO9tQ/Wmf5Yeyl7kYzkLYrYXTg=="))
            {
                iFaith.xml_board = "n72ap";
                iFaith.xml_ios = "3.1.1 (7C145)";
                iFaith.xml_ipsw_md5 = "e0c97bdbb9efbf411b22a81327ad48dc";
            }
            else if (box.Text.Contains("QAAAADhwPQAToo1U4HjplrqVJtq8eIqXY7wnnA=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "3.1.1 (7C145)";
                iFaith.xml_ipsw_md5 = "4ad01a2c6fc82bcac2300253b0368f6e";
            }
            else if (box.Text.Contains("QAAAAHjKPgDg3XZIKbq9bLn7hE77lWLCCBP7jw=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "3.1.2 (7D11)";
                iFaith.xml_ipsw_md5 = "089769d37b846917394ffe11da9d2e17";
            }
            else if (box.Text.Contains("QAAAALhAPADHxW2jwEvCnZm3gkjwA2lgdgn/Wg=="))
            {
                iFaith.xml_board = "n72ap";
                iFaith.xml_ios = "3.1.2 (7D11)";
                iFaith.xml_ipsw_md5 = "35c66be376201082a005f0a289f26a65";
            }
            else if (box.Text.Contains("QAAAALhxPQDOpPhRPAe/mVP5J89iIhtaQEmJgg=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "3.1.2 (7D11)";
                iFaith.xml_ipsw_md5 = "13938eaca91e12e7cefb47717e7cadc8";
            }
            else if (box.Text.Contains("QAAAALjJPgBgA8j5+OhiflqfhTB4tqxO9W6ezg=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "3.1.3 (7E18)";
                iFaith.xml_ipsw_md5 = "4117e4b22565e69205a84e9eeef0583e";
            }
            else if (box.Text.Contains("QAAAADhFPACtgKtNBgl8T23wrvZ5wuye0msL8w=="))
            {
                iFaith.xml_board = "n72ap";
                iFaith.xml_ios = "3.1.3 (7E18)";
                iFaith.xml_ipsw_md5 = "33df8d6ae5d8a695bba267ae89fe37f1";
            }
            else if (box.Text.Contains("QAAAADhvPQCnYHUKnS+DwCey4mmFj2FaEMhXTQ=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "3.1.3 (7E18)";
                iFaith.xml_ipsw_md5 = "a73de2cfafef3463e9afa491f20c5213";
            }
            else if (box.Text.Contains("QAAAADjtSgDkLoKoqet+zFKuOo4wMgnSFBTd0A=="))
            {
                iFaith.xml_board = "k48ap";
                iFaith.xml_ios = "3.2 (7B367)";
                iFaith.xml_ipsw_md5 = "2912cefa0304e5430594c576ad88d398";
            }
            else if (box.Text.Contains("QAAAADjtSgBZX2t1hs/IlYo/o2/mMDhgIdXE8g=="))
            {
                iFaith.xml_board = "k48ap";
                iFaith.xml_ios = "3.2.1 (7B405)";
                iFaith.xml_ipsw_md5 = "5ccf846d96a677f42ac183f5a137dc92";
            }
            else if (box.Text.Contains("QAAAAPjxSgCUhdPG7lHfoE9ZtOcWMI8+bFrZzA=="))
            {
                iFaith.xml_board = "k48ap";
                iFaith.xml_ios = "3.2.2 (7B500)";
                iFaith.xml_ipsw_md5 = "cf6d93fffdc60dcca487a80004d250fa";
            }
            else if (box.Text.Contains("QAAAALjIRwBk2Xl0S70zqBdwhrvZWw6xKHi3fg=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "4.0 (8A293)";
                iFaith.xml_ipsw_md5 = "f9819ad9a52324ac6f10e4a0ea581cbd";
            }
            else if (box.Text.Contains("QAAAALgGTgBUg01SU/RoKqgS9T3lFhhDUx93vA=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "4.0 (8A293)";
                iFaith.xml_ipsw_md5 = "8717be79fb38cd83aa5e5956eb0608b7";
            }
            else if (box.Text.Contains("QAAAADjVPwCE4IFtMXuxBvN6Rm6KkE86YeI91Q=="))
            {
                iFaith.xml_board = "n72ap";
                iFaith.xml_ios = "4.0 (8A293)";
                iFaith.xml_ipsw_md5 = "41dd8ab40159a13d7be42cd7e5f3a479";
            }
            else if (box.Text.Contains("QAAAADhwRgBNMG1ly1aJXHGRBiA3va3bDEhXvw=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "4.0 (8A293)";
                iFaith.xml_ipsw_md5 = "6b9d65c9f63792968bad57e44a73434f";
            }
            else if (box.Text.Contains("QAAAAPjIRwD4xixB/Kpgv14TVO6FECaE+v9BGw=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "4.0.1 (8A306)";
                iFaith.xml_ipsw_md5 = "a3104ca3b72a91bc7eff037ee320ecc5";
            }
            else if (box.Text.Contains("QAAAAPgGTgBo4bzg+qpq9iqB7Ks2ulh2mtZ8DQ=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "4.0.1 (8A306)";
                iFaith.xml_ipsw_md5 = "40ebacb47fb32d7f33ba0fd596e150e9";
            }
            else if (box.Text.Contains("QAAAAHjLRwCYlXgAOwRLbPkZR7n5qtKBcoqisg=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "4.0.2 (8A400)";
                iFaith.xml_ipsw_md5 = "9cb5684457fb41886827d727d91313c3";
            }
            else if (box.Text.Contains("QAAAALgITgCEt/8HUzAIj8PWPGrHNLfuTYvODA=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "4.0.2 (8A400)";
                iFaith.xml_ipsw_md5 = "790b24fe7515084f457ce413618b2709";
            }
            else if (box.Text.Contains("QAAAALjWPwDr8IGD3YhyVMWOplxClTZa8Q1MRQ=="))
            {
                iFaith.xml_board = "n72ap";
                iFaith.xml_ios = "4.0.2 (8A400)";
                iFaith.xml_ipsw_md5 = "e706efcf835de9fcf6f96c7a420a7a22";
            }
            else if (box.Text.Contains("QAAAALhyRgD1YNwStGWNh5ogVtW1Z9IjeXPYvQ=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "4.0.2 (8A400)";
                iFaith.xml_ipsw_md5 = "dc7741b9e4353895c3910237a5b10a4d";
            }
            else if (box.Text.Contains("QAAAAPieSAB1A3roOWUa/etcyQLHh2g0nOJ84A=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "4.1 (8B117)";
                iFaith.xml_ipsw_md5 = "e07bee3c03e7a18e5d75fcaa23db17b5";
            }
            else if (box.Text.Contains("QAAAADj8UACYYaqRyIPRPDEAi1SecetP5R/+lg=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "4.1 (8B117)";
                iFaith.xml_ipsw_md5 = "ac3031a7b5c013d6a09952b691985878";
            }
            else if (box.Text.Contains("QAAAAPinQACUsDP1+y5KFUKyxZ1jQr5hewihEg=="))
            {
                iFaith.xml_board = "n72ap";
                iFaith.xml_ios = "4.1 (8B117)";
                iFaith.xml_ipsw_md5 = "9f8a1978f053ec96926e535bb57ac171";
            }
            else if (box.Text.Contains("QAAAADiRRwCNw+g6mhnzVOWWErGQlKN7ZFR2fA=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "4.1 (8B117)";
                iFaith.xml_ipsw_md5 = "f3877c6f309730ee31297a06c7a9e82c";
            }
            else if (box.Text.Contains("QAAAALi4UQBKDUbbGQXlDIxOQAnR7FUSkB5Zog=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "4.1 (8B117)";
                iFaith.xml_ipsw_md5 = "2e634d16d0e01ef70070c9a289e488ca";
            }
            else if (box.Text.Contains("QAAAADi5UQCCXuT6v4WnNO+AObJgsbsccmgNHQ=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "4.1 (8B118)";
                iFaith.xml_ipsw_md5 = "0564fcd3f53dd6262b9eb636b7fbe540";
            }
            else if (box.Text.Contains("QAAAADixSgBFt/TOJaOO8RfsQHQoxshqxm1sCA=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "4.1 (8M89)";
                iFaith.xml_ipsw_md5 = "35c8ab4b7e70ab0e47e2f5981e52ba55";
            }
            else if (box.Text.Contains("QAAAAPhnSwCzCWjrZywzePT28rKgZUTa/pDa+w=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "4.2 (8C150)";
                iFaith.xml_ipsw_md5 = "4980214fe7a65e2624aaff394f106e06";
            }
            else if (box.Text.Contains("QAAAAHg6SgBVLvMmKFRaUfSl1hms2T8qQlJE/w=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "4.2.1 (8C148a)";
                iFaith.xml_ipsw_md5 = "d688d2d48c8b054367adef8e7ab4f5ea";
            }
            else if (box.Text.Contains("QAAAALgPVgCjNp01FNXcBlKi7y97ziXr7o4d5Q=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "4.2.1 (8C148)";
                iFaith.xml_ipsw_md5 = "93957e7bd21f0549b60a60485c13206a";
            }
            else if (box.Text.Contains("QAAAADi1QgA+U7T8FsOvcPA6hPYBPkTdI4RrWQ=="))
            {
                iFaith.xml_board = "n72ap";
                iFaith.xml_ios = "4.2.1 (8C148)";
                iFaith.xml_ipsw_md5 = "0045e3543647e23470b84c2c1de96ab1";
            }
            else if (box.Text.Contains("QAAAAPgWSQAS18r1qLZdkZA0c1co6Y1EDCjRTQ=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "4.2.1 (8C148)";
                iFaith.xml_ipsw_md5 = "25dbf5b3e5ca39edd0aab8fcab888503";
            }
            else if (box.Text.Contains("QAAAAHjVVADu8xJUkTHh01ZgS3EF/YBoV2a08g=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "4.2.1 (8C148)";
                iFaith.xml_ipsw_md5 = "14d1508954532e91172f8704fd941a93";
            }
            else if (box.Text.Contains("QAAAAPhCVgD5ODdrY+RbJK1t0ehhFR7KnkWzRQ=="))
            {
                iFaith.xml_board = "k48ap";
                iFaith.xml_ios = "4.2.1 (8C148)";
                iFaith.xml_ipsw_md5 = "9402d5f05348fd68c87f885ff4cb4717";
            }
            else if (box.Text.Contains("QAAAAPhnSwAjlHUuaJuV2VnkWUzosmmCtVuqgg=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "4.2.1 (8C154)";
                iFaith.xml_ipsw_md5 = "3fe1a01b8f5c8425a074ffd6deea7c86";
            }
            else if (box.Text.Contains("QAAAAPg8WAAVReQaMyfzWgu/T+t3BRVWiqVjvg=="))
            {
                iFaith.xml_board = "n92ap";
                iFaith.xml_ios = "4.2.6 (8E200)";
                iFaith.xml_ipsw_md5 = "eb3c205debb52c237c37f92335e6369c";
            }
            else if (box.Text.Contains("QAAAALgoWAAKGOgg+bVLkvZHkfrhYQ0kiB2+aQ=="))
            {
                iFaith.xml_board = "n92ap";
                iFaith.xml_ios = "4.2.7 (8E303)";
                iFaith.xml_ipsw_md5 = "30fc03783453d23aaa0d13f89fd36c28";
            }
            else if (box.Text.Contains("QAAAALgoWABhTNEsE/87WIah/KxGUMCUeaNbyw=="))
            {
                iFaith.xml_board = "n92ap";
                iFaith.xml_ios = "4.2.8 (8E401)";
                iFaith.xml_ipsw_md5 = "0e0e4bf8f0d7c37b9a252fcbed60ac0c";
            }
            else if (box.Text.Contains("QAAAAPgoWAAjq5elrkkyrM6iDtGLmXimVhuz+g=="))
            {
                iFaith.xml_board = "n92ap";
                iFaith.xml_ios = "4.2.9 (8E501)";
                iFaith.xml_ipsw_md5 = "1bf3fdd8566c67e0961c06b8a2abfc65";
            }
            else if (box.Text.Contains("QAAAALgoWABlq6+ptX5m3mUCLmb9ynyb9Buazg=="))
            {
                iFaith.xml_board = "n92ap";
                iFaith.xml_ios = "4.2.10 (8E600)";
                iFaith.xml_ipsw_md5 = "2c0dd880982f0f8e47dc3dadfb733ad7";
            }
            else if (box.Text.Contains("QAAAADh+UwDFQKniMuWdfAaNqN/hkce9mVEqrQ=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "4.3 (8F191m)";
                iFaith.xml_ipsw_md5 = "85647af7e281cfca4f4e0d1c412f668f";
            }
            else if (box.Text.Contains("QAAAAHhCUgBHNj9qKW5JiBpJzPAih19g1NQ3LQ=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "4.3 (8F190)";
                iFaith.xml_ipsw_md5 = "87ebb9b2c025fb5f87a4cab0631b1547";
            }
            else if (box.Text.Contains("QAAAADiIXgDrD6/aVttwdq0TtH4qlxwtnBm3Ig=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "4.3 (8F190)";
                iFaith.xml_ipsw_md5 = "e0a463bded8f5b1e076b466535b18c75";
            }
            else if (box.Text.Contains("QAAAALgkUQB4K7BjKF8NY07yEAS3nXUCaGF3dg=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "4.3 (8F190)";
                iFaith.xml_ipsw_md5 = "43383f2d5cd181f2af1e01ec62a3f1d6";
            }
            else if (box.Text.Contains("QAAAAHi9XAA8db7WCZL+6WJK7d8Si8EkQDwfjQ=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "4.3 (8F190)";
                iFaith.xml_ipsw_md5 = "0c8cdbbb729508811fa5bd29d8e1143b";
            }
            else if (box.Text.Contains("QAAAADgqXgB7trCZXN8OtigkYdMv5JQ+1+xcXg=="))
            {
                iFaith.xml_board = "k48ap";
                iFaith.xml_ios = "4.3 (8F190)";
                iFaith.xml_ipsw_md5 = "9a889ba48bc2715292f199f50c70ed60";
            }
            else if (box.Text.Contains("QAAAALiMUwC00hHIgsA4mYYqn++PTndf1lEtmA=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "4.3 (8F202)";
                iFaith.xml_ipsw_md5 = "893cdf844a49ae2f7368e781b1ccf6d1";
            }
            else if (box.Text.Contains("QAAAADiMUwDAcGLiemI/aZCmQO7v15HnBy/RxA=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "4.3 (8F305)";
                iFaith.xml_ipsw_md5 = "4726cfb30f322f8cdbb5f20df7ca836f";
            }
            else if (box.Text.Contains("QAAAAHiPUwCJhDqTEGMtfvm+jokHXPXlIpyoHg=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "4.3 (8F455)";
                iFaith.xml_ipsw_md5 = "785f859b63edd329e9b5039324ebaf49";
            }
            else if (box.Text.Contains("QAAAAHhBUgAJUPpBno8rtOztBgDgbZi4oFbpCw=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "4.3.1 (8G4)";
                iFaith.xml_ipsw_md5 = "694c93b5b608513136ba8956dff28ba7";
            }
            else if (box.Text.Contains("QAAAADiHXgB5tKzY64AoAaA0+E3vtplIBhLpqQ=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "4.3.1 (8G4)";
                iFaith.xml_ipsw_md5 = "32f9a71430c4dd025adab3b73d4a5242";
            }
            else if (box.Text.Contains("QAAAAPgjUQBfNkyYmCvRAvzwquLtlSW6GgtMZw=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "4.3.1 (8G4)";
                iFaith.xml_ipsw_md5 = "47827ca8d127f28663d5b70b0784236e";
            }
            else if (box.Text.Contains("QAAAAHi8XADgyRY0/T5e1bEEjf/8vDGC8ETKVg=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "4.3.1 (8G4)";
                iFaith.xml_ipsw_md5 = "b0e356267a1407e4d7a7b0f48a07c5c2";
            }
            else if (box.Text.Contains("QAAAADgpXgDB3YK3YObRU2GmZLPKO3Wo69JQ5w=="))
            {
                iFaith.xml_board = "k48ap";
                iFaith.xml_ios = "4.3.1 (8G4)";
                iFaith.xml_ipsw_md5 = "fe4f80f8ff2fa298559b392b64e84bb8";
            }
            else if (box.Text.Contains("QAAAALhbUgBrN6aDBCtixSYWCpxecVRgPpqr0Q=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "4.3.2 (8H7)";
                iFaith.xml_ipsw_md5 = "7c1c714f24a89c2f2c71e26d37cde3f0";
            }
            else if (box.Text.Contains("QAAAADiiXgBG04O5rgx4RxynOngnwuI0lCDVRA=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "4.3.2 (8H7)";
                iFaith.xml_ipsw_md5 = "8cb3a9964a2a99414030f662d3009deb";
            }
            else if (box.Text.Contains("QAAAADg+UQDg8btrqsdoeg5MhMSnL+wHwV8mVw=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "4.3.2 (8H7)";
                iFaith.xml_ipsw_md5 = "7f831b30d33f80c7f92442cb041227ab";
            }
            else if (box.Text.Contains("QAAAALjWXADGxQXiPa++IzpQTnUIL0QJSlZZDw=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "4.3.2 (8H7)";
                iFaith.xml_ipsw_md5 = "4a002a4596a681efd9cdbf6f2fd72e74";
            }
            else if (box.Text.Contains("QAAAALhDXgDbeN/y3JibU2nlsdh/A3qLCAvq4A=="))
            {
                iFaith.xml_board = "k48ap";
                iFaith.xml_ios = "4.3.2 (8H7)";
                iFaith.xml_ipsw_md5 = "24027c4381a6cdfdd8a03a17177d1d6c";
            }
            else if (box.Text.Contains("QAAAALhbUgDfI3qcafxhZoKn3CzBr0j4PNy6jg=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "4.3.3 (8J2)";
                iFaith.xml_ipsw_md5 = "d9a02961311ffac8197e8db3b48e449d";
            }
            else if (box.Text.Contains("QAAAADiiXgDWsreRDaepTYgwJuduVeMQvh65CA=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "4.3.3 (8J2)";
                iFaith.xml_ipsw_md5 = "a0cb7313c5535991d62890c7eef60f9a";
            }
            else if (box.Text.Contains("QAAAAHg+UQBfHz1ws7i/KSJO/0UY6vTq6YB55w=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "4.3.3 (8J2)";
                iFaith.xml_ipsw_md5 = "7c8d3ccaccd1573dc31d6de555b987f9";
            }
            else if (box.Text.Contains("QAAAALjWXACIyo+gy95VTpDMG10osfMGOyGqcw=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "4.3.3 (8J2)";
                iFaith.xml_ipsw_md5 = "dd5003cc00dbaa9fbf0182c5a2e5d6ed";
            }
            else if (box.Text.Contains("QAAAAPhCXgB8XF4oN/e9E0mzRQ0ILso5QdhZ+w=="))
            {
                iFaith.xml_board = "k48ap";
                iFaith.xml_ios = "4.3.3 (8J2)";
                iFaith.xml_ipsw_md5 = "d20493bb1ba0450f2ee01d081ba8eb27";
            }
            else if (box.Text.Contains("QAAAAHhbUgBXL84EpBQduGcYpvG3fjS0rzfCeg=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "4.3.4 (8K2)";
                iFaith.xml_ipsw_md5 = "a172829312329d3bda87d23abf5ec4d4";
            }
            else if (box.Text.Contains("QAAAADiiXgAXMvv6tWztSjD3QMOtLPKmKHcO5A=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "4.3.4 (8K2)";
                iFaith.xml_ipsw_md5 = "c6862a24f9166c313043459013159194";
            }
            else if (box.Text.Contains("QAAAAHg+UQDJKJlNn2eaN2EDzF9ye+KaaFKRMg=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "4.3.4 (8K2)";
                iFaith.xml_ipsw_md5 = "5ea7630383fdad9ab3adba0220b408fc";
            }
            else if (box.Text.Contains("QAAAAHjWXAAn6+C0CYc4zYN9GZdwfK+3OnbNSg=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "4.3.4 (8K2)";
                iFaith.xml_ipsw_md5 = "ed3f720c8a02ceac99adcc663f9ad409";
            }
            else if (box.Text.Contains("QAAAALhDXgDPFFtJiSmSCFtRa3WP4ZyFLBneDg=="))
            {
                iFaith.xml_board = "k48ap";
                iFaith.xml_ios = "4.3.4 (8K2)";
                iFaith.xml_ipsw_md5 = "3414ec10c6e7f50d8eca5d5c25d60ff0";
            }
            else if (box.Text.Contains("QAAAALhbUgDsziO9uas0DUTIrN+h1+riIUj1GA=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "4.3.5 (8L1)";
                iFaith.xml_ipsw_md5 = "a7586eaf9cce93db06c2114f23e8ed90";
            }
            else if (box.Text.Contains("QAAAAHiiXgBCd/EmDT999afVHc2IEdgf5Xdktw=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "4.3.5 (8L1)";
                iFaith.xml_ipsw_md5 = "7219c43eea7362c7a402861f71b4496e";
            }
            else if (box.Text.Contains("QAAAAHg+UQAW4U3NaxosEnidOpDzmorrkjf4Tg=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "4.3.5 (8L1)";
                iFaith.xml_ipsw_md5 = "c5c45b58618b38fb4d2fba242eb0ba1e";
            }
            else if (box.Text.Contains("QAAAAHjWXADhJBvhV+gBH3aMSYwsyUvWYCjTSA=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "4.3.5 (8L1)";
                iFaith.xml_ipsw_md5 = "c750f69c14d5fa8295edafd7212aac7d";
            }
            else if (box.Text.Contains("QAAAALhDXgD7kjRA5D+sqrzGWGFCkfmYjrJe4A=="))
            {
                iFaith.xml_board = "k48ap";
                iFaith.xml_ios = "4.3.5 (8L1)";
                iFaith.xml_ipsw_md5 = "d80101eb0b1b460e319f9124f252813c";
            }
            else if (box.Text.Contains("QAAAALg1UwAfq5jvj62az5Uja0GjXuhX+RsAiQ=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "4.4 (9A334v)";
                iFaith.xml_ipsw_md5 = "eca151961d28fd0e085b2645a51c1755";
            }
            else if (box.Text.Contains("QAAAAPg1UwD3U0YOoWw1GgOd8pZ4w7RGD6tElQ=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "4.4.1 (9A335a)";
                iFaith.xml_ipsw_md5 = "a2c06f31d13bdd63f768a8991a2f64e5";
            }
            else if (box.Text.Contains("QAAAADg2UwD8KnplVeZGaUDnmRhiOcbE2b8cIg=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "4.4.2 (9A336a)";
                iFaith.xml_ipsw_md5 = "c64da86de651b049cc8d2cd06f234a3d";
            }
            else if (box.Text.Contains("QAAAAPg7UwAbAG66RMniJ8EVeV9aMFrNbxNe1g=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "4.4.3 (9A405l)";
                iFaith.xml_ipsw_md5 = "9227a33a560ea4dcaff3dabb33424df2";
            }
            else if (box.Text.Contains("QAAAADg4UwCVJHFqTbSzsVkF4RDNFIEjImLiJQ=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "4.4.4 (9A406a)";
                iFaith.xml_ipsw_md5 = "ea805b0aaf9f8e79d8dedf3e009f907a";
            }
            else if (box.Text.Contains("QAAAAHhlYwBlmtqPDqkBdYre11O0Omjtop9TuA=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "5.0 (9A334)";
                iFaith.xml_ipsw_md5 = "8ac7ae6a7f6fadbd34ba9abacd15e2dc";
            }
            else if (box.Text.Contains("QAAAALizYgAm8BK757MnpRB9WjGcCUtrwMj3pw=="))
            {
                iFaith.xml_board = "n92ap";
                iFaith.xml_ios = "5.0 (9A334)";
                iFaith.xml_ipsw_md5 = "ac3985f710f3a0fe373881c98d322667";
            }
            else if (box.Text.Contains("QAAAAHiiVABd2E4Edrjq3xGQOkVEQlMFl+Pcfg=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "5.0 (9A334)";
                iFaith.xml_ipsw_md5 = "54aa71d20263580653eafd06064c557c";
            }
            else if (box.Text.Contains("QAAAAPhMUwDvwCKYBfj+AW0sgWxIY7Cn9SxNtg=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "5.0 (9A334)";
                iFaith.xml_ipsw_md5 = "989b8327acab76e7632443a0e179250c";
            }
            else if (box.Text.Contains("QAAAAHjbXQANj0vVSZfbKx08FzTqlWzSCkdL6g=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "5.0 (9A334)";
                iFaith.xml_ipsw_md5 = "29ead00d2726f688d393a5fdd0de4a97";
            }
            else if (box.Text.Contains("QAAAAHjEYQBLlYVzhJ/l2d5tZlKEWLKrRFxaAA=="))
            {
                iFaith.xml_board = "k48ap";
                iFaith.xml_ios = "5.0 (9A334)";
                iFaith.xml_ipsw_md5 = "729c72a9880b82b37aef5997173d9578";
            }
            else if (box.Text.Contains("QAAAAPjEYQDRcL5EELOljAnXh3YczPHSgq18yw=="))
            {
                iFaith.xml_board = "k48ap";
                iFaith.xml_ios = "5.0.1 (9A405)";
                iFaith.xml_ipsw_md5 = "0cd4ebbf9a94dd8b67623941bd2add62";
            }
            else if (box.Text.Contains("QAAAADilVABgAmMy3czmYa+FxLklHK2cBxoucw=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "5.0.1 (9A405)";
                iFaith.xml_ipsw_md5 = "812a3dd8c6eb849cec62d16ed7e20ac9";
            }
            else if (box.Text.Contains("QAAAALhmYwCgK339fnnA1D6AzpFkWBgSOk0jjw=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "5.0.1 (9A405)";
                iFaith.xml_ipsw_md5 = "e9500cfaa75e4a0a5ef88c3d92a21563";
            }
            else if (box.Text.Contains("QAAAALi0YgCNUZLEFoZ4cwVR2Z7LG2JyxG+ZCA=="))
            {
                iFaith.xml_board = "n92ap";
                iFaith.xml_ios = "5.0.1 (9A405)";
                iFaith.xml_ipsw_md5 = "70aa97b9b4902ae32ea9d3d7850a5c58";
            }
            else if (box.Text.Contains("QAAAALhPUwBUkLp9T4j0DDPZEcCpaI1kfbpnCw=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "5.0.1 (9A405)";
                iFaith.xml_ipsw_md5 = "c13c14abcde18bbdb7d70c8563f56ac1";
            }
            else if (box.Text.Contains("QAAAADjfXQDPtZiq35EzQxYP+brZ6E7eIb4UeA=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "5.0.1 (9A405)";
                iFaith.xml_ipsw_md5 = "ba36e09d830a088b10e4a342d372a6c7";
            }
            else if (box.Text.Contains("QAAAADjEYAAD64Jm7nJtzTBb+EnWk+MKwzT3sA=="))
            {
                iFaith.xml_board = "k48ap";
                iFaith.xml_ios = "5.1 (9B176)";
                iFaith.xml_ipsw_md5 = "1259442021913f1d9f0fffd006e3d0de";
            }
            else if (box.Text.Contains("QAAAAPhFUwBtxwVAQc1J0lEOSP+ER3dGhmY7Bg=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "5.1 (9B176)";
                iFaith.xml_ipsw_md5 = "51e253431b1080cbbb5073efc87b1bb3";
            }
            else if (box.Text.Contains("QAAAADhrYgAa5KTI69N9qioOVtMe0bd/6Lr3Zg=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "5.1 (9B176)";
                iFaith.xml_ipsw_md5 = "706b8987b37bc99e349197ecc5e26c3b";
            }
            else if (box.Text.Contains("QAAAALi3YQDAz7YPWBRyNX2KSVnL+e33ZFcBhQ=="))
            {
                iFaith.xml_board = "n92ap";
                iFaith.xml_ios = "5.1 (9B176)";
                iFaith.xml_ipsw_md5 = "304a0863e83aff0f88215e3f6a193002";
            }
            else if (box.Text.Contains("QAAAAHjtUQAjDANtiePoc+pDsvlnDE/n+Z267A=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "5.1 (9B176)";
                iFaith.xml_ipsw_md5 = "ec5debe71307ce8926499029ad35a450";
            }
            else if (box.Text.Contains("QAAAAPjkXAAXvvuA5uormijgVAnfOe8v0icwTQ=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "5.1 (9B176)";
                iFaith.xml_ipsw_md5 = "3eeb95d6b80b50d33708471d2e1b9306";
            }
            else if (box.Text.Contains("QAAAAHgRUwDE8rJ9whoSfRDOvZDBIDt1JrwVAQ=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "5.0 (9B179b)";
                iFaith.xml_ipsw_md5 = "2120a21b0ec6a8991078b1b88934baee";
            }
            else if (box.Text.Contains("QAAAALgTUwAotz74b6WXyszr5NOXKCMtL6zJ2A=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "5.0 (9B206f)";
                iFaith.xml_ipsw_md5 = "59bdbc355b5d0e17671edf5d6a069ce1";
            }
            else if (box.Text.Contains("QAAAAPjGYAB6Gt26rTnHvLUovJazoLFsthpPGw=="))
            {
                iFaith.xml_board = "k48ap";
                iFaith.xml_ios = "5.1.1 (9B206)";
                iFaith.xml_ipsw_md5 = "859a6ded3129af1ed0d56f3e25c873a2";
            }
            else if (box.Text.Contains("QAAAADhJUwBB8KdTLrCsIXu2icTtH23NneQ8MQ=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "5.1.1 (9B206)";
                iFaith.xml_ipsw_md5 = "38312b9d4da5decce7e6a4f042ac9472";
            }
            else if (box.Text.Contains("QAAAADhtYgAYagofryCUjZiq03EOGFVCSM9snw=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "5.1.1 (9B206)";
                iFaith.xml_ipsw_md5 = "a4f25b2af99580f1e69c8277d0208237";
            }
            else if (box.Text.Contains("QAAAAHhpYgDEOwP3A8+Yov+iTIG83QYzW680MA=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "5.1.1 (9B208)";
                iFaith.xml_ipsw_md5 = "89d5c926c7e4c2fba4d31c55cc25b8bf";
            }
            else if (box.Text.Contains("QAAAADi6YQDEmcn6yNoU1y91gu0CiWWA5Zba4w=="))
            {
                iFaith.xml_board = "n92ap";
                iFaith.xml_ios = "5.1.1 (9B206)";
                iFaith.xml_ipsw_md5 = "730d2a2c760c3b8267258447ff19a064";
            }
            else if (box.Text.Contains("QAAAAPjvUQA0n64z+v0Od0AfC+PBQnV2ddUGXg=="))
            {
                iFaith.xml_board = "n18ap";
                iFaith.xml_ios = "5.1.1 (9B206)";
                iFaith.xml_ipsw_md5 = "5dfa8310e4a62ce0ce57259b55cabb07";
            }
            else if (box.Text.Contains("QAAAAPjmXAA7R5eS1E78hrhxvZkgu8vErs/2vg=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "5.1.1 (9B206)";
                iFaith.xml_ipsw_md5 = "4aadc473058136d1ea37ec3436eb28ef";
            }
            else if (box.Text.Contains("QAAAAPgTUwDn5oyAJdQbAzDT0r5XR0AY0VYPdg=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "5.0.2 (9B830)";
                iFaith.xml_ipsw_md5 = "faa71fa66e0cdfbf3e4d13a5f496b270";
            }
            else if (box.Text.Contains("QAAAALhQbwDKWiLWPkWljUBUd5QJNRjL/JcY7w=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "6.0 (10A403)";
                iFaith.xml_ipsw_md5 = "21e199beb88199bb542cf48532e42d1f";
            }
            else if (box.Text.Contains("QAAAADhkZQDBy/PPyeShYZo2sUXYquD2ZkFI3A=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "6.0 (10A403)";
                iFaith.xml_ipsw_md5 = "80d8c49b2587127c77d391703fbc2bba";
            }
            else if (box.Text.Contains("QAAAADhucQBqh7Lbm7m8P+yYEHDLvAgrCjOIXQ=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "6.0 (10A403)";
                iFaith.xml_ipsw_md5 = "7323c6b0bac1a367d1ae59b2897b7e89";
            }
            else if (box.Text.Contains("QAAAALhZcgDbnvC/JBeP7Fsgh8OMNF2nGyHg5g=="))
            {
                iFaith.xml_board = "n92ap";
                iFaith.xml_ios = "6.0 (10A403)";
                iFaith.xml_ipsw_md5 = "0003dc6b9dacca06f6325a7af0334b49";
            }
            else if (box.Text.Contains("QAAAALiHcQA9P9wj4vhF3r9PNk+21oKCk4bFaw=="))
            {
                iFaith.xml_board = "n90bap";
                iFaith.xml_ios = "6.0 (10A403)";
                iFaith.xml_ipsw_md5 = "57206fff6428b27a62932837c18fc1cd";
            }
            else if (box.Text.Contains("QAAAAPhjZQABhWWzlsiFiWz4iZSCQiu4Y1ulYQ=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "6.0.1 (10A523)";
                iFaith.xml_ipsw_md5 = "42c1e6ec73d5a8c340f2f4963a016a64";
            }
            else if (box.Text.Contains("QAAAAPhucQBjPiXDS1/hdO7UITvb3gpih6lGgQ=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "6.0.1 (10A523)";
                iFaith.xml_ipsw_md5 = "83548080f405e81e1223acb4d3f8bccc";
            }
            else if (box.Text.Contains("QAAAADiIcQD0uiji0O0HoVTzHL3S3OvY9t+kzA=="))
            {
                iFaith.xml_board = "n90bap";
                iFaith.xml_ios = "6.0.1 (10A523)";
                iFaith.xml_ipsw_md5 = "3ded74ea874786412cfb3ede58b74d36";
            }
            else if (box.Text.Contains("QAAAAPhZcgDZBJaPiYxKz7JIIyizmpTlIo1EkA=="))
            {
                iFaith.xml_board = "n92ap";
                iFaith.xml_ios = "6.0.1 (10A523)";
                iFaith.xml_ipsw_md5 = "5b8590ced08f275ad5446a16e68b77a5";
            }
            else if (box.Text.Contains("QAAAADhRbwDnpgJa3fDJgHdtAlVuKaziFdr8QA=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "6.0.1 (10A523)";
                iFaith.xml_ipsw_md5 = "e9fc36dedc8857129c53b261ae4e1802";
            }
            else if (box.Text.Contains("QAAAAPg5YAAGY8Eez4SLfLwSDDalF/gmSysHEA=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "5.1 (10A406e)";
                iFaith.xml_ipsw_md5 = "675ff9546ef2797b571e7023591317ca";
            }
            else if (box.Text.Contains("QAAAAHg6YAAJzzV881MTE3oansePdpX48IvJ7g=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "5.1.1 (10A831)";
                iFaith.xml_ipsw_md5 = "dde4fa0286fc3b802e6e48751e7f9f44";
            }
            else if (box.Text.Contains("QAAAADjfXwCsKwebeygg7bcrFYnbSJSkbkNMNg=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "5.2 (10B144b)";
                iFaith.xml_ipsw_md5 = "1f88e97e4e869b53d8be6ee7ef2e58ee";
            }
            else if (box.Text.Contains("QAAAADjmXwCBnKureOFjkReLn0Htvp7rJWXqTQ=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "5.3 (10B809)";
                iFaith.xml_ipsw_md5 = "88b6cc22c4482562c0c056ac47da7d05";
            }
            else if (box.Text.Contains("QAAAAHgKZQAbxKAbPIe2+Xmt1U/epEwe10msjA=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "6.1 (10B141)";
                iFaith.xml_ipsw_md5 = "7e372c8bbae7f0cb8e77c328c091190c";
            }
            else if (box.Text.Contains("QAAAALgIcQC4EoFn916OD/dnaj4TYmiZT30Zmw=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "6.1 (10B144)";
                iFaith.xml_ipsw_md5 = "f5f6588902174948c3e9d4f9b3a89e5d";
            }
            else if (box.Text.Contains("QAAAALgjcQA2UqfTOxXycHT0ypIvHeLG3iz8JQ=="))
            {
                iFaith.xml_board = "n90bap";
                iFaith.xml_ios = "6.1 (10B144)";
                iFaith.xml_ipsw_md5 = "5a14a53b21c8c01621117d57643dcf73";
            }
            else if (box.Text.Contains("QAAAAPgGcgCyGYH5aTWoqTe2M41BYriEw2Vq2A=="))
            {
                iFaith.xml_board = "n92ap";
                iFaith.xml_ios = "6.1 (10B141)";
                iFaith.xml_ipsw_md5 = "f94dec674a21f295d78e9c09e2f4ce63";
            }
            else if (box.Text.Contains("QAAAAPjkbgDIO8CzTenrfujFtIGljml9ayy3Lg=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "6.1 (10B144)";
                iFaith.xml_ipsw_md5 = "b41c5b614983ea1c0696369fb0af5e17";
            }
            else if (box.Text.Contains("QAAAAHgKZQBevyjTDjaTfr7deM4Noc7hGRSI+w=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "6.1.2 (10B146)";
                iFaith.xml_ipsw_md5 = "96daf1876e35399dc6954db998e1463c";
            }
            else if (box.Text.Contains("QAAAADgacQA6I/Q/UzJW7o9ZO7bp1Cau3odETQ=="))
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "6.1.2 (10B146)";
                iFaith.xml_ipsw_md5 = "65c85f58df8bb68b00aebdc46081248c";
            }
            else if (box.Text.Contains("QAAAADg1cQDtWdC1LO3lNoaZpxigbWXQ78hHtA=="))
            {
                iFaith.xml_board = "n90bap";
                iFaith.xml_ios = "6.1.2 (10B146)";
                iFaith.xml_ipsw_md5 = "2184c60a04b4dd221136d307976c1dec";
            }
            else if (box.Text.Contains("QAAAAPgGcgC5XTjEbEdPl3poUmdQdsEOn7Ac+w=="))
            {
                iFaith.xml_board = "n92ap";
                iFaith.xml_ios = "6.1.2 (10B146)";
                iFaith.xml_ipsw_md5 = "35f587e5943e74fc1fb571a6d30eb003";
            }
            else if (box.Text.Contains("QAAAAPj1bgCf5BdXd/D7UukUWQi0jH4kYECHvw=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "6.1.2 (10B146)";
                iFaith.xml_ipsw_md5 = "e4ce2e3d90ba2f2fe3b24a5c1ce412b2";
            }
            else if (box.Text.Contains("QAAAADjlXwBe4vFqhPhsPO2OM50Mx5IKDLh06Q=="))
            {
                iFaith.xml_board = "k66ap";
                iFaith.xml_ios = "5.2.1 (10B329a)";
                iFaith.xml_ipsw_md5 = "4d8b9d5c193a5e22d6c7b73e8632d26c";
            }
            else if (box.Text.Contains("QAAAALgPZQA9Xlr7K4ijM4OzxHMjPQfcdloe+Q=="))
            {
                iFaith.xml_board = "n88ap";
                iFaith.xml_ios = "6.1.3 (10B329)";
                iFaith.xml_ipsw_md5 = "fa4605a61404b45c2087a360b692d531";
            }
            else if (box.Text.Contains("QAAAAPggcQDHm31123T5nGzCyMSmXi+s6r5Orw==")) //iPhone 4 iOS 6.1.3 (Partial Digest)
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "6.1.3 (10B329)";
                iFaith.xml_ipsw_md5 = "845a42c0139cdad6d373bf3414b25786";
            }
            else if (box.Text.Contains("YxqssnFubLDOxmtM4zwP/0T20wc=")) //iPhone 4 iOS 7.0 (Digest)
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "7.0 (11A465)";
                iFaith.xml_ipsw_md5 = "36f803242a01f35e47b9227babccd8c3";
            }
            else if (box.Text.Contains("CD4oe8MH6Mwmxhk7Aw2WZPoEWJY=")) //iPhone 4 iOS 7.0.2 (Digest)
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "7.0.2 (11A501)";
                iFaith.xml_ipsw_md5 = "1c51e1883bc5b3017077672c8c20aa17";
            }
            else if (box.Text.Contains("pG01qABFSdDiIK8y70YQTZciDkU=")) //iPhone 4 iOS 7.0.3 (Digest)
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "7.0.3 (11B511)";
                iFaith.xml_ipsw_md5 = "46b107f3c3c29d842f767b991001e23e";
            }
            else if (box.Text.Contains("q3Zc4oekFqUR3bfuwTzA6lgdhFk=")) //iPhone 4 iOS 7.0.4 (Digest)
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "7.0.4 (11B554a)";
                iFaith.xml_ipsw_md5 = "103619647ccbcd365c0ad816280133c1";
            }
            else if (box.Text.Contains("AYFSEt/2mzZCyybeLNhIbWPKQoE=")) //iPhone 4 iOS 7.0.6 (Digest)
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "7.0.6 (11B651)";
                iFaith.xml_ipsw_md5 = "3d0d8227bd07c3b0e0d7549a0ecd463d";
            }
            else if (box.Text.Contains("AD0A6xb0y9cp66audOFJQOPZ4l8=")) //iPhone 4 iOS 7.1 (Digest)
            {
                iFaith.xml_board = "n90ap";
                iFaith.xml_ios = "7.1 (11D169)";
                iFaith.xml_ipsw_md5 = "9b41651768dd716472577646ed1876db";
            }
            else if (box.Text.Contains("QAAAAPg7cQC0vgS/XPHYjSL8boOMzdLUp3orJg=="))
            {
                iFaith.xml_board = "n90bap";
                iFaith.xml_ios = "6.1.3 (10B329)";
                iFaith.xml_ipsw_md5 = "97c001bf6e4bcf5d832af97b762cae24";
            }
            else if (box.Text.Contains("QAAAADgNcgADzWlAgmKpFIPeAfgvs/jD84tF1A=="))
            {
                iFaith.xml_board = "n92ap";
                iFaith.xml_ios = "6.1.3 (10B329)";
                iFaith.xml_ipsw_md5 = "db301240840daee619d98d87f6c89921";
            }
            else if (box.Text.Contains("QAAAAPj7bgCjXxFCDGB38bPA9+nwGsBro9a3Rw=="))
            {
                iFaith.xml_board = "n81ap";
                iFaith.xml_ios = "6.1.3 (10B329)";
                iFaith.xml_ipsw_md5 = "fadffd38b7de78b715eb7f3907042226";
            }
            else
            {
                iFaith.xml_revision = "GTFO";
            }
            if (iFaith.xml_board == "n72ap")
            {
                iFaith.xml_model = "iPod Touch 2G";
            }
            else if (iFaith.xml_board == "n18ap")
            {
                iFaith.xml_model = "iPod Touch 3";
            }
            else if (iFaith.xml_board == "n81ap")
            {
                iFaith.xml_model = "iPod Touch 4";
            }
            else if (iFaith.xml_board == "n88ap")
            {
                iFaith.xml_model = "iPhone 3GS";
            }
            else if (((iFaith.xml_board == "n90ap") | (iFaith.xml_board == "n92ap")) | (iFaith.xml_board == "n90bap"))
            {
                iFaith.xml_model = "iPhone 4";
            }
            else if (iFaith.xml_board == "n94ap")
            {
                iFaith.xml_model = "iPhone 4S";
            }
            else if (iFaith.xml_board == "k48ap")
            {
                iFaith.xml_model = "iPad 1G";
            }
            else if (iFaith.xml_board == "k66ap")
            {
                iFaith.xml_model = "Apple TV 2";
            }
            else if (iFaith.xml_board == "k93ap")
            {
                iFaith.xml_model = "iPad 2 [WiFi]";
            }
            else if (iFaith.xml_board == "k93aap")
            {
                iFaith.xml_model = "iPad 2 [WiFi-Rev2]";
            }
            else if (iFaith.xml_board == "k94ap")
            {
                iFaith.xml_model = "iPad 2 [GSM]";
            }
            else if (iFaith.xml_board == "k95ap")
            {
                iFaith.xml_model = "iPad 2 [CDMA]";
            }
            else if (iFaith.xml_board == "j1ap")
            {
                iFaith.xml_model = "iPad 3 [WiFi]";
            }
            else if (iFaith.xml_board == "j2ap")
            {
                iFaith.xml_model = "iPad 3 [CDMA]";
            }
            else if (iFaith.xml_board == "j2aap")
            {
                iFaith.xml_model = "iPad 3 [GSM]";
            }
            else if ((iFaith.xml_board == "j33ap") | (iFaith.xml_board == "j33iap"))
            {
                iFaith.xml_model = "Apple TV 3";
            }
            else
            {
                iFaith.xml_model = "Unknown";
            }
            iFaith.xml_apticket = Conversions.ToString(SubParserSHSHRaw("APTicket", box.Text));
            iFaith.blob_logo = Conversions.ToString(SubParserSHSHRaw("AppleLogo", box.Text));
            iFaith.blob_glyc = Conversions.ToString(SubParserSHSHRaw("BatteryCharging", box.Text));
            iFaith.blob_chg0 = Conversions.ToString(SubParserSHSHRaw("BatteryCharging0", box.Text));
            iFaith.blob_chg1 = Conversions.ToString(SubParserSHSHRaw("BatteryCharging1", box.Text));
            iFaith.blob_batf = Conversions.ToString(SubParserSHSHRaw("BatteryFull", box.Text));
            iFaith.blob_bat0 = Conversions.ToString(SubParserSHSHRaw("BatteryLow0", box.Text));
            iFaith.blob_bat1 = Conversions.ToString(SubParserSHSHRaw("BatteryLow1", box.Text));
            iFaith.blob_glyp = Conversions.ToString(SubParserSHSHRaw("BatteryPlugin", box.Text));
            iFaith.blob_dtre = Conversions.ToString(SubParserSHSHRaw("DeviceTree", box.Text));
            iFaith.blob_krnl = Conversions.ToString(SubParserSHSHRaw("KernelCache", box.Text));
            iFaith.blob_illb = Conversions.ToString(SubParserSHSHRaw("LLB", box.Text));
            iFaith.blob_ibss = Conversions.ToString(SubParserSHSHRaw("iBSS", box.Text));
            iFaith.blob_ibec = Conversions.ToString(SubParserSHSHRaw("iBEC", box.Text));
            iFaith.blob_nsrv = Conversions.ToString(SubParserSHSHRaw("NeedService", box.Text));
            iFaith.blob_recm = Conversions.ToString(SubParserSHSHRaw("RecoveryMode", box.Text));
            iFaith.blob_ibot = Conversions.ToString(SubParserSHSHRaw("iBoot", box.Text));
            iFaith.xml_ecid = Conversions.ToString(ParseECIDFromRawSHSHBlob(iFaith.blob_krnl));
        }

        public static void IPSW_vars()
        {
            if (iFaith.xml_ipsw_md5 == "38638d6056b53f2d87a0f5fcb5584cdd")
            {
                iBSSIV = "421e5f0f481a3344c9f04c45a13482da";
                iBSSKey = "b831608d0ff9f84d3c35d5c3849cd147808763646ce7695c8bbc39adccb112fb";
                RamdiskIV = "74b6379adc29a078a69c502bfc850b51";
                RamdiskKey = "cbc20b0045f8ee317b08f3481f4f255f77ab48745546c7095e8bbb696746638b";
                rootfs_key = "b9cd10dd88ab615c1963e8aa04950b12dd64e0e5b11ea63c79a02af6db62334c710d21da";
                rootfs_size = "650";
            }
            else if (iFaith.xml_ipsw_md5 == "e0c97bdbb9efbf411b22a81327ad48dc")
            {
                iBSSIV = "9036da1779c8d2a2a3b460b0a52146b1";
                iBSSKey = "ea15c674bf6086a4a3840c310521dd24";
                RamdiskIV = "8e84afa0b935d969ced792b5c91c4018";
                RamdiskKey = "b3978d4668c85b11e9c16628f3bfe96c";
                rootfs_key = "abef664a55de10472c076fa633f47a7c3567239e9af3c73dac4c8683c75f3be27b508eb2";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "4ad01a2c6fc82bcac2300253b0368f6e")
            {
                iBSSIV = "e1b99c5b491a6c9dc2ec421a10853b1e";
                iBSSKey = "5685c34824a7873ef1dca42dec535efbf202e0e90915d2acc8c36a78283671b8";
                RamdiskIV = "663139ba9ff0d04dbbd68534629994b7";
                RamdiskKey = "7345a9977b97937c4fca2f53a1475aff8d5d39e3b91191110b233be4be7d281c";
                rootfs_key = "de14c16e21ad5bb12fe572ca9400d29a4443ff208ec49c120ad72d6c3269fd5553047cdd";
                rootfs_size = "700";
            }
            else if (iFaith.xml_ipsw_md5 == "089769d37b846917394ffe11da9d2e17")
            {
                iBSSIV = "41639d34547ae3dd7921bf3539dba529";
                iBSSKey = "9121de4a038675d92e1a28683b2138b7a3bdb80994273d090398051c7f5af53c";
                RamdiskIV = "fd19726dc6b555b6bb4dbbcd91d1e7c0";
                RamdiskKey = "fb2792b935fb9cd183341cb24539376556f8b7b8f887eb90fcebaa0daf2d6d9c";
                rootfs_key = "47d76295817f74953f8e557b4917fe2201e9778a9900e43fbf311a83f176fe521b996a4b";
                rootfs_size = "650";
            }
            else if (iFaith.xml_ipsw_md5 == "35c66be376201082a005f0a289f26a65")
            {
                iBSSIV = "083528a985c2e3f90f8324e1e9dce4e4";
                iBSSKey = "c7af1cfc980b24e2464b70310e2b1713";
                RamdiskIV = "f37bd3f9597c1acba87bfd4029a08ff9";
                RamdiskKey = "de0e35e16213e69cb9c19dc527d9a96c";
                rootfs_key = "fc68c25f1dcc929f37c2be82b94e4c92b48eac3ddd67adefd462404663265e3dca43a930";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "13938eaca91e12e7cefb47717e7cadc8")
            {
                iBSSIV = "8caeaf0aac65bbf6f3f345f7a0c95f76";
                iBSSKey = "891747c374707b5cc6f5d4ffe9f0f24cc166858affc15f34d4bd0f41cb3077a0";
                RamdiskIV = "1a388bbc8ceed19a46474af07d58480b";
                RamdiskKey = "e6361c6c999807f6069871b43b2421841057dc200f5543277bf8a7e07c68f400";
                rootfs_key = "1e05ef21821280869d4029a2328836b9f60bc63907c6e951c0f1c80c2d8c66aef5c39a44";
                rootfs_size = "700";
            }
            else if (iFaith.xml_ipsw_md5 == "4117e4b22565e69205a84e9eeef0583e")
            {
                iBSSIV = "bb70b0109c0f6a323dba00df5806b111";
                iBSSKey = "a95c62b3665493c92eebf3d471ea5949827fd9aab4248cd99d66bc2edf7ac4fe";
                RamdiskIV = "50a5d7418e3091a2c1d878495a6dbc6a";
                RamdiskKey = "217c7c38387264f2a2fef7a661d1bbeb705e7c90581c5b73055fe44f5bbc0498";
                rootfs_key = "9b3fd35bad7d5307d85ce4b38b8e56bd680ef5a72a8f3b615f8d4f16c14bdcf3c3b24c6c";
                rootfs_size = "650";
            }
            else if (iFaith.xml_ipsw_md5 == "33df8d6ae5d8a695bba267ae89fe37f1")
            {
                iBSSIV = "656dd7144de645da835aaedf9354e153";
                iBSSKey = "6021f2e35db67c4aaec9c1f27a4226bf";
                RamdiskIV = "2e7aa9f6dc823657f1c930a00db8efe1";
                RamdiskKey = "9781d55350f58c3ff72c7a3e50b70288";
                rootfs_key = "2360d83b606481a5ca080fe7a6fc64f8d5a5556413dfcf3e1277fe564734ee0b188798b8";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "a73de2cfafef3463e9afa491f20c5213")
            {
                iBSSIV = "cc884d83083849382cb33c59d61ca427";
                iBSSKey = "1750aa8b05b8bf78c76d35784f690b3684bf5107485db09a17e284974fbd695a";
                RamdiskIV = "ed40b5cf7bc9f3eb2db62b8f11d86a7d";
                RamdiskKey = "4ef57007bebb68849b715bfdf37d53d73192b1f5c2228947268c1deb4cad689d";
                rootfs_key = "1402974cba4702e831fb821ef9090229f7bad6fd3084fa99bfc8a76de4d839f9bf4533eb";
                rootfs_size = "670";
            }
            else if (iFaith.xml_ipsw_md5 == "2912cefa0304e5430594c576ad88d398")
            {
                iBSSIV = "b83ba3ecc1919d4dc80a560230b80910";
                iBSSKey = "eb3c9eabd45eb5701fe1998d570fa38a31ba2807918345a5c7efe0fff7ce1bea";
                RamdiskIV = "9c051576ddd94f48c324cf7ac3197fe1";
                RamdiskKey = "31e7ecd9c364414205a8fa0092cc80c0d67eae40e75ffa27b37048c42335a106";
                rootfs_key = "2be8f3a0a02f2d259c9b297cb2d156a85adf79fed4ffe88c546a42c2a47aa55f70cadebd";
                rootfs_size = "1024";
            }
            else if (iFaith.xml_ipsw_md5 == "5ccf846d96a677f42ac183f5a137dc92")
            {
                iBSSIV = "3a0ad7cf0f172bc3736af2099a4a89b4";
                iBSSKey = "0dcdc5bdc0d991020222c0e7b7d11305466f0ef831964c5fc9325f05e32b1a09";
                RamdiskIV = "3ed149b2f67690e5376880fc0323fbe2";
                RamdiskKey = "aa616f53beff0bd86dfb7aa53954614395fc13e31be97fb16e0f886f0d6cdcfc";
                rootfs_key = "c3d15c6dc3b289db4d90b59199c485486043bb534c14d21993e35f68f2c6c1804a9125a8";
                rootfs_size = "1024";
            }
            else if (iFaith.xml_ipsw_md5 == "cf6d93fffdc60dcca487a80004d250fa")
            {
                iBSSIV = "3a0ad7cf0f172bc3736af2099a4a89b4";
                iBSSKey = "0dcdc5bdc0d991020222c0e7b7d11305466f0ef831964c5fc9325f05e32b1a09";
                RamdiskIV = "309d4fc146b47f43e8bc27133d7ce6bc";
                RamdiskKey = "f45eaec8b6c17af7c6071635a313572b1b0208bc445cfb6c1d60b6bf377e6653";
                rootfs_key = "18ae1e76e7bcf6478321f42888606ca2d998cffab1ee8c7ca6b15d57b1a7254f8a608823";
                rootfs_size = "1024";
            }
            else if (iFaith.xml_ipsw_md5 == "f9819ad9a52324ac6f10e4a0ea581cbd")
            {
                iBSSIV = "d044caa6b4fe10d75e081f1950b986e7";
                iBSSKey = "16785e6ec40dcd29ce5cfb46e18d6031ac2834f4efccdce6deea967c006c8018";
                RamdiskIV = "d506ed68e4cd9955fe09be284ff5c60c";
                RamdiskKey = "adb1b9da63b54cb07e8c4a22481bf2c7dee17519506085b07b1a8057a8980003";
                rootfs_key = "5d79765bc3233cbee58727c17a9487e5dc1e38400c2a98c30997bb02d00e97ae3ce5fab8";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "8717be79fb38cd83aa5e5956eb0608b7")
            {
                iBSSIV = "91f94e5d726a2d2f2c7ffad58d4f3b77";
                iBSSKey = "d05c3c40db40e738926f811b8b1314038d26096c4102461698a49098c47a3fe6";
                RamdiskIV = "0ab135879934fdd0d689b3d0f8cf8374";
                RamdiskKey = "62aabe3e763eb3669b4922468be2acb787199c6b0ef8ae873c312e458d9b9be3";
                rootfs_key = "8b2915719d9f90ba5521faad1eadbb3d942991bd55e5a0709f26e9db3931517e054afa50";
                rootfs_size = "1024";
            }
            else if (iFaith.xml_ipsw_md5 == "41dd8ab40159a13d7be42cd7e5f3a479")
            {
                iBSSIV = "93463b2dc308eecb29a1732312a9f0c9";
                iBSSKey = "47a818705e66dc67c42002553a10adb2";
                RamdiskIV = "b2b8949bd33b65f22bd0c7e55ccc836b";
                RamdiskKey = "1e3e144a55fa8d1782db5153796238f0";
                rootfs_key = "fcada08311f553b2d7194c97922e01c821b632bf62e64500056ea37e56343e6131a9839b";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "6b9d65c9f63792968bad57e44a73434f")
            {
                iBSSIV = "27c5becf5fea4936b44ab27f22ede19b";
                iBSSKey = "2e9374bcf55a4d14495141eddfdb9cca039a7dc0913676586cb3e19bcde135db";
                RamdiskIV = "0b02c0daefed9ec93e52a20b9575b754";
                RamdiskKey = "8d9f400ee20e54947b143a64d0356c215800b83191fed8e73f3c0e84d1e32d37";
                rootfs_key = "ec6eb0268c4e9f8ab9d003f601e8f4b36f4fc4311c61e5ebed07ce718424ffee7e7d924d";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "a3104ca3b72a91bc7eff037ee320ecc5")
            {
                iBSSIV = "d044caa6b4fe10d75e081f1950b986e7";
                iBSSKey = "16785e6ec40dcd29ce5cfb46e18d6031ac2834f4efccdce6deea967c006c8018";
                RamdiskIV = "5e8b26560b3f18f4f1aa76bc6d526d86";
                RamdiskKey = "53896fea899c222b84791128d383268b579b5acd5230d2b35ed066294b2273e8";
                rootfs_key = "5d9385452d9ce0fe0185dfc59a7cbb1015d086ce53ff769e78dd45bc6e4eeb48c60e2952";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "40ebacb47fb32d7f33ba0fd596e150e9")
            {
                iBSSIV = "91f94e5d726a2d2f2c7ffad58d4f3b77";
                iBSSKey = "d05c3c40db40e738926f811b8b1314038d26096c4102461698a49098c47a3fe6";
                RamdiskIV = "5dfa31e28674d44e19ac05148ae7f668";
                RamdiskKey = "79cb7f8c64b6302a58a63a693ce0582df269fad68f6eb9d85340c1f75dbe89d6";
                rootfs_key = "ebd8aea30e78053112c4062690723fc5ee8e53865d4d6591b64a08216337c5a7aefbc806";
                rootfs_size = "1024";
            }
            else if (iFaith.xml_ipsw_md5 == "9cb5684457fb41886827d727d91313c3")
            {
                iBSSIV = "d044caa6b4fe10d75e081f1950b986e7";
                iBSSKey = "16785e6ec40dcd29ce5cfb46e18d6031ac2834f4efccdce6deea967c006c8018";
                RamdiskIV = "987c49205269a380d2b26be9c601a610";
                RamdiskKey = "87c9676ed157488e49fd638b179e8f6106396d8ab85e730031a0987918e591d5";
                rootfs_key = "812288d52a0845a41c3cd61e6b5a0f85731ce3fc04aa631895d40ca77d8f325ff02c70e9";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "790b24fe7515084f457ce413618b2709")
            {
                iBSSIV = "91f94e5d726a2d2f2c7ffad58d4f3b77";
                iBSSKey = "d05c3c40db40e738926f811b8b1314038d26096c4102461698a49098c47a3fe6";
                RamdiskIV = "b662a1cd9b51d43029624adf2c65e56a";
                RamdiskKey = "7f3ed4bd7773bd07f9f4a697b56bc85dc0040907cf1282077d477e9b5c92c53f";
                rootfs_key = "28bded3ee52eda2f36a241009a493db357b8f19543c07bd3820a35498a1788ce4aa0c54c";
                rootfs_size = "1024";
            }
            else if (iFaith.xml_ipsw_md5 == "e706efcf835de9fcf6f96c7a420a7a22")
            {
                iBSSIV = "93463b2dc308eecb29a1732312a9f0c9";
                iBSSKey = "47a818705e66dc67c42002553a10adb2";
                RamdiskIV = "5b65f57eddaffa682585258f5fab1499";
                RamdiskKey = "637842bc3a51d8cd5984d7d685bd6dd7";
                rootfs_key = "5d1655d3cd7c6ffb4a5e48a52ea8a265579c655aa39ed8613239e57f20f132e4e3b5ffa1";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "dc7741b9e4353895c3910237a5b10a4d")
            {
                iBSSIV = "27c5becf5fea4936b44ab27f22ede19b";
                iBSSKey = "2e9374bcf55a4d14495141eddfdb9cca039a7dc0913676586cb3e19bcde135db";
                RamdiskIV = "66fbf227d7a74cf39cbc149f0ebb3926";
                RamdiskKey = "0108ebb075ca534df759e849a47d79ad757f39a51a3352a9d3d760ce1c6609af";
                rootfs_key = "4e164b7c39c8e0234787f7b9ae204adf1e3a66d472f1dce1db41e42ba87d1ff5722a7689";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "e07bee3c03e7a18e5d75fcaa23db17b5")
            {
                iBSSIV = "966fdb6312a3cd35703e7a1e8bb4cce6";
                iBSSKey = "81d26076947f2a50c0d31766f7e5f3b73ec198a1e0c50064a03b9e74bd0cbf91";
                RamdiskIV = "214388b7e0589464bf59966524ae2ea4";
                RamdiskKey = "581f739963fc3fdbf70dfc695b35d43662a0069b501cb715264c32428e759cba";
                rootfs_key = "01155a88dc41d6bdb6ba368719853e7e68fb0076dbfaafe8e0801256c724b103f2e271ca";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "ac3031a7b5c013d6a09952b691985878")
            {
                iBSSIV = "c2c5416472e5a0d6f0a25a123d5a2b1c";
                iBSSKey = "1fbc7dcafaec21a150a51eb0eb99367550e24a077b128831b28c065e61f894a0";
                RamdiskIV = "103ae8786d55bebdea996a56706641c9";
                RamdiskKey = "a80b3c27041f09d4554bbf4af59dd5bcea38bd4fe2faf82d8d6f62853ec6b337";
                rootfs_key = "2ab6aea67470994ec3453791ac75f6497c081edd1991e560a61dd666ac4b73f43c781739";
                rootfs_size = "830";
            }
            else if (iFaith.xml_ipsw_md5 == "9f8a1978f053ec96926e535bb57ac171")
            {
                iBSSIV = "f7ed98e14e9f7f01397639a4424ef175";
                iBSSKey = "abcc0848b65d7e2e675f8030ea37f325";
                RamdiskIV = "58df0d0655bbdda2a0f1c09333940701";
                RamdiskKey = "fbf443110eb11d8d1aacdbe39167de09";
                rootfs_key = "aec4d2f3c6a4e70ea6cea074f65812d2a34b180cc92b817edcd867167e7a91c5beb942f0";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "f3877c6f309730ee31297a06c7a9e82c")
            {
                iBSSIV = "45e620eb1cabe17cb5e25018e628160e";
                iBSSKey = "1b45e1726a88d3e89a203a59544b853c531758dd3c6f15fd9fa24cfa4ebc949d";
                RamdiskIV = "1edc4378f31ce728a2412ff93c78b8dd";
                RamdiskKey = "425f8a7bdac80a9678c78317a0ddbb91abb52a2fd1ff45f46c3e6db392155db9";
                rootfs_key = "69e2ca7a250765c95a703081d1195e681fbe82f31162b79fd2b70754629b0352694b9eda";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "2e634d16d0e01ef70070c9a289e488ca")
            {
                iBSSIV = "c58929f652c1c086f70f941f3bb31058";
                iBSSKey = "358e67475d675410517ccbfcbbc38fa4c56d0e892b627460851a1fa5e9b430ab";
                RamdiskIV = "103ae8786d55bebdea996a56706641c9";
                RamdiskKey = "a80b3c27041f09d4554bbf4af59dd5bcea38bd4fe2faf82d8d6f62853ec6b337";
                rootfs_key = "e7de54b25167afc66e381ade1d5e25c6392757497cfd92826a3111772731ba0b70742b90";
                rootfs_size = "850";
            }
            else if (iFaith.xml_ipsw_md5 == "0564fcd3f53dd6262b9eb636b7fbe540")
            {
                iBSSIV = "c58929f652c1c086f70f941f3bb31058";
                iBSSKey = "358e67475d675410517ccbfcbbc38fa4c56d0e892b627460851a1fa5e9b430ab";
                RamdiskIV = "e869e35fae9877bcba930e66e1536fc6";
                RamdiskKey = "205d0b4636400b08cd0b86a4afdec9d7fc79267f3146a00ce9bb4f3d987a1547";
                rootfs_key = "770b58765a3345004528fd9a2cbb7c3105137d0bd3a134a24679e6e173f32636d0485d06";
                rootfs_size = "850";
            }
            else if (iFaith.xml_ipsw_md5 == "35c8ab4b7e70ab0e47e2f5981e52ba55")
            {
                iBSSIV = "921a1405543996714f80d2fb1f8d0242";
                iBSSKey = "e31d511699eb9564c7174932e595e75f08aaa11d392d9026f28fe60ede336fc2";
                RamdiskIV = "193f5c4a3a8fb0643e9cac4020b67e1c";
                RamdiskKey = "1cdaede20dfc5f2299f23c237688b341390088c71b8aa63793770e5c1f3e6dc9";
                rootfs_key = "31c700a852f1877c88efc05bc5c63e8c7f081c4cb28d024ed7f9b0dbc98c7e1406e499c6";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "d688d2d48c8b054367adef8e7ab4f5ea")
            {
                iBSSIV = "6c69b68fe261460cdc047145f22e6647";
                iBSSKey = "68509ca37881fe7ae9508e559d57c6c54e5307dc883584a264470ed5685ce34f";
                RamdiskIV = "50932f5bd4dbd51fc0073851fe8b073c";
                RamdiskKey = "15a37ae180c0f51d119c9709f244211fc27334b8c7367dd80147f5e5952d0327";
                rootfs_key = "ec413e58ef2149a2c5a2669d93a4e1a9fe4d7d2f580af2b2ee55c399efc3c22250b8d27a";
                rootfs_size = "850";
            }
            else if (iFaith.xml_ipsw_md5 == "93957e7bd21f0549b60a60485c13206a")
            {
                iBSSIV = "45bbf0fa98573425fa21dc6e529eba6b";
                iBSSKey = "32398d3d1328ed3f0e1949446a1357585ae1973b3c8434b83df49ac55cf45d06";
                RamdiskIV = "9b20ae16bebf4cf1b9101374c3ab0095";
                RamdiskKey = "06849aead2e9a6ca8a82c3929bad5c2368942e3681a3d5751720d2aacf0694c0";
                rootfs_key = "b2ee5018ef7d02e45ef67449d9e2ed5f876efae949de64a9a93dbcf7ff9ed84e041e9167";
                rootfs_size = "900";
            }
            else if (iFaith.xml_ipsw_md5 == "0045e3543647e23470b84c2c1de96ab1")
            {
                iBSSIV = "856c2d9884ab1485cfd75b533246282b";
                iBSSKey = "c3f0f7a3093a74b4ead7fa0b4f80dd38";
                RamdiskIV = "654eb6a2290da5b9b77e6570f2d8ba2b";
                RamdiskKey = "8d53585fa73916d27016abc81e41da82";
                rootfs_key = "519ec112b4af0a65eab6ea65b222c5b7f605ce52ad9195640e3309de58dd54ab0a0c9607";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "25dbf5b3e5ca39edd0aab8fcab888503")
            {
                iBSSIV = "962cc5a04ef71269630fd05b7d43f0d6";
                iBSSKey = "f51e414ba2cb4bf68fcd1ea70d420207d4e269d048265b6480764e65af511904";
                RamdiskIV = "9064ae28aef1db52cde6f7568c188766";
                RamdiskKey = "1229ee227f260b8746021d4a46366ec42f987c36d4910a4925e6ca0ce369f69f";
                rootfs_key = "abd68f16920490865a09e559123db1f471ff19743ad15ea8b970a73e28f5efc6c6e76925";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "14d1508954532e91172f8704fd941a93")
            {
                iBSSIV = "5a01a1e31d2ae895690cd279dbd5e3c0";
                iBSSKey = "6d26c20f472d9ed5ab6219e632e35b4c582c1c402104aa39d75471171c88d473";
                RamdiskIV = "9b20ae16bebf4cf1b9101374c3ab0095";
                RamdiskKey = "06849aead2e9a6ca8a82c3929bad5c2368942e3681a3d5751720d2aacf0694c0";
                rootfs_key = "982437b30d334c744c94b9a73ab70e0fc6ed94c181b2a8b0fde6ee03f2546cc9b2c5b01c";
                rootfs_size = "850";
            }
            else if (iFaith.xml_ipsw_md5 == "9402d5f05348fd68c87f885ff4cb4717")
            {
                iBSSIV = "873ca0583dbfd0b40d42bd86bca09a07";
                iBSSKey = "619a29ff91bf924bd1edf8efe48d280f5ec85865dd55dfe5f8989a1077f71a0e";
                RamdiskIV = "9b20ae16bebf4cf1b9101374c3ab0095";
                RamdiskKey = "06849aead2e9a6ca8a82c3929bad5c2368942e3681a3d5751720d2aacf0694c0";
                rootfs_key = "6380bc27ef713750c21759ce770cb6540a8e31fca4c78820fd7be3a02030365a59257582";
                rootfs_size = "850";
            }
            else if (iFaith.xml_ipsw_md5 == "4980214fe7a65e2624aaff394f106e06")
            {
                iBSSIV = "03baadf8801e8b7cdcee5a9f53609d0c";
                iBSSKey = "c9f8bd4e52530ec8ef3e2b5926777f624061a38d09f07785287de6e88353f752";
                RamdiskIV = "bab07889e0d5ba26521e7f141e213178";
                RamdiskKey = "3e88b3dfbb432a60d3475ebee8570c83c52612465f4db7686f62415f4b6e1bdd";
                rootfs_key = "fd73cd898b7e55f9dc24092a4c574f1f284087075520a7d30232b0b6af8871743a0f0b82";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "3fe1a01b8f5c8425a074ffd6deea7c86")
            {
                iBSSIV = "03baadf8801e8b7cdcee5a9f53609d0c";
                iBSSKey = "c9f8bd4e52530ec8ef3e2b5926777f624061a38d09f07785287de6e88353f752";
                RamdiskIV = "7c256102d0580b960213540965618b5b";
                RamdiskKey = "5d4e967158ab75ba27ec281bff4e714dacc88123ea4913ae2bee6a719c15496c";
                rootfs_key = "5407d28e075f5a2e06fddb7ad00123aa5a528bd6c2850d5fa0908a4dcae7dd3e00a9cdb2";
                rootfs_size = "750";
            }
            else if (iFaith.xml_ipsw_md5 == "eb3c205debb52c237c37f92335e6369c")
            {
                iBSSIV = "6863087c07128d170db61316205c5a45";
                iBSSKey = "cabd9afa6d7678f3f95d0ccf43d18f54e4ee2e6ac8025b2a528f3b07579ec305";
                RamdiskIV = "c28f3cc7af09a94258a82ea9d1817088";
                RamdiskKey = "cf105a226edc8ff168300eb176d36c5f2de1f712985b7eff12d55868a61288c4";
                rootfs_size = "850";
                rootfs_key = "723ded674deb1cba56a142542a0b06d2a483297f8056c0cfa70346c0724e1b0e03feded6";
            }
            else if (iFaith.xml_ipsw_md5 == "30fc03783453d23aaa0d13f89fd36c28")
            {
                iBSSIV = "6863087c07128d170db61316205c5a45";
                iBSSKey = "cabd9afa6d7678f3f95d0ccf43d18f54e4ee2e6ac8025b2a528f3b07579ec305";
                RamdiskIV = "8d612fbab555c8e8f548898b0e6d3cb0";
                RamdiskKey = "9ca594fef56655a7ad4dc1312dc4a499851d832c2eeb86f5a9ebfabe08ccedb6";
                rootfs_key = "612f78042ddc5337ab1abecfb59a07e88ed3e80665a035ef02c3c48045057fc29ab0a4b5";
                rootfs_size = "850";
            }
            else if (iFaith.xml_ipsw_md5 == "0e0e4bf8f0d7c37b9a252fcbed60ac0c")
            {
                iBSSIV = "6863087c07128d170db61316205c5a45";
                iBSSKey = "cabd9afa6d7678f3f95d0ccf43d18f54e4ee2e6ac8025b2a528f3b07579ec305";
                RamdiskIV = "e9ab0492bb9f14bf17f0810c64e5f874";
                RamdiskKey = "9ea3e9eb1de46882cb2ecdd0b27013c4f91f225e559de446c9a346b52f9cfe4d";
                rootfs_key = "d8e162215f27c016ed8d1849c6059f99984c766c72cec4a1df63724491c8e5b19c0e6fb2";
                rootfs_size = "850";
            }
            else if (iFaith.xml_ipsw_md5 == "1bf3fdd8566c67e0961c06b8a2abfc65")
            {
                iBSSIV = "713a8187a72a8e60d8f66cfe63e0e729";
                iBSSKey = "13c05174ea0c0760a9b59f721e662441198e4547a70e4617bed71528fd771f91";
                RamdiskIV = "9284deec6684a6d8733d0cb38a159f2d";
                RamdiskKey = "91c7dbe6e67bb2eb0583dc7211514edad79409d2d780bfce4f35255d70ae3c38";
                rootfs_key = "e5eed79ba8d2341dfaddeebfa38a86d8b95af4a711054a0ab2e058694c13c814fa39a4ba";
                rootfs_size = "900";
            }
            else if (iFaith.xml_ipsw_md5 == "2c0dd880982f0f8e47dc3dadfb733ad7")
            {
                iBSSIV = "713a8187a72a8e60d8f66cfe63e0e729";
                iBSSKey = "13c05174ea0c0760a9b59f721e662441198e4547a70e4617bed71528fd771f91";
                RamdiskIV = "592d8ed4d1f64a4e6d90f7ddf0815399";
                RamdiskKey = "49a73347d6533e2d42b39bfb56821bca1af1b3eacb1334debe43ab2c6adb7641";
                rootfs_key = "d36aa0ce62b84a9a31a9c33a551809213dcf5f764850c28b3885f00bc1f5664224c13a3b";
                rootfs_size = "900";
            }
            else if (iFaith.xml_ipsw_md5 == "87ebb9b2c025fb5f87a4cab0631b1547")
            {
                iBSSIV = "25042db8abe54ba2a740dffd909bd1ed";
                iBSSKey = "377c13630bc9370242e68b28bab1cb1cc21aa8220d3a1c79bc11471e763392d8";
                RamdiskIV = "9f4ca36fb6b30edaebdfbec3c67ce128";
                RamdiskKey = "8bb1fd99c264f46e9b5219cf4817d6e8c0d5915a893a037f31f82bd43f97ce60";
                rootfs_key = "95028f5804a6d675190adedc0aa91385e17589f720c517615367d69c63e0c969121aaec6";
                rootfs_size = "820";
            }
            else if (iFaith.xml_ipsw_md5 == "e0a463bded8f5b1e076b466535b18c75")
            {
                iBSSIV = "37f4d36494ac9d83ab8a9e4936c885f8";
                iBSSKey = "f5e50c94dfee05ed52b4003750007f4c2d1801f7e90e768774ac656dc62c69db";
                RamdiskIV = "d11772b6a3bdd4f0b4cd8795b9f10ad9";
                RamdiskKey = "9873392c91743857cf5b35c9017c6683d5659c9358f35c742be27bfb03dee77c";
                rootfs_key = "34904e749a8c5cfabecc6c3340816d85e7fc4de61c968ca93be621a9b9520d6466a1456a";
                rootfs_size = "900";
            }
            else if (iFaith.xml_ipsw_md5 == "43383f2d5cd181f2af1e01ec62a3f1d6")
            {
                iBSSIV = "54774ea902fe56e59223b2b78d46b975";
                iBSSKey = "7b2c0dcc0a9d4b0bcfe7946d5e4563f5516c69495daa65a13ad630471f453007";
                RamdiskIV = "c4ddd3ed329d243488a99aa50f693d2e";
                RamdiskKey = "26082729ed2afb965b396cccc8f16dc44e0372a9b02095ebf523956edca40a4f";
                rootfs_key = "cca43b420c4ffefb23a9b5e1605db40df1d89cb13d5951e22b7dda5a35a5cb2dcde85e4a";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "0c8cdbbb729508811fa5bd29d8e1143b")
            {
                iBSSIV = "0fc5175caf46f3328ef43346a25bd1d1";
                iBSSKey = "b482abee8c3e9d5727f96ed39a15d43c7a8c737bc87269a5b95356ec9338f8b8";
                RamdiskIV = "d11772b6a3bdd4f0b4cd8795b9f10ad9";
                RamdiskKey = "9873392c91743857cf5b35c9017c6683d5659c9358f35c742be27bfb03dee77c";
                rootfs_key = "b5eefbaf0046a79c689ff07e66ee8045f859dab1ee16d44d15606c1918e5afd323f2db07";
                rootfs_size = "900";
            }
            else if (iFaith.xml_ipsw_md5 == "9a889ba48bc2715292f199f50c70ed60")
            {
                iBSSIV = "d9df21fc5a610752d6b2d1fc9acee1de";
                iBSSKey = "9f628d5b2a317473405154ad312b2ca2bd56cc1fc0d3eb8c90a1bc96d077e267";
                RamdiskIV = "d11772b6a3bdd4f0b4cd8795b9f10ad9";
                RamdiskKey = "9873392c91743857cf5b35c9017c6683d5659c9358f35c742be27bfb03dee77c";
                rootfs_key = "890650c3aa3be7c4d6f3473776580acf6781688e6342ed15441a299142fe4c5e933fc89d";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "85647af7e281cfca4f4e0d1c412f668f")
            {
                iBSSIV = "17742baec33113889e5cbfcaa12fb4f0";
                iBSSKey = "998bd521b5b54641fbeb3f73d9959bae126db0bc7e90b7ede7440d3951016010";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "a30f67f64a546a28bfd5c6e2e43d14fd8111c55641fc6dc392891e2b14e7e8138db9344c";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "893cdf844a49ae2f7368e781b1ccf6d1")
            {
                iBSSIV = "17742baec33113889e5cbfcaa12fb4f0";
                iBSSKey = "998bd521b5b54641fbeb3f73d9959bae126db0bc7e90b7ede7440d3951016010";
                RamdiskIV = "87af8e20133c17d45dab42702eeb136d";
                RamdiskKey = "d9a0258b4f25101b15e260663937c3f6cda748d051bebe09212f51f0be72a89f";
                rootfs_key = "7fb6a5a1a5d74ceb61180c8740065b79ac87a5c15e554ad4b147ae9e1446254acc9d5e4a";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "4726cfb30f322f8cdbb5f20df7ca836f")
            {
                iBSSIV = "17742baec33113889e5cbfcaa12fb4f0";
                iBSSKey = "998bd521b5b54641fbeb3f73d9959bae126db0bc7e90b7ede7440d3951016010";
                RamdiskIV = "d3a3b18903be2446fb2902783c258420";
                RamdiskKey = "8225502b126c0e0c3b65454bd0a9ebaef801b49ca3c0a2f3d27602115516ab14";
                rootfs_key = "f607711d4db94bba7a4866f095aed082c8485bfbcd0f411f1e65158f585915edd5cfeec1";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "785f859b63edd329e9b5039324ebaf49")
            {
                iBSSIV = "17742baec33113889e5cbfcaa12fb4f0";
                iBSSKey = "998bd521b5b54641fbeb3f73d9959bae126db0bc7e90b7ede7440d3951016010";
                RamdiskIV = "7bf76ec1fdf382d70ea9581e223943f6";
                RamdiskKey = "f91256406327befe3c5c495abcef342fad14a28227a120e04139e1220814a31a";
                rootfs_key = "32c6a922fdc1a474371fcfcbf8b5bf4a87ce01b6e672c360405a0dd238ad693769f0ce77";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "694c93b5b608513136ba8956dff28ba7")
            {
                iBSSIV = "086c8a91cbda60d094e5f96c8a62c9bd";
                iBSSKey = "795b5196dfcbe8aea9803154148c087d59a1659a0543c59d4bf7e2ff889b7ed7";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "c338fb2858bd5dad4cfb073d4fab2fbed4a3f2d1541bc50d8443f3b18475cd1b62c25983";
                rootfs_size = "850";
            }
            else if (iFaith.xml_ipsw_md5 == "32f9a71430c4dd025adab3b73d4a5242")
            {
                iBSSIV = "a441763f051b5537aeefefedf3cf52c9";
                iBSSKey = "bbf0968d5799f444aae52bdf9a2f9ae26d30d94e8de1f9867fba82da220fc852";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "f6331068497fa4741e135329c399f69b3c109854835789cc6f23f759f333f5e7bbfcdde7";
                rootfs_size = "900";
            }
            else if (iFaith.xml_ipsw_md5 == "47827ca8d127f28663d5b70b0784236e")
            {
                iBSSIV = "24bb9af0d82d9c22f864dcd408038d20";
                iBSSKey = "1cacc4990ee4eca8a8ab800b00ab0951fd51c2b90bc8845c55101ce78665b52d";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "f466f8ab4e9accb91ed1098ebda56b27b5dd06ddc62485394a53eb77bf190afd80274c02";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "b0e356267a1407e4d7a7b0f48a07c5c2")
            {
                iBSSIV = "28893e051476c5b321751a0f8ee7cff7";
                iBSSKey = "826789656e0cf85bdc280b8e39490388c32b6c39004283624a6f3a6c69adef31";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "2cce34479eeb3701b3888f81c0465d2d98133af3a2761d0a82ad5074ca8efc1c5593eccb";
                rootfs_size = "900";
            }
            else if (iFaith.xml_ipsw_md5 == "fe4f80f8ff2fa298559b392b64e84bb8")
            {
                iBSSIV = "815f744558bb65d991dd1dd2502301b2";
                iBSSKey = "99f0194451de1ac1eb4a254dd843f89a9cae2537b130793bff2c5e16a5b2b851";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "c309657d0abe1b66b4be046bb4b03fb540741f9cbc1e49951cf21e11332d9b0b66afd31e";
                rootfs_size = "750";
            }
            else if (iFaith.xml_ipsw_md5 == "7c1c714f24a89c2f2c71e26d37cde3f0")
            {
                iBSSIV = "181ee0b292538b2052dbd119770d7348";
                iBSSKey = "f131471b406795abf7ae70617dd30968e5ad6b847b84a1671d5801c04fc07643";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "69a370c1b64b35f692e87e866bcd460a98a10c56ed05055eb7c675f101894ea504f7bc46";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "8cb3a9964a2a99414030f662d3009deb")
            {
                iBSSIV = "cdd50b45ca1bac4f718d9eb23ce9f0a8";
                iBSSKey = "8ef00005aa2c01ae409d55e330171589af79d76ac86639e76003835d5d82ffc4";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "30804cac61ba4df96999aa4e1ea3a2a18bfbe875534a66a0bb1add095e307a19a7176c82";
                rootfs_size = "900";
            }
            else if (iFaith.xml_ipsw_md5 == "7f831b30d33f80c7f92442cb041227ab")
            {
                iBSSIV = "a4ff016010ce6831cae2a7009113c16f";
                iBSSKey = "f7e6576f69cbcefbe96939efa39600cada2e13d12a6372499eff67f0566b86d2";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "7085a2976bd57eceedcbbe88a270e1a5028133c288b8afb122c2f886830a9a641daf8bd4";
                rootfs_size = "900";
            }
            else if (iFaith.xml_ipsw_md5 == "4a002a4596a681efd9cdbf6f2fd72e74")
            {
                iBSSIV = "682684c4247740ca26c6823e58a36cdf";
                iBSSKey = "ac17ad4e4e65d5d988b28bde260ade08a7b3f284a22b03a386b53dd761b4ccb5";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "401b22ae26cca1aa2e119c17a6c389a1ba6aea0fbff4912000a77df953f010637b35d0ee";
                rootfs_size = "900";
            }
            else if (iFaith.xml_ipsw_md5 == "24027c4381a6cdfdd8a03a17177d1d6c")
            {
                iBSSIV = "de7258db01e653c6d8ffe0ab18a5a5c7";
                iBSSKey = "6d0afca5d390a7e48399cd534c8fd7db5a3f8fd4361631f6dffca75fbafca6aa";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "25c0b2a27afd23b9ddc9555a28ba8e77548e62d9e2ef56700bc40d22b2c50416aee9313c";
                rootfs_size = "750";
            }
            else if (iFaith.xml_ipsw_md5 == "d9a02961311ffac8197e8db3b48e449d")
            {
                iBSSIV = "181ee0b292538b2052dbd119770d7348";
                iBSSKey = "f131471b406795abf7ae70617dd30968e5ad6b847b84a1671d5801c04fc07643";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "148f4fca734e973551fc8fa65a04883041854b060e3fe1e6c3ca4499a3204d1d97594a47";
                rootfs_size = "850";
            }
            else if (iFaith.xml_ipsw_md5 == "a0cb7313c5535991d62890c7eef60f9a")
            {
                iBSSIV = "cdd50b45ca1bac4f718d9eb23ce9f0a8";
                iBSSKey = "8ef00005aa2c01ae409d55e330171589af79d76ac86639e76003835d5d82ffc4";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "246f17ec6660672b3207ece257938704944a83601205736409b61fc3565512559abd0f82";
                rootfs_size = "900";
            }
            else if (iFaith.xml_ipsw_md5 == "7c8d3ccaccd1573dc31d6de555b987f9")
            {
                iBSSIV = "a4ff016010ce6831cae2a7009113c16f";
                iBSSKey = "f7e6576f69cbcefbe96939efa39600cada2e13d12a6372499eff67f0566b86d2";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "affbe8f884694f4a3848097fa22a71bc1de24b070aa7e79f58a0880602dd21444cd559f2";
                rootfs_size = "900";
            }
            else if (iFaith.xml_ipsw_md5 == "dd5003cc00dbaa9fbf0182c5a2e5d6ed")
            {
                iBSSIV = "682684c4247740ca26c6823e58a36cdf";
                iBSSKey = "ac17ad4e4e65d5d988b28bde260ade08a7b3f284a22b03a386b53dd761b4ccb5";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "d2877c05eb72e55d52d4e5e71c523a503c5bb8c85f6c7077d821140beea967782d30858d";
                rootfs_size = "930";
            }
            else if (iFaith.xml_ipsw_md5 == "d20493bb1ba0450f2ee01d081ba8eb27")
            {
                iBSSIV = "de7258db01e653c6d8ffe0ab18a5a5c7";
                iBSSKey = "6d0afca5d390a7e48399cd534c8fd7db5a3f8fd4361631f6dffca75fbafca6aa";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "765d0fecc4f714ca20fa6eceeabb454b04cd2998cc3ab3bba290866788a8c6cf555945ac";
                rootfs_size = "800";
            }
            else if (iFaith.xml_ipsw_md5 == "3414ec10c6e7f50d8eca5d5c25d60ff0")
            {
                iBSSIV = "645a939b52f048e4b118c93d789dc822";
                iBSSKey = "6c240794a223efc88b782a5933a5f11cf52f3d164b6ff4b1f78d20b62c17a347";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "aa3f737295c1d7a1e0539b8b1a02310b9ec7503be6ed05b88520e50a1a006f4b270b3e9f";
                rootfs_size = "930";
            }
            else if (iFaith.xml_ipsw_md5 == "a172829312329d3bda87d23abf5ec4d4")
            {
                iBSSIV = "397d887af465a48c481d31af5f744c0c";
                iBSSKey = "668a8416cbc95531ce47c165b384315baee8c8e2b992345fb1ea448469dfc8e5";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "fb9480e2b80a26cd75d923d7918539edb19caed5a72dfe7a78734cd2a82597869b9ceaf5";
                rootfs_size = "930";
            }
            else if (iFaith.xml_ipsw_md5 == "c6862a24f9166c313043459013159194")
            {
                iBSSIV = "00ba61665022e97cacb71493f3e92533";
                iBSSKey = "85d0388a2b1ce6b4fc68aebb3cb87014b6dd57fde5d9599381db4083a30c3803";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "f3b2e5122cfd8b8215ed8271d83af0183f6d6634afd63444dfd7787e274b7520fc9d5c40";
                rootfs_size = "930";
            }
            else if (iFaith.xml_ipsw_md5 == "5ea7630383fdad9ab3adba0220b408fc")
            {
                iBSSIV = "0883a549b9d4e1bc0b6efd995e91ae20";
                iBSSKey = "8e01489f416ff819089b1a2027802ea96ca6100107164686fe4c1cbeaa138c3c";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "49d6739cd5fcc1009756ed520fbeb4a5ffd4f7f460ffb5c6eb9e3d49932fe40cf7cf3e8c";
                rootfs_size = "930";
            }
            else if (iFaith.xml_ipsw_md5 == "ed3f720c8a02ceac99adcc663f9ad409")
            {
                iBSSIV = "b5eb13b4c4b4796fc8d4b1aeaf27fe41";
                iBSSKey = "5de8fa1ac1938a4213610f47d062c8b92415dcd2eb7155825bbe55985863fbb0";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "c71cb13f356620fdf7b1ab93470e3cff4d7f0f005d36bff5a6c3f8e60ab48e5b9d93841c";
                rootfs_size = "930";
            }
            else if (iFaith.xml_ipsw_md5 == "d80101eb0b1b460e319f9124f252813c")
            {
                iBSSIV = "645a939b52f048e4b118c93d789dc822";
                iBSSKey = "6c240794a223efc88b782a5933a5f11cf52f3d164b6ff4b1f78d20b62c17a347";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "e002a32650a28f4ecd0793d2e36d8bc93bf4a60bb010dbe9ef2ed41821fc5463b24c791b";
                rootfs_size = "930";
            }
            else if (iFaith.xml_ipsw_md5 == "a7586eaf9cce93db06c2114f23e8ed90")
            {
                iBSSIV = "397d887af465a48c481d31af5f744c0c";
                iBSSKey = "668a8416cbc95531ce47c165b384315baee8c8e2b992345fb1ea448469dfc8e5";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "8b04eb7e4c4c3bea36693fee2369d48c667083ae79ddea8c02f5ce9da30a74cb20707328";
                rootfs_size = "930";
            }
            else if (iFaith.xml_ipsw_md5 == "7219c43eea7362c7a402861f71b4496e")
            {
                iBSSIV = "00ba61665022e97cacb71493f3e92533";
                iBSSKey = "85d0388a2b1ce6b4fc68aebb3cb87014b6dd57fde5d9599381db4083a30c3803";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "e5e061077217c4937e14d9c4ae1eeb8d69827aa4838168033dd5f1806ab485306a8aa3cf";
                rootfs_size = "930";
            }
            else if (iFaith.xml_ipsw_md5 == "c5c45b58618b38fb4d2fba242eb0ba1e")
            {
                iBSSIV = "0883a549b9d4e1bc0b6efd995e91ae20";
                iBSSKey = "8e01489f416ff819089b1a2027802ea96ca6100107164686fe4c1cbeaa138c3c";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "527d77b552fa1fa3708f5c3c2feff8641c7716a24df4dbb49613d0776a7afa3ab9cf95dd";
                rootfs_size = "930";
            }
            else if (iFaith.xml_ipsw_md5 == "c750f69c14d5fa8295edafd7212aac7d")
            {
                iBSSIV = "b5eb13b4c4b4796fc8d4b1aeaf27fe41";
                iBSSKey = "5de8fa1ac1938a4213610f47d062c8b92415dcd2eb7155825bbe55985863fbb0";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "bb8cc0947739d77f7c800fea823f37f34ebf1dfd77fefadaef163d3556a73b20ce411ed7";
                rootfs_size = "930";
            }
            else if (iFaith.xml_ipsw_md5 == "eca151961d28fd0e085b2645a51c1755")
            {
                iBSSIV = "fdb60523529674bcc466aaa3a43d5a16";
                iBSSKey = "4323d03260a27f33ac1e129bce748209af5daf2a775e1944a2f0267a9dc18abc";
                iBECIV = "502bc40bfc92f093114824ef604b6d69";
                iBECKey = "140a6f602c0cd2345fb9ff72437c569cbb84485d4c561c653ad77c61c3920073";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "e04125691fea59da7bedc605667f459c78d243d1b4df4c6127d154dc84b3657902538aee";
            }
            else if (iFaith.xml_ipsw_md5 == "a2c06f31d13bdd63f768a8991a2f64e5")
            {
                iBSSIV = "a62a9590eec7d0e60ef7bffa3721aa31";
                iBSSKey = "3dddab1793f27151bf9748f730837b894e960165aeb6f26219f730b17d62a7b4";
                iBECIV = "7fc23d2153b98e672798fc61f0f3d968";
                iBECKey = "927357e9f22be77cf5040254b6935b9162a5dfeb133379b795d6020d7b3b1ab6";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "490e91667d6aa9cccc08b6d8e2aa0b354205426d9d00fd9e1b17bcfa3b8b34e5c2aa195a";
            }
            else if (iFaith.xml_ipsw_md5 == "c64da86de651b049cc8d2cd06f234a3d")
            {
                iBSSIV = "a62a9590eec7d0e60ef7bffa3721aa31";
                iBSSKey = "3dddab1793f27151bf9748f730837b894e960165aeb6f26219f730b17d62a7b4";
                iBECIV = "7fc23d2153b98e672798fc61f0f3d968";
                iBECKey = "927357e9f22be77cf5040254b6935b9162a5dfeb133379b795d6020d7b3b1ab6";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "2aca488e83805a9e8190a8960a4c694d7b4967cf97ab133b48ab6f688726c37af5e28f3f";
            }
            else if (iFaith.xml_ipsw_md5 == "9227a33a560ea4dcaff3dabb33424df2")
            {
                iBSSIV = "4d74e8728184fb3c512406be00b1e04e";
                iBSSKey = "0b9a871abbe670a9c8391df347b5331a770cb09d870f81293b974ca9253f4c29";
                iBECIV = "3cb31d78e4096fb3a596c869e1780a3f";
                iBECKey = "02d501bc4a5be0d80df13aac8265ebba6739a6b5722e003b971dd5917892affb";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "6a0dce2b25b42f7de328b6121a16fd282b8bc16a1718f14cc2dbee0f1a4f0e81d0e1eb03";
            }
            else if (iFaith.xml_ipsw_md5 == "ea805b0aaf9f8e79d8dedf3e009f907a")
            {
                iBSSIV = "4d74e8728184fb3c512406be00b1e04e";
                iBSSKey = "0b9a871abbe670a9c8391df347b5331a770cb09d870f81293b974ca9253f4c29";
                iBECIV = "3cb31d78e4096fb3a596c869e1780a3f";
                iBECKey = "02d501bc4a5be0d80df13aac8265ebba6739a6b5722e003b971dd5917892affb";
                RamdiskIV = "";
                RamdiskKey = "";
                rootfs_key = "a303410067be58e99544200b8b0de7d01b20232ccf1e6cb42fbc7a22e572367715691db2";
            }
            else if (iFaith.xml_ipsw_md5 == "729c72a9880b82b37aef5997173d9578")
            {
                iBSSIV = "6ff48f4735a6aff1ffb3dfbdc2ac72d6";
                iBSSKey = "55fa5096e801823685b27864914926d1c251057257490e24b3573aa9e9ea9cc2";
                iBECIV = "e4d340d9bb22d4a42eaf1869e0978c8b";
                iBECKey = "2177595e32aa2eb51e20bc31f6aed2d7818e3fb75f0321a338ed6fe6f42acccb";
                RamdiskIV = "277d39cbbf681df205db5808396baa44";
                RamdiskKey = "e70dfb6d3b6cf2e05366afcf328385425500e67b212e59a6d08b52bb35732da6";
                rootfs_key = "c7e01f3db404f325eee5062368fc6a795487d859518ee498b4d7f4950a281c5421ffbebf";
            }
            else if (iFaith.xml_ipsw_md5 == "989b8327acab76e7632443a0e179250c")
            {
                iBSSIV = "580904dfefa76c52185c1f06a6d7152b";
                iBSSKey = "fcd9b12b40b5b71c487fe464f67559678c1aea3bdc4de74e6eef236a98d3e3aa";
                iBECIV = "55f413e7683fede0a383129fe195aa61";
                iBECKey = "b47591cfda74819c1d1696bd0ba6b6ff047fe1a7eeb45635d652d8aadf48a21f";
                RamdiskIV = "5630ab5a75440c57f80f42a41f2a86a1";
                RamdiskKey = "3ea246f2b53066f329b5f4b10c973c7b62d6736e2b8706cf396caf5e7ba364ca";
                rootfs_key = "e77431d46dedd65cf73df82a823e32e131a76a7caa6d95112bcaede156eb566ce0e8a57d";
            }
            else if (iFaith.xml_ipsw_md5 == "29ead00d2726f688d393a5fdd0de4a97")
            {
                iBSSIV = "48f224258ed236032d0993b828bccc49";
                iBSSKey = "d28d2281bdf5b6b5f3e45cfc6adddf3682c60348aa6fe6543e184814ae54bebb";
                iBECIV = "585ea0ab6a68e72b686292cc2b30cb9c";
                iBECKey = "f9b6055db270eff397baffe34989b154da745c017a8d5ce8e6e73b0660ed6a27";
                RamdiskIV = "6aa6791d75e07b6866e42b04f80b90d6";
                RamdiskKey = "203ff7f82ff601232c7f8d3b53d3005d3218a5121ccf7eca4c9bf8a6870b7bf3";
                rootfs_key = "575bcb4f9290a28bc00451f7e444973fd8b0afc529d2d84db4ae227bdd779563f070eaea";
            }
            else if (iFaith.xml_ipsw_md5 == "54aa71d20263580653eafd06064c557c")
            {
                iBSSIV = "463c1df37709847d44be3e945e6212a1";
                iBSSKey = "2b04e967fe3409f3529f2c9c2de8e00d208be0262c751a37ca85616968ca9997";
                iBECIV = "f47e04e542b16fa7bbed2e6a86e5afa1";
                iBECKey = "b1144d68c5c2097b5c1a164a044f1a0a03dd4067b9dd073659218fb981d63bd5";
                RamdiskIV = "57da5d3b645d4accee13806b7f46a3b3";
                RamdiskKey = "38231b8e8c4348a375288884ae230a52ceaf9b8fd05f57d6812d6b9d9c5357cb";
                rootfs_key = "0827b7d632abf92f397471cd7f77c037817e56d0ab1bade692b29f311f0fbcdfd6fc3bef";
            }
            else if (iFaith.xml_ipsw_md5 == "8ac7ae6a7f6fadbd34ba9abacd15e2dc")
            {
                iBSSIV = "afd80e647e22d22a26b6e58fb5846823";
                iBSSKey = "dc5e8dcd58628a25865fb77c2fddb9d2a17f7c933aa27c53ce2d8c4173d6a8da";
                iBECIV = "d435f60732b322140217f21f1589b8b4";
                iBECKey = "240580fa75a672a810100daec3bfc0cd189270c621e575b469e02e62029de12b";
                RamdiskIV = "370db2a05be567e5ff81363bbb69b89e";
                RamdiskKey = "774998512d4042a9da350aa07c7cd2f0e2b7c30730e88f3923e9bf029c556798";
                rootfs_key = "5e5c52fd7e439936d89659b5aa4f79206cd64f09c9961e9d4712a0131075966e2271b354";
            }
            else if (iFaith.xml_ipsw_md5 == "ac3985f710f3a0fe373881c98d322667")
            {
                iBSSIV = "c5be32a80c8a8679eb5675cd2e9610b6";
                iBSSKey = "593423e762a40dc1ef55e3d65dc2578db18011ff27649b6c2912e3cf4f95cd03";
                iBECIV = "e463e93748b846e438b10a7efb997552";
                iBECKey = "1736586735471f4c07d6e1e3232868fa4690de9b6f9fd3c7358bba0300600b9c";
                RamdiskIV = "dcf83cf3aa56954eafca7e3b562a137e";
                RamdiskKey = "b8ca7792a46a881099d4220617d6abbcc5548caf4e956649962d8964c3f825da";
                rootfs_key = "cbb21346634c5754f3e956f09ca7c93542b87286d7b11de71f18c5d72da529746ab27094";
            }
            else if (iFaith.xml_ipsw_md5 == "812a3dd8c6eb849cec62d16ed7e20ac9")
            {
                iBSSIV = "10b618d7d5731e326b018e95de6b8554";
                iBSSKey = "34a2126f10e985c36f8d3aa9fee1210fb21eff5fb00b92312df6d83e5e5c22a1";
                iBECIV = "27eddd5c22cff6a32d02ac99669f8395";
                iBECKey = "98299b12ee0eafe9028c2af13a2a5da96678848c0c2cf068ea3af269f5332985";
                RamdiskIV = "ed03628b68b98887702ef6ef277f2b48";
                RamdiskKey = "18c6320fee6ce45a7983f4ed9aeb8eed90c08c05cc0d406a207d3c1ccf3587b3";
                rootfs_key = "afde073c5b7a637f0a40b37c7b59f22836074d1f1ec8e85f05573da87da77629624179b4";
            }
            else if (iFaith.xml_ipsw_md5 == "e9500cfaa75e4a0a5ef88c3d92a21563")
            {
                iBSSIV = "05b6de2e7dc95144021a83202f61ab62";
                iBSSKey = "f21078779f95dd9aa1b2d96d21ac34fc385657aa6cde77c44db051012dbaad2d";
                iBECIV = "1d30d95c43807e465d63840e36d68df0";
                iBECKey = "9d35cc090e305faf449b8428fe4b0fb9d3b79fbd42da59746bec393b13582f8f";
                RamdiskIV = "861ef22a2220f5791180191dedf28453";
                RamdiskKey = "73c382680d3d724e5851554d1ca3ce4287e958f33021a5f084920030b6faf2af";
                rootfs_key = "a8c7fe8c4698684db2b315cdf9b0c569e6769ed721b83799bad4dadfaf6186c6fd6e0fb1";
            }
            else if (iFaith.xml_ipsw_md5 == "70aa97b9b4902ae32ea9d3d7850a5c58")
            {
                iBSSIV = "f12c8bac769bc818db694a4e4ce431a5";
                iBSSKey = "ba01d8cc57ecb0e6ac5760efc8a3b007c45e58c4b97aed78d34b8f2a4ad12822";
                iBECIV = "12a4ef7d5541c6a7aacbee76aaba93d0";
                iBECKey = "a53678bb23ac9782c6dc5a9c41276d79be63ce7e270b717c2ec2a309b98dfbce";
                RamdiskIV = "02d55437d1fd7c9d24aeb312a78dbd49";
                RamdiskKey = "3aeab4ceb61e011f3013873998bfcf01c585bb548f91e19c9085473f93f6a708";
                rootfs_key = "36895be7d36aa1415695ec3cd7d33ccf9b088bfc179d48f4d8fc5fd220a2c6f07c9b76d2";
            }
            else if (iFaith.xml_ipsw_md5 == "c13c14abcde18bbdb7d70c8563f56ac1")
            {
                iBSSIV = "e069af55f6d92d8f0a1a7678d799121a";
                iBSSKey = "9d307bba8b9b2aa1a3777590c226397110f6c1dfc9ec0fd4397671691a2016d9";
                iBECIV = "b9db93e06cdcc20ae46ded57e1356ded";
                iBECKey = "2faea9f60edaa820e93d707a7fc15f4aec3fc519d02754b5b0a07e07bf290d1b";
                RamdiskIV = "f91e451ab030e7a2eb219e51ef523c80";
                RamdiskKey = "2c5a60cd63f31e133c280dbc95c4640efdb39e70088d6f2de731ce6deb58f603";
                rootfs_key = "9890bc603e37b7ef74bc211cc9bfa09362c251c10c5abe9dcac43716e104cb717419e271";
            }
            else if (iFaith.xml_ipsw_md5 == "ba36e09d830a088b10e4a342d372a6c7")
            {
                iBSSIV = "00c5d93c3c7e25723a6ff56814c4b9ee";
                iBSSKey = "971b74cfc725377e2b26463edacbded91a75936cc334b092a3fad903335cd09b";
                iBECIV = "1e47ed6650470bc12880f9a169a83ec3";
                iBECKey = "edfcfffd313d5277a778f804ce905d970e67d0791eb21e1a2216255afc453c3a";
                RamdiskIV = "da655731593ebcfc46f4f64249ee6c12";
                RamdiskKey = "fad09c44bc8b29b54b0682d2a4a23d0bcbab832d9d7cb823d1e69d7c6cb2fa53";
                rootfs_key = "7ed37d8c051da8f8d31b0ccf0980fa5ffa54770c7e68ecb5ebf28abe683cadf21a4a99ed";
            }
            else if (iFaith.xml_ipsw_md5 == "0cd4ebbf9a94dd8b67623941bd2add62")
            {
                iBSSIV = "35ab5ac9ede6f05f3d35b9d2f4fa4a91";
                iBSSKey = "f2f614c0e5bac071da384e24b8035f59f7c49cc572ef11dfa8c1d322d45ea2bd";
                iBECIV = "91984b64f1e7bf45d0925e113e208591";
                iBECKey = "d07cbce9ecc535cf757e89b03460b8eb889f675807b632a94636452565915d05";
                RamdiskIV = "6071de3113ff3b4a5f6f2ebc907af054";
                RamdiskKey = "db49f7a53a326ef954ac5f14e9070b772630c35c66a9a8552827d4923b774818";
                rootfs_key = "e4b60a747ab372bc047b8a7ca08b62f7524fa5888c76f0db709fd1aa9bcea79076952639";
            }
            else if (iFaith.xml_ipsw_md5 == "1259442021913f1d9f0fffd006e3d0de")
            {
                iBSSIV = "9c69f81db931108e8efc268de3f5d94d";
                iBSSKey = "92f1cc2ca8362740734d69386fa6dde5582e18786777e1f9772d5dd364d873fb";
                iBECIV = "bde7b0d5cf7861479d81eb23f99d2e9e";
                iBECKey = "1ba1f38e6a5b4841c1716c11acae9ee0fb471e50362a3b0dd8d98019f174a2f2";
                RamdiskIV = "286bedd39e0b2b44aa77f49dd0cbde8d";
                RamdiskKey = "6442d5e7849ff2136a9dba4e8fb77511444e3031bbebd34b596c8534535fb2b7";
                rootfs_key = "be9d3af82b127eecaabe0eebe712a16a9b1b30de73ee640855479fb93b4387b3f388508c";
            }
            else if (iFaith.xml_ipsw_md5 == "51e253431b1080cbbb5073efc87b1bb3")
            {
                iBSSIV = "0cbb6ea94192ba4c4f215d3f503279f6";
                iBSSKey = "36782ee3df23e999ffa955a0f0e0872aa519918a256a67799973b067d1b4f5e0";
                iBECIV = "1fe15472e85b169cd226ce18fe6de524";
                iBECKey = "677be330d799ffafad651b3edcb34eb787c2d6c56c07e6bb60a753eb127ffa75";
                RamdiskIV = "4782da1f21aa63915b3e34cfe31acb20";
                RamdiskKey = "2828455b978137fd2e53b3f9c8dd09a4081f5df840f98bddeb6f9c54649286d1";
                rootfs_key = "8ef999bcac523ec3daac7e80297b9fc2c7a22e32afeeaac7a14681b7bfee506f9866fda1";
            }
            else if (iFaith.xml_ipsw_md5 == "706b8987b37bc99e349197ecc5e26c3b")
            {
                iBSSIV = "345a34eb3caf3af6a72a0a9559c2e1f9";
                iBSSKey = "3ffb0887082322cc696a3b1dd69547880363a6f54f1a03b20132d401278f3e47";
                iBECIV = "19db86b7cfa95c222d50c9e3fe6c2681";
                iBECKey = "84b1dc2baa4573899128ae72cd8d2e27cd024ef563755362e6c07547463a6e6b";
                RamdiskIV = "452cd6f17561636ccce83d19d02cbb94";
                RamdiskKey = "33ae12d4d5e0d108067dbe78e341fe7111f496af94b08bc6eed6e79290ac8920";
                rootfs_key = "b51d75f7ba8c51397ccd5dbcf8929b14419bfc5e4b21b0370f73d2c9f9e1067b03336989";
            }
            else if (iFaith.xml_ipsw_md5 == "304a0863e83aff0f88215e3f6a193002")
            {
                iBSSIV = "30121b6c9866b26fe5082ea3001de870";
                iBSSKey = "4ce2b4a380a6df017ca101020e4af9aca892ce96a80bd664b12a75ac9d8f8ce4";
                iBECIV = "0c5fb4c7efaae483f2dc5234fb3159dd";
                iBECKey = "ed45e85db0eb1b28185078106b10a4bee08d2df12522aa371ed50a11219f8f3a";
                RamdiskIV = "84685441459f61fd86b46494a2a5b014";
                RamdiskKey = "53e7181418f483f30cf51fc650702c26738e619ecb2ab66ba40e28ce6ef10f1b";
                rootfs_key = "e1025ad63ff73af6fa941dc85ce449ad67a471bc0cf51c9c5c3507736f2430f6c91dba84";
            }
            else if (iFaith.xml_ipsw_md5 == "ec5debe71307ce8926499029ad35a450")
            {
                iBSSIV = "e803340e5c0ac2d1c898f6980285e67e";
                iBSSKey = "406b5d2678c9292cf3c5af199ba962aa23db115a9d7160fa1bd09fd9932160d9";
                iBECIV = "a160c55d4571b060bb82689e2f10d4ef";
                iBECKey = "457d910bc7fd0ef6b71d2714d20d9a70cbc741e863ae57883dba4568d29912d4";
                RamdiskIV = "18c96b48f3246151eb93f18a8b1cbc2d";
                RamdiskKey = "61c74c9cf976f9d53f9590fe63d268463005451fe0a9f58e1c4ab5c25a0305b5";
                rootfs_key = "d399b1aa72b97fdab4e2124a1c165f291af7ccf7c6ac222c11ec205bb0a1a9108679b482";
            }
            else if (iFaith.xml_ipsw_md5 == "3eeb95d6b80b50d33708471d2e1b9306")
            {
                iBSSIV = "ecd5bd9e4762aaae35637063420d54d1";
                iBSSKey = "a8ef6d02fa5958099fc58fbcc160066faafda0053916905c0009d33ef53c7d0d";
                iBECIV = "c8c8b3eba9a257c4ce0227098e431f23";
                iBECKey = "0f6e9c878784f3b2ce6fa248cbf256dbfc77ca771c202f8c27e461ea4aa9e1aa";
                RamdiskIV = "78a5b5e905b90e50c0cdfa4f0859540e";
                RamdiskKey = "e9a0754c02f259651587f150ee7a1e093429b9948c32157c58fb5619b0f9ee49";
                rootfs_key = "1ab3765d93aaf106b918829db85c18e724473e51106ad89f3d10e863cc3864974c2852f5";
            }
            else if (iFaith.xml_ipsw_md5 == "2120a21b0ec6a8991078b1b88934baee")
            {
                iBSSIV = "67b821977559b8035887d46d68eb5b7b";
                iBSSKey = "2f442e98254dde0a75ca8d35ea48644bce9efdeb34332e0ec730e848539acead";
                iBECIV = "d12f8e04287683c4e2b67feb5fa6e308";
                iBECKey = "975cf31cc14d83389e8136f68f0c26303d7d1db4b197c5f0a3c12b152cde626f";
                RamdiskIV = "89aa32f2a98f6e9b55640e77f7dbd705";
                RamdiskKey = "1de9d6e54b29e0c7968cb8659bfaf1424a5a02dd22695ecc6ae9d33942985d72";
                rootfs_key = "6789bda31f66e753da9e87e9424659430846049109726cced20319f1a9c43621c5b402b4";
            }
            else if (iFaith.xml_ipsw_md5 == "859a6ded3129af1ed0d56f3e25c873a2")
            {
                iBSSIV = "9c69f81db931108e8efc268de3f5d94d";
                iBSSKey = "92f1cc2ca8362740734d69386fa6dde5582e18786777e1f9772d5dd364d873fb";
                iBECIV = "bde7b0d5cf7861479d81eb23f99d2e9e";
                iBECKey = "1ba1f38e6a5b4841c1716c11acae9ee0fb471e50362a3b0dd8d98019f174a2f2";
                RamdiskIV = "3f009f46be4c7bfe4be64183e38b68b8";
                RamdiskKey = "e3b3c8fca6f358eb18a38d611f3b60caac1d3638b38ff5cec50c151226400988";
                rootfs_key = "f7bb9fd8aa3102484ab9c847dacfd3d73f1f430acb49ed7a422226f2410acee17664c91b";
            }
            else if (iFaith.xml_ipsw_md5 == "38312b9d4da5decce7e6a4f042ac9472")
            {
                iBSSIV = "0cbb6ea94192ba4c4f215d3f503279f6";
                iBSSKey = "36782ee3df23e999ffa955a0f0e0872aa519918a256a67799973b067d1b4f5e0";
                iBECIV = "1fe15472e85b169cd226ce18fe6de524";
                iBECKey = "677be330d799ffafad651b3edcb34eb787c2d6c56c07e6bb60a753eb127ffa75";
                RamdiskIV = "26ec90f47073acaa0826c55bdeddf4bb";
                RamdiskKey = "7af575ca159ba58b852dfe1c6f30c68220a7a94be47ef319ce4f46ba568b7a81";
                rootfs_key = "c936f013db5f0e70d8f662fd717bc2e3a208e043afa004fc61e2719720d04906679fbd58";
            }
            else if (iFaith.xml_ipsw_md5 == "a4f25b2af99580f1e69c8277d0208237")
            {
                iBECIV = "19DB86B7CFA95C222D50C9E3FE6C2681";
                iBECKey = "84B1DC2BAA4573899128AE72CD8D2E27CD024EF563755362E6C07547463A6E6B";
                iBSSIV = "345A34EB3CAF3AF6A72A0A9559C2E1F9";
                iBSSKey = "3FFB0887082322CC696A3B1DD69547880363A6F54F1A03B20132D401278F3E47";
                RamdiskIV = "3474F5208FC45AAE32734213171B95EE";
                RamdiskKey = "4F1419111E1C66C2E597FA79ACFAD75CE501D043AF6EDA8A980B6CD4332823EC";
                rootfs_key = "e897c2d0aaaea8f2752ff6e144c6efc3158a5dad13c6b95ebe8b99885fc6ad0f6b5448f7";
            }
            else if (iFaith.xml_ipsw_md5 == "89d5c926c7e4c2fba4d31c55cc25b8bf")
            {
                iBECIV = "d27e94c066831e6148937622ea4b41aa";
                iBECKey = "cce6b84ba39aaa034dfe63efd8fac8c745f7585b8dc305f2c514d5ac9d530c02";
                iBSSIV = "cc7a22d7a097266bab807fd6dc52056d";
                iBSSKey = "1a669876c9e6e1423fe153ca7ab817a9c210fea5e96fb5e52cb4c6e8b3ef4a1c";
                RamdiskIV = "4e1f7c7bbd422056d58c71ac09fb4f1e";
                RamdiskKey = "6c8e84211adeee5f4118869d82c371237f589cf77ec12e89d58d08d7a41a8aca";
                rootfs_key = "d5811298fa4bb88e0bb15c7d16abe69ee74666bd263db16b59eb64b98872de0f9438a56b";
            }
            else if (iFaith.xml_ipsw_md5 == "730d2a2c760c3b8267258447ff19a064")
            {
                iBECIV = "0c5fb4c7efaae483f2dc5234fb3159dd";
                iBECKey = "ed45e85db0eb1b28185078106b10a4bee08d2df12522aa371ed50a11219f8f3a";
                iBSSIV = "30121b6c9866b26fe5082ea3001de870";
                iBSSKey = "4ce2b4a380a6df017ca101020e4af9aca892ce96a80bd664b12a75ac9d8f8ce4";
                RamdiskIV = "c248e221c08ece5862fea42a58dad552";
                RamdiskKey = "28db49d00990ced317a7bcd24755b3426bb246cb135111126d8b3f7bb8ba9252";
                rootfs_key = "2e3166688336a53fbc0ca014ff2fa458007d44048646764604bde3b73c88dac625c49d18";
            }
            else if (iFaith.xml_ipsw_md5 == "5dfa8310e4a62ce0ce57259b55cabb07")
            {
                iBECIV = "a160c55d4571b060bb82689e2f10d4ef";
                iBECKey = "457d910bc7fd0ef6b71d2714d20d9a70cbc741e863ae57883dba4568d29912d4";
                iBSSIV = "e803340e5c0ac2d1c898f6980285e67e";
                iBSSKey = "406b5d2678c9292cf3c5af199ba962aa23db115a9d7160fa1bd09fd9932160d9";
                RamdiskIV = "b05f288ccfce3f870f8572ccf9c939c5";
                RamdiskKey = "9a3b51af7076f4f93febbfae9d1a5388a161076c312732f9ef25ed4b8a0c0306";
                rootfs_key = "33148831d924a6ea1e9f1051cb0abfb15dc481a91617f37d950612de185c5f182d963a15";
            }
            else if (iFaith.xml_ipsw_md5 == "4aadc473058136d1ea37ec3436eb28ef")
            {
                iBECIV = "c8c8b3eba9a257c4ce0227098e431f23";
                iBECKey = "0f6e9c878784f3b2ce6fa248cbf256dbfc77ca771c202f8c27e461ea4aa9e1aa";
                iBSSIV = "ecd5bd9e4762aaae35637063420d54d1";
                iBSSKey = "a8ef6d02fa5958099fc58fbcc160066faafda0053916905c0009d33ef53c7d0d";
                RamdiskIV = "ce6b50a8351823e0f21857d58bfc1dd1";
                RamdiskKey = "da5438ed6938b3eeaca8a3b543bcf2e461e9d9cb4f327722b5f8c3c55b197ea1";
                rootfs_key = "f21b4cc1ae39ed3b36d33374f77e224b9e77f375f8722c551acdc7033ef16383c0b99b39";
            }
            else if (iFaith.xml_ipsw_md5 == "59bdbc355b5d0e17671edf5d6a069ce1")
            {
                iBECIV = "d12f8e04287683c4e2b67feb5fa6e308";
                iBECKey = "975cf31cc14d83389e8136f68f0c26303d7d1db4b197c5f0a3c12b152cde626f";
                iBSSIV = "67b821977559b8035887d46d68eb5b7b";
                iBSSKey = "2f442e98254dde0a75ca8d35ea48644bce9efdeb34332e0ec730e848539acead";
                RamdiskIV = "61062814b88c0a5802a7baa379d96a9c";
                RamdiskKey = "33c72207db71770fcc47e5c5807d39fa8ee451f1ca96ec351b434cd353bcf0f9";
                rootfs_key = "f8213d5a6daef80ef0f61b0e5524df8d45dab2ff7155db079538e904bb7c4195d7d8c802";
            }
            else if (iFaith.xml_ipsw_md5 == "faa71fa66e0cdfbf3e4d13a5f496b270")
            {
                iBECIV = "d12f8e04287683c4e2b67feb5fa6e308";
                iBECKey = "975cf31cc14d83389e8136f68f0c26303d7d1db4b197c5f0a3c12b152cde626f";
                iBSSIV = "67b821977559b8035887d46d68eb5b7b";
                iBSSKey = "2f442e98254dde0a75ca8d35ea48644bce9efdeb34332e0ec730e848539acead";
                RamdiskIV = "2125250914722704700cc735cbe4a3ae";
                RamdiskKey = "9f0a7066672af338ae182f0e4885097e83331004fdbca9d3915fa043b781d935";
                rootfs_key = "edb697411e210af363ec602d27e73745e86b426c8f710d0b63efeb5e609072e7920c3846";
            }
            else if (iFaith.xml_ipsw_md5 == "675ff9546ef2797b571e7023591317ca")
            {
                iBECIV = "4c6dde5eabbbd39870c9a922513af94c";
                iBECKey = "ddf3fc5a6394ad7c5781d2c613bd54d0ee84f0b9c39a2c4ba857f7ef19bbd72a";
                iBSSIV = "03c4fb6ab4104c693bd52991cea35dea";
                iBSSKey = "1b081d4ea8f29d4535726c4a5fbf284d77484f25fc145c446d7e9e8acffbcf7e";
                RamdiskIV = "a6aac3af81e234cadf46b56905eca484";
                RamdiskKey = "5513783229d943c744f7c15efdddc5431caf0e38eb5b2e35ba617ac053247710";
                rootfs_key = "18354ead5a9e133c5979be55211c73d45cbdd36d6bd959386e2010093256e454459587d3";
            }
            else if (iFaith.xml_ipsw_md5 == "dde4fa0286fc3b802e6e48751e7f9f44")
            {
                iBECIV = "aac3f3fd4cd55925c548eba979a619bc";
                iBECKey = "cedeac04955b09d43ded783bd5d6144cf22ecc0f3b896af15677af9a1b887b9d";
                iBSSIV = "173044c02240ae5fed473913383d1acd";
                iBSSKey = "2aff0ec73e280224062d7478ae397731669c64261d0643b385acded2535e5931";
                RamdiskIV = "9f23c76c9352ead1ae040ede2b7b503b";
                RamdiskKey = "f79071ebef1f9ae1d3d6d391fa781edb6508d6b9d1443b91bd561dd2ae9b5a21";
                rootfs_key = "d7fd5868fd5533b894658cef38852f3bf0bbacea6375e2f579a9e8bf92f34007a71dcc85";
            }
            else if (iFaith.xml_ipsw_md5 == "80d8c49b2587127c77d391703fbc2bba")
            {
                iBECIV = "2efa019c5b20a427daf6a3cbf4452209";
                iBECKey = "1a982cd5566a3476db211c4c1aafbd2450843d79f6478a87a3b0b9f1562fc1d1";
                iBSSIV = "c32ff0788530d2650a3c1108a959aed0";
                iBSSKey = "bf3b69f9bd9c3ec1a6aae962d9cb3da4bbc4e1de12608df2756dc0814857fee8";
                RamdiskIV = "404e344acc3ab148a3d90997f00aa308";
                RamdiskKey = "96bbec672f7c5533250f4fa14798647031b5913843d00d8a4c9b6ebec0664f55";
                rootfs_key = "9b80f46bffd90f2507fd1978df3d72e933ac0f894b8ff13aca73f2ef6b822186a4075236";
            }
            else if (iFaith.xml_ipsw_md5 == "7323c6b0bac1a367d1ae59b2897b7e89")
            {
                iBECIV = "5d4cae31fee59b4f671ec3823dffc421";
                iBECKey = "5f607a42b35d8935c559ba1216cc19acd1c51dde09c994950c5976b5b4e5fbbd";
                iBSSIV = "b65e495bc0f82471575301f38c9216d8";
                iBSSKey = "5986012d3cd4174b82d1ed5cdc2d0bc0a2bf1ac2f1afeab2ec7b04446be08ce6";
                RamdiskIV = "6e6031e8a7757fa54aed9ac05dafde0a";
                RamdiskKey = "ed31e88ae13d996c868b86712d2831d8f8272842b3ecc7baa4bb186dd4b5b4e1";
                rootfs_key = "ca9ed452f256fa47ccdb387fdc0daa53d19428b2569fb089af58df576f6e54a4fb6de0ae";
            }
            else if (iFaith.xml_ipsw_md5 == "57206fff6428b27a62932837c18fc1cd")
            {
                iBECIV = "4788a687d9cf611de77bdf3d99de52c5";
                iBECKey = "543fba3559d74a2011c2c646c41c1ea269c08e2f86e5adb6ac919f0b7015127a";
                iBSSIV = "fc18dfa9a6808b431daaa17b5dca5521";
                iBSSKey = "d09343a75e2cbcdc759449ceafe3c56a7630bcb425323d881b33ef1a92e1be72";
                RamdiskIV = "1fd94ddb456bf699461a416b69c0575c";
                RamdiskKey = "2e471734ab607e35fc25c793a4927e68d3e2a3d5e6a3a6da28ffe2b529d8da2d";
                rootfs_key = "6055317e3c3f6021c8e0b9e1daa26ec387d2e798c9ee8cf0ec3150665b049ef6d4c10e67";
            }
            else if (iFaith.xml_ipsw_md5 == "0003dc6b9dacca06f6325a7af0334b49")
            {
                iBECIV = "2c4eedbdec65e724a309b73865ad39f6";
                iBECKey = "1a44d9460136b0b11394da63b761c594e88ea4fde24be5cec47a9fba47ebec6f";
                iBSSIV = "75c0c6ece308ef95caeab8b8889046c8";
                iBSSKey = "f6bc9f655629330d4ceca465adbc4fecd71a3be8aad604e01675142c5b4827a3";
                RamdiskIV = "612931d878d77ab86f1d5e89bdad0594";
                RamdiskKey = "a9e1160537469543d0dc16c3051bf5c4791c845ba5473dec2c159fcecbace173";
                rootfs_key = "e2f187cced00643b2129df66f2d22a6bc0c58235ca3cf85d95d7593382c3178737cf7113";
            }
            else if (iFaith.xml_ipsw_md5 == "21e199beb88199bb542cf48532e42d1f")
            {
                iBECIV = "f5a848aea8e5b7be6ab892c654d744bd";
                iBECKey = "2dec22bfcb15adda85f646ad651910f351f782aed6dc5cf2473d7faf3acdfc59";
                iBSSIV = "ae7cb1a68476d60b5f46e990715df7bc";
                iBSSKey = "ce35e0ba4bf6f6386f2646c623969d6e0afe6034d4cf1e2100f0a54293ed9a7d";
                RamdiskIV = "a73034ff909de56717cfa1104b02f670";
                RamdiskKey = "ebf05aa38d621df8620b9a7cc596da20b326c624a3ad269e4c98fa3cc6fc657e";
                rootfs_key = "4ca4fb440f88acfd8333efa0b171b02bc76942d448a71922da6a4502e63d8f28bc9d958f";
            }
            else if (iFaith.xml_ipsw_md5 == "42c1e6ec73d5a8c340f2f4963a016a64")
            {
                iBECIV = "5f91818c8a5bc2027627aee8ac14ef42";
                iBECKey = "741bfbc205572d57161288a84b1348ec4bce49951fa50a5818e586b672742f1f";
                iBSSIV = "36b83fb0f15fc824927f64714b572966";
                iBSSKey = "108be8e2f40673d347acaa48142df54ebbc17b4b321f8d1392bc311e9cb92347";
                RamdiskIV = "0ae997b55b71a32d9b4948a5e51393b8";
                RamdiskKey = "b949d4f33fc2c5d18c4f5c621492780f0bb5c98b3ab4fb9026da91d989f47e8f";
                rootfs_key = "3ac8725bed4f5f5cdad44c224dcfbc230e1480d081cbbd625713edc85a9b54015d6165c3";
            }
            else if (iFaith.xml_ipsw_md5 == "83548080f405e81e1223acb4d3f8bccc")
            {
                iBECIV = "4517bebd4d0fad488bd88f671831609d";
                iBECKey = "151c7f6bacb40445de2daf2c148a30e575e08d2838d748c8bdd3b5035c2c89c7";
                iBSSIV = "11fd507c05c222c9ea30634eada9d391";
                iBSSKey = "9b9bd947f740e60ca8952f8f9a5761dc8eab731cf89e23e8691d64e16074234f";
                RamdiskIV = "194e366b834b8f9e9198501c97bce415";
                RamdiskKey = "cae9139cc5acc140d56af65a6fb99635dd6d404ae928f359030c34dccea4b5c6";
                rootfs_key = "43fd5c7357cfc1bfa0c16ab72343bcb9e8974922b3c00af7a27cf522d2dca3ec471a6f83";
            }
            else if (iFaith.xml_ipsw_md5 == "3ded74ea874786412cfb3ede58b74d36")
            {
                iBECIV = "26c45c74ce6e8134537525ae4437ad2c";
                iBECKey = "a38bcd7c78c6ff41d0913d622a8ecc49975cce20fe206331f5528eb23e31c0bb";
                iBSSIV = "39bc68dd59357807c778c9d4e1feaaf5";
                iBSSKey = "1342f6a40ada33b16372e38bf18f5c8d40fc778ab74821a94a31e8a9d5f6f860";
                RamdiskIV = "62ad02fef7cc21902056800ac207c1e2";
                RamdiskKey = "901c189778dad9fce1ea02f5de7bba020719da4ee1799a49b32a741ee29096c7";
                rootfs_key = "f5705c4d5d7ce7972ab915e62cac8096e254c3bf3635d7d205eab868d447959135d06c7a";
            }
            else if (iFaith.xml_ipsw_md5 == "5b8590ced08f275ad5446a16e68b77a5")
            {
                iBECIV = "d8cf8b9b2043b88a29ee63d01ff900dd";
                iBECKey = "5c06fe1539c5bc1e9ad5ae4025b682cc08ad5e256c4e844828e7bd3186627fd6";
                iBSSIV = "23385066017c0b14542736d5308fc9bb";
                iBSSKey = "7616fe3865cbb9c98e764a0c26c4911d58f14048cf3faffa50417cc55a6a296b";
                RamdiskIV = "8b1ff0c8c73405dfe723f6be3f9290dd";
                RamdiskKey = "4f97bc9ba21e3535e640b3a69f0b88ae260506a499a991ece2b1b9797eba5052";
                rootfs_key = "1076b3547842e1822a10eaa22723c0ef9145f823899be7f876d5bde1f853f4fb496ab606";
            }
            else if (iFaith.xml_ipsw_md5 == "e9fc36dedc8857129c53b261ae4e1802")
            {
                iBECIV = "00aaa6ef709023422dc8cabe0624cf46";
                iBECKey = "b3eb6a85b1ea2eef63b7de99ca12648b2b46b27df74dc2b024860a5507614613";
                iBSSIV = "d28ad8b5ca4cac1c178fd3465ceea743";
                iBSSKey = "753cc79032d95b847c1b50e13815b57f887e152e68a96b59386a40f811bbe5a6";
                RamdiskIV = "c0bcc59087dda18d0627af51700b80b1";
                RamdiskKey = "51b355da8a84ad677d44eb37a88085e9f506aa196e45d1cd2cbc27e63ef5747d";
                rootfs_key = "82465a9f8328fdb0a4ef2b3fb19bb8934cb96e773d10d7e2b4ab29a5df482870c6737f7a";
            }
            else if (iFaith.xml_ipsw_md5 == "1f88e97e4e869b53d8be6ee7ef2e58ee")
            {
                iBECIV = "54d5ab595179a136429c15b26cbc981d";
                iBECKey = "8480b0fd46fd32761ad897415711312b116eaa8e62d52ff64c105b2378524914";
                iBSSIV = "c14caf1c929efa37727e7b232304d184";
                iBSSKey = "2fc6581e36855589f93dc002db82fb7c9a301b2585d2bb165040e9de27e36dcc";
                RamdiskIV = "adb23f4a624b9436ae2f7cf6e17fd0f7";
                RamdiskKey = "6aad58a5d981dda1fddadcebdafd632f8a9df5ef7ae2c2474fa307fc7f7de8e1";
                rootfs_key = "693d830170427c2441248f39c56bfbf3bf37b4364859ae00083d88952ffc4df6191825c8";
            }
            else if (iFaith.xml_ipsw_md5 == "88b6cc22c4482562c0c056ac47da7d05")
            {
                iBECIV = "0b368a5454c74a91534dc9bab5218011";
                iBECKey = "6b0c7684585b96e04746589d44ce721c9ed50e4b7233642cf7d955047220f40d";
                iBSSIV = "7f1d2e4cd0755e987f12695bae4a448d";
                iBSSKey = "84f2b75874d2e086029f83c359f95eec0df675a217263be88e1cb3409bdbf058";
                RamdiskIV = "bee7f2bc37b701853d030543d2c643e0";
                RamdiskKey = "cd71b0bd3ec86426f60576de00bfb28af0b7c059e897597f589558c29a49282d";
                rootfs_key = "363c94645e9a481eb7911210fa9ff1d4eaec046ddf52428ec02e50b71be06e69cb25de62";
            }
            else if (iFaith.xml_ipsw_md5 == "7e372c8bbae7f0cb8e77c328c091190c")
            {
                iBECIV = "9ca57196cedafeca2b51bede45822a18";
                iBECKey = "f28a258f8ffb1de8cf7f630c6c769a2a503201cadc736d70f8f53bb24b399d9c";
                iBSSIV = "5ecc87c1efb6d8fcf22147d0e7e035ed";
                iBSSKey = "a9c9c3e585dc44de759108d7e61a0a94599d85afafadd239fa75f0ce09365585";
                RamdiskIV = "81b3ed385351e496bdf2db0db8ff0c1d";
                RamdiskKey = "553273df8f2bfc90509354fdfcfebe3e4c5709a790ef9e72923ad5e5a124c2e3";
                rootfs_key = "47d6a1e876dbd3807d2628c32f934525e48fa787f605ee90d209701a76868831def85c5a";
            }
            else if (iFaith.xml_ipsw_md5 == "f5f6588902174948c3e9d4f9b3a89e5d")
            {
                iBECIV = "1168b9ddb4c5df062892810fec574f55";
                iBECKey = "061695b0ba878657ae195416cff88287f222b50baabb9f72e0c2271db6b58db5";
                iBSSIV = "24af28537e544ebf981ce32708a7e21f";
                iBSSKey = "f7f5fd61ea0792f13ea84126c3afe33944ddc543b62b552e009cbffaf7e34e28";
                RamdiskIV = "da6418ec5e6a7dc19ea86ee122c3f399";
                RamdiskKey = "2ae87a59e7e7915676a24b36688a550e7856304f2fba9ce9395cabf7916f2dca";
                rootfs_key = "751775f5f345c9632fcc52876c63cffffd5b05b28b402986e3e0898c2fb8c58051bfd575";
            }
            else if (iFaith.xml_ipsw_md5 == "5a14a53b21c8c01621117d57643dcf73")
            {
                iBECIV = "039cd6946437e47a0f7aabc5f3dcd90b";
                iBECKey = "4862d93fb89f27c9583e08658f83d32270e0b6fb0dc828dd47082b4437d43e30";
                iBSSIV = "123204d93f3a619d25a65b0ce630aff6";
                iBSSKey = "25410a268e28aca74344ce44d97d560691211864c4c2084662802f13a60697af";
                RamdiskIV = "e8f36b03abe76a7b67113b81a03bc815";
                RamdiskKey = "9a60d98bfe9113a9266c127b3ea1518ddd9a4a2f476195823138c49a780bfdfb";
                rootfs_key = "d408ec0ee309f3e574e85e71765b71f49a5830bebf5e5b45b28d8804461200e1bb20902f";
            }
            else if (iFaith.xml_ipsw_md5 == "f94dec674a21f295d78e9c09e2f4ce63")
            {
                iBECIV = "43fd6664d26e19b90bdacb6ec9cd6de5";
                iBECKey = "852bd17bd52a8b6e1226ca4a80ca7a6767deaabff9a576eb62fd5ba3a5b7b299";
                iBSSIV = "dc63a51857ccb1172d8e04ec22316e67";
                iBSSKey = "f4509af1eece8d894e73f9907aefaebf66df8368a60099ec91741d5761cabaa2";
                RamdiskIV = "d04a5bd786cc54b4dc86bd0a1794d1a1";
                RamdiskKey = "ad0a912030ded80bbbf853b8bfb9136f257b9b276b61f9c5054325241007ca1e";
                rootfs_key = "47609f7d1fda9002160be1ea48a75c0497eab5c31051d221fc520974a4804b5fbe1103ff";
            }
            else if (iFaith.xml_ipsw_md5 == "b41c5b614983ea1c0696369fb0af5e17")
            {
                iBECIV = "695a697f82978c55769b11f9753d404f";
                iBECKey = "56a77bbbc4e9a493a9bd6a28d41bdd6dcf8681d77f4098e9e9837beca064261e";
                iBSSIV = "0a0e6f2107a61726132d2add8b6aa599";
                iBSSKey = "b16fc642fd06e45b0e1bd9f7f1a869d7124a5206bc901d23562f3318aa4c3af7";
                RamdiskIV = "6124b49aefe5d9d021e2a00212db67bd";
                RamdiskKey = "a79d15c937448703644ddcb2f18384dda9ad3d2efe9f9448473deca1a2581ced";
                rootfs_key = "9d4aa60414c4656ef3ecded3315e8717f0c6f308f19bc34e7a8d3fcb8787f7ea58ac2f1a";
            }
            else if (iFaith.xml_ipsw_md5 == "96daf1876e35399dc6954db998e1463c")
            {
                iBECIV = "8c8254cd2bcdcb9f99c1b97d6296a966";
                iBECKey = "425e89fd80735fdcc664c0a04ee8a2ee73a9239ff33338329bf793ec9e736718";
                iBSSIV = "c4ed1ffe6fc1bd993f0db795145b5d6c";
                iBSSKey = "b6b018fc6727fdce291415f678fd19fe4f945e073b742525de380193e50966c3";
                RamdiskIV = "e955fb2268d1d94996831492dac9664d";
                RamdiskKey = "3a3b7b0768fef2f02e8601558640d75ce590c9f5ffbc7c515d2e022d1a21077a";
                rootfs_key = "7ea65d98990caf5db0d92c4a7f5938fe850b3c511de24fdf1f8460ec98caa8698f5fc779";
            }
            else if (iFaith.xml_ipsw_md5 == "65c85f58df8bb68b00aebdc46081248c")
            {
                iBECIV = "007445532f74d73324809bb89dd4e227";
                iBECKey = "4ba778de8d22c9aa4c82227933d87fff3dd895d4dbbab9566a76b9d58ab331fc";
                iBSSIV = "f6a85b0dc5f5fdd1bf60dbebabf7cbb4";
                iBSSKey = "96cb3c60be2dca2c80d1ea9f569da4745c87bc76e3db3e2bb5de950b2ceb4e06";
                RamdiskIV = "fab2441195cde907e72d8edc17313173";
                RamdiskKey = "2d37685fadf19212f422c37dd6beb5191098b56c9efecb0d894946c5079d33b6";
                rootfs_key = "01c1fb58265e6a22ebe784edf55cc549d4f7f73d5a5769eb7365d1145b3f9e0798b8851a";
            }
            else if (iFaith.xml_ipsw_md5 == "2184c60a04b4dd221136d307976c1dec")
            {
                iBECIV = "a6eef8fe8db73282be908226a74c10c0";
                iBECKey = "da3c339e4bfeae8da41b053bdab5eedbc63a77287d9b11b3e3524eab046ef42c";
                iBSSIV = "33447a994978cf60ce89cd17d3f5f174";
                iBSSKey = "e3a8d236888de01d85b314c503a85bafd2bf6b913026e51b05aa3b1b77e8a4a0";
                RamdiskIV = "7937ee0cc3f72976a085e738f9e55732";
                RamdiskKey = "0cbce3c14497d4a24f195a969b60c681d2aac9d18ef18f52925d30924745b9e8";
                rootfs_key = "ff85a46a289544a28bce5391961b7017a5a80dcc6cee8a0f0bd240be10cd19872877108e";
            }
            else if (iFaith.xml_ipsw_md5 == "35f587e5943e74fc1fb571a6d30eb003")
            {
                iBECIV = "913e8cde71ef57f76bf7bfc5a60483f4";
                iBECKey = "cc6930bb43b1205202bc7952bfe773a294b3124e591c8965d4570901658947ce";
                iBSSIV = "fbe453ddf2767e1e8aca5fdad68ef10b";
                iBSSKey = "ef853015c55869d9d984f42b2003030d8b23e19cb178c508e12f75feaf4377c6";
                RamdiskIV = "bc4ea80b6dc4947fae40d48487c0eb65";
                RamdiskKey = "687d3758520df052a631776e38113a30c430e1b44392dfa9f4c8698b88e5cc34";
                rootfs_key = "f620b7ac829004a7cdf68ffd10008e751878f18331655dbd6b7e08587d0655507cd9319e";
            }
            else if (iFaith.xml_ipsw_md5 == "e4ce2e3d90ba2f2fe3b24a5c1ce412b2")
            {
                iBECIV = "f6a150839bc52d8dba3e892fabe6d8d6";
                iBECKey = "6975604cfbc853b01988f798305098ab8a70008d5fa7d6596a18e47ebbe2eff8";
                iBSSIV = "d4cce4a1b57ada6f3e388e3c31261ded";
                iBSSKey = "9e92bc35749e9112228ff6ed842e079aec3f51b297dca3155b492eaee6247f07";
                RamdiskIV = "1d107690c21fcb7d5dfe517c083a4cc1";
                RamdiskKey = "b69f58f4e73d8fc8b27b4ceedc455e9a79da3002771535073684ee699962e8a2";
                rootfs_key = "67aeecbad72fb61ae972895abaf4e903df26cfd6950e089e63fdac21715e078d3f8dac3e";
            }
            else if (iFaith.xml_ipsw_md5 == "4d8b9d5c193a5e22d6c7b73e8632d26c")
            {
                iBECIV = "efa1897c00457af3d32eca8ceed03b58";
                iBECKey = "e30689dc5ec354944161fcf22648e3ee0751d2c47032fb710f78c5c729ea4d51";
                iBSSIV = "1f8d5f19473f66ced46ec6f1814a1c53";
                iBSSKey = "3c52a466f4729e7949863be363b97e0e240ac38beb2e2dabc07eaea364fb4373";
                RamdiskIV = "073ce9b9da17b327502e2458c2e5a277";
                RamdiskKey = "7b68e4426dd760ec650a555ce966301503a374eda8ebe4121b96884e3767f6a7";
                rootfs_key = "331196c34863c2c9e49beec4ca5c53729049d4874d4b5a4f50d585b2d72adeb7fbb6f3ec";
            }
            else if (iFaith.xml_ipsw_md5 == "fa4605a61404b45c2087a360b692d531")
            {
                iBECIV = "dbe2d9627bec33a227bde550517ebdec";
                iBECKey = "cd9c4aeb0341d648833483f7aab1f69a06652e75d621651a758cb3a701f06c1b";
                iBSSIV = "539ae1e76ec0630ced66d091aa076b22";
                iBSSKey = "1828ec8910a254a1b42c9442ac9f75afdd66026f77c9f8bbec51132b8c7a2f85";
                RamdiskIV = "cea6367cd0819c15d6065295271f28d0";
                RamdiskKey = "ec1b68ffc225d13e0887adb96448e35d02462852514da00205c982d38a523575";
                rootfs_key = "4bcdd29f167775f32fd7c6bfec2e1f2ffec9b8d7bf72832092a8be71501e347c459e9bc5";
            }
            else if (iFaith.xml_ipsw_md5 == "845a42c0139cdad6d373bf3414b25786") //iPhone 4 GSM iOS 6.1.3
            {
                iBECIV = "de6c1e093883b4d09df38259d4a79a3f";
                iBECKey = "22c6cb95034618338ce479927132c5dde85f1d7e936ccefb0f1689c1dcce7724";
                iBSSIV = "19cb1b58bbf7f392a86b009219978b0a";
                iBSSKey = "ffd8a1e3b07179a92a6ff40fa2859091c1994e56b8bdb86e99fab9191ec92e91";
                RamdiskIV = "69317c3838ea9c0bc0af5989782dedab";
                RamdiskKey = "4671e53cf6d38df7082b3171eaa60d32d7097c16574386975496b66549dd65b3";
                rootfs_key = "c676166434b79cbde341e094000dfc47b3161034371b25a9054880a757d73aead553a317";
            }
            else if (iFaith.xml_ipsw_md5 == "36f803242a01f35e47b9227babccd8c3") //iPhone 4 GSM iOS 7.0
            {
                //SIN DATOS
                iBECIV = "de6c1e093883b4d09df38259d4a79a3f";
                iBECKey = "22c6cb95034618338ce479927132c5dde85f1d7e936ccefb0f1689c1dcce7724";
                iBSSIV = "19cb1b58bbf7f392a86b009219978b0a";
                iBSSKey = "ffd8a1e3b07179a92a6ff40fa2859091c1994e56b8bdb86e99fab9191ec92e91";
                RamdiskIV = "69317c3838ea9c0bc0af5989782dedab";
                RamdiskKey = "4671e53cf6d38df7082b3171eaa60d32d7097c16574386975496b66549dd65b3";
                rootfs_key = "c676166434b79cbde341e094000dfc47b3161034371b25a9054880a757d73aead553a317";
            }
            else if (iFaith.xml_ipsw_md5 == "1c51e1883bc5b3017077672c8c20aa17") //iPhone 4 GSM iOS 7.0.2
            {
                //SIN DATOS
                iBECIV = "de6c1e093883b4d09df38259d4a79a3f";
                iBECKey = "22c6cb95034618338ce479927132c5dde85f1d7e936ccefb0f1689c1dcce7724";
                iBSSIV = "19cb1b58bbf7f392a86b009219978b0a";
                iBSSKey = "ffd8a1e3b07179a92a6ff40fa2859091c1994e56b8bdb86e99fab9191ec92e91";
                RamdiskIV = "69317c3838ea9c0bc0af5989782dedab";
                RamdiskKey = "4671e53cf6d38df7082b3171eaa60d32d7097c16574386975496b66549dd65b3";
                rootfs_key = "c676166434b79cbde341e094000dfc47b3161034371b25a9054880a757d73aead553a317";
            }
            else if (iFaith.xml_ipsw_md5 == "46b107f3c3c29d842f767b991001e23e") //iPhone 4 GSM iOS 7.0.3
            {
                //SIN DATOS
                iBECIV = "de6c1e093883b4d09df38259d4a79a3f";
                iBECKey = "22c6cb95034618338ce479927132c5dde85f1d7e936ccefb0f1689c1dcce7724";
                iBSSIV = "19cb1b58bbf7f392a86b009219978b0a";
                iBSSKey = "ffd8a1e3b07179a92a6ff40fa2859091c1994e56b8bdb86e99fab9191ec92e91";
                RamdiskIV = "69317c3838ea9c0bc0af5989782dedab";
                RamdiskKey = "4671e53cf6d38df7082b3171eaa60d32d7097c16574386975496b66549dd65b3";
                rootfs_key = "c676166434b79cbde341e094000dfc47b3161034371b25a9054880a757d73aead553a317";
            }
            else if (iFaith.xml_ipsw_md5 == "103619647ccbcd365c0ad816280133c1") //iPhone 4 GSM iOS 7.0.4
            {
                //SIN DATOS
                iBECIV = "de6c1e093883b4d09df38259d4a79a3f";
                iBECKey = "22c6cb95034618338ce479927132c5dde85f1d7e936ccefb0f1689c1dcce7724";
                iBSSIV = "19cb1b58bbf7f392a86b009219978b0a";
                iBSSKey = "ffd8a1e3b07179a92a6ff40fa2859091c1994e56b8bdb86e99fab9191ec92e91";
                RamdiskIV = "69317c3838ea9c0bc0af5989782dedab";
                RamdiskKey = "4671e53cf6d38df7082b3171eaa60d32d7097c16574386975496b66549dd65b3";
                rootfs_key = "c676166434b79cbde341e094000dfc47b3161034371b25a9054880a757d73aead553a317";
            }
            else if (iFaith.xml_ipsw_md5 == "3d0d8227bd07c3b0e0d7549a0ecd463d") //iPhone 4 GSM iOS 7.0.6
            {
                //SIN DATOS
                iBECIV = "de6c1e093883b4d09df38259d4a79a3f";
                iBECKey = "22c6cb95034618338ce479927132c5dde85f1d7e936ccefb0f1689c1dcce7724";
                iBSSIV = "19cb1b58bbf7f392a86b009219978b0a";
                iBSSKey = "ffd8a1e3b07179a92a6ff40fa2859091c1994e56b8bdb86e99fab9191ec92e91";
                RamdiskIV = "69317c3838ea9c0bc0af5989782dedab";
                RamdiskKey = "4671e53cf6d38df7082b3171eaa60d32d7097c16574386975496b66549dd65b3";
                rootfs_key = "c676166434b79cbde341e094000dfc47b3161034371b25a9054880a757d73aead553a317";
            }
            else if (iFaith.xml_ipsw_md5 == "9b41651768dd716472577646ed1876db") //iPhone 4 GSM iOS 7.1
            {
                //CON DATOS PARCIALES
                iBECIV = "b3cc19dfe3f44987ef84d56e18396cfa";
                iBECKey = "28a96dac939f217bb835dc94d32163b05abe7d1f8a4a8273a3f5eb2a89a74eec";
                iBSSIV = "6073dcb7441392f4f3a882e7d0b8b398";
                iBSSKey = "109ae12f073401f16aa19cc87b223335e2aa6a603a53e1022fb1ce904000a091";
                RamdiskIV = "9d599763f090db2c53e9bdb23f9e2200";//USANDO RECOVERY MODE
                RamdiskKey = "2a248f817574d1da7d632b2277eb0d9a299f18eb03b408484ee1bc790cfcf921";//USANDO RECOVERY MODE
                rootfs_key = "7aa3a515804a8c256da56c065ff28db9150c8ee3898a4525ab2c40100d1bb0ff8b287e78";
            }
            else if (iFaith.xml_ipsw_md5 == "97c001bf6e4bcf5d832af97b762cae24")
            {
                iBECIV = "253b947b22fc66ca5be504379f8b3d05";
                iBECKey = "709a4eae59d737f53d910b4110f060c0b33212f5efc0c7305b01af083ca09445";
                iBSSIV = "6317777d94e4ff978ed0f58bd6c6ab8c";
                iBSSKey = "cd851654229d80b7e9e48f0fbc5748f499595727e9f5635bd0ea2d195fe81557";
                RamdiskIV = "2a86581ac1cdce1bde2a3ad9e179d982";
                RamdiskKey = "8919343e99f38e8c0688aea588118d982e9d5441ebab6608744b39ec416e60ef";
                rootfs_key = "7e2dbf591820349a41df18e39e18b351422e2ca424f3cba61709d7384545f5d8dd7674de";
            }
            else if (iFaith.xml_ipsw_md5 == "db301240840daee619d98d87f6c89921")
            {
                iBECIV = "da1a960590726a66f23d6418602d6e63";
                iBECKey = "6e8292914a5597610f7d76fdc25ee88ad5240c19591f071f4993cbdefa902019";
                iBSSIV = "293cc706282984db11e1d4e44d9d5709";
                iBSSKey = "a383c3055d8a6f5350226cb2af458e29aa38abda3672c38d8d63dfa1118988c8";
                RamdiskIV = "e7c082ad98b5fe0ed45bc95531db50e6";
                RamdiskKey = "6328b0e17dd264f5eaa21ce9b135119924407cce39f7d396e631f5e7ee3e6087";
                rootfs_key = "3ad3f6163e6d6307f7149ae980df922725718f32f28554d8969cbdb92349e3c79de9b623";
            }
            else if (iFaith.xml_ipsw_md5 == "fadffd38b7de78b715eb7f3907042226")
            {
                iBECIV = "8277ba98e6dc7e1607558f5ecf1b7360";
                iBECKey = "d69db458d5250f9447f9051681585dd878a311e21f7434c98a9e6aed46268bf8";
                iBSSIV = "e4a4ead4ed015a70f0a8dec720d16641";
                iBSSKey = "5c127599783645675cdfff691599f6dbccabf84e5b846251a0eade2a75649465";
                RamdiskIV = "bd08885edcbb892945416c230ab4ebba";
                RamdiskKey = "ee836dd681f7387d8cc1ab69e2f7e935016a096379539432720436b909e9de03";
                rootfs_key = "625003a349494ed7fc1972d30f7cf840f8bbff11b2d493593017d0606d88605b7fad5e61";
            }
            modProcessCmd.iDevice = iFaith.xml_model;
        }

        public static object ParseECIDFromRawSHSHBlob(string shshblob)
        {
            return iFaith.ByteFlipper(shshblob.Substring(0x18, 0x10));
        }

        public static object SubParserSHSHRaw(string WhichBlob, string shshstuffz)
        {
            if (shshstuffz.Contains(WhichBlob))
            {
                RichTextBox box = new RichTextBox();
                box.Text = shshstuffz;
                i = 0;
                while (!(box.Lines[i].Contains("<key>" + WhichBlob + "</key>") | (i == (box.Lines.Length - 1))))
                {
                    i++;
                    Application.DoEvents();
                }
                if (i != (box.Lines.Length - 1))
                {
                    while (!box.Lines[i].Contains("<data>"))
                    {
                        i++;
                        Application.DoEvents();
                    }
                    string s = box.Lines[i].Replace("</data>", "").Replace("<data>", "");
                    while (!box.Lines[i].Contains("</data>"))
                    {
                        s = s + box.Lines[i];
                        i++;
                        Application.DoEvents();
                    }
                    s = s.Replace("</data>", "").Replace("<data>", "");
                    box.Dispose();
                    return iFaith.ByteArrayToString(Convert.FromBase64String(s)).ToUpper();
                }
            }
            return string.Empty;
        }
    }
}

