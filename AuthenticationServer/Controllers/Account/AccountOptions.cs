// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace AuthenticationServer
{
    public class AccountOptions
    {
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
        public static string InvalidCredentialsErrorMessage = "Invalid username or password";
    }
}
