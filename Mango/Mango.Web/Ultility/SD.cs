﻿namespace Mango.Web.Ultility
{
    public class SD
    {
        public static string CouponAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JWTTokent";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
