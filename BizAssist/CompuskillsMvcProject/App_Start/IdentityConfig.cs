﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using CompuskillsMvcProject.Models;
using MvcProjectDbConn;
using MvcProjectDbConn.Identity;

namespace CompuskillsMvcProject
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }


    // Configure the application sign-in manager which is used in this application.
    public class TtpSignInManager : SignInManager<Employee, string>
    {
        public TtpSignInManager(BizAssistUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(Employee user)
        {
            return ((BizAssistUserManager)UserManager).CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public static TtpSignInManager Create(IdentityFactoryOptions<TtpSignInManager> options, IOwinContext context)
        {
            return new TtpSignInManager(context.GetUserManager<BizAssistUserManager>(), context.Authentication);
        }
    }
}
